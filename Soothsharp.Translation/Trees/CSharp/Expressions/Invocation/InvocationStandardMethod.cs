using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp.Expressions;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationStandardMethod : InvocationSubroutine
    {
        private ExpressionSyntax methodGroup;
        private ExpressionSharpnode methodGroupSharpnode;
        private readonly SymbolInfo _method;

        public InvocationStandardMethod(ExpressionSyntax methodGroup, ExpressionSharpnode methodGroupSharpnode, SymbolInfo method)
        {
            this.methodGroup = methodGroup;
            this.methodGroupSharpnode = methodGroupSharpnode;
            this._method = method;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var methodSymbol = this._method.Symbol as IMethodSymbol;
            var identifier = context.Process.IdentifierTranslator.GetIdentifierReference(this._method.Symbol as IMethodSymbol);
            IMethodSymbol theMethod = (this._method.Symbol as IMethodSymbol);
            this.Impure = !(ContractsTranslator.IsMethodPureOrPredicate(theMethod));

            if (!theMethod.IsStatic)
            {
                if (this.methodGroupSharpnode is IdentifierExpressionSharpnode)
                {
                    arguments.Insert(0, new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, this.methodGroup));
                }
                else if (this.methodGroupSharpnode is MemberAccessExpressionSharpnode)
                {
                    arguments.Insert(0, ((MemberAccessExpressionSharpnode)this.methodGroupSharpnode).Container);
                }
                else
                {
                    this.Errors.Add(new Error(Diagnostics.SSIL102_UnexpectedNode, this.methodGroup, this.methodGroup.Kind()));
                }
            }
            Error error = null;
            this.SilverType = TypeTranslator.TranslateType(methodSymbol.ReturnType, this.methodGroup, out error);
            if (error != null)
            {
                this.Errors.Add(error);
            }
            var expressions = ConvertToSilver(arguments, context);
            this.Silvernode = new CallSilvernode(
                      identifier,
                      expressions, this.SilverType,
                      originalNode
                  );
        }
    }
}
