using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{
    public class VariableEnvironment
    {
        public VariableEnvironment ParentEnvironment = null;
        public Dictionary<string, BogieLangTypeInstance> DeclaredVariables = new Dictionary<string, BogieLangTypeInstance>();
        
        public bool IsVariableDeclared(string identifier)
        {
            return DeclaredVariables.ContainsKey(identifier);
        }
        
        public object GetVariableValue(string identifier)
        {
            if (IsVariableDeclared(identifier))
            {
                return DeclaredVariables[identifier].Value;
            }
            else if(ParentEnvironment != null)
            {
                return ParentEnvironment.GetVariableValue(identifier);
            }
            throw new Exception("Undeclared identifer: " + identifier);
        }

        public void Clear()
        {
            DeclaredVariables.Clear();
        }

        public void DeclareVariable(string identifier, BogieLangType bogieLangType)
        {
            if(!IsVariableDeclared(identifier))
            {
                BogieLangTypeInstance bogieLangTypeInstance = new BogieLangTypeInstance();
                bogieLangTypeInstance.BogieLangType = bogieLangType;
                bogieLangTypeInstance.Identifer = identifier;
                DeclaredVariables.Add(identifier, bogieLangTypeInstance);
            }
            else
            {
                throw new Exception("Redeclaration of: " + identifier);
            }
        }

        public void DefineVariable(string identifier, object obj)
        {
            if (IsVariableDeclared(identifier))
            {
                if(DeclaredVariables[identifier].BogieLangType == BogieLangTypeHelpr.ObjectToType(obj))
                {
                    DeclaredVariables[identifier].Value = obj;
                }
                else
                {
                    throw new Exception(identifier + " is of type " + DeclaredVariables[identifier].BogieLangType + ", got a " + BogieLangTypeHelpr.ObjectToType(obj));
                }
            }
            else if(ParentEnvironment != null)
            {
                ParentEnvironment.DefineVariable(identifier, obj);
            }
            else
            {
                throw new Exception("Undeclaration variable: " + identifier);
            }
        }

        public BogieLangTypeInstance this[string key]
        {
            get => DeclaredVariables[key];
        }
    }
}
