using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{
    using VariableEnvironment = Dictionary<string, BogieLangTypeInstance>;

    public class RuntimeEnvironment
    {
        public VariableEnvironment GlobalVariables = new VariableEnvironment();
        public Dictionary<string, FunctionDefinition> FunctionDefinitions = new Dictionary<string, FunctionDefinition>();

        public object GetVariableValue(string identifier, VariableEnvironment variableEnvironment)
        {
            if(variableEnvironment.ContainsKey(identifier))
            {
                return variableEnvironment[identifier].Value;
            }
            else if(GlobalVariables.ContainsKey(identifier))
            {
                return GlobalVariables[identifier].Value;
            }
            throw new Exception("Unknown identifer: " + identifier);
        }
    }
}
