﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public readonly string CommandName;
        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class CommandParameterAttribute : Attribute
    {
        public readonly string ParameterName;
        public CommandParameterAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}
