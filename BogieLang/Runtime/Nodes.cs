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

    public class Expression
    {
        public string Identifier = null;
        public Literal Literal = null;
        public string Operator = null;
        public Expression SubExpression = null;

        public static Expression Compile(BogieLangParser.ExpressionContext expressionContext)
        {
            Expression result = new Expression();
            if (expressionContext.IDENTIFIER() != null) { result.Identifier = expressionContext.IDENTIFIER().GetText(); }
            else if (expressionContext.literal() != null) { result.Literal = Literal.Compile(expressionContext.literal()); }

            if (expressionContext.OPERATOR() != null) { result.Operator = expressionContext.OPERATOR().GetText(); }
            if (expressionContext.expression() != null) { result.SubExpression = Expression.Compile(expressionContext.expression()); }

            return result;
        }
    }
}
