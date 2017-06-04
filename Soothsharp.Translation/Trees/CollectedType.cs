using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a C# class that is to be used in Viper. 
    /// 
    /// To translate a class, we must put its fields into Viper code and we must create an initializer (which is also the default constructor,
    /// if no constructor was defined).
    /// </summary>
    public class CollectedType
    {
        private ISymbol ClassSymbol;
        private readonly bool _isStatic;
        public List<CollectedField> InstanceFields = new List<CollectedField>();

        public CollectedType(ISymbol classSymbol, bool isStatic)
        {
            this.ClassSymbol = classSymbol;
            this._isStatic = isStatic;
        }

        /// <summary>
        /// Generates the Viper text that contains fields and the initializer of this class.
        /// </summary>
        /// <param name="process">The TranslationProcess.</param>
        public Silvernode GenerateGlobalSilvernode(TranslationProcess process)
        {

            var node = new SimpleSequenceSilvernode(null);

            // Translate fields
            foreach (CollectedField field in this.InstanceFields)
            {
                node.List.Add("field ");
                node.List.Add(new IdentifierSilvernode(field.Name));
                node.List.Add(": ");
                node.List.Add(new TypeSilvernode(null, field.SilverType));
                if (this.InstanceFields[this.InstanceFields.Count - 1] != field)
                {
                    node.List.Add("\n");
                }
            }

            // Translate the initializer/default constructor
            if (!this._isStatic)
            {
                Identifier initializer = process.IdentifierTranslator.RegisterAndGetIdentifierWithTag(this.ClassSymbol,
                    Constants.InitializerTag);

                // The initializer gives write access to all fields
                var accessToAllFields = new List<ContractSilvernode>();
                foreach (CollectedField field in this.InstanceFields)
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
            }

            return node;
        }
    }
}
