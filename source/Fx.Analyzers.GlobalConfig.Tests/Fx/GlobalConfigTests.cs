namespace Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class Extensions
    {
        public static async Task<string> GetManifestResourceString(this Assembly assembly, string resourceName)
        {
            //// TODO have a sync overload?

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception("TODO");
                }

                //// TODO parameterize the constructor parameters
                using (var streamReader = new StreamReader(resourceStream))
                {
                    return await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }
    }

    [TestClass]
    public class GlobalConfigTests
    {
        private static async Task<string> GetManifestResourceString(string resourceName)
        {
            return await typeof(GlobalConfigTests).Assembly.GetManifestResourceString(resourceName).ConfigureAwait(false);
        }

        private static async Task<string> GetTestString([CallerMemberName] string testName = "")
        {
            //// TODO do you like this method name?
            return await GlobalConfigTests.GetManifestResourceString("Content." + testName + ".cs").ConfigureAwait(false);
        }

        private static async Task<string> GetEditorConfigString()
        {
            return await GetManifestResourceString("Fx.Analyzers.GlobalConfig.globalconfig").ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Test()
        {
            //// TODO this test should actually go in fx.globalconfig.tests

            var test = await GetTestString().ConfigureAwait(false); //// TODO do you like this variable name?
            var editorConfig = await GetEditorConfigString().ConfigureAwait(false); //// TODO do you like this variable name?

            //// TODO add a test for the custom analyzer config
            //// TODO move below helper types into their own project
            //// TODO remove any unnecessary dependencies
            //// TODO can you rename the Content folder to `_resources`?
            //// TODO is there a better way to combine resource paths?

            var tester = new CustomAnalyzerTest2()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    }
                },
                CompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true),
                ParseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose),
                DiagnosticAnalyzers = CustomAnalyzerTest.DefaultAnalyzers(),
            };

            tester.ExpectedDiagnostics.Add(new DiagnosticResult("CA1510", DiagnosticSeverity.Warning).WithSpan(13, 13, 16, 14).WithSpan(13, 17, 13, 27).WithArguments("ArgumentNullException", "ThrowIfNull"));

            await tester.RunAsync().ConfigureAwait(false);
        }

        [TestMethod]
        public async Task MethodWithTaskReturnType()
        {
            var test = await GetTestString().ConfigureAwait(false); //// TODO do you like this variable name?
            var editorConfig = await GetEditorConfigString().ConfigureAwait(false); //// TODO do you like this variable name?

            //// TODO use the factory methods, you are creating one that allows taking testcode and teststate
            var tester = new CustomAnalyzerTest2()
            {
                TestCode = test,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/.editorconfig", SourceText.From(editorConfig)),
                    }
                },
                CompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true),
                ParseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose),
                DiagnosticAnalyzers = CustomAnalyzerTest.LoadAnalyzers(typeof(Microsoft.CodeAnalysis.CSharp.LowercaseTypeNameAnalyzer).Assembly.Location),
            };

            tester.ExpectedDiagnostics.Add(new DiagnosticResult("FX0001", DiagnosticSeverity.Error).WithSpan(8, 25, 8, 28).WithArguments("Foo"));

            await tester.RunAsync().ConfigureAwait(false);
        }
    }

    public class CustomAnalyzerTest2 : CustomAnalyzerTest1
    {
        public override string Language { get; } = LanguageNames.CSharp;

        protected override string DefaultFileExt { get; } = "cs";
    }

    public abstract class CustomAnalyzerTest1 : AnalyzerTest<MSTestVerifier>
    {
        public required CompilationOptions CompilationOptions { private get; init; }

        public required ParseOptions ParseOptions { private get; init; }

        public required IEnumerable<DiagnosticAnalyzer> DiagnosticAnalyzers { private get; init; }

        protected override CompilationOptions CreateCompilationOptions()
        {
            return this.CompilationOptions;
        }

        protected override ParseOptions CreateParseOptions()
        {
            return this.ParseOptions;
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            return this.DiagnosticAnalyzers;
        }
    }

    public sealed class CustomAnalyzerTest : AnalyzerTest<MSTestVerifier>
    {
        private readonly CompilationOptions compilationOptions;
        private readonly ParseOptions parseOptions;
        private readonly IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers;

        public CustomAnalyzerTest(
            string language, 
            string defaultFileExtension,
            CompilationOptions compilationOptions,
            ParseOptions parseOptions, 
            IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers,
            SolutionState testState)
        {
            this.Language = language;
            this.DefaultFileExt = defaultFileExtension;
            this.compilationOptions = compilationOptions;
            this.parseOptions = parseOptions;
            this.diagnosticAnalyzers = diagnosticAnalyzers;
        }

        public override string Language { get; }

        protected override string DefaultFileExt { get; }

        protected override CompilationOptions CreateCompilationOptions()
        {
            return this.compilationOptions;
        }

        protected override ParseOptions CreateParseOptions()
        {
            return this.parseOptions;
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            return this.diagnosticAnalyzers;
        }



        /*public static CustomAnalyzerTest Csharp(string testCode, SolutionState testState, IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers, CompilationOptions compilationOptions, CSharpParseOptions parseOptions)
        {
            return new CustomAnalyzerTest(
                LanguageNames.CSharp,
                "cs",
                compilationOptions,
                parseOptions,
                diagnosticAnalyzers)
            {
                TestCode = testCode,
                TestState =
                {
                    AdditionalFiles = testState.AdditionalFiles,
                },
            };
        }

        public static CustomAnalyzerTest Csharp(string testCode, SolutionState testState, IEnumerable<DiagnosticAnalyzer> diagnosticAnalyzers)
        {
            return CustomAnalyzerTest.Csharp(
                testCode,
                testState,
                diagnosticAnalyzers,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true),
                new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Diagnose));
        }

        public static CustomAnalyzerTest Csharp()
        {
            return CustomAnalyzerTest.Csharp(
                DefaultAnalyzers());
        }

        public static CustomAnalyzerTest Csharp(string testCode, SolutionState testState)
        {
            return CustomAnalyzerTest.Csharp(
                DefaultAnalyzers());
        }*/

        public static IEnumerable<DiagnosticAnalyzer> DefaultAnalyzers() //// TODO make this a property
        {
            /*var foo = Path.GetDirectoryName(typeof(Microsoft.NetFramework.Analyzers.TypesShouldNotExtendCertainBaseTypesAnalyzer).Assembly.Location);
            foo = Path.Combine(foo, "Microsoft.CodeAnalysis.CSharp.CodeStyle.dll");*/

            var paths = new[]
            {
                typeof(Microsoft.NetFramework.Analyzers.TypesShouldNotExtendCertainBaseTypesAnalyzer).Assembly.Location,
                //foo,
                ////typeof(Microsoft.CodeAnalysis.Completion.CompletionTags).Assembly.Location,
                //typeof(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext).Assembly.Location, // TODO foo depends on this
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.Analyzers.dll",
                ////@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.CSharp.dll",
            };

            ////var assembly = Assembly.Load(@"C:\github\OddTrotter\Fx.Core\Fx.Test.Tests\Microsoft.CodeAnalysis.NetAnalyzers.dll");

            /*var type = typeof(Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.DiagnosticAnalyzerAttributeAnalyzer);
            var another = (DiagnosticAnalyzer)Activator.CreateInstance(type)!;*/

            return paths.SelectMany(path => LoadAnalyzers(path))/*.Append(another).ToImmutableArray()*/;
        }

        public static IEnumerable<DiagnosticAnalyzer> LoadAnalyzers(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            var types = assembly.GetTypes();
            var analyzers = types
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract);

            var publicAnalyzers = analyzers.Where(analyzer => analyzer.IsPublic);

            return analyzers.Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
        }

    }
    public class MSTestVerifier : IVerifier
    {
        public MSTestVerifier()
            : this(ImmutableStack<string>.Empty)
        {
        }

        protected MSTestVerifier(ImmutableStack<string> context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected ImmutableStack<string> Context { get; }

        public virtual void Empty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.IsFalse(collection?.Any() == true, CreateMessage($"expected '{collectionName}' to be empty, contains '{collection?.Count()}' elements"));
        }

        public virtual void Equal<T>(T expected, T actual, string? message = null)
        {
            if (message is null && Context.IsEmpty)
            {
                Assert.AreEqual(expected, actual);
            }
            else
            {
                Assert.AreEqual(expected, actual, CreateMessage(message));
            }
        }

        public virtual void True([DoesNotReturnIf(false)] bool assert, string? message = null)
        {
            if (message is null && Context.IsEmpty)
            {
                Assert.IsTrue(assert);
            }
            else
            {
                Assert.IsTrue(assert, CreateMessage(message));
            }
        }

        public virtual void False([DoesNotReturnIf(true)] bool assert, string? message = null)
        {
            if (message is null && Context.IsEmpty)
            {
                Assert.IsFalse(assert);
            }
            else
            {
                Assert.IsFalse(assert, CreateMessage(message));
            }
        }

        [DoesNotReturn]
        public virtual void Fail(string? message = null)
        {
            if (message is null && Context.IsEmpty)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Fail(CreateMessage(message));
            }

            throw ExceptionUtilities.Unreachable;
        }

        public virtual void LanguageIsSupported(string language)
        {
            Assert.IsFalse(language != LanguageNames.CSharp && language != LanguageNames.VisualBasic, CreateMessage($"Unsupported Language: '{language}'"));
        }

        public virtual void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.IsTrue(collection?.Any() == true, CreateMessage($"expected '{collectionName}' to be non-empty, contains"));
        }

        public virtual void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? equalityComparer = null, string? message = null)
        {
            var comparer = new SequenceEqualEnumerableEqualityComparer<T>(equalityComparer);
            var areEqual = comparer.Equals(expected, actual);
            if (!areEqual)
            {
                Assert.Fail(CreateMessage(message));
            }
        }

        public virtual IVerifier PushContext(string context)
        {
            Assert.AreEqual(typeof(MSTestVerifier), GetType());
            return new MSTestVerifier(Context.Push(context));
        }

        protected virtual string CreateMessage(string? message)
        {
            foreach (var frame in Context)
            {
                message = "Context: " + frame + Environment.NewLine + message;
            }

            return message ?? string.Empty;
        }

        private sealed class SequenceEqualEnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>?>
        {
            private readonly IEqualityComparer<T> _itemEqualityComparer;

            public SequenceEqualEnumerableEqualityComparer(IEqualityComparer<T>? itemEqualityComparer)
            {
                _itemEqualityComparer = itemEqualityComparer ?? EqualityComparer<T>.Default;
            }

            public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
            {
                if (ReferenceEquals(x, y)) { return true; }
                if (x is null || y is null) { return false; }

                return x.SequenceEqual(y, _itemEqualityComparer);
            }

            public int GetHashCode(IEnumerable<T>? obj)
            {
                if (obj is null)
                {
                    return 0;
                }

                // From System.Tuple
                //
                // The suppression is required due to an invalid contract in IEqualityComparer<T>
                // https://github.com/dotnet/runtime/issues/30998
                return obj
                    .Select(item => _itemEqualityComparer.GetHashCode(item!))
                    .Aggregate(
                        0,
                        (aggHash, nextHash) => ((aggHash << 5) + aggHash) ^ nextHash);
            }
        }
    }


    internal static class ExceptionUtilities
    {
        public static Exception Unreachable => new InvalidOperationException("This program location is thought to be unreachable.");
    }
}
