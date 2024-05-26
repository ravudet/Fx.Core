namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public class Program
    {
        public static void Main(string[] args)
        {
            OptimizeExpressionTree((int a, int b, int c) => 1 + a * (b + c));
            OptimizeExpressionTree((int a, int b, int c) => 1 + (a + 2) * (b + (c * 3)));

            Expression expression = (bool a) => !a && a;
            var reduced = expression.Reduce();

            var contradiction = OptimizeExpressionTree((bool a) => a && !a);
            contradiction = OptimizeExpressionTree((bool a) => false || a && !a);
            contradiction = OptimizeExpressionTree((bool a) => false || a && true && true && true && !a);

            var isInnerJoin = IsInnerJoin((Foo foo, Bar bar) => foo.Id == bar.Id);
            isInnerJoin = IsInnerJoin((Foo foo, Bar bar) => true && foo.Id == bar.Id);
            isInnerJoin = IsInnerJoin((Foo foo, Bar bar) => foo.Id == bar.Id && true);
            isInnerJoin = IsInnerJoin((Foo foo, Bar bar) => foo.Id != bar.Id);
        }

        /*private static bool IsInnerJoin(Expression expression, Func<Foo, string> fooId, Func<Bar, string> barId)
        {
            expression.Replace((Foo foo, Bar bar) => )
        }*/

        private static bool IsInnerJoin(Expression expression)
        {
            if (expression.TryExtractParameters((Foo foo, Bar bar) => foo.Id == bar.Id, out var _))
            {
                return true;
            }

            if (expression.TryExtractParameters((Foo foo, Bar bar, bool x) => x && foo.Id == bar.Id, out var _))
            {
                return true;
            }

            /*if (expression.TryExtractParameters((Foo foo, Bar bar, bool x) => foo.Id == bar.Id && x, out var _));
            {
                return true;
            }*/

            if (expression.TryExtractParameters((Foo foo, Bar bar, bool x) => foo.Id == bar.Id && x, out var _))
            {
                return true;
            }

            return false;
        }

        private class Foo
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }

        private class Bar
        {
            public string Id { get; set; }

            public int BarProp { get; set; }
        }

        private static Expression OptimizeExpressionTree(Expression expression)
        {
            // distributive property of multiplication of addition
            expression = OptimizeExpressionTree(
                expression, 
                (int x, int y, int z) => x * (y + z),
                (int x, int y, int z) => x * y + x * z);

            // boolean contradiction
            expression = OptimizeExpressionTree(expression, (bool x) => x && !x, (bool x) => false);
            expression = OptimizeExpressionTree(expression, (bool x, bool y) => x && y && !x, (bool x, bool y) => false);

            // boolean tautology
            expression = OptimizeExpressionTree(expression, (bool x) => x || !x, (bool x) => true);
            expression = OptimizeExpressionTree(expression, (bool x, bool y) => x || y || !x, (bool x, bool y) => true);

            // boolean axioms
            Expression optimized;
            while ((optimized = OptimizeBooleanLogic(expression)) != expression)
            {
                expression = optimized;
            }

            return expression;
        }

        private static Expression OptimizeBooleanLogic(Expression expression)
        {
            expression = OptimizeExpressionTree(expression, (bool x) => x && false, (bool x) => false);
            expression = OptimizeExpressionTree(expression, (bool x) => false && x, (bool x) => false);
            expression = OptimizeExpressionTree(expression, (bool x) => x && true, (bool x) => x);
            expression = OptimizeExpressionTree(expression, (bool x) => true && x, (bool x) => x);
            expression = OptimizeExpressionTree(expression, (bool x) => x || false, (bool x) => x);
            expression = OptimizeExpressionTree(expression, (bool x) => false || x, (bool x) => x);
            expression = OptimizeExpressionTree(expression, (bool x) => x || true, (bool x) => true);
            expression = OptimizeExpressionTree(expression, (bool x) => true || x, (bool x) => true);

            //// TODO this si still broken
            expression = OptimizeExpressionTree(expression, () => UnaryExpression.Not(Expression.Constant(false, typeof(bool))), () => true);
            expression = OptimizeExpressionTree(expression, () => UnaryExpression.Not(Expression.Constant(true, typeof(bool))), () => false);

            return expression;
        }

        private static Expression OptimizeExpressionTree(Expression expression, LambdaExpression template, LambdaExpression replacement)
        {
            Expression optimized;
            while (true)
            {
                optimized = expression.Replace(template, replacement);
                if (optimized == expression)
                {
                    break;
                }

                expression = optimized;
            }

            return optimized;
        }
    }

    public static class Extensions
    {
        public static Expression Replace(this Expression expression, LambdaExpression template, LambdaExpression replacement)
        {
            if (TryExtractParameters(expression, template.Body, out var parameters))
            {
                //// TODO should this be expression = Argumentize(...) so that you keep replacing? that could mean there's no guarantee to terminate
                //// the caller could just loop, saying while (expression != expression.Replace(...)) instead of us continuing here...
                return Argumentize(replacement.Body, parameters);
            }

            if (expression is LambdaExpression lambdaExpression)
            {
                var lambda = Replace(lambdaExpression.Body, template, replacement);
                if (lambda == lambdaExpression.Body)
                {
                    return expression;
                }
                else
                {
                    return lambda;
                }
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                var left = Replace(binaryExpression.Left, template, replacement);
                var right = Replace(binaryExpression.Right, template, replacement);
                if (left == binaryExpression.Left && right == binaryExpression.Right)
                {
                    return expression;
                }
                else
                {
                    return BinaryExpression.MakeBinary(binaryExpression.NodeType, left, right);
                }
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                var operand = Replace(unaryExpression.Operand, template, replacement);
                if (operand == unaryExpression.Operand)
                {
                    return expression;
                }
                else
                {
                    return UnaryExpression.MakeUnary(unaryExpression.NodeType, operand, unaryExpression.Type);
                }
            }
            else if (expression is MemberExpression)
            {
                return expression;
            }
            else if (expression is ConstantExpression)
            {
                return expression;
            }
            else if (expression is ParameterExpression)
            {
                return expression;
            }
            //// TODO support other expression types here or preferably with a visitor

            throw new NotSupportedException($"Expression type {expression.NodeType} is not supported");
        }

        public static bool TryExtractParameters(this Expression expression, LambdaExpression template, out List<(ParameterExpression toReplace, Expression replacement)> parameters)
        {
            if (expression is LambdaExpression lambdaExpression)
            {
                expression = lambdaExpression.Body;
            }

            return expression.TryExtractParameters(template.Body, out parameters);
        }

        public static bool TryExtractParameters(this Expression expression, Expression template, out List<(ParameterExpression toReplace, Expression replacement)> parameters)
        {
            //// TODO better type for parameters

            parameters = new List<(ParameterExpression, Expression)>();
            if (template is ParameterExpression parameterExpressionTemplate)
            {
                parameters.Add((parameterExpressionTemplate, expression));
                return true;
            }
            else if (expression is BinaryExpression binaryExpression && template is BinaryExpression binaryTemplate)
            {
                if (binaryExpression.NodeType == binaryTemplate.NodeType)
                {
                    if (TryExtractParameters(binaryExpression.Left, binaryTemplate.Left, out var leftParameters))
                    {
                        parameters.AddRange(leftParameters);
                        if (TryExtractParameters(binaryExpression.Right, binaryTemplate.Right, out var rightParamters))
                        {
                            parameters.AddRange(rightParamters);
                            return true;
                        }
                    }
                }
            }
            else if (expression is UnaryExpression unaryExpression && template is UnaryExpression unaryTemplate)
            {
                if (unaryExpression.NodeType == unaryTemplate.NodeType)
                {
                    if (TryExtractParameters(unaryTemplate.Operand, unaryTemplate.Operand, out var operandParameters))
                    {
                        parameters.AddRange(operandParameters);
                        return true;
                    }
                }
            }
            else if (expression is MemberExpression memberExpression && template is MemberExpression memberExpressionTemplate)
            {
                if (memberExpression.NodeType == memberExpressionTemplate.NodeType)
                {
                    if (memberExpression.Member == memberExpressionTemplate.Member) //// TODO is reference equality enough?
                    {
                        return true;
                    }
                }
            }
            else if (expression is ConstantExpression constantExpression && template is ConstantExpression constantExpressionTemplate)
            {
                if (constantExpression.NodeType == constantExpressionTemplate.NodeType)
                {
                    var constEqual = constantExpression == constantExpressionTemplate;
                    var equal = object.ReferenceEquals(constantExpression.Value, constantExpressionTemplate.Value);
                    ////if (constantExpression.Value == constantExpressionTemplate.Value) //// TODO is reference equality enough? it is not, below is the workaround
                    if (constantExpression.Type == typeof(bool) && constantExpressionTemplate.Type == typeof(bool) && (bool)constantExpression.Value == false && (bool)constantExpressionTemplate.Value == false)
                    {
                        return true;
                    }
                }
            }
            //// TODO support other expression types here or preferably with a visitor

            return false;
        }

        private static Expression Argumentize(this Expression expression, IEnumerable<(ParameterExpression, Expression)> arguments)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                var left = Argumentize(binaryExpression.Left, arguments);
                var right = Argumentize(binaryExpression.Right, arguments);
                return BinaryExpression.MakeBinary(binaryExpression.NodeType, left, right);
            }
            else if (expression is ParameterExpression parameterExpression)
            {
                if (arguments.TryGet(parameter => string.Equals(parameter.Item1.Name, parameterExpression.Name), out var replacement))
                {
                    return replacement.Item2;
                }

                return expression;
            }
            else if (expression is ConstantExpression)
            {
                return expression;
            }
            //// TODO support other expression types here or preferably with a visitor

            return expression;
            //// TODO ensure "arguments" is of the type (someParam) => someExpr and nothing else, because we will match someParam with the parameters in "expression"
        }

        private static bool TryGet<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T? found)
        {
            foreach (var element in source)
            {
                if (predicate(element))
                {
                    found = element;
                    return true;
                }
            }

            found = default;
            return false;
        }
    }
}
