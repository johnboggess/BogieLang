using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime.Operators
{
    class OpEqual
    {
        public static object Equal(object left, object right)
        {
            if (left is int)
            {
                if (right is int) { return (int)left == (int)right; }
                return false;
            }
            else if (left is double)
            {
                if (right is double) { return (double)left == (double)right; }
                return false;
            }
            else if (left is string)
            {
                if (right is string) { return (string)left == (string)right; }
                return false;
            }
            else if (left is bool)
            {
                if (right is bool) { return (bool)left == (bool)right; }
                return false;
            }
            return false;
        }
    }
}
