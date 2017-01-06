using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation;
using Soothsharp.Translation.Trees.CSharp;

namespace Soothsharp.Translation.Trees.CSharp
{
    class LambdaSharpnode : ExpressionSharpnode
    {
        private Error errorneousResult = null;
        private ParameterSharpnode parameter;
        private ExpressionSharpnode body;


        public LambdaSharpnode(ParenthesizedLambdaExpressionSyntax syntax) : base(syntax)
        {
           if (syntax.ParameterList.Parameters.Count != 1)
           {
               errorneousResult = new Soothsharp.Translation.Error(
                   Diagnostics.SSIL125_LambdasMustHaveSingleParameter, syntax.ParameterList);
               return;
           }
            parameter = new ParameterSharpnode(syntax.ParameterList.Parameters[0]);
            if (syntax.Body is ExpressionSyntax)
            {
                body = RoslynToSharpnode.MapExpression((ExpressionSyntax) syntax.Body);
            }
            else
            {
                errorneousResult = new Soothsharp.Translation.Error(
                    Diagnostics.SSIL126_LambdasMustBeExpressions, syntax.Body);
            }

        }
        public LambdaSharpnode(SimpleLambdaExpressionSyntax syntax) : base(syntax)
        {
            parameter = new ParameterSharpnode(syntax.Parameter);
            if (syntax.Body is ExpressionSyntax)
            {
                body = RoslynToSharpnode.MapExpression((ExpressionSyntax)syntax.Body);
            }
            else
            {
                errorneousResult = new Soothsharp.Translation.Error(
                    Diagnostics.SSIL126_LambdasMustBeExpressions, syntax.Body);
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            if (errorneousResult != null)
            {
                return TranslationResult.Error(errorneousResult.Node,
                    errorneousResult.Diagnostic,
                    errorneousResult.DiagnosticArguments);
            }
            return TranslationResult.Error(this.OriginalNode,
                Diagnostics.SSIL127_LambdasOnlyInContracts);
        }
    }
}
