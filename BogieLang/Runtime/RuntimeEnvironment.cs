using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{
    using VariableEnvironment = Dictionary<string, BogieLangTypeInstance>;

    public class RuntimeEnvironment
    {
        public Dictionary<string, FunctionDefinition> FunctionDefinitions = new Dictionary<string, FunctionDefinition>();

        public object GetVariableValue(string identifier, VariableEnvironment variableEnvironment)
        {
            if(variableEnvironment.ContainsKey(identifier))
            {
                return variableEnvironment[identifier].Value;
            }
            throw new Exception("Unknown identifer: " + identifier);
        }

        public void DefineVariable(string identifier, object value, VariableEnvironment variableEnvironment)
        {
            BogieLangType bogieLangType = BogieLangTypeHelpr.ObjectToType(value);
            BogieLangTypeInstance instance = new BogieLangTypeInstance();
            instance.BogieLangType = bogieLangType;
            instance.Identifer = identifier;
            instance.Value = value;

            if (!variableEnvironment.ContainsKey(identifier))
            {
                variableEnvironment.Add(identifier, instance);
                return;
            }
            throw new Exception("Redefinition of variable: " + identifier);
        }
    }
}
