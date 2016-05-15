using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Linq;
using System;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class MethodSilvernode : Silvernode
    {
        private IdentifierSilvernode identifierSilvernode;
        private MethodDeclarationSyntax methodDeclarationSyntax;
        private BlockSilvernode block;
        private TypeSilvernode returnType;
        private List<Silvernode> verificationConditions;
        private List<ParameterSilvernode> Parameters = new List<ParameterSilvernode>();
        public MethodSilvernode(MethodDeclarationSyntax methodDeclarationSyntax,
            IdentifierSilvernode identifierSilvernode, 
            List<ParameterSilvernode> parameters,
            TypeSilvernode returnType, 
            List<Silvernode> verificationConditions,
            BlockSilvernode block)
            : base(methodDeclarationSyntax)
        {
            this.methodDeclarationSyntax = methodDeclarationSyntax;
            this.identifierSilvernode = identifierSilvernode;
            this.returnType = returnType;
            this.verificationConditions = verificationConditions;
            this.block = block;
            this.Parameters = parameters;
        }
        public override string ToString()
        {
            string parametersString = string.Join(", ", Parameters.Select(p => p.Identifier + " : " + p.Type)); 
            // TODO handle identifiers better

            return "method " + identifierSilvernode
                + " ("
                + parametersString
                + ") returns ("
                + Constants.SILVER_RETURN_VARIABLE_NAME + " : " + returnType
                + ")"
                + (verificationConditions.Any() ? "\n" + VerificationConditionsToString() + "\n" : " ")
                + block.ToString();
        }

        private string VerificationConditionsToString()
        {
            string conditions = String.Join("\n", verificationConditions.Select(cond => "\t" + cond.ToString()));
            return conditions;
        }
    }
}