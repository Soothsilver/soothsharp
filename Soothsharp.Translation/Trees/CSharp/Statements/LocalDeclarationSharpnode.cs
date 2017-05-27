using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class LocalDeclarationSharpnode : StatementSharpnode
    {
        private List<SimpleLocalDeclarationSharpnode> Declarations = new List<SimpleLocalDeclarationSharpnode>();

        public LocalDeclarationSharpnode(VariableDeclarationSyntax stmt) : base(null)
        {
            Init(stmt);
        }
        public LocalDeclarationSharpnode(LocalDeclarationStatementSyntax stmt) : base(stmt)
        {
            Init(stmt.Declaration);
        }

        private void Init(VariableDeclarationSyntax syntax)
        {
            var typeSyntax = syntax.Type;
            foreach (var variable in syntax.Variables)
            {
                ExpressionSharpnode initialValue = null;
                if (variable.Initializer != null)
                {
                    initialValue = RoslynToSharpnode.MapExpression(variable.Initializer.Value);
                }
                SimpleLocalDeclarationSharpnode declaration =
                    new SimpleLocalDeclarationSharpnode(
                        typeSyntax,
                        initialValue,
                        variable)
                    ;
                this.Declarations.Add(declaration);
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            List<StatementSilvernode> statements = new List<StatementSilvernode>();
            List<Error> diagnostics = new List<Error>();
            foreach (var declaration in this.Declarations)
            {
                var statementResult = declaration.Translate(context);
                if (statementResult.Silvernode != null)
                {
                    statements.Add(statementResult.Silvernode as StatementSilvernode);
                }
                diagnostics.AddRange(statementResult.Errors);
            }
            StatementsSequenceSilvernode statementsSequence = new StatementsSequenceSilvernode(this.OriginalNode, statements.ToArray());
            return TranslationResult.FromSilvernode(statementsSequence, diagnostics);
        }
    }

    internal class SimpleLocalDeclarationSharpnode : Sharpnode
    {
        private readonly ExpressionSharpnode initialValue;
        private readonly TypeSyntax typeSyntax;
        private readonly VariableDeclaratorSyntax variable;

        public SimpleLocalDeclarationSharpnode(
            TypeSyntax typeSyntax, 
            ExpressionSharpnode initialValue,
            VariableDeclaratorSyntax variable) : base(variable)
        {
            this.variable = variable;
            this.typeSyntax = typeSyntax;
            this.initialValue = initialValue;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var symbol = context.Semantics.GetDeclaredSymbol(this.variable);
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(symbol);

            Error err;
            List<Error> errors = new List<Error>();
            VarStatementSilvernode intro =
                    new VarStatementSilvernode(identifier, 
                                       TypeTranslator.TranslateType(
                                           context.Semantics.GetSymbolInfo(this.typeSyntax).Symbol as ITypeSymbol, this.typeSyntax,
                                           out err), this.OriginalNode);

            if (err != null) errors.Add(err);
            if (this.initialValue == null)
            {
                return TranslationResult.FromSilvernode(intro, errors);
            }

            // Add assignment if there is an initial value.
            var res = this.initialValue.Translate(context.ChangePurityContext(PurityContext.Purifiable));

            AssignmentSilvernode assignmentSilvernode =
                new AssignmentSilvernode(new IdentifierSilvernode(identifier), res.Silvernode, this.OriginalNode);
            errors.AddRange(res.Errors);
            return TranslationResult.FromSilvernode(new StatementsSequenceSilvernode(this.OriginalNode,
                intro,
                new StatementsSequenceSilvernode(null, res.PrependTheseSilvernodes.ToArray()),
                assignmentSilvernode), errors);
        }
    }
}
