using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using MoreLinq;
using System;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
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
        public override IEnumerable<Silvernode> Children
        {
            get
            {
                var children = new List<Silvernode>();
                children.Add("method ");
                children.Add(Identifier);
                children.Add(" (");
                children.AddRange(Parameters.WithSeparator<Silvernode>(new TextSilvernode(", ")));
                children.Add(")");
                if (!ReturnType.RepresentsVoid())
                {
                    children.Add(" returns (");
                    children.Add(ReturnValueName);
                    children.Add(" : ");
                    children.Add(ReturnType);
                    children.Add(")");
                }
                AddVerificationConditions(children);
                AddBlock(children);
                return children;
            }
        }

      
    }
}