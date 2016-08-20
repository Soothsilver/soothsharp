using Sharpsilver.Contracts.Internals;
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
        /// Within contracts, represents the return value of a function or a method, if it is of the type System.Int32. This is a shortcut for <see cref="Contract.Result{Int32}"/> with <see cref="int"/> as the type parameter.
        /// </summary>       
        [WithinContractsOnly]
        public static int IntegerResult
        {
            [WithinContractsOnly]
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Within contracts, represents the return value of a method. Can only be used in method postconditions.
        /// </summary>
        /// <typeparam name="T">Return type of the method.</typeparam>
        /// <returns>Value returned by the method.</returns>
        [WithinContractsOnly]
        public static T Result<T>()
        {
            return default(T);
        }

        /// <summary>
        /// The argument must be a variable. Within contracts, represents the value of the variable at the start of the method.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="value">Variable whose value at start of method should be returned.</param>
        /// <returns>Value of the variable as it was at start of method.</returns>
        [WithinContractsOnly]
        public static T Old<T>(T value)
        {
            return default(T);
        }

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
        /// Specifies an invariant for the enclosing loop. The verifier must prove that the invariant holds before the condition is tested (i.e. at entry) and then after each iteration.
        /// </summary>
        /// <param name="invariant">The condition that must hold at entry and after each iteration.</param>
        public static void Invariant(bool invariant)
        {

        }

        /// <summary>
        /// Adds a proof obligation: The verifier must prove that the specified condition holds at this point.
        /// </summary>
        /// <param name="proofObligation">The condition that must be proven to be true.</param>
        public static void Assert(bool proofObligation)
        {

        }

        /// <summary>
        /// Adds an assumption into a method body. The verifier will assume the specified condition to be true at this point.
        /// </summary>
        /// <param name="assumption">The condition that the verifier will assume to be true without proving it.</param>
        public static void Assume(bool assumption)
        {
        }

        /// <summary>
        /// Performs the Silver inhale operation. TODO what does it actually do?
        /// </summary>
        /// <param name="expression">TODO.</param>
        public static void Inhale(bool expression)
        {

        }
        /// <summary>
        /// Performs the Silver exhale operation. TODO what does it actually do?
        /// </summary>
        /// <param name="expression">TODO.</param>
        public static void Exhale(bool expression)
        {

        }

        /// <summary>
        /// Represents the permission to access the specified field. TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="field">TODO</param>
        /// <returns>TODO</returns>
        public static bool Acc<T>(T field)
        {
            return true;
        }

        /// <summary>
        /// Represents a read-only epsilon permission to access the specified field. TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool Read<T>(T field)
        {
            return true;
        }
        /// <summary>
        /// Represents a full write permission to access the specified field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool Write<T>(T field)
        {
            return true;
        }
    }
}
