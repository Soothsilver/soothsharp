using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// Represents a type of the Silver language.
    /// </summary>
    enum SilverType
    {
        /// <summary>
        /// No type, for example, this represents the type of a method without a return value.
        /// </summary>
        Void,
        /// <summary>
        /// The signed integer.
        /// </summary>
        Int,
        /// <summary>
        /// The fractional permission.
        /// </summary>
        Perm,
        /// <summary>
        /// The reference type.
        /// </summary>
        Ref,
        /// <summary>
        /// The boolean type.
        /// </summary>
        Bool,
        /// <summary>
        /// Represents a reference to a C# type that could not be translated to Silver, such as float or double.
        /// </summary>
        Error
    }
}
