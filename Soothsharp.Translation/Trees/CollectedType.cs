using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    public class CollectedType
    {
        public Identifier Name;
        public Identifier Supertype;
        public ISymbol ClassSymbol;
        public List<CollectedField> InstanceFields = new List<CollectedField>();

        public CollectedType(ISymbol classSymbol, Identifier name, Identifier supertype)
        {
            this.ClassSymbol = classSymbol;
            Name = name;
            Supertype = supertype;
        }

        public Silvernode GenerateGlobalSilvernode(TranslationProcess process)
        {

            var node = new SimpleSequenceSilvernode(null);

            foreach (CollectedField field in InstanceFields)
            {
                node.List.Add("field ");
                node.List.Add(new IdentifierSilvernode(field.Name));
                node.List.Add(": ");
                node.List.Add(new TypeSilvernode(null, field.SilverType));
                if (InstanceFields[InstanceFields.Count - 1] != field)
                {
                    node.List.Add("\n");
                }
            }

            Identifier initializer = process.IdentifierTranslator.RegisterAndGetIdentifierWithTag(ClassSymbol, Constants.InitializerTag);

            var accessToAllFields = new List<VerificationConditionSilvernode>();
            foreach (CollectedField field in InstanceFields)
            {
                var protectedField = new SimpleSequenceSilvernode(null,
                    Constants.SilverThis,
                    ".",
                    new IdentifierSilvernode(field.Name)
                    );
                accessToAllFields.Add(new EnsuresSilvernode(new AccSilvernode(protectedField, "write", null), null));

            }

            var initializerContents = new BlockSilvernode(null, new List<StatementSilvernode>());
            initializerContents.Add(new AssignmentSilvernode(Constants.SilverThis, new NewStarSilvernode(null), null));

            var initializerMethod = new MethodSilvernode(null,
                new IdentifierSilvernode(initializer),
                new List<ParameterSilvernode>(),
                Constants.SilverThis,
                new TypeSilvernode(null, SilverType.Ref),
                accessToAllFields,
                initializerContents);

            node.List.Add("\n");
            node.List.Add(initializerMethod);


            return node;
        }

        public Silvernode GenerateSilvernodeInsideCSharpType()
        {
            return new SimpleSequenceSilvernode(null,
                "\tunique function ",
                new IdentifierSilvernode(Name),
                "() : ",
                Constants.CSharpTypeDomain
                );
        }
    }
}
