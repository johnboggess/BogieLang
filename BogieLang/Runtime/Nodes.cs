﻿using System;
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
            if (literalContext.REAL() != null) { result.Real = double.Parse(literalContext.REAL().GetText()); }
            else if (literalContext.INTEGER() != null) { result.Integer = int.Parse(literalContext.INTEGER().GetText()); }
            else if (literalContext.BOOL() != null) { result.Bool = bool.Parse(literalContext.BOOL().GetText()); }
            else if (literalContext.STRING() != null)
            {
                result.String = literalContext.STRING().GetText();
                result.String = result.String.Remove(0, 1);
                result.String = result.String.Remove(result.String.Length - 1, 1);
            }

            return result;
        }
    }

    public class Expression
    {
        public string Identifier = null;
        public Literal Literal = null;
        public string Operator = null;
        public FunctionCall FunctionCall = null;
        public Expression SubExpression = null;

        public static Expression Compile(BogieLangParser.ExpressionContext expressionContext)
        {
            Expression result = new Expression();
            if (expressionContext.IDENTIFIER() != null) { result.Identifier = expressionContext.IDENTIFIER().GetText(); }
            else if (expressionContext.literal() != null) { result.Literal = Literal.Compile(expressionContext.literal()); }
            else if (expressionContext.functionCall() != null) { result.FunctionCall = FunctionCall.Compile(expressionContext.functionCall()); }

            if (expressionContext.OPERATOR() != null) { result.Operator = expressionContext.OPERATOR().GetText(); }
            if (expressionContext.expression() != null) { result.SubExpression = Expression.Compile(expressionContext.expression()); }

            return result;
        }
    }

    public class FunctionCall
    {
        public string Identifier = null;
        public List<Expression> Arguments = new List<Expression>();

        public static FunctionCall Compile(BogieLangParser.FunctionCallContext functionCallContext)
        {
            FunctionCall result = new FunctionCall();
            if (functionCallContext.IDENTIFIER() != null) { result.Identifier = functionCallContext.IDENTIFIER().GetText(); }
            if (functionCallContext.expression().Length != 0)
            {
                List<Expression> expressions = new List<Expression>();
                foreach (BogieLangParser.ExpressionContext expressionContext in functionCallContext.expression())
                {
                    expressions.Add(Expression.Compile(expressionContext));
                }
                result.Arguments = expressions;
            }

            return result;
        }
    }

    public class VarDefinition
    {
        public string Identifier = null;
        public Expression Expression = null;

        public static VarDefinition Compile(BogieLangParser.VarDefinitionContext varDefinitionContext)
        {
            VarDefinition result = new VarDefinition();
            if (varDefinitionContext.IDENTIFIER() != null) { result.Identifier = varDefinitionContext.IDENTIFIER().GetText(); }
            if (varDefinitionContext.expression() != null) { result.Expression = Expression.Compile(varDefinitionContext.expression()); }

            return result;
        }
    }

    public class VarDeclaration
    {
        public BogieLangType? BogieLangType = null;
        public string Identifier = null;
        public Expression Expression = null;

        public static VarDeclaration Compile(BogieLangParser.VarDeclarationContext varDeclarationContext)
        {
            VarDeclaration result = new VarDeclaration();
            if (varDeclarationContext.TYPE() != null) { result.BogieLangType = Runtime.BogieLangTypeHelpr.StringToType(varDeclarationContext.TYPE().GetText()); }
            if (varDeclarationContext.IDENTIFIER() != null) { result.Identifier = varDeclarationContext.IDENTIFIER().GetText(); }
            if (varDeclarationContext.expression() != null) { result.Expression = Expression.Compile(varDeclarationContext.expression()); }

            return result;
        }
    }

    public class FunctionReturn
    {
        public Expression Expression = null;

        public static FunctionReturn Compile(BogieLangParser.FunctionReturnContext functionReturnContext)
        {
            FunctionReturn result = new FunctionReturn();
            if (functionReturnContext.expression() != null) { result.Expression = Expression.Compile(functionReturnContext.expression()); }

            return result;
        }
    }

    public class Body
    {
        public VarDeclaration VarDeclaration = null;
        public VarDefinition VarDefinition = null;
        public FunctionCall FunctionCall = null;
        public FunctionReturn FunctionReturn = null;
        public IfControl IfControl = null;

        public static Body Compile(BogieLangParser.BodyContext bodyContext)
        {
            Body result = new Body();
            if (bodyContext.varDeclaration() != null) { result.VarDeclaration = VarDeclaration.Compile(bodyContext.varDeclaration()); }
            else if (bodyContext.varDefinition() != null) { result.VarDefinition = VarDefinition.Compile(bodyContext.varDefinition()); }
            else if (bodyContext.functionCall() != null) { result.FunctionCall = FunctionCall.Compile(bodyContext.functionCall()); }
            else if (bodyContext.functionReturn() != null) { result.FunctionReturn = FunctionReturn.Compile(bodyContext.functionReturn()); }
            else if (bodyContext.ifControl() != null) { result.IfControl = IfControl.Compile(bodyContext.ifControl()); }

            return result;
        }
    }

    public class IfControl
    {
        public Expression Expression;
        public List<Body> Body = new List<Body>();

        public static IfControl Compile(BogieLangParser.IfControlContext ifControlContext)
        {
            IfControl result = new IfControl();
            if (ifControlContext.expression() != null) { result.Expression = Expression.Compile(ifControlContext.expression()); }
            if (ifControlContext.body().Length != 0)
            {
                foreach (BogieLangParser.BodyContext context in ifControlContext.body())
                {
                    result.Body.Add(Runtime.Body.Compile(context));
                }
            }

            return result;
        }
    }
}
