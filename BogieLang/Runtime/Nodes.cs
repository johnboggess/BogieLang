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
        public Expression ParentExpression = null;

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            Expression rightExpression = _getRightmostExpression();
            return rightExpression._execute(functionEnvironment, variableEnvironment);
            /*if (SubExpression == null)
            {
                if (Identifier != null) { return variableEnvironment.GetVariableValue(Identifier); }
                else if (Literal != null) { return Literal.Execute(); }
                else if (FunctionCall != null) { return FunctionCall.Execute(functionEnvironment, variableEnvironment); }
                else { throw new Exception("Unknown expression"); }
            }
            else
            {
                object left;
                if (Identifier != null) { left = variableEnvironment.GetVariableValue(Identifier); }
                else if (Literal != null) { left = Literal.Execute(); }
                else if (FunctionCall != null) { left = FunctionCall.Execute(functionEnvironment, variableEnvironment); }
                else { throw new Exception("Unknown expression"); }

                object right = SubExpression.Execute(functionEnvironment, variableEnvironment);
                return Operators.OperatorHelper.Execute(Operator, left, right);
            }*/
        }

        private Expression _getRightmostExpression()
        {
            if(SubExpression != null) { return SubExpression._getRightmostExpression(); }
            return this;
        }

        private object _execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            if (ParentExpression == null)
            {
                if (Identifier != null) { return variableEnvironment.GetVariableValue(Identifier); }
                else if (Literal != null) { return Literal.Execute(); }
                else if (FunctionCall != null) { return FunctionCall.Execute(functionEnvironment, variableEnvironment); }
                else { throw new Exception("Unknown expression"); }
            }
            else
            {
                object right;
                if (Identifier != null) { right = variableEnvironment.GetVariableValue(Identifier); }
                else if (Literal != null) { right = Literal.Execute(); }
                else if (FunctionCall != null) { right = FunctionCall.Execute(functionEnvironment, variableEnvironment); }
                else { throw new Exception("Unknown expression"); }

                object left = ParentExpression._execute(functionEnvironment, variableEnvironment);
                return Operators.OperatorHelper.Execute(ParentExpression.Operator, left, right);
            }
        }

        public static Expression Compile(BogieLangParser.ExpressionContext expressionContext, Expression parentExpression)
        {
            Expression result = new Expression();
            result.ParentExpression = parentExpression;
            if (expressionContext.IDENTIFIER() != null) { result.Identifier = expressionContext.IDENTIFIER().GetText(); }
            else if (expressionContext.literal() != null) { result.Literal = Literal.Compile(expressionContext.literal()); }
            else if (expressionContext.functionCall() != null) { result.FunctionCall = FunctionCall.Compile(expressionContext.functionCall()); }

            if (expressionContext.OPERATOR() != null) { result.Operator = expressionContext.OPERATOR().GetText(); }
            if (expressionContext.expression() != null) { result.SubExpression = Expression.Compile(expressionContext.expression(), result); }

            return result;
        }
    }

    public class FunctionCall
    {
        public string Identifier = null;
        public List<Expression> Arguments = new List<Expression>();

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            if(!functionEnvironment.IsFunctionDefined(Identifier))
            {
                throw new Exception("Calling undefined function: " + Identifier);
            }
            VariableEnvironment localVariables = new VariableEnvironment();
            localVariables.ParentEnvironment = variableEnvironment;//todo: switch

            FunctionDefinition functionDefinition = functionEnvironment.GetDefinedFunction(Identifier);

            for(int i = 0; i < Arguments.Count; i++)
            {
                object obj = Arguments[i].Execute(functionEnvironment, variableEnvironment);
                BogieLangType bogieLangType = BogieLangTypeHelpr.ObjectToType(obj);
                if (functionDefinition.Parameters[i].Item1 == bogieLangType)
                {
                    localVariables.DeclareVariable(functionDefinition.Parameters[i].Item2, functionDefinition.Parameters[i].Item1);
                    localVariables.DefineVariable(functionDefinition.Parameters[i].Item2, obj);
                }
            }

            return functionDefinition.Execute(functionEnvironment, localVariables);
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
                    expressions.Add(Expression.Compile(expressionContext, null));
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

        public void Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            object value = Expression.Execute(functionEnvironment, variableEnvironment);
            variableEnvironment.DefineVariable(Identifier, value);
        }

        public static VarDefinition Compile(BogieLangParser.VarDefinitionContext varDefinitionContext)
        {
            VarDefinition result = new VarDefinition();
            if (varDefinitionContext.IDENTIFIER() != null) { result.Identifier = varDefinitionContext.IDENTIFIER().GetText(); }
            if (varDefinitionContext.expression() != null) { result.Expression = Expression.Compile(varDefinitionContext.expression(), null); }

            return result;
        }
    }

    public class VarDeclaration
    {
        public BogieLangType? BogieLangType = null;
        public string Identifier = null;
        public Expression Expression = null;

        public void Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            object value = null;
            if(Expression != null) { value = Expression.Execute(functionEnvironment, variableEnvironment); }
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
            if (varDeclarationContext.expression() != null) { result.Expression = Expression.Compile(varDeclarationContext.expression(), null); }

            return result;
        }
    }

    public class FunctionReturn
    {
        public Expression Expression = null;

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            return Expression.Execute(functionEnvironment, variableEnvironment);
        }

        public static FunctionReturn Compile(BogieLangParser.FunctionReturnContext functionReturnContext)
        {
            FunctionReturn result = new FunctionReturn();
            if (functionReturnContext.expression() != null) { result.Expression = Expression.Compile(functionReturnContext.expression(), null); }

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

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            if (VarDeclaration != null) { VarDeclaration.Execute(functionEnvironment, variableEnvironment); return null; }
            else if (VarDefinition != null) { VarDefinition.Execute(functionEnvironment, variableEnvironment); return null; }
            else if (FunctionCall != null) { return FunctionCall.Execute(functionEnvironment, variableEnvironment); }
            else if (FunctionReturn != null) { return FunctionReturn.Execute(functionEnvironment, variableEnvironment); }
            else if (IfControl != null) { return IfControl.Execute(functionEnvironment, variableEnvironment); }
            else if (WhileControl != null) { return WhileControl.Execute(functionEnvironment, variableEnvironment); }
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

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            object obj = Expression.Execute(functionEnvironment, variableEnvironment);
            VariableEnvironment localVariables = new VariableEnvironment();
            localVariables.ParentEnvironment = variableEnvironment;

            if(obj is bool && (bool)obj == true)
            {
                foreach(Body body in Body)
                {
                    object val = body.Execute(functionEnvironment, localVariables);
                    if (val != null) { return val; };
                }
            }
            return null;
        }

        public static IfControl Compile(BogieLangParser.IfControlContext ifControlContext)
        {
            IfControl result = new IfControl();
            if (ifControlContext.expression() != null) { result.Expression = Expression.Compile(ifControlContext.expression(), null); }
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

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
        {
            object obj = Expression.Execute(functionEnvironment, variableEnvironment);

            VariableEnvironment localVariables = new VariableEnvironment();
            localVariables.ParentEnvironment = variableEnvironment;
            while (obj is bool && (bool)obj == true)
            {
                foreach (Body body in Body)
                {
                    object val = body.Execute(functionEnvironment, localVariables);
                    if (val != null) { return val; };
                }
                obj = Expression.Execute(functionEnvironment, variableEnvironment);
                localVariables.Clear();
            }
            return null;
        }

        public static WhileControl Compile(BogieLangParser.WhileControlContext whileControlContext)
        {
            WhileControl result = new WhileControl();
            if (whileControlContext.expression() != null) { result.Expression = Expression.Compile(whileControlContext.expression(), null); }
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

        public object Execute(FunctionEnvironment functionEnvironment, VariableEnvironment variableEnvironment)
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
                object obj = body.Execute(functionEnvironment, variableEnvironment);
                if(obj != null)
                {
                    BogieLangType returningType = BogieLangTypeHelpr.ObjectToType(obj);
                    if (returningType == ReturnBogieLangType)
                    {
                        return obj;
                    }
                    else
                    {
                        throw new Exception("Function " + Identifier + " returned a " + returningType + ", but should return a " + ReturnBogieLangType);
                    }
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
        public VariableEnvironment VariableEnvironment = new VariableEnvironment();

        public object Execute()
        {
            if(FunctionEnvironment.IsFunctionDefined("Main"))
            {
                return FunctionEnvironment.GetDefinedFunction("Main").Execute(FunctionEnvironment, VariableEnvironment);
            }
            else
            {
                throw new Exception("Cannot find Main.");
            }
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
