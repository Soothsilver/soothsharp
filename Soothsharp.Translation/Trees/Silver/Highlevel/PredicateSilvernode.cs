using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class PredicateSilvernode : SubroutineSilvernode
    {
        public PredicateSilvernode(SyntaxNode methodDeclarationSyntax,
            IdentifierSilvernode identifierSilvernode, 
            List<ParameterSilvernode> parameters,
            string returnName,
            TypeSilvernode returnType, 
            List<VerificationConditionSilvernode> verificationConditions,
            BlockSilvernode block)
            : base(methodDeclarationSyntax,
                  identifierSilvernode,
                  parameters,
                  returnName,
                  returnType,
                  verificationConditions,
                  block)
        {
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var children = new List<Silvernode>();
                children.Add("predicate ");
                children.Add(this.Identifier);
                children.Add(" (");
                children.AddRange(this.Parameters.WithSeparator<Silvernode>(new TextSilvernode(", ")));
                children.Add(")");
                AddVerificationConditions(children);
                AddBlock(children);
                return children;
            }
        }

      
    }
}