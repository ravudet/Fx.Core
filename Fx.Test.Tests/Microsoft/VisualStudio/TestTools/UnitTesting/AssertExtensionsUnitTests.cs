/*namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Attribute required by any type that is returned by <see cref="IDynamicInterfaceCastable.GetInterfaceImplementation(RuntimeTypeHandle)"/>.
    /// </summary>
    /// <remarks>
    /// This attribute is used to enforce policy in the runtime and make
    /// <see cref="IDynamicInterfaceCastable" /> scenarios trimming friendly.
    /// </remarks>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class DynamicInterfaceCastableImplementationAttribute : Attribute
    {
        public DynamicInterfaceCastableImplementationAttribute()
        {
        }
    }
}*/

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Foo
{
    using System;

    public static class Demo
    {
        public static void DoWork()
        {
            Console.WriteLine("overridden");
        }
    }
}

namespace Foo
{
    using System;

    public static class Demo
    {
        public static void DoWork()
        {
            Console.WriteLine("standard");
        }
    }
}

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Linq.V2;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices.Marshalling;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    using Foo;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.CodeAnalysis.VisualBasic.Syntax;
    using Microsoft.VisualBasic;

    public class Buzz(int c)
    {
        public int C { get; } = c;
    }

    [TestClass]
    public sealed class EditorConfigTests
    {
        [TestMethod]
        public async Task Test()
        {
            var editorConfigPath = @"C:\github\OddTrotter\Fx.Core\.editorconfig";
            var editorConfigText = System.IO.File.ReadAllText(editorConfigPath);

            var analyzerConfig = AnalyzerConfig.Parse(
                SourceText.From(editorConfigText),
                editorConfigPath
            );

            ////var optionsProvider = new OptionsProvider(analyzerConfig);

            // 2. Create ScriptOptions
            var scriptOptions = ScriptOptions.Default
                .WithReferences(typeof(object).Assembly)
                .WithImports("System");

            // 3. Create a script
            var script = CSharpScript.Create(@"
class C {
    void M() {
        int x = 0;
    }
}
", scriptOptions);

            // 4. Force Roslyn to build the compilation
            var compilation = script.GetCompilation();

            // 5. Apply .editorconfig options to analyzers
            var analyzers = new DiagnosticAnalyzer[]
            {
    ////new Microsoft.CodeAnalysis.CSharp.Diagnostics.CSharpCompilerDiagnosticAnalyzer()
            };


            var compilationWithAnalyzers = compilation.WithAnalyzers(
                ImmutableArray<DiagnosticAnalyzer>.Empty,
                new AnalyzerOptions(ImmutableArray<AdditionalText>.Empty.Add(new AdditionalFile(editorConfigPath)).ToImmutableArray())
            );

            // 6. Run analyzers
            var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();

            foreach (var d in diagnostics)
            {
                Console.WriteLine($"{d.Id}: {d.GetMessage()}");
            }
        }

        private sealed class OptionsProvider : AnalyzerConfigOptionsProvider
        {
            public OptionsProvider(AnalyzerConfigOptions analyzerConfigOptions)
            {
                this.GlobalOptions = analyzerConfigOptions;
            }

            public override AnalyzerConfigOptions GlobalOptions { get; }

            public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
            {
                return this.GlobalOptions;
            }

            public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
            {
                return this.GlobalOptions;
            }
        }

        private sealed class AdditionalFile : AdditionalText
        {
            public AdditionalFile(string path)
            {
                this.Path = path;
            }

            public override string Path { get; }

            public override SourceText? GetText(CancellationToken cancellationToken = default)
            {
                var text = System.IO.File.ReadAllText(this.Path);
                return SourceText.From(text);
            }
        }
    }



    public sealed class ArgumentValidation
    {
        private ArgumentValidation()
        {
        }

        public static ArgumentValidation Throw { get; } = new ArgumentValidation();
    }

    public static class ArgumentValidationExtensions
    {
        public static void IfNull<T>(this ArgumentValidation argumentValidation, T value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }


    public static class CastExtensions
    {
        public static Source<TSource> Is<TSource>(this ref TSource source)
            where TSource : struct, System.Runtime.InteropServices.IDynamicInterfaceCastable, allows ref struct
        {
            return new Source<TSource>(ref source);
        }

        public readonly ref struct Source<TSource>
            where TSource : struct, System.Runtime.InteropServices.IDynamicInterfaceCastable, allows ref struct
        {
            private readonly TSource source;

            public Source(ref TSource source)
            {
                this.source = source; //// TODO use a pointer?
            }

            public bool Type<TCast>(out TCast cast)
            {
                var typeHandle = this.source.GetInterfaceImplementation(typeof(TCast).TypeHandle);
                cast = System.Runtime.InteropServices.Marshal.PtrToStructure<TCast>(typeHandle.Value);


                ////cast = default!; //// TODO implement this
                return true;
            }
        }
    }


    [TestClass]
    public sealed class AssertExtensionsUnitTests
    {


        //// TODO dotnet_diagnostic.CA1000.severity = warning doesn't flag the method call like the doc says
        //// TODO https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1031 the first example says `systemexception` but should say `system.exception`
        //// TODO broken xml: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5393
        //// TODO ide0007 and ide0008 links both "appear" as ide0008 (compare with ide0003 and ide0009; compare with ide0020 and ide0038) in the list on the left https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0007-ide0008
        //// TODO couldn't get this to trigger: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0016
        //// TODO ide0023 and ide0024 both "appear" as ide0024 (compare with ide0003 and ide0009; compare with ide0020 and ide0038); https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0023-ide0024
        //// TODO ide0029 and ide0030 both "appear" as ide0030 https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0029-ide0030-ide0270
        //// TODO i can't get ide0270 to trigger
        //// TODO ide0047 and ide0048 both "appear" as ide0048 https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0047-ide0048
        //// TODO i can't get IDE0063 to trigger`
        //// TODO ide0160 and ide0161 both "appear" as ide0161 https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0160-ide0161
        //// TODO https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide2004 example is missing `class` keyword and should capitalize `base`
        //// TODO `csharp_style_prefer_method_group_conversion = false:warning` flags `public static void Fizz(Action action) { } Fizz(() => Foo());` does the same as `csharp_style_prefer_method_group_conversion = true:warning` does; the same happens if you enable ide0200 and leave the severity off of the option
        //// TODO https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/naming-rules#symbol-group-properties applicable_accessibilities is not required, it defaults to `*`
        //// TODO follow up and file bugs for all of this from the spellchecker:
        ////    # TODO this doesn't trigger anything on build, even in the IDE; it only works for "currently opened files"
        ////    # TODO i think this feature might still be too much in infancy; or you need to update visual studio; if an update is required, note that somewhere in your onboarding docs
        ////    # TODO also, it might be a moot point because *enabling* spell checking is a visual studio setting, *not* an editorconfig setting
        ////    # TODO get this working https://learn.microsoft.com/en-us/visualstudio/ide/text-spell-checker?view=visualstudio
        ////    # %localappdata%\Microsoft\VisualStudio\<Version>
        ////    # TODO you have to restart visual studio for changes to the exclusion.dic file to take effect
        ////    # TODO saving this file, unchanged, results in all of the options being reset...
        ////    # TODO you *have* to have the "section header"; this *is* documented, but it's still nonsense //// TODO i guess this is actually true for all of the rules
        ////    [*.cs]
        ////    # TODO this can't be `en-US` # TODO an error in any *one* of the options breaks *all* of the options, and then the defaults are used for everything
        ////    spelling_languages = en-us
        ////    spelling_checkable_types = strings,identifiers,comments
        ////    spelling_error_severity = error
        ////    # TODO it *finds* the file, but it doesn't honor it; you can tell it "finds" it by adding a misspelled word to the dictionary, which updates this file instead of the global dictionary
        ////    spelling_exclusion_path = .\exclusion.dic
        ////    spelling_use_default_exclusion_dictionary = false
        //// TODO i think `csharp_style_inlined_variable_declaration` would make a lot more sense if `while (!int.TryParse(value, out var parsed)) { } Console.WriteLine(parsed);` worked https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0018

        //// TODO the editorconfig needs to note if an error is because of ignorance so that the person who encounters it can know if they need to investigate the rule; you want this part in source control
        //// TODO how are you going to know that there are new rules?
        //// TODO keep this link somewhere https://github.com/dotnet/roslyn/blob/main/docs/roslyn-analyzers/README.md
        //// TODO as you're writing the articles, remove your comments and add links to the articles
        //// TODO as you're writing the articles, update any `NOTE` comments to not use the word "note"; it's only there as a reminder for yourself to include it as a comment and differentiate it from the comments that are article reminders
        //// TODO does `is_global=true` (https://github.com/dotnet/roslyn/issues/42219) help you ship a nuget package where the "purely style" rules are in a separate file? //// NOTE: the "global" configs don't apply rules that are under "section headers" (things like `[*.cs]`)
        //// TODO for json001, you realized a quirk about ide-only rules: if someone doesn't use the ide and they commit with a violation, the next person to pull is the one who has to deal with the violations; maybe this is fine for the "code style rules" because they are self-fixing with quick actions (TODO verify that this is 100% accurate); but for json001 it's not great
        //// TODO before writing articles, you need a phrase to refer to the consumer of an API; "user" tends to refer to "end-user", "developer" is ambiguous between the caller and the callee, "client" is specific to server/client stuff, "customer" implies payment; maybe "consumer" is best, but i don't think i've seen others using this term
        //// TODO can you create a roslyn analyzer for the editorconfig?
        ////    TODO don't set the same value twice
        ////    TODO indent options
        ////    TODO spacing between data, `#`, and comment
        ////    TODO put all of the entries of a naming rule, naming style, or naming symbol with the same entity name together; seperate the different entity names with a newline
        ////    TODO entity names should be snake cased
        //// TODO for ide0055, some of them, i don't really care how people write it, but i'd like to have the auto-generated code follow a rule to "encourage" people; for this reason, it *seems* i want 0055 to be warning, but some of the options to be `silent` or `suggestion`; i can't seem to get this to work, though
        //// TODO write articles in a github pages repo (or set up your current repo to publish pages if that's possible?) and link to the articles from the editorconfig
        //// TODO go through your TODOs below and write down your guiding principles, and create a page that has those listed so you can reference them from the articles
        //// TODO you maybe should go through all of the notes in the editorconfig to see if there are any guiding principles there that you missed
        //// TODO it's best to catch an issue as close to the code writing as possible (i.e. correct by construction, then ide/intellisense, then "fall into the pit of success" (i.e. design), then compiler, then unit testing, then component testing, then integration testing, then production (likely with steps in between that i've missed))
        //// TODO make things errors if you don't know about them, so that the first person who encounters them must confront it and update the editorconfig
        //// TODO redundant rules is ok because it makes the person removing the rule work a little harder, and it makes the person reviewing consider the magnitude of the change
        //// TODO public vs internal vs private: what's good for our customer is good for us, and what's good for us is good for our customer
        //// TODO when you get to the IDE rules, some of these really are just style, and i think it's fine to admit that; //// TODO is there a way to separate these into their own editorconfig so that folks can make their own decision?
        //// TODO it'd be good to have a demo for all of the perf ones
        //// TODO you have a guiding principle to enable multiple rules even if they are duplicates or one supersedes another, because thought was put into enabling the rule, so disabling it should be painful (but not hard) and thought-provoking
        //// TODO ide0037 makes a good point about options on disabled rules; you should do another pass to add all of the options as disabled for the `none` rules, making the same note that ide0037 has
        //// TODO you should do the same thing for enabled rules, and explicitly select the default; this is in regards to "don't just enable something because it's a rule, you need to understand it
        //// TODO distributing via nuget: https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#distribution-in-nuget-packages
        ////    TODO consider that you have the rules; but you also have your custom rules, and some of those custom rules will rely on fx.core (like the `assert.that` rule), and some of those rules will not rely on fx.core (like exception documentation); how can you combine all of these cases? are there too many nuget package permutations, or can you make it work? (also, think about the fact that you might have rules that rely on libraries *other than* fx.core, like fx.test or something (actually this is the exact `assert.that` situation))
        ////    TODO dotnet_style_namespace_match_folder is really good for me, but the article https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0130 mentions it's not going to work for command line builds without additional csproj maniupulation... (though it will work for vs builds)
        //// TODO "suggestion" really means "message", but anything becomes noise when you don't suppress them; would a good SOP be to suppress messages into the suppression file? (so that you don't reduce readability); note this early on in the article series; these suppressions shouldn't need to be justified
        //// TODO note early on in the article series that you are big on "don't just enable something because it's a rule; you need to understand it"; anything else is superstition
        //// TODO when you go back through to write the articles, double check that, for the IDE rules, that the option severities are always overridden by the rule severity, and if that is the desireable behavior
        //// TODO when you go back through to write the articles, consider the severity of the rules; some of them should likely be `suggestion` or `silent`
        //// TODO when you go back through to write the articles, consider if any rules should be `error`
        //// TODO should you have a test project for your config file?








        //// TODO i think you should have a .gitattributes file
        //// TODO nuget.config
        //// TODO add a .vsconfig
        //// TODO go through all the other files in the repo
        //// TODO add custom Roslyn analayzer for `assert.that`
        ////    TODO allow unsafe is an example of a codefix that updates the project
        //// TODO look into "contracts" (https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.contracts.contract?view=net-10.0) and the `pure` attribute (https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.contracts.pureattribute?view=net-10.0)













        //// TODO fix this: https://github.com/dotnet/roslyn/issues/24209 and write a rule for yourself
        //// TODO you should have a rule to always prefix static member access; same for `const` member access; this lets code be copied without having to changing these, and it helps distinguish (at a glance) "external" and "nonexternal" static calls within a method (see `dotnet_naming_rule`s for more information)
        //// TODO when you are creating new rules for your coding guidelines, the one about suppressions being minimalist maybe could use IDE0079 as insipiration










        //// TODO does CA2008 change anything about your `either` stuff?
        //// TODO ca2261 your tasks should have all the options and also strongly type that task<T> shouldn't allow this option //// TODO i think you're using task<nothing> as equivalent to task, so maybe just have an extension on task<nothing> that has the additional option
        //// TODO you can use ref properties now for interlocked methods probably










        //// TODO is it possible to have visual studio *display* according to your editor config, but *commit* according to the repo? maybe something like this:
        /*
        await this.Extensibility.Editor().EditAsync(
            batch =>
            {
                var editor = document.AsEditable(batch);
                var snapshot = editor.TextViewSnapshot;
                var selection = snapshot.Selection;

                // Example: Replace selected text with modified version
                var newText = "Modified text";
                editor.Replace(selection.Extent, newText);
            },
            cancellationToken);
        */
        //// TODO actually, you have a chat labeled "vs extension for rosylyn display", which i think will actually do what you want; you can use `dotnet format --verify-no-changes --report report.json` to determine what text needs to be changed without actually changing the underyling files
        //// TODO actually, probably the chat labeled "Visual Studio Extension for Custom Code Display" can tell you if roslyn directly is the better approach








        //// TODO create scaffolding for all of the below packages
        //// TODO can you remove the lib folder from the nuget package?
        ////    use <IncludeBuildOutput>true</IncludeBuildOutput>
        //// TODO how to deal with the version numbers?
        //// TODO analyzer2 now brings fx.core with it as a dependency



        // standard.globalconfig

        // style.globalconfig

        // exceptiondocanalyzer
        // exceptiondocanalyzer.globalconfig <- you, the consumer of the package, need to be using exceptiondocanalyzer

        // useitaskanalyzer <- you, the consumer of the package, need to depend on fx.core
        // fx.core.globalconfig <- you, the consumer of the package, need to be using useitaskanalyzer

        // assertthatanalyzer <- you, the consumer of the package, need to depend on fx.test
        // fx.test.globalconfig <- you, the consumer of the packacke, need to be using assertthatanalyzer







        public static void StyleGlobalConfigTest()
        {
            Func<int, int> square = x => x * x;
            Func<int, int> square2 = x => { return x * x; }; // IDE0053

            var value = $"{square.ToString()}"; // IDE0071
            var value2 = $"{square}";
        }

        public static void Analyzer2DependencyTest()
        {
            var array = new[] { 1, 2, 3, 4 }.ToV2Enumerable();
        }

        public static void Analyzer2AnalysisTest()
        {
            var actual = 5;
            Assert.AreEqual(5, actual);
            Assert.That.AreEqual(5, actual);
        }














        public class GenericTest<T> : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                new object();

                throw new NotImplementedException();
            }
        }



        public class Fake : Attribute
        {
        }

        public class Fake2 : Fake
        {
        }


        class ThrowExpression
        {
            public static readonly string Foo = "asdf";

            private readonly string s;

            public ThrowExpression(string s)
            {
                if (s == null)
                {
                    throw new ArgumentNullException(nameof(s));
                }

                this.s = s;
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public string S => this.s;
        }


        public interface MyInterface 
        {
            string AnotherValue { get; }
        }

        public class MyImplementation 
        {
            public MyImplementation(string value, string otherValue, string anotherValue)
            {

                var foo = new ThrowExpression("asdf");
                AdvancedMember.Class1.DoWork();

                var item = new Microsoft.VisualBasic.FileIO.TextFieldParser(string.Empty).CommentTokens;


                /*var thing = this["asdf"];
                this["asdf"] = 5;*/

                this.Value = value;
                this.OtherValue = otherValue;
            }

            /*public class Base
            {
                public static implicit operator Base(string value)
                {
                    return new Base();
                }
            }

            public class Derived
            {
                public static implicit operator Derived(string value)
                {
                    return new Derived();
                }
            }

            public IEnumerable<int> this[Derived asdf]
            {
                get
                {
                    return new[] { 1234 };
                }
            }

            public int this[Base asdf]
            {
                set
                {
                }
            }*/

            public string Value { get; }
            public string OtherValue { get; }
            public void DoWork()
            {
            }

        }


        [TestMethod]
        public void Regex()
        {
            var match = MethodGroupConversion.Regex.Match("asdf");

            
        }


        public static class MethodGroupConversion
        {
            public static readonly Regex Regex = new Regex(@"\b[M]\w+\");

            public static void Fizz(Action action) { }

            public static void Foo()
            {
                if (true &&
                    false)
                {
                }

                Fizz(() => Foo());

                //// lang=json,strict
                var json =
"""
{
  "key": "value",
}
""";
            }
        }




        private static class NewLine { internal static void DoWork() { } }




        public class CopilotProp : ICopilotProp
        {
            public const int Foo = 5;

            private readonly bool _value;

            public event Action Something;

            public void DoWork2(int value)
            {
                DoWork();
            }

            public string DoWork()
            {
                if (
                    true &&
                    false)
                {
                }

                if (true)
                {
                    Console.WriteLine("asdf");
                } else {
                }

                try
                {
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                }

                var foo = new CopilotProp()
                {
                    Bar = "Asdf",
                    Fizz = "qwer",
                };

                var bar = new
                {
                    Bar = "asdf",
                    Fizz = "qwer",
                };

                var data = new byte[] { 1, 2, 3 };
                var selected = from @byte in data
                               where @byte % 2 == 0
                               select @byte * 2;

                for (int i = 0; i < 10; ++i)
                {
                }

                switch (true)
                {
                    case true:{
                        break;
                    }
                    
                    default:
                        throw new Exception();
                }

                var fizz = (object)foo;
                var buzz = ((2 + 4) * 6) + 1;
                
                var @int=5;

                throw new NotImplementedException();
            }

            public string Foo2 { get { throw new Exception("TODO"); } }

            public string Bar
            {
                get; set;
            }

            public string Fizz
            {
                get; set;
            }
        }


        public interface ICopilotProp
        {
            string DoWork();
        }



        public class Base
        {
        }

        public class Class : 
            Base
        {
        }



        public class ResultType
        {
            public MemberType? Member { get; }

            public class MemberType
            {
                public string SubMember { get; }
            }
        }


        public void NullDelegate(Func<ResultType>? func)
        {

            var thing = true ?
                false
                    ? "asdf"
                    : "qwer" :
                "zxcv";


            var foo = func?.Invoke().Member?.SubMember;

            if (true) { Console.WriteLine("asdf"); }

            Func<int> bar = ()
                => 1;
        }



        [TestMethod]
        public void CollectionInit()
        {
            IEnumerable<int> foo = [1, 2, 3];
        }


        static partial class MyCollection
        {
            public static MyCollection<TThing> Create<TThing>(System.ReadOnlySpan<TThing> values) => default;
            public static MyCollection<T> Create<T>(T t1, T t2, T t3) => throw new InvalidDataException("TODO");
        }

        [CollectionBuilder(typeof(MyCollection), "Create")]
        class MyCollection<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator() => default;
            IEnumerator IEnumerable.GetEnumerator() => default;
        }



        class Primary(int i)
        {
            private readonly int i = i;

            public void Foo()
            {
                MyCollection<int> foo = [1, 2, 3];
            }

        }



        [TestMethod]
        public void ImplicitCast()
        {
            ////var y = stackalloc int[] { 1, 2, 3 };

            var primary = new Primary(1);

            Span<int> x = stackalloc int[] { 1, 2, 3 };
            ////var y = stackalloc int[] { 1, 2, 3 };

            var data = new object[] { "qwer", "asdf" };
            foreach (int datum in data)
            {
            }
        }



        static void TestOneToMany(IOneToMany<string, int> oneToMany)
        {
            ////oneToMany["Asdf"] += (Many<int>)4;


            if (oneToMany is not IOneToMany<string, int>)
            {
            }

            var data = new object[0];

            foreach (string datum in data)
            {
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        class OneToMany<TKey, TValue> : IOneToMany<TKey, TValue>
#pragma warning restore IDE0060 // Remove unused parameter
        {
            private readonly Dictionary<TKey, List<TValue>> dictionary;

            public OneToMany()
            {
                var value = 1234;
                value += 5;

                this.dictionary = new Dictionary<TKey, List<TValue>>();
            }

            Many<TValue> IOneToMany<TKey, TValue, Many<TValue>>.this[TKey key]
            {
                get
                {
                    if (!this.dictionary.TryGetValue(key, out var values))
                    {
                        values = new List<TValue>();
                        this.dictionary[key] = values;
                    }

                    return new Many<TValue>(values);
                }

                set
                {
                    throw new NotImplementedException();
                }
            }
        }



        public ref struct One<TValue> : IMany<TValue, One<TValue>>
        {
            private readonly TValue value;

            public One(TValue value)
            {
                this.value = value;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                ////yield return this.value;
                throw new Exception("TODO");
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public static IMany<TValue, One<TValue>> operator +(One<TValue> value)
            {
                return default;
            }

            public static implicit operator One<TValue>(TValue value)
            {
                return new One<TValue>(value);
            }
        }

        public interface IMany<TValue> : IMany<TValue, One<TValue>>
        {
        }

        public interface IMany<TValue, TOne> : IEnumerable<TValue>
            where TOne : IMany<TValue, TOne>, allows ref struct
        {
            static abstract IMany<TValue, TOne> operator +(TOne value);
        }

        public ref struct Many<TValue> : IMany<TValue>
        {
            public Many(List<TValue> values)
            {
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            static IMany<TValue, One<TValue>> IMany<TValue, One<TValue>>.operator +(One<TValue> value) => throw new NotImplementedException();

            public static IMany<TValue, One<TValue>> operator +(Many<TValue> value) => throw new NotImplementedException();

            public static implicit operator Many<TValue>(TValue value)
            {
                return default;
            }
        }

        public interface IOneToMany<TKey, TValue> : IOneToMany<TKey, TValue, Many<TValue>>
        {
        }

        public interface IOneToMany<TKey, TValue, TMany>
            where TMany : IMany<TValue>, allows ref struct
        {
            TMany this[TKey key] { get; set; }
        }






        static void UseIndex(IIndexer<string, int> indexer)
        {
            ////indexer["asdf"]
        }

        class Indexer<TKey, TValue> : IIndexer<TKey, TValue>
        {
            public TValue this[TKey key]
            {
                set
                {
                    throw new NotImplementedException();
                }
            }

            IEnumerable<TValue> IIndexer<TKey, TKey, TValue>.this[TKey key]
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }


        interface IIndexer<TKey, TValue> : IIndexer<TKey, TKey, TValue>
        {
        }

        interface IIndexer<TKey1, TKey2, TValue>
        {
            TValue this[TKey1 key] { set; }


            IEnumerable<TValue> this[TKey2 key] { get; }
        }



        [TestMethod]
        public void NamespacePlacement()
        {
            new ReadOnlyAssignment().Foo();
        }



        public struct ReadOnlyAssignment
        {
            public readonly int Bar;

            public void Foo()
            {
                var result2 = true ? true : false;

                var value = 4;
                var value2 = 5;
                ////if (value is > 5 or value2 is < 3)
                {
                }

                Demo.DoWork();

                ////this = new ReadOnlyAssignment();
            }
        }


        public interface IModifiersExample
        {
            void DoWork();

            internal void DoWork2();

            protected void DoWork3();
        }


        public class ModifiersExample : IModifiersExample
        {
            private readonly int test;

            public void DoWork()
            {
                using (var foo = File.OpenRead("asdf"))
                {
                }

                void Hello()
                {
                    Console.WriteLine(this.test);
                }

                var unused = int.TryParse("asdf", out var value);

                throw new NotImplementedException();
            }

            void IModifiersExample.DoWork3()
            {
                throw new NotImplementedException();
            }

            void IModifiersExample.DoWork2()
            {
                throw new NotImplementedException();
            }

            public static void Test(ModifiersExample modifiersExample)
            {
                modifiersExample.DoWork();
            }

            public static void Test(IModifiersExample modifiersExample)
            {
                modifiersExample.DoWork();
                modifiersExample.DoWork2(); // TODO i am pretty sure this won't be available when called from another library
            }
        }

        public class ModifiersExample2 : ModifiersExample
        {
            public void DoWork4()
            {
                AsBase(this).DoWork2();
            }

            private static IModifiersExample AsBase(ModifiersExample foo)
            {
                return foo;
            }
        }





        sealed public class ObjectInitializer
        {
            public string Foo { private get; init; } = "test";

            public static implicit operator string(ObjectInitializer objectInitializer)
            {
                return objectInitializer.Foo;
            }

            public static int operator +(ObjectInitializer first, ObjectInitializer second)
            {
                Func<int, int> func = val => val * 2;

                IEnumerable<int> foo = new List<int> { 1, 2, 3 };

                var something = first.Foo != null ? first.Foo : "Asdf";

                something = first.Foo is object ? first.Foo : throw new Exception();

                return 0;
            }

            class C
            {
                static async public void Foo()
                {
                }

                void M()
                {
                    var item = FindItem() as C;
                    if (item == null)
                        throw new System.InvalidOperationException();
                }

                object? FindItem() => null;
            }
        }

        public static class ObjectInitializerDriver
        {
            public static void DoWork()
            {
                int value;
                while (int.TryParse("42", out value))
                {
                }

                var foo = new ObjectInitializer()
                {
                    Foo = "asdf",
                };

                //Console.WriteLine(foo.Foo);
            }
        }





        class ThisAccessor
        {
            private int foo;


        }





        struct StructEquals
        {
            private string foo;

            public StructEquals(string foo)
            {
                this.foo = foo ?? throw new ArgumentNullException(nameof(foo));

                if (foo == null)
                {
                    throw new ArgumentNullException(nameof(foo));
                }

                this.foo = foo;
            }

            public void Foo(string? something)
            {
                if (something == null)
                {
                    throw new ArgumentNullException(nameof(something));
                }

                this.foo = something;




                Foo(something);
            }

            public static bool operator ==(StructEquals first, StructEquals second)
            {
                throw new Exception("HERE");
            }
            public static bool operator !=(StructEquals first, StructEquals second)
            {
                return !(first == second);
            }
        }





        [TestMethod]
        public void NullStruct()
        {
            StructEquals foo = default;
            if (foo == null)
            {

            }



            int value = 3;
            Console.Write(value);
        }




        [TestMethod]
        public void NullSpan()
        {
            Span<int> span = new[] { 1, 2, 3 };
            if (span == null)
            {

            }
        }









        public interface ICastBase
        {
            void Dispose();
        }


        [System.Runtime.InteropServices.DynamicInterfaceCastableImplementation]
        public interface ICast : ICastBase
        {
            void ICastBase.Dispose()
            {
                //// TODO look at this sample (`getvtbl`) to learn how to use members from the instance being cast to `icast`: https://github.com/dotnet/samples/blob/main/core/interop/IDynamicInterfaceCastable/src/ManagedApp/NativeObject.cs

                Console.WriteLine("hello");
            }
        }

        //[System.Runtime.InteropServices.DynamicInterfaceCastableImplementation]
        class TestCast
        {
            public void Dispose()
            {
                Console.WriteLine("hello from class");
            }
        }

        ref struct Castable2 : System.Runtime.InteropServices.IDynamicInterfaceCastable
        {
            public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
            {
                return typeof(ICast).TypeHandle;
            }

            public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
            {
                return true;
            }
        }

        class Castable : System.Runtime.InteropServices.IDynamicInterfaceCastable
        {
            public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
            {
                return typeof(ICast).TypeHandle;
            }

            public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
            {
                return true;
            }
        }

        public class Castable3 : System.Runtime.InteropServices.IDynamicInterfaceCastable
        {
            public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
            {
                return typeof(TestCast).TypeHandle;
            }

            public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
            {
                return true;
            }
        }

        [TestMethod]
        public void Cast()
        {
            var castable = new Castable();
            if (castable is ICast disposable)
            {
                disposable.Dispose();
            }
            else
            {
                Assert.Fail();
            }

            /*var castable2 = new Castable2();
            if (castable2 is Castable3)
            {
                Console.WriteLine("could cast");
            }*/

            /*if (castable2.Is().Type<ICast>(out var disposable2))
            {
                disposable2.Dispose();
            }
            else
            {
                Assert.Fail();
            }*/
        }




        [TestMethod]
        public void Enumerate()
        {
            new TaskCompletionSource<int>(TaskContinuationOptions.RunContinuationsAsynchronously);

            var enumerable = GetSomeEnumerable();

            var enumerator = enumerable.GetEnumerator();

            Assert.ThrowsException<InvalidOperationException>(() => enumerator.MoveNext());

            /*Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.MoveNext());*/
        }

        public System.Collections.Generic.IEnumerable<string> GetSomeEnumerable()
        {
            var disposable = new Disposable();
            try
            {
                yield return "asdf";
                yield return "asdf";
            }
            finally
            {
                Console.WriteLine("asdf");
                disposable.Dispose();
            }
        }

        private sealed class Disposable : IDisposable
        {
            private static int initializations = 0;

            public Disposable()
            {
                if (initializations == 0)
                {
                    ++initializations;
                    ////throw new InvalidOperationException("TODO");
                }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }











        [TestMethod]
        public void DoStaticInit()
        {
            var array = new StaticInit[10];
            Assert.AreEqual(0, Value);

            StaticInit staticinit = default;
            Assert.AreEqual(0, Value);
        }

        public struct StaticInit
        {
            static StaticInit()
            {
                Value = 1;
            }
        }

        public static int Value;




        public static class CertTest
        {
            public static void Foo(byte[] certBytes)
            {
                var path = "foo";
                File.WriteAllBytes(path, certBytes);
                using (new X509Certificate2(path))
                {
                }

                var otherCertBytes = new byte[] { 1, 2, 3 };
                File.WriteAllBytes(path, otherCertBytes);
                using (new X509Certificate2(path))
                {
                }
            }
        }


        abstract class A
        {
            public virtual void M() { }
        }

        sealed class B : A
        { }

        internal class C
        {
            private readonly IEnumerable<int> _a = new List<int>();

            private static readonly string format = "test {0}";

            private readonly int value;

            public C()
            {
                this.value = 0;
            }

            public void Trigger(string value)
            {
                ArgumentNullException.ThrowIfNull(value);

                // This performs a virtual call because
                // _a is defined as an abstract class.
                var dictionary = this._a.ToDictionary(_ => _);

                if (value.CompareTo("asdf") == 0)
                {
                }

                if (value.StartsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                }

                if (value.StartsWith("a"))
                {
                }

                if (value.StartsWith("a", StringComparison.InvariantCulture))
                {
                }

                Console.WriteLine(string.Format(format, "asdf"));
            }
        }



        public static class Required
        {
            public static readonly string Foo = "asdf";

            public static void Bar()
            {
                _ = new Holder();

                Frob();
            }

            public static Holder Frob()
            {
                return new Holder();
            }

            public static async Task Stream(Stream stream)
            {
                var buffer = new byte[] { 0x38 };
                await stream.WriteAsync(buffer);
            }

            public static async Task StreamAsync(Stream stream)
            {
                var buffer = new byte[] { 0x38 };
                await stream.WriteAsync(buffer);
            }

            public static Task Stream2(Stream stream)
            {
                var buffer = new byte[1024];
                var read = Stream(stream);

                return Task.CompletedTask;
            }

            public static IEnumerable<int> Enumerate(IEnumerable<string> enumerable)
            {
                var values = enumerable.Select(_ => _.Length).ToList();

                return Enumerate2(enumerable);
            }

            private static IEnumerable<int> Enumerate2(IEnumerable<string> enumerable)
            {
                return enumerable.Select(_ => _.Length).ToList().Select(_ => _ * 2).ToList();
            }
        }




        public class Holder
        {
            static Holder()
            {
                Holder.Property = 1;
            }

            public static int Property { get; private set; }

            public static void DoWork()
            {
            }
        }

        private interface IMany : IEquatable<IMany>, IEnumerable<string>, IComparable
        {
        }

        public class NonGenericCollection : IEnumerable<string>, IEnumerable
        {
            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        public enum LongEnum : int
        {
        }


        public enum Foo4
        {
            first = 0,
        }

        public const int SomeConst = 0;

        public struct ByRef
        {
        }

        public ref struct Container<T>
        {
            private readonly ref ByRef foo;

            public Container(ref ByRef foo)
            {
                this.foo = ref foo;
            }

            public static void DoWork(ref ByRef foo)
            {
            }

            public List<T> List { get; }
        }

        public class AnotherContainer
        {
            public void DoWork<T>()
            {
            }

            public virtual event EventHandler Foo;
        }


        public interface ITest
        {
            void Test();
        }

        public class BaseTest : ITest
        {
            void ITest.Test()
            {
            }
        }

        public class DerivedTest : BaseTest, ITest
        {
            public void Test()
            {
                /*var baseTest = AsTest(base);
                var iTest = (ITest)baseTest;

                ((ITest)(BaseTest)this).Test();*/
            }

            private static ITest AsTest(ITest test)
            {
                return test;
            }
        }

        [TestMethod]
        public void DerivedRecursion()
        {
            new DerivedTest().Test();
        }

        public record Point(int X, int Y);
        public record Segment(Point Start, Point End);

        // Violates IDE0170.
        static bool IsEndOnXAxis(Segment segment) =>
            segment is { Start: { Y: 0, X: 5 } } or { End: { Y: 0 } };

        // Fixed code.
        static bool IsEndOnXAxis2(Segment segment) =>
            segment is { Start.Y: 0, Start.X: 5 } or { End.Y: 0 };


        public static void Frub(Action<int> action)
        {
            new AnotherContainer().DoWork<int>();
        }

        public static void Frob(String? thing, object bar)
        {
            try
            {
                Frub((int x) => { });
            }
            catch (Exception)
            {
            }

            Bar fizz = new("asdf");

            var derived = bar as Derived;

            if (derived is not Derived)
            {
            }

            ////var cValue = derived.Buzz.C;


            var sum = GetValue() * 3 + 2;

            if ((3 > 2) == (5 < 4))
            {
            }

            if (sum is default(int) or 5)
            {
            }


            var other = thing != null ? thing : "asdf";
            other = thing == null ? "Asdf" : thing;

            if (thing == null)
            {
                throw new Exception("TODO");
            }

            var another = thing as object;
            var assigned = another ?? throw new Exception("TODO");

            if (another == null)
            {
                throw new Exception("tODO");
                Console.WriteLine("asdf");
            }


            if (assigned == null) throw new Exception("tODO");


            string something = true ? "asdf" : "qwer";
            Console.WriteLine(something);


            var customer = GetTuple();
            Console.WriteLine(customer.Item1);

            (string name, int age) anotherCustomer = GetTuple();

            var more = (name2: customer.name, age: customer.age);

            Console.WriteLine(customer.ToString());
            Console.WriteLine($"{customer.ToString()}");

            Exception? subsequent = default;

            var again = default(Exception);

            Exception? again2 = default;

            int.TryParse("asdf", out var testingReturnValue);

            var tuple = GetTuple();
            var notTuple = AnotherParse("asdf");

            AnotherParse("asdf");

            Console.WriteLine("asdF");
        }

        public static int GetValue()
        {
            return 5;
        }

        public static (string name, int age) GetTuple()
        {
            return ("ASdf", 1234);
        }

        public static bool TryCustomParse(string toParse, [NotNullWhen(true)] out int? parsed)
        {
            var value = int.TryParse(toParse, out var intermediate);
            parsed = intermediate;
            return value;
        }

        public struct Structure
        {
        }

        public static Structure AnotherParse(string toParse)
        {
            return new Structure();
        }

        public static void Foo2()
        {
            var wasParsed = AnotherParse("asdf");

            while (!TryCustomParse("asdf", out var parsed))
            {
                parsed = 5;
            }

            ////Console.WriteLine(parsed);

            byte[] bytes = "ABC"u8.ToArray();

            var buzz = new Buzz(1234);

            var value = 1234;
            value += 5;

            var something = new { C2 = buzz.C, value2 = value };


            var data = new[] { 1, 2, 3 };
            var index = data[^1];
            var more = data[1..4];

            var subArray = new int[2];
            Array.Copy(data, 0, subArray, 0, 4);

            var doubled = from datum in data from datum2 in data 
                          select datum * datum2;

            Fizz(() => Foo2());

            var result = data.Select(
                foo
                    => foo * 2);
        }

        public static void Fizz(Action action) { }


        public class Bar
        {
            public string? Fizz { get; set; }

            public Bar(string? fizz) =>
                Fizz = fizz;

            public string? GetOther() => this.Fizz;
        }

        public class Derived :
            Bar,
            ISomething
        {
            public Derived(string? fizz)
                : base(
                      fizz)
            {
            }

            public void DoWork(int parameter)
            {
            }

            public Buzz Buzz { get; set; }
        }

        public interface ISomething
        {
            void DoWork(int parameter);
        }




        [TestMethod]
        public void Equality()
        {
            Console.WriteLine(nameof(System.Collections.Generic.List<string>));

            var first = new EqualsThing();

            Assert.IsTrue(first == null);
            Assert.IsFalse(object.ReferenceEquals(first, null));

            var second = new EqualsThing();

            Assert.IsFalse(object.ReferenceEquals(first, second));
            Assert.IsTrue(object.ReferenceEquals(first, first));

            var third = new EqualsStruct();
            Assert.IsFalse(object.ReferenceEquals(third, null));

            /*if (third is null)
            {
            }*/
        }

        // TODO: i think this plus the struct thing (plus maybe something with generics) demonstrate the differences if you look at the decompiled code
        public void M(Foo foo)
        {
            if (foo is null)
            {
            }

            if (object.ReferenceEquals(foo, null))
            {
            }

            if (foo == null)
            {
            }
        }

        public class Foo
        {
            public static bool operator ==(Foo? first, Foo? second)
            {
                return true;
            }

            public static bool operator !=(Foo? first, Foo? second)
            {
                return false;
            }
        }
        // ENDTODO

        public void Equality2<T>(T something)
        {
            // notice, this won't be noticed by CA2013 if t is a struct
            Assert.IsTrue(object.ReferenceEquals(something, null));

            if (something is null) // TODO unclear if this is just as bad as `object.referenceequals`
            {
            }
        }

        public struct EqualsStruct
        {
        }

        public class EqualsThing
        {
            public static bool operator ==(EqualsThing? left, EqualsThing? right)
            {
                return true;
            }

            public static bool operator !=(EqualsThing left, EqualsThing right)
            {
                return false;
            }
        }

        //// TODO query result *could* look like this

        public interface IQueryResult<TElement, TError>
        {
            IQueryResultEnumerator<TElement, TError> GetEnumerator();
        }

        public interface IQueryResultEnumerator<TElement, TError>
        {
            TElement Current { get; }

            bool MoveNext([NotNullWhen(false)] out NewNullable<TError>? error);
        }

        public struct NewNullable<T>
        {
            public bool TryGetValue([MaybeNullWhen(false)] out T value)
            {
                value = default;
                return false;
            }
        }

        public void UseQueryResult(IQueryResult<string, Exception> queryResult)
        {
            /*
            foreach (var element in queryResult)
            {
            }
            followedBy (Exception exception) //// TODO this block *could* be optional, but i'm not sure that's a good idea; it would only make sense to do in an "iterator"; so, maybe it's optional if `yield return` is used?
            {
                // error found
                throw new InvalidOperationException("TODO", exception); 
            }

            // no error found, or we already processed the error; now do something else
            */

            //// TODO what about if they want to just passthrough the error?
        }

        public static IQueryResult<TResultElement, TError> Select<TSourceElement, TError, TResultElement>(
            IQueryResult<TSourceElement, TError> source,
            Func<TSourceElement, TResultElement> selector)
        {
            /*foreach (var element in source)
            {
                yield return selector(element);
            }
            followedBy (TError error)
            {
                process; // pass the error through
                process new InvalidOperationException($"{error}"); // or translate it (wouldn't be used for `select`, but maybe `selecterror`)
            }*/

            return new SelectIterator<TSourceElement, TError, TResultElement>(source, selector);
        }

        private sealed class SelectIterator<TSourceElement, TError, TResultElement> : IQueryResult<TResultElement, TError>
        {
            private readonly IQueryResult<TSourceElement, TError> source;
            private readonly Func<TSourceElement, TResultElement> selector;

            public SelectIterator(
                IQueryResult<TSourceElement, TError> source,
                Func<TSourceElement, TResultElement> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public IQueryResultEnumerator<TResultElement, TError> GetEnumerator()
            {
                return new Enumerator(this.source.GetEnumerator(), this.selector);
            }

            private sealed class Enumerator : IQueryResultEnumerator<TResultElement, TError>
            {
                private readonly IQueryResultEnumerator<TSourceElement, TError> source;
                private readonly Func<TSourceElement, TResultElement> selector;

                public Enumerator(
                    IQueryResultEnumerator<TSourceElement, TError> source,
                    Func<TSourceElement, TResultElement> selector)
                {
                    this.source = source;
                    this.selector = selector;
                }

                public TResultElement Current
                {
                    get
                    {
                        return this.selector(this.source.Current);
                    }
                }

                public bool MoveNext([MaybeNullWhen(false)] out NewNullable<TError>? error)
                {
                    return this.source.MoveNext(out error);
                }
            }
        }






























        [TestMethod]
        public void IsTrue()
        {
            bool condition = true;
            Assert.That.IsTrue(condition);

            condition = false;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));
        }

        [TestMethod]
        public void IsTrueNullable()
        {
            bool? condition = null;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));

            condition = true;
            Assert.That.IsTrue(condition);

            condition = false;
            Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition));
        }

        [TestMethod]
        public void IsTrueMessage()
        {
            var message = "some message";
            bool condition = true;
            Assert.That.IsTrue(condition, message);

            condition = false;
            var exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));
        }

        [TestMethod]
        public void IsTrueNullableMessage()
        {
            var message = "some message";
            bool? condition = null;
            var exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));

            condition = true;
            Assert.That.IsTrue(condition);

            condition = false;
            message = "another message";
            exception = Assert.ThrowsException<AssertFailedException>(() => Assert.That.IsTrue(condition, message));
            Assert.IsTrue(exception.Message.Contains(message));
        }
    }
}
