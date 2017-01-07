using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    abstract class InvocationTranslation
    {
        public List<Error> Errors { get; } = new List<Error>();

        public Silvernode Silvernode { get; protected set; }
        public List<StatementSilvernode> Prependors { get; } = new List<StatementSilvernode>();
        public bool Impure { get; protected set; } = false;
        protected List<Silvernode> ConvertToSilver(List<ExpressionSharpnode> arguments, TranslationContext context)
        {
            List<Silvernode> expressions = new List<Silver.Silvernode>();
            foreach (var argument in arguments)
            {
                var result = argument.Translate(context);
                expressions.Add(result.Silvernode);
                this.Prependors.AddRange(result.PrependTheseSilvernodes);
                this.Errors.AddRange(result.Errors);
            }
            return expressions;
        }
    

        public abstract void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context);
        public virtual void PostprocessPurity(TranslationResult result, TranslationContext context)
        {

        }
    }
}
