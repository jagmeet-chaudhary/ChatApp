﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class CommandLineExecutor
    {

        public static void InvokeCommand(string[] args, Type commandClassType)
        {
            Dictionary<string, string> commandLineParameters = new Dictionary<string, string>();
            var validCommand = args.Length > 0;
            if (!validCommand)
                throw new InvalidCommandException("Not a valid command.");

            var commandName = args[0].Trim();
            for (int index = 1; index < args.Length; index++)
            {
                if (args[index].StartsWith("-"))
                {
                    var flag = args[index].Split('-')[1];
                    if (args.Length < index)
                    {
                        throw new InvalidCommandException("Insufficient parameters");
                    }

                    var parameter = args[++index];
                    commandLineParameters.Add(flag, parameter);
                }
            }

            foreach (var method in commandClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                var attributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Count() > 0)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute is CommandAttribute && ((CommandAttribute)attribute).CommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                        {
                            var methodParamValues = new List<object>();

                            var methodParams = method.GetParameters();
                            foreach (var methodParam in methodParams)
                            {
                                var parameterAttribute = methodParam.GetCustomAttributes(typeof(CommandParameterAttribute), true).FirstOrDefault();
                                if (parameterAttribute == null) new InvalidCommandException("Not all parameters have command attributes associated.");
                                methodParamValues.Add(commandLineParameters[(parameterAttribute as CommandParameterAttribute).ParameterName]);
                            }

                            var instance = Activator.CreateInstance(commandClassType); ;
                            method.Invoke(instance, methodParamValues.ToArray());
                            return;
                        }
                    }

                }

            }
        }
    }
}
