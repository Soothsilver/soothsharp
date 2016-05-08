using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// Contains static functions that translate into Silver language constructs, such as preconditions or postconditions.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Within contracts, represents the return value of a function or a method, if it is of the type System.Int32.
        /// </summary>
        public static int IntegerResult { get; } = 0;

        /// <summary>
        /// Adds a proof obligation: The verifier must ensure that the specified postcondition is true when this method returns.
        /// 
        /// <para>
        /// <c>Remarks:</c> You must put the "Contract.Ensures()" call on the top level of the statement block for the method. By convention, you should put all Contact.Requires() and Contract.Ensures() calls at the top of the function before any other statements. This C# method itself has an empty body but it is translated into Silver constructs. 
        /// </para>
        /// <para>
        /// The boolean expression given as the argument must be a pure expression.
        /// </para>
        /// </summary>
        /// <param name="postcondition">The condition that will be verified to be true at each exit point.</param>
        public static void Ensures(bool postcondition)
        {
        }

        /// <summary>
        /// Adds a precondition: The verifier can assume that it is true when this method is entered, and callsites must prove the condition when they call this method.
        /// <para>
        /// Remarks: You must put the "Contract.Ensures()" call on the top level of the statement block for the method. By convention, you should put all Contact.Requires() and Contract.Ensures() calls at the top of the function before any other statements. This C# method itself has an empty body but it is translated into Silver constructs. 
        /// </para>
        /// <para>
        /// The boolean expression given as the argument must be a pure expression.
        /// </para>
        /// </summary>
        /// <param name="precondition">The condition that will hold whenever this method is entered.</param>
        public static void Requires(bool precondition)
        {
        }

        /// <summary>
        /// Adds a proof obligation: The verifier must prove that the specified condition holds at this point.
        /// </summary>
        /// <param name="proofObligation">The condition that must be proven to be true.</param>
        public static void Assert(bool proofObligation)
        {

        }
    }
}
