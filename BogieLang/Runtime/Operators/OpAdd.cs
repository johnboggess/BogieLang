using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime.Operators
{
    class OpAdd
    {
        public static object Add(object left, object right)
        {
            if (left is int)
            {
                if (right is int) { return (int)left + (int)right; }
                else if (right is double) { return (int)left + (double)right; }
                else if (right is string) { return (int)left + (string)right; }
                else if (right is bool) { return (int)left + ((bool)right ? 1 : 0); }
                else { throw new Exception("Cannot add " + left + " and " + right); }
            }
            else if (left is double)
            {
                if (right is int) { return (double)left + (int)right; }
                else if (right is double) { return (double)left + (double)right; }
                else if (right is string) { return (double)left + (string)right; }
                else if (right is bool) { return (double)left + ((bool)right ? 1 : 0); }
                else { throw new Exception("Cannot add " + left + " and " + right); }
            }
            else if (left is string)
            {
                if (right is int) { return (string)left + (int)right; }
                else if (right is double) { return (string)left + (double)right; }
                else if (right is string) { return (string)left + (string)right; }
                else if (right is bool) { return (string)left + (bool)right; }
                else { throw new Exception("Cannot add " + left + " and " + right); }
            }
            else if (left is bool)
            {
                if (right is int) { return ((bool)left ? 1 : 0) + (int)right; }
                else if (right is double) { return ((bool)left ? 1 : 0) + (double)right; }
                else if (right is string) { return (bool)left + (string)right; }
                else { throw new Exception("Cannot add " + left + " and " + right); }
            }
            else { throw new Exception("Cannot add " + left + " and " + right); }
        }
    }
}
