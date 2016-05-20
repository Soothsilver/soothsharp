using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Statements
{
    class LocalDeclarationSharpnode : StatementSharpnode
    {
        public List<SimpleLocalDeclarationSharpnode> Declarations = new List<SimpleLocalDeclarationSharpnode>();

        public LocalDeclarationSharpnode(LocalDeclarationStatementSyntax stmt) : base(stmt)
        {
            var typeSyntax = stmt.Declaration.Type;
            foreach (var variable in stmt.Declaration.Variables)
            {
                SyntaxToken identifier = variable.Identifier;
                ExpressionSharpnode initialValue = null;
                if (variable.Initializer != null)
                {
                    initialValue = RoslynToSharpnode.MapExpression(variable.Initializer.Value);
                }
                SimpleLocalDeclarationSharpnode declaration =
                    new SimpleLocalDeclarationSharpnode(
                        identifier,
                        typeSyntax,
                        initialValue,
                        variable)
                    ;
                Declarations.Add(declaration);
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            List<Silvernode> statements = new List<Silvernode>();
            List<Error> diagnostics = new List<Error>();
            foreach (var declaration in Declarations)
            {
                var statementResult = declaration.Translate(context);
                if (statementResult.Silvernode != null)
                {
                    statements.Add(statementResult.Silvernode);
                }
                diagnostics.AddRange(statementResult.Errors);
            }
            SequenceSilvernode sequence = new SequenceSilvernode(OriginalNode, statements.ToArray());
            return TranslationResult.FromSilvernode(sequence, diagnostics);
        }
    }

    internal class SimpleLocalDeclarationSharpnode : Sharpnode
    {
        private SyntaxToken identifier;
        private ExpressionSharpnode initialValue;
        private TypeSyntax typeSyntax;

        public SimpleLocalDeclarationSharpnode(SyntaxToken identifier, TypeSyntax typeSyntax, ExpressionSharpnode initialValue, VariableDeclaratorSyntax variable) : base(variable)
        {
            this.identifier = identifier;
            this.typeSyntax = typeSyntax;
            this.initialValue = initialValue;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            // TODO improve
            // TODO FIX!
            Error err;
            List<Error> errors = new List<Error>();
            TextSilvernode intro =
                    new TextSilvernode("var " + identifier.Text + " : " +
                                       TypeTranslator.TranslateTypeToString(
                                           context.Semantics.GetSymbolInfo(typeSyntax).Symbol as ITypeSymbol, typeSyntax,
                                           out err), OriginalNode);
            if (err != null) errors.Add(err);
            if (initialValue == null)
            {
                return TranslationResult.FromSilvernode(intro, errors);
            }
            var res = initialValue.Translate(context);
            AssignmentSilvernode assignmentSilvernode =
                new AssignmentSilvernode(new TextSilvernode(identifier.Text, OriginalNode), res.Silvernode, OriginalNode);
            errors.AddRange(res.Errors);
            return TranslationResult.FromSilvernode(new SequenceSilvernode(OriginalNode,
                intro,
                assignmentSilvernode), errors);
        }
    }
}
