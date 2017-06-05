using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Represents a way in which a C# invocation expression can be translated into Viper. For example, some invocation 
    /// expressions might be translated as operators (.Implies is translated as ==>), some as keywords (acc, forall),
    /// and some method calls.
    /// </summary>
    abstract class InvocationTranslation
    {
        /// <summary>
        /// Gets the errors triggered during the translation. This should be read only after <see cref="Run(List{ExpressionSharpnode}, SyntaxNode, TranslationContext)"/> completes. 
        /// </summary>
        public List<Error> Errors { get; } = new List<Error>();
        /// <summary>
        /// Gets or sets the silvernode that is the result of the translation. This should be set inside the <see cref="Run(List{ExpressionSharpnode}, SyntaxNode, TranslationContext)"/> method. 
        /// </summary>
        public Silvernode Silvernode { get; protected set; }
        /// <summary>
        /// Gets the statements that should be prepended at an appropriate position before the resulting silvernode. This is 
        /// filled during Run.
        /// </summary>
        public List<StatementSilvernode> Prependors { get; } = new List<StatementSilvernode>();
        /// <summary>
        /// Gets or sets a value indicating whether the resulting silvernode can occur within a Viper expression or not.
        /// If not, then during additional processing, this will be extracted out as a prepended statement and stored in
        /// a temporary variable.
        /// </summary>
        protected bool Impure { get; set; }
        /// <summary>
        /// Translates all arguments and returns the translations. Errors and prepended statements that result from the translation
        /// of these arguments are stored within this instance.
        /// </summary>
        /// <param name="arguments">The arguments to translate.</param>
        /// <param name="context">The context.</param>
        protected List<Silvernode> ConvertToSilver(List<ExpressionSharpnode> arguments, TranslationContext context)
        { 
            List<Silvernode> expressions = new List<Silvernode>();
            foreach (var argument in arguments)
            {
                var result = argument.Translate(context);
                expressions.Add(result.Silvernode);
                this.Prependors.AddRange(result.PrependTheseSilvernodes);
                this.Errors.AddRange(result.Errors);
            }
            return expressions;
        }

        /// <summary>
        /// Translates this invocation with the given arguments and stores the result within this instance.
        /// </summary>
        /// <param name="arguments">The arguments to the invocation.</param>
        /// <param name="originalNode">The Roslyn node that corresponds to the C# method call.</param>
        /// <param name="context">The context.</param>
        public abstract void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context);

        /// <summary>
        /// If the translation resulted in a silvernode that can't be an expression, this method performs appropriate
        /// prepending to ensure a legal Viper code.
        /// </summary>
        /// <param name="result">The translation result that was the result of the translation.</param>
        /// <param name="context">The context.</param>
        public virtual void PostprocessPurity(TranslationResult result, TranslationContext context)
        {

        }
    }
}
