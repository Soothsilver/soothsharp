using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// Contains constant names used in Silver.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// In Silver, methods' return values have names that one used to refer to the return value
        /// inside the method. C# methods don't have this, so this is the name that is always given to 
        /// the return value. The identifier translator guarantees name safety.
        /// </summary>
        public const string SilverReturnVariableName = "res";
        /// <summary>
        /// This text is used instead of abstract syntax nodes that could not be translated. This text
        /// must be syntactically incorrect in Silver in any context (by using an illegal character).
        /// </summary>
        public const string SilverErrorString = "!ERROR!";
        /// <summary>
        /// Name of the label that is used in Silver for the end of the method (this is used when translating
        /// return statements).
        /// </summary>
        public const string SilverMethodEndLabel = "end";

        /// <summary>
        /// Name of the "silver type" for the superclass of all classes, System.Object.
        /// </summary>
        public const string SilverSystemObject = "System_Object";

        public const string CSharpTypeDomain = "CSharpType";
        public const string TypeOfFunction = "typeof";
        public const string IsSubTypeFunction = "isSubtype";
    }
}
