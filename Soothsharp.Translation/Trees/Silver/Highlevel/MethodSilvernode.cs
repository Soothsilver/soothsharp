using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class MethodSilvernode : SubroutineSilvernode
    {

        public MethodSilvernode(SyntaxNode methodDeclarationSyntax,
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
                var children = new List<Silvernode>();
                children.Add("method ");
                children.Add(this.Identifier);
                children.Add(" (");
                children.AddRange(this.Parameters.WithSeparator<Silvernode>(new TextSilvernode(", ")));
                children.Add(")");
                if (!this.ReturnType.RepresentsVoid())
                {
                    children.Add(" returns (");
                    children.Add(this.ReturnValueName);
                    children.Add(" : ");
                    children.Add(this.ReturnType);
                    children.Add(")");
                }
                AddVerificationConditions(children);
                AddBlock(children);
                return children;
            }
        }

      
    }
}