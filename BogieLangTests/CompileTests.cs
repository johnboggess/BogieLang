using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Antlr4.Runtime;

using BogieLang;
using BogieLang.Runtime;
namespace BogieLangTests
{
    class CompileTests
    {
        [SetUp]
        public void Setup()
        {
        }

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
            Assert.True(literal.Real == null);
            Assert.True(literal.Integer == 1);
            Assert.True(literal.Bool == null);
            Assert.True(literal.String == null);


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
            Assert.True(literal.Real == 1.0);
            Assert.True(literal.Integer == null);
            Assert.True(literal.Bool == null);
            Assert.True(literal.String == null);


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
            Assert.True(literal.Real == null);
            Assert.True(literal.Integer == null);
            Assert.True(literal.Bool == false);
            Assert.True(literal.String == null);


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
            Assert.True(literal.Real == null);
            Assert.True(literal.Integer == null);
            Assert.True(literal.Bool == null);
            Assert.True(literal.String == "asd8 99asd");
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
            Assert.True(expression.Identifier == null);
            Assert.True(expression.Literal.Integer == 1);
            Assert.True(expression.FunctionCall == null);
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);


            txt = "1.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Identifier == null);
            Assert.True(expression.Literal.Real == 1.0);
            Assert.True(expression.FunctionCall == null);
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);


            txt = "false";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Identifier == null);
            Assert.True(expression.Literal.Bool == false);
            Assert.True(expression.FunctionCall == null);
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);


            txt = "\"asd899asd\"";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Identifier == null);
            Assert.True(expression.Literal.String == "asd899asd");
            Assert.True(expression.FunctionCall == null);
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);


            txt = "VarName";
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
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);


            txt = "VarName + 1+ true*0";
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
            Assert.True(expression.SubExpression.SubExpression.SubExpression.Literal.Integer == 0);


            txt = "funcCall()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext);
            Assert.True(expression.Identifier == null);
            Assert.True(expression.Literal == null);
            Assert.True(expression.FunctionCall.Identifier == "funcCall");
            Assert.True(expression.Operator == null);
            Assert.True(expression.SubExpression == null);
        }

        [Test]
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
        }

        [Test]
        public void VarDefinitionTests()
        {
            string txt = "var=123";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.VarDefinitionContext varDefinitionContext = parser.varDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDefinitionContext);
            VarDefinition varDefinition = VarDefinition.Compile(varDefinitionContext);
            Assert.True(varDefinition.Identifier == "var");
            Assert.True(varDefinition.Expression.Literal.Integer == 123);


            txt = "var=funcCall(\"arg\")";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDefinitionContext = parser.varDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDefinitionContext);
            varDefinition = VarDefinition.Compile(varDefinitionContext);
            Assert.True(varDefinition.Identifier == "var");
            Assert.True(varDefinition.Expression.FunctionCall.Identifier == "funcCall");
        }
    }
}
