using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime.Operators
{
    class OperatorHelper
    {
        public static object Execute(string op, object left, object right)
        {
            if(op == "+") { return OpAdd.Add(left, right); }
            else if (op == "-") { return OpSubtract.Subtract(left, right); }
            else if (op == "==") { return OpEqual.Equal(left, right); }
            else { throw new Exception("Unknown operator: " + op); }
        }
    }
}
