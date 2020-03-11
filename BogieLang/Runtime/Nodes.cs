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

        public object Execute()
        {
            if (Real != null) { return Real; }
            else if (Integer != null) { return Integer; }
            else if (Bool != null) { return Bool; }
            else if (String != null) { return String; }
            else { throw new Exception("Unknown literal"); }
        }

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

        public object Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            if (Identifier != null) { return variableEnvironment.GetVariableValue(Identifier); }
            else if (Literal != null) { return Literal.Execute(); }
            else if (FunctionCall != null) { return FunctionCall.Execute(); }
            else { throw new Exception("Unknown expression"); }
        }

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

        public object Execute()
        {
            throw new NotImplementedException();
        }

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

        public void Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            object value = Expression.Execute(runtimeEnvironment, variableEnvironment);
            variableEnvironment.DefineVariable(Identifier, value);
        }

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

        public void Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            object value = null;
            if(Expression != null) { value = Expression.Execute(runtimeEnvironment, variableEnvironment); }
            variableEnvironment.DeclareVariable(Identifier, (BogieLangType)BogieLangType);
            if (value != null)
            {
                variableEnvironment.DefineVariable(Identifier, value);
            }
        }

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

        public object Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            return Expression.Execute(runtimeEnvironment, variableEnvironment);
        }

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
        public WhileControl WhileControl;

        public object Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            if (VarDeclaration != null) { VarDeclaration.Execute(runtimeEnvironment, variableEnvironment); return null; }
            else if (VarDefinition != null) { VarDefinition.Execute(runtimeEnvironment, variableEnvironment); return null; }
            else if (FunctionCall != null) { FunctionCall.Execute(); return null; }
            else if (FunctionReturn != null) { return FunctionReturn.Execute(runtimeEnvironment, variableEnvironment); }
            else if (IfControl != null) { IfControl.Execute(runtimeEnvironment, variableEnvironment); return null; }
            else if (WhileControl != null) { WhileControl.Execute(runtimeEnvironment, variableEnvironment); return null; }
            else { throw new NotImplementedException(); }
        }

        public static Body Compile(BogieLangParser.BodyContext bodyContext)
        {
            Body result = new Body();
            if (bodyContext.varDeclaration() != null) { result.VarDeclaration = VarDeclaration.Compile(bodyContext.varDeclaration()); }
            else if (bodyContext.varDefinition() != null) { result.VarDefinition = VarDefinition.Compile(bodyContext.varDefinition()); }
            else if (bodyContext.functionCall() != null) { result.FunctionCall = FunctionCall.Compile(bodyContext.functionCall()); }
            else if (bodyContext.functionReturn() != null) { result.FunctionReturn = FunctionReturn.Compile(bodyContext.functionReturn()); }
            else if (bodyContext.ifControl() != null) { result.IfControl = IfControl.Compile(bodyContext.ifControl()); }
            else if (bodyContext.whileControl() != null) { result.WhileControl = WhileControl.Compile(bodyContext.whileControl()); }

            return result;
        }
    }

    public class IfControl
    {
        public Expression Expression;
        public List<Body> Body = new List<Body>();

        public object Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            object obj = Expression.Execute(runtimeEnvironment, variableEnvironment);
            VariableEnvironment localVariables = new VariableEnvironment();
            localVariables.ParentEnvironment = variableEnvironment;

            if(obj is bool && (bool)obj == true)
            {
                foreach(Body body in Body)
                {
                    object val = body.Execute(runtimeEnvironment, localVariables);
                    if (val != null) { return val; };
                }
            }
            return null;
        }

        public static IfControl Compile(BogieLangParser.IfControlContext ifControlContext)
        {
            IfControl result = new IfControl();
            if (ifControlContext.expression() != null) { result.Expression = Expression.Compile(ifControlContext.expression()); }
            if (ifControlContext.body().Length != 0)
            {
                foreach (BogieLangParser.BodyContext context in ifControlContext.body())
                {
                    result.Body.Add(Runtime.Body.Compile(context));//todo: handle funciton returns;
                }
            }

            return result;
        }
    }

    public class WhileControl
    {
        public Expression Expression;
        public List<Body> Body = new List<Body>();

        public object Execute(FunctionEnvironment runtimeEnvironment, VariableEnvironment variableEnvironment)
        {
            object obj = Expression.Execute(runtimeEnvironment, variableEnvironment);

            VariableEnvironment localVariables = new VariableEnvironment();
            localVariables.ParentEnvironment = variableEnvironment;
            while (obj is bool && (bool)obj == true)
            {
                foreach (Body body in Body)
                {
                    object val = body.Execute(runtimeEnvironment, localVariables);
                    if (val != null) { return val; };
                }
                obj = Expression.Execute(runtimeEnvironment, variableEnvironment);
                localVariables.Clear();
            }
            return null;
        }

        public static WhileControl Compile(BogieLangParser.WhileControlContext whileControlContext)
        {
            WhileControl result = new WhileControl();
            if (whileControlContext.expression() != null) { result.Expression = Expression.Compile(whileControlContext.expression()); }
            if (whileControlContext.body().Length != 0)
            {
                foreach (BogieLangParser.BodyContext context in whileControlContext.body())
                {
                    result.Body.Add(Runtime.Body.Compile(context));
                }
            }

            return result;
        }
    }

    public class FunctionDefinition
    {
        public BogieLangType? ReturnBogieLangType = null;
        public string Identifier = null;
        public List<Tuple<BogieLangType, string>> Parameters = new List<Tuple<BogieLangType, string>>();
        public List<Body> Body = new List<Body>();

        public object Execute(FunctionEnvironment environment, VariableEnvironment variableEnvironment)
        {
            foreach(Tuple<BogieLangType, string> param in Parameters)
            {
                if(!variableEnvironment.IsVariableDeclared(param.Item2))
                {
                    throw new Exception("Missing parameter: " + param.Item2);
                }
            }

            foreach(Body body in Body)
            {
                object obj = body.Execute(environment, variableEnvironment);
                if(obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        public static FunctionDefinition Compile(BogieLangParser.FunctionDefinitionContext functionDefinitionContext)
        {
            FunctionDefinition result = new FunctionDefinition();
            
            Antlr4.Runtime.Tree.ITerminalNode[] types = functionDefinitionContext.TYPE();
            Antlr4.Runtime.Tree.ITerminalNode[] identifiers = functionDefinitionContext.IDENTIFIER();

            result.ReturnBogieLangType = BogieLangTypeHelpr.StringToType(types[0].GetText());
            result.Identifier = identifiers[0].GetText();

            for (int i = 1; i < types.Length; i++)
            {
                BogieLangType bogieLangType = BogieLangTypeHelpr.StringToType(types[i].GetText());
                string identifier = identifiers[i].GetText();
                result.Parameters.Add(new Tuple<BogieLangType, string>(bogieLangType, identifier));
            }

            foreach (BogieLangParser.BodyContext context in functionDefinitionContext.body())
            {
                result.Body.Add(Runtime.Body.Compile(context));
            }

            return result;
        }
    }

    public class Program
    {
        public FunctionEnvironment FunctionEnvironment = new FunctionEnvironment();

        public object Execute(VariableEnvironment variableEnvironment)
        {
            throw new NotImplementedException();
        }

        public static Program Compile(BogieLangParser.ProgramContext programContext)
        {
            Program result = new Program();
            foreach(BogieLangParser.FunctionDefinitionContext functionDefinitionContext in programContext.functionDefinition())
            {
                FunctionDefinition functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
                result.FunctionEnvironment.DefineFunction(functionDefinition.Identifier, functionDefinition);
            }

            return result;
        }
    }
}
