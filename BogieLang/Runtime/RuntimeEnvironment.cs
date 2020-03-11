using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{

    public class RuntimeEnvironment
    {
        public Dictionary<string, FunctionDefinition> FunctionDefinitions = new Dictionary<string, FunctionDefinition>();
    }
}
