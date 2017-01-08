using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.CSharp.Expressions;
using Soothsharp.Translation.Trees.CSharp.Highlevel;
using Soothsharp.Translation.Trees.CSharp.Statements;

namespace Soothsharp.Translation
{
    /// <summary>
    /// This class contains static methods that convert Roslyn instances into sharpnode.
    /// </summary>
    public static class RoslynToSharpnode
    {
        /// <summary>
        /// Converts a Roslyn statement into a sharpnode.
        /// </summary>
        /// <param name="statement">The statement.</param>
        public static StatementSharpnode MapStatement(StatementSyntax statement)
        {
            switch(statement.Kind())
            {
                case SyntaxKind.WhileStatement:
                    return new WhileStatementSharpnode(statement as WhileStatementSyntax);
                case SyntaxKind.DoStatement:
                    return new DoStatementSharpnode(statement as DoStatementSyntax);
                case SyntaxKind.ForStatement:
                    return new ForStatementSharpnode(statement as ForStatementSyntax);
                case SyntaxKind.IfStatement:
                    return new IfStatementSharpnode(statement as IfStatementSyntax);
                case SyntaxKind.ExpressionStatement:
                    return new ExpressionStatementSharpnode(statement as ExpressionStatementSyntax);
                case SyntaxKind.ReturnStatement:
                    return new ReturnStatementSharpnode(statement as ReturnStatementSyntax);
                case SyntaxKind.Block:
                    return new BlockSharpnode(statement as BlockSyntax);
                case SyntaxKind.LocalDeclarationStatement:
                    return new LocalDeclarationSharpnode(statement as LocalDeclarationStatementSyntax);
                case SyntaxKind.EmptyStatement:
                    return new EmptyStatementSharpnode(statement as EmptyStatementSyntax);
                case SyntaxKind.GotoStatement:
                    return new GotoStatementSharpnode(statement as GotoStatementSyntax);
                case SyntaxKind.LabeledStatement:
                    return new LabeledStatementSharpnode(statement as LabeledStatementSyntax);
                case SyntaxKind.YieldBreakStatement:
                case SyntaxKind.YieldReturnStatement:
                    return new UnknownStatementSharpnode(statement, "iterator functions");
                case SyntaxKind.FixedStatement:
                case SyntaxKind.UnsafeStatement:
                    return new UnknownStatementSharpnode(statement, "unsafe code");
                case SyntaxKind.BreakStatement:
                case SyntaxKind.ContinueStatement:
                case SyntaxKind.ForEachStatement:
                case SyntaxKind.LockStatement:
                case SyntaxKind.UsingStatement:
                    return new UnknownStatementSharpnode(statement);
                case SyntaxKind.SwitchStatement:
                case SyntaxKind.GotoCaseStatement:
                case SyntaxKind.GotoDefaultStatement:
                    return new UnknownStatementSharpnode(statement, "switch");
                case SyntaxKind.ThrowStatement:
                case SyntaxKind.TryStatement:
                    return new UnknownStatementSharpnode(statement, "exceptions");
                case SyntaxKind.CheckedStatement:
                case SyntaxKind.UncheckedStatement:
                    return new UnknownStatementSharpnode(statement, "checked statements");
                default:
                    return new UnknownStatementSharpnode(statement);
            }
        }

        /// <summary>
        /// Converts a Roslyn expression into a sharpnode.
        /// </summary>
        /// <param name="expression">The expression.</param>
        internal static ExpressionSharpnode MapExpression(ExpressionSyntax expression)
        {
            
            switch(expression.Kind())
            {
                // Relational operators
                case SyntaxKind.LessThanOrEqualExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "<=");
                case SyntaxKind.LessThanExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "<");
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, ">=");
                case SyntaxKind.GreaterThanExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, ">");
                case SyntaxKind.EqualsExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "==");
                case SyntaxKind.NotEqualsExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "!=");
                case SyntaxKind.LogicalOrExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "||");
                case SyntaxKind.LogicalAndExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax, "&&");

                // Arithmetical operators
                case SyntaxKind.AddExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax,
                        "+");
                case SyntaxKind.SubtractExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax,
                        "-");
                case SyntaxKind.MultiplyExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax,
                        "*");
                case SyntaxKind.DivideExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax,
                        "\\"); // In Silver, integer division is "\", not "/" ("/" is used for fractional permissions).
                case SyntaxKind.ModuloExpression:
                    return new BinaryExpressionSharpnode(expression as BinaryExpressionSyntax,
                        "%");

                // Unary operators
                case SyntaxKind.UnaryMinusExpression:
                    return new PrefixUnaryExpressionSharpnode(expression as PrefixUnaryExpressionSyntax, "-");
                case SyntaxKind.UnaryPlusExpression:
                    return new PrefixUnaryExpressionSharpnode(expression as PrefixUnaryExpressionSyntax, "+");
                case SyntaxKind.LogicalNotExpression:
                    return new PrefixUnaryExpressionSharpnode(expression as PrefixUnaryExpressionSyntax, "!");

                // Increment operators
                case SyntaxKind.PostIncrementExpression:
                    return new IncrementExpressionSharpnode(expression as PostfixUnaryExpressionSyntax,
                        IncrementExpressionDirection.Increment);
                case SyntaxKind.PostDecrementExpression:
                    return new IncrementExpressionSharpnode(expression as PostfixUnaryExpressionSyntax,
                        IncrementExpressionDirection.Decrement);
                case SyntaxKind.PreIncrementExpression:
                    return new IncrementExpressionSharpnode(expression as PrefixUnaryExpressionSyntax,
                        IncrementExpressionDirection.Increment);
                case SyntaxKind.PreDecrementExpression:
                    return new IncrementExpressionSharpnode(expression as PrefixUnaryExpressionSyntax,
                        IncrementExpressionDirection.Decrement);

                // Assignment
                case SyntaxKind.SimpleAssignmentExpression:
                    return new SimpleAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax);

                // Compound assignment
                case SyntaxKind.AddAssignmentExpression:
                    return new CompoundAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax, "+");
                case SyntaxKind.SubtractAssignmentExpression:
                    return new CompoundAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax, "-");
                case SyntaxKind.MultiplyAssignmentExpression:
                    return new CompoundAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax, "*");
                case SyntaxKind.DivideAssignmentExpression:
                    return new CompoundAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax, "/");
                case SyntaxKind.ModuloAssignmentExpression:
                    return new CompoundAssignmentExpressionSharpnode(expression as AssignmentExpressionSyntax, "\\");

                // Invocation
                case SyntaxKind.InvocationExpression:
                    return new InvocationExpressionSharpnode(expression as InvocationExpressionSyntax);
                case SyntaxKind.ObjectCreationExpression:
                    return new ObjectCreationExpressionSharpnode(expression as ObjectCreationExpressionSyntax);

                // Keywords
                case SyntaxKind.ThisExpression:
                    return new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, expression);

                // Literals
                case SyntaxKind.TrueLiteralExpression:
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, true);
                case SyntaxKind.FalseLiteralExpression:
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, false);
                case SyntaxKind.NumericLiteralExpression:
                    object value = (((LiteralExpressionSyntax) expression).Token.Value);
                    if (value is int)
                    {
                        return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, (int) value);
                    }
                    if (value is float)
                    {
                        return new DiagnosticExpressionSharpnode(expression as LiteralExpressionSyntax,
                            Diagnostics.SSIL109_FeatureNotSupportedBecauseSilver, "floating-point numbers");
                    }
                    if (value is double)
                    {
                        return new DiagnosticExpressionSharpnode(expression as LiteralExpressionSyntax,
                            Diagnostics.SSIL109_FeatureNotSupportedBecauseSilver, "double-precision numbers");
                    }
                    return new UnknownExpressionSharpnode(expression);
                case SyntaxKind.NullLiteralExpression:
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, null);


                // Variables
                case SyntaxKind.IdentifierName:
                    return new IdentifierExpressionSharpnode(expression as IdentifierNameSyntax);
                case SyntaxKind.SimpleMemberAccessExpression:
                    return new MemberAccessExpressionSharpnode(expression as MemberAccessExpressionSyntax);


                // Others
                case SyntaxKind.ConditionalExpression:
                    return new ConditionalExpressionSharpnode(expression as ConditionalExpressionSyntax);
                case SyntaxKind.ParenthesizedExpression:
                    return new ParenthesizedExpressionSharpnode(expression as ParenthesizedExpressionSyntax);


                case SyntaxKind.PointerMemberAccessExpression:
                case SyntaxKind.PointerIndirectionExpression:
                    return new UnknownExpressionSharpnode(expression, "pointers");
                case SyntaxKind.SimpleLambdaExpression:
                    return new LambdaSharpnode(expression as SimpleLambdaExpressionSyntax);
                case SyntaxKind.ParenthesizedLambdaExpression:
                    return new LambdaSharpnode(expression as ParenthesizedLambdaExpressionSyntax);
                case SyntaxKind.ElementAccessExpression:
                    return new ElementAccessSharpnode(expression as ElementAccessExpressionSyntax);
                default:
                    return new UnknownExpressionSharpnode(expression);
            }
        }

        /// <summary>
        /// Converts a Roslyn class member into a sharpnode.
        /// </summary>
        /// <param name="node">The class member declaration node.</param>
        public static Sharpnode MapClassMember(MemberDeclarationSyntax node)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch(node.Kind())
            {
                // Supported declarations:
                case SyntaxKind.MethodDeclaration:
                    return new MethodSharpnode(node as MethodDeclarationSyntax);

                case SyntaxKind.FieldDeclaration:
                    return new FieldDeclarationSharpnode(node as FieldDeclarationSyntax);
                    
                case SyntaxKind.ConstructorDeclaration:
                    return new ConstructorSharpnode(node as ConstructorDeclarationSyntax);

                // Declarations that are not supported 
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.AnonymousObjectMemberDeclarator:
                case SyntaxKind.CatchDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    return new UnknownSharpnode(node);
                case SyntaxKind.GlobalStatement:
                    return new UnknownSharpnode(node);
                default:
                    return new UnknownSharpnode(node);
            }
        }
    }
}
