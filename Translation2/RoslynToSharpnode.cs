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
                case SyntaxKind.IfStatement:
                    return new IfStatementSharpnode(statement as IfStatementSyntax);
                case SyntaxKind.ExpressionStatement:
                    return new ExpressionStatementSharpnode(statement as ExpressionStatementSyntax);
                case SyntaxKind.ReturnStatement:
                    return new ReturnStatementSharpnode(statement as ReturnStatementSyntax);
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
                        "/");

                // Unary operators
                case SyntaxKind.UnaryMinusExpression:
                    return new PrefixUnaryExpressionSharpnode(expression as PrefixUnaryExpressionSyntax, "-");

                    // TODO unary plus
                    // TODO modulo

                // Invocation
                case SyntaxKind.InvocationExpression:
                    return new InvocationExpressionSharpnode(expression as InvocationExpressionSyntax);

                // Literals
                case SyntaxKind.TrueLiteralExpression:
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, true);
                case SyntaxKind.FalseLiteralExpression:
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, false);
                case SyntaxKind.NumericLiteralExpression: 
                    return new LiteralExpressionSharpnode(expression as LiteralExpressionSyntax, 
                        (int)((expression as LiteralExpressionSyntax).Token.Value));

                // Variables
                case SyntaxKind.IdentifierName:
                case SyntaxKind.SimpleMemberAccessExpression:
                    return new IdentifierExpressionSharpnode(expression as ExpressionSyntax);

                // Others
                case SyntaxKind.ConditionalExpression:
                    return new ConditionalExpressionSharpnode(expression as ConditionalExpressionSyntax);
                default:
                    return new UnknownExpressionSharpnode(expression);
            }
        }

        public static Sharpnode Map(SyntaxNode node, Sharpnode parent = null)
        {
            switch(node.Kind())
            {
                // Supported declarations:
                case SyntaxKind.MethodDeclaration:
                    return new MethodSharpnode(node as MethodDeclarationSyntax, parent as ClassSharpnode);

                // Declarations that are not supported (yet)
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.AnonymousObjectMemberDeclarator:
                case SyntaxKind.CatchDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                // Handled elsewhere: case SyntaxKind.ClassDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                // Handled elsewhere: case SyntaxKind.NamespaceDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                case SyntaxKind.VariableDeclaration:
                    return new UnknownSharpnode(node);
                case SyntaxKind.FieldDeclaration:
                    return new DiagnosticSharpnode(node, Diagnostics.SSIL105_FeatureNotYetSupported, "fields");
                case SyntaxKind.CompilationUnit:
                    return new CompilationUnitSharpnode(node as CompilationUnitSyntax);
                default:
                    return new UnknownSharpnode(node);
            }
        }
        
        /*
        public static TranslationResult TranslateSyntaxNode(SyntaxNode node, TranslationContext context, TranslationProcess process)
        {
            switch (node.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    return ContainerTranslator.TranslateCompilationUnit(node as CompilationUnitSyntax, context, process);
                case SyntaxKind.NamespaceDeclaration:
                    return ContainerTranslator.TranslateNamespace(node as NamespaceDeclarationSyntax, context, process);
                case SyntaxKind.ClassDeclaration:
                    return ContainerTranslator.TranslateClassDeclaration(node as ClassDeclarationSyntax, context, process);
                case SyntaxKind.MethodDeclaration:
                    return ContainerTranslator.TranslateMethodDeclaration(node as MethodDeclarationSyntax, context, process);
                case SyntaxKind.Block:
                    return StatementTranslator.TranslateBlock(node as BlockSyntax, context, process);
                default:
                    return TranslationResult.Error(node, Diagnostics.SSIL101, node.Kind());
            }
        }*/
    }
}
