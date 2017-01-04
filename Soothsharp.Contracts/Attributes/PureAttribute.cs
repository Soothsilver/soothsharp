﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annnotated method is pure and should be translated as a Viper function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PureAttribute : Attribute
    {

    }
}
