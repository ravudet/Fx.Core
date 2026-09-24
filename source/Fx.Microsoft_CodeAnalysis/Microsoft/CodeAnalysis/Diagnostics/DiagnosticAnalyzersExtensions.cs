namespace Microsoft.CodeAnalysis.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class DiagnosticAnalyzersExtensions
    {
        public static IEnumerable<DiagnosticAnalyzer> Load(this DiagnosticAnalyzers _, string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            var types = assembly.GetTypes();
            var analyzers = types
                .Where(t => typeof(DiagnosticAnalyzer).IsAssignableFrom(t) && !t.IsAbstract);

            var publicAnalyzers = analyzers.Where(analyzer => analyzer.IsPublic);

            return analyzers.Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
        }
    }
}
