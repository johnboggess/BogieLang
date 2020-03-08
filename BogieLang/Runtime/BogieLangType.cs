using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{
    public enum BogieLangType
    {
        REAL,
        INTEGER,
        BOOL,
        STRING,
        VOID
    }

    public class BogieLangTypeHelpr
    {
        public static BogieLangType StringToType(string str)
        {
            if (str.ToLower() == "real") { return BogieLangType.REAL; }
            else if (str.ToLower() == "int") { return BogieLangType.INTEGER; }
            else if (str.ToLower() == "bool") { return BogieLangType.BOOL; }
            else if (str.ToLower() == "string") { return BogieLangType.STRING; }
            else if (str.ToLower() == "void") { return BogieLangType.VOID; }
            else { throw new Exception("Unknown type: " + str); }
        }
    }
}
