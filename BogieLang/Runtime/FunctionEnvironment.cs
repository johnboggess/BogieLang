using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{

    public class FunctionEnvironment
    {
        public Dictionary<string, FunctionDefinition> FunctionDefinitions = new Dictionary<string, FunctionDefinition>();

        public bool IsFunctionDefined(string functionName)
        {
            return FunctionDefinitions.ContainsKey(functionName);
        }

        public FunctionDefinition GetDefinedFunction(string functionName)
        {
            if (IsFunctionDefined(functionName))
            {
                return FunctionDefinitions[functionName];
            }
            throw new Exception("Undefined function: " + functionName);
        }

        public void Clear()
        {
            FunctionDefinitions.Clear();
        }

        public void DefineFunction(string functionName, FunctionDefinition functionDefinition)
        {
            if (!IsFunctionDefined(functionName))
            {
                FunctionDefinitions.Add(functionName, functionDefinition);
            }
            else
            {
                throw new Exception("Redefinition of function: " + functionName);
            }
        }

        public FunctionDefinition this[string key]
        {
            get => FunctionDefinitions[key];
        }
    }
}
