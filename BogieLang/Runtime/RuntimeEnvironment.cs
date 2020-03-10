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

            if (variableEnvironment.ContainsKey(identifier))
            {
                if(variableEnvironment[identifier].BogieLangType == bogieLangType)
                {
                    variableEnvironment[identifier].Value = value;
                    return;
                }
                throw new Exception(identifier + " is of type " + variableEnvironment[identifier].BogieLangType + ", got a " + bogieLangType);
            }
            throw new Exception("Undeclarated variable: " + identifier);
        }

        public void DeclareVariable(string identifier, BogieLangType bogieLangType, object value, VariableEnvironment variableEnvironment)
        {
            BogieLangTypeInstance instance = new BogieLangTypeInstance();
            instance.BogieLangType = bogieLangType;
            instance.Identifer = identifier;
            instance.Value = value;

            if(value != null)
            {
                if(bogieLangType != BogieLangTypeHelpr.ObjectToType(value))
                {
                    throw new Exception(identifier + " is of type " + BogieLangTypeHelpr.ObjectToType(value) + ", got a " + bogieLangType);
                }
            }

            if (!variableEnvironment.ContainsKey(identifier))
            {
                variableEnvironment.Add(identifier, instance);
                return;
            }
            throw new Exception("Redeclaration of variable: " + identifier);
        }
    }
}
