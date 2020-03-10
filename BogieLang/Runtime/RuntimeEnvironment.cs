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
    }
}
