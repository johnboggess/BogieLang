using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Antlr4.Runtime;

using BogieLang;
using BogieLang.Runtime;

namespace BogieLangTests
{
    using VariableEnvironment = Dictionary<string, BogieLangTypeInstance>;
    class ExecutionTests
    {
        RuntimeEnvironment environment = new RuntimeEnvironment();
        
        [Test]
        public void LiteralTests()
        {
            string txt = "1";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.LiteralContext literalContext = parser.literal();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(literalContext);
            Literal literal = Literal.Compile(literalContext);
            Assert.True(literal.Execute() is int);
            Assert.True((int)literal.Execute() == 1);


            txt = "1.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            literalContext = parser.literal();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(literalContext);
            literal = Literal.Compile(literalContext);
            Assert.True(literal.Execute() is double);
            Assert.True((double)literal.Execute() == 1.0);


            txt = "false";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            literalContext = parser.literal();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(literalContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
            literal = Literal.Compile(literalContext);
            Assert.True(literal.Execute() is bool);
            Assert.True((bool)literal.Execute() == false);


            txt = "\"asd8 99asd\"";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            literalContext = parser.literal();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(literalContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
            literal = Literal.Compile(literalContext);
            Assert.True(literal.Execute() is string);
            Assert.True((string)literal.Execute() == "asd8 99asd");
        }

        [Test]
        public void ExpressionTests()
        {
            string txt = "1";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.ExpressionContext expressionContext = parser.expression();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Expression expression = Expression.Compile(expressionContext);
            Assert.True(expression.Execute(environment, new VariableEnvironment()) is int);
            Assert.True((int)expression.Execute(environment, new VariableEnvironment()) == 1);


            txt = "1.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Execute(environment, new VariableEnvironment()) is double);
            Assert.True((double)expression.Execute(environment, new VariableEnvironment()) == 1.0);


            txt = "false";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Execute(environment, new VariableEnvironment()) is bool);
            Assert.True((bool)expression.Execute(environment, new VariableEnvironment()) == false);


            txt = "\"asd899asd\"";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Execute(environment, new VariableEnvironment()) is string);
            Assert.True((string)expression.Execute(environment, new VariableEnvironment()) == "asd899asd");


            txt = "VarName";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Execute(environment, new VariableEnvironment() { { txt, new BogieLangTypeInstance() { Value = 1, BogieLangType = BogieLangType.INTEGER } } }) is int);
            Assert.True((int)expression.Execute(environment, new VariableEnvironment() { { txt, new BogieLangTypeInstance() { Value = 1, BogieLangType = BogieLangType.INTEGER } } }) == 1);

            //todo: test operators when they are setup to be executed
            /*txt = "VarName + 1+ true*0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Identifier == "VarName");
            Assert.True(expression.Literal == null);
            Assert.True(expression.FunctionCall == null);
            Assert.True(expression.Operator == "+");
            Assert.True(expression.SubExpression.Literal.Integer == 1);
            Assert.True(expression.SubExpression.Operator == "+");
            Assert.True(expression.SubExpression.SubExpression.Literal.Bool == true);
            Assert.True(expression.SubExpression.SubExpression.Operator == "*");
            Assert.True(expression.SubExpression.SubExpression.SubExpression.Literal.Integer == 0);*/


            //todo: test funcCall after execution of funcDefinitions are possible
            /*txt = "funcCall()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);*/
        }

        //todo: setup once funcCalls can be executed
        /*[Test]
        public void FunctionCallTests()
        {
            string txt = "funcCall()";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.FunctionCallContext functionCallContext = parser.functionCall();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionCallContext);
            FunctionCall functionCall = FunctionCall.Compile(functionCallContext);
            Assert.True(functionCall.Identifier == "funcCall");
            Assert.True(functionCall.Arguments.Count == 0);


            txt = "funcCall(\"arg\")";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            functionCallContext = parser.functionCall();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionCallContext);
            functionCall = FunctionCall.Compile(functionCallContext);
            Assert.True(functionCall.Identifier == "funcCall");
            Assert.True(functionCall.Arguments[0].Literal.String == "arg");


            txt = "funcCall(10.0,funcCall2())";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            functionCallContext = parser.functionCall();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionCallContext);
            functionCall = FunctionCall.Compile(functionCallContext);
            Assert.True(functionCall.Identifier == "funcCall");
            Assert.True(functionCall.Arguments[0].Literal.Real == 10.0);
            Assert.True(functionCall.Arguments[1].FunctionCall.Identifier == "funcCall2");
        }*/

        [Test]
        public void VarDefinitionTests()
        {
            VariableEnvironment variables = new VariableEnvironment();

            string txt = "var=123";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.VarDefinitionContext varDefinitionContext = parser.varDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDefinitionContext);
            VarDefinition varDefinition = VarDefinition.Compile(varDefinitionContext);
            varDefinition.Execute(environment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["var"].Value is int);
            Assert.True((int)variables["var"].Value == 123);
            

            //todo: test funcCall after execution of funcDefinitions are possible
            /*txt = "var=funcCall(\"arg\")";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDefinitionContext = parser.varDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDefinitionContext);
            varDefinition = VarDefinition.Compile(varDefinitionContext);
            Assert.True(varDefinition.Identifier == "var");
            Assert.True(varDefinition.Expression.FunctionCall.Identifier == "funcCall");*/
        }
    }
}
