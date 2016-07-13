using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Statements;

namespace Sharpsilver.Translation
{
    public class RoslynToSharpnode
    {
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
                case SyntaxKind.GlobalStatement:
                case SyntaxKind.GotoCaseStatement:
                case SyntaxKind.GotoDefaultStatement:
                case SyntaxKind.LockStatement:
                case SyntaxKind.SwitchStatement:
                case SyntaxKind.UsingStatement:
                    return new UnknownStatementSharpnode(statement);
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
                // Invocation
                case SyntaxKind.InvocationExpression:
                    return new InvocationExpressionSharpnode(expression as InvocationExpressionSyntax);

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


                // Variables
                case SyntaxKind.IdentifierName:
                case SyntaxKind.SimpleMemberAccessExpression:
                    return new IdentifierExpressionSharpnode(expression as ExpressionSyntax);

                // Others
                case SyntaxKind.ConditionalExpression:
                    return new ConditionalExpressionSharpnode(expression as ConditionalExpressionSyntax);
                case SyntaxKind.ParenthesizedExpression:
                    return new ParenthesizedExpressionSharpnode(expression as ParenthesizedExpressionSyntax);
                    

                default:
                    return new UnknownExpressionSharpnode(expression);
            }
        }

        public static Sharpnode MapClassMember(MemberDeclarationSyntax node)
        {
            switch(node.Kind())
            {
                // Supported declarations:
                case SyntaxKind.MethodDeclaration:
                    return new MethodSharpnode(node as MethodDeclarationSyntax);

                // Declarations that are not supported (yet)
                case SyntaxKind.FieldDeclaration:
                    return new DiagnosticSharpnode(node, Diagnostics.SSIL105_FeatureNotYetSupported, "fields");
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.AnonymousObjectMemberDeclarator:
                case SyntaxKind.CatchDeclaration:
                case SyntaxKind.ConstructorDeclaration:
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
                default:
                    return new UnknownSharpnode(node);
            }
        }
    }
}
