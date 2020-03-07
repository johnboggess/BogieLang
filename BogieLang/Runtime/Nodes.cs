using System;
using System.Collections.Generic;
using System.Text;

namespace BogieLang.Runtime
{
    public class Literal
    {
        public double? Real = null;
        public int? Integer = null;
        public bool? Bool = null;
        public string String = null;

        public static Literal Compile(BogieLangParser.LiteralContext literalContext)
        {
            Literal result = new Literal();
            if(literalContext.REAL() != null) { result.Real = double.Parse(literalContext.REAL().GetText()); }
            else if (literalContext.INTEGER() != null) { result.Integer = int.Parse(literalContext.INTEGER().GetText()); }
            else if (literalContext.BOOL() != null) { result.Bool = bool.Parse(literalContext.BOOL().GetText()); }
            else if (literalContext.STRING() != null)
            {
                result.String = literalContext.STRING().GetText();
                result.String = result.String.Remove(0, 1);
                result.String = result.String.Remove(result.String.Length-1, 1);
            }

            return result;
        }
    }
}
