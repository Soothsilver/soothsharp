using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using MoreLinq;
using System;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class MethodSilvernode : ComplexSilvernode
    {
        private IdentifierSilvernode identifierSilvernode;
        private BlockSilvernode block;
        private TypeSilvernode returnType;
        private List<VerificationConditionSilvernode> verificationConditions;
        private List<ParameterSilvernode> parameters;
        public MethodSilvernode(SyntaxNode methodDeclarationSyntax,
            IdentifierSilvernode identifierSilvernode, 
            List<ParameterSilvernode> parameters,
            TypeSilvernode returnType, 
            List<VerificationConditionSilvernode> verificationConditions,
            BlockSilvernode block)
            : base(methodDeclarationSyntax)
        {
            this.identifierSilvernode = identifierSilvernode;
            this.returnType = returnType;
            this.verificationConditions = verificationConditions;
            this.block = block;
            this.parameters = parameters;
        }
        public override IEnumerable<Silvernode> Children
        {
            get
            {
                var children = new List<Silvernode>();
                children.AddRange(new Silvernode[]
                {
                    new TextSilvernode("method "),
                    identifierSilvernode,
                    new TextSilvernode(" (")
                });
                //  parametersSilvernodes,
                children.AddRange(parameters.WithSeparator<Silvernode>(new TextSilvernode(", ")));
                children.Add(new TextSilvernode(")"));

                if (!returnType.RepresentsVoid())
                {
                    children.AddRange(new Silvernode[]
                    {
                        " returns (",
                        Constants.SilverReturnVariableName,
                        " : ",
                        returnType,
                        ")"
                    });
                }
                if (verificationConditions.Any())
                {
                    children.Add("\n");
                    children.AddRange(verificationConditions.WithSeparator<Silvernode>("\n").SelectMany(
                        condition =>
                        {
                            if (condition is TextSilvernode) return new[] {condition};
                            return new[] {"\t", condition};
                        }
                        ));
                    children.Add("\n");
                }
                else
                {
                    children.Add(" ");
                }
                children.Add(block);
                return children;
            }
        }
    }
}