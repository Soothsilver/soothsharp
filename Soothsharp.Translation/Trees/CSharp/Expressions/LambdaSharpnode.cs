using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace Soothsharp.Translation.Trees.CSharp
{
    class LambdaSharpnode : ExpressionSharpnode
    {
        private Error errorneousResult;
        private ParameterSharpnode parameter;
        private ExpressionSharpnode body;
        public Identifier VariableIdentifier { get; private set; }
        public SilverType VariableSilverType { get; private set; }
        public Silvernode BodySilvernode { get; private set; }
        private TranslationResult failedResult;

        public LambdaSharpnode(ParenthesizedLambdaExpressionSyntax syntax) : base(syntax)
        {
           if (syntax.ParameterList.Parameters.Count != 1)
           {
               this.errorneousResult = new Error(
                   Diagnostics.SSIL125_LambdasMustHaveSingleParameter, syntax.ParameterList);
               return;
           }
            this.parameter = new ParameterSharpnode(syntax.ParameterList.Parameters[0]);
            if (syntax.Body is ExpressionSyntax)
            {
                this.body = RoslynToSharpnode.MapExpression((ExpressionSyntax) syntax.Body);
            }
            else
            {
                this.errorneousResult = new Error(
                    Diagnostics.SSIL126_LambdasMustBeExpressions, syntax.Body);
            }

        }
        public LambdaSharpnode(SimpleLambdaExpressionSyntax syntax) : base(syntax)
        {
            this.parameter = new ParameterSharpnode(syntax.Parameter);
            if (syntax.Body is ExpressionSyntax)
            {
                this.body = RoslynToSharpnode.MapExpression((ExpressionSyntax)syntax.Body);
            }
            else
            {
                this.errorneousResult = new Error(
                    Diagnostics.SSIL126_LambdasMustBeExpressions, syntax.Body);
            }
        }



        public override TranslationResult Translate(TranslationContext context)
        {
            if (this.errorneousResult != null)
            {
                return TranslationResult.Error(this.errorneousResult.Node, this.errorneousResult.Diagnostic, this.errorneousResult.DiagnosticArguments);
            }
            return TranslationResult.Error(this.OriginalNode,
                Diagnostics.SSIL127_LambdasOnlyInContracts);
        }

        public bool PrepareForInsertionIntoQuantifier(TranslationContext context)
        {
            if (this.errorneousResult != null)
            {
                this.failedResult = TranslationResult.Error(this.errorneousResult.Node, this.errorneousResult.Diagnostic, this.errorneousResult.DiagnosticArguments);
                return false;
            }
            var parameterSymbol = context.Semantics.GetDeclaredSymbol(this.parameter.ParameterSyntax);
            this.VariableIdentifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(parameterSymbol);
            this.VariableSilverType = TypeTranslator.TranslateType(parameterSymbol.Type, this.parameter.OriginalNode,
                out this.errorneousResult);
            if (this.errorneousResult != null)
            {
                this.failedResult = TranslationResult.Error(this.errorneousResult.Node, this.errorneousResult.Diagnostic, this.errorneousResult.DiagnosticArguments);
                return false;
            }
            TranslationResult res = this.body.Translate(context.ChangePurityContext(PurityContext.PureOrFail));
            if (res.WasTranslationSuccessful)
            {
                this.BodySilvernode = res.Silvernode;
            }
            else
            {
                this.failedResult = res;
                return false;
            }
            return true;
        }

        internal TranslationResult GetErrorTranslationResult()
        {
            if (this.failedResult == null) throw new InvalidOperationException("failedResult was null");
            return this.failedResult; 
        }
    }
}
