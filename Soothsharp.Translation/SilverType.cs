using System;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a type of the Silver language.
    /// </summary>
    public class SilverType : IEquatable<SilverType>
    {
        public override int GetHashCode()
        {
            return this._silvername?.GetHashCode() ?? 0;
        }

        private SilverType(string simplename)
        {
            this._silvername = simplename;
        }
        private readonly string _silvername;
        public bool Equals(SilverType other)
        {
            return other != null && this._silvername == other._silvername;
        }

        public override bool Equals(object obj)
        {
            return obj is SilverType && this.Equals((SilverType) obj);
        }

        public static bool operator==(SilverType one, SilverType other)
        {
            if (Object.ReferenceEquals(one, other))
            {
                return true;
            }
            return one?.Equals(other) ?? false;
        }

        public static bool operator !=(SilverType one, SilverType other)
        {
            return !(one == other);
        }

        public override string ToString()
        {
            return _silvername;
        }

        /// <summary>
        /// No type, for example, this represents the type of a method without a return value.
        /// </summary>
        public static SilverType Void => new SilverType("?!VOID?!");

        /// <summary>
        /// The signed integer.
        /// </summary>
        public static SilverType Int => new SilverType("Int");

        /// <summary>
        /// The fractional permission.
        /// </summary>
        public static SilverType Perm => new SilverType("Perm");

        /// <summary>
        /// The reference type.
        /// </summary>
        public static SilverType Ref => new SilverType("Ref");

        /// <summary>
        /// The boolean type.
        /// </summary>
        public static SilverType Bool => new SilverType("Bool");

        /// <summary>
        /// Represents a reference to a C# type that could not be translated to Silver, such as float or double.
        /// </summary>
        public static SilverType Error => new SilverType(Constants.SilverErrorString);

        public static SilverType Seq(SilverType innerType) => new SilverType("Seq[" + innerType + "]");
    }
}
