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

        [Test]
        public void VarDeclarationTests()
        {
            string txt = "int var";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.VarDeclarationContext varDeclarationContext = parser.varDeclaration();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            VarDeclaration varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            Assert.True(varDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(varDeclaration.Identifier == "var");


            txt = "int var=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            Assert.True(varDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(varDeclaration.Identifier == "var");
            Assert.True(varDeclaration.Expression.Literal.Integer == 123);


            txt = "int var=funcCall()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            Assert.True(varDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(varDeclaration.Identifier == "var");
            Assert.True(varDeclaration.Expression.FunctionCall.Identifier == "funcCall");
        }

        [Test]
        public void FunctionReturnTests()
        {
            string txt = "return abc";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.FunctionReturnContext functionReturnContext = parser.functionReturn();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionReturnContext);
            FunctionReturn functionReturn = FunctionReturn.Compile(functionReturnContext);
            Assert.True(functionReturn.Expression.Identifier == "abc");


            txt = "return 10.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionReturnContext = parser.functionReturn();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionReturnContext);
            functionReturn = FunctionReturn.Compile(functionReturnContext);
            Assert.True(functionReturn.Expression.Literal.Real == 10.0);
        }

        [Test]
        public void BodyTests()
        {
            string txt = "int b=true";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.BodyContext bodyContext = parser.body();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            Body body = Body.Compile(bodyContext);
            Assert.True(body.VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(body.VarDeclaration.Identifier == "b");
            Assert.True(body.VarDeclaration.Expression.Literal.Bool == true);


            txt = "abc=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.VarDefinition.Identifier == "abc");
            Assert.True(body.VarDefinition.Expression.Literal.Integer == 123);


            txt = "func()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.FunctionCall.Identifier == "func");


            txt = "return 0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.FunctionReturn.Expression.Literal.Integer == 0);


            txt = "if(1){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.IfControl.Expression.Literal.Integer == 1);
            Assert.True(body.IfControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(body.IfControl.Body[0].VarDeclaration.Identifier == "b");


            txt = "while(1){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.WhileControl.Expression.Literal.Integer == 1);
            Assert.True(body.WhileControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(body.WhileControl.Body[0].VarDeclaration.Identifier == "b");
        }

        [Test]
        public void IfTests()
        {
            string txt = "if(true){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.IfControlContext ifControlContext = parser.ifControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            IfControl ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Expression.Literal.Bool == true);
            Assert.True(ifControl.Body.Count == 0);



            txt = "if(true){int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            ifControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Expression.Literal.Bool == true);
            Assert.True(ifControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(ifControl.Body[0].VarDeclaration.Identifier == "b");
            Assert.True(ifControl.Body[0].VarDeclaration.Expression.Literal.Integer == 0);


            txt = @"if(true)
{
    int b=0
    b = 1
    funcCall(b)
    if(false){return 1}
    return 0
}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            ifControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Expression.Literal.Bool == true);
            Assert.True(ifControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(ifControl.Body[0].VarDeclaration.Identifier == "b");
            Assert.True(ifControl.Body[0].VarDeclaration.Expression.Literal.Integer == 0);

            Assert.True(ifControl.Body[1].VarDefinition.Identifier == "b");
            Assert.True(ifControl.Body[1].VarDefinition.Expression.Literal.Integer == 1);

            Assert.True(ifControl.Body[2].FunctionCall.Identifier == "funcCall");
            Assert.True(ifControl.Body[2].FunctionCall.Arguments[0].Identifier == "b");

            Assert.True(ifControl.Body[3].IfControl.Expression.Literal.Bool == false);
            Assert.True(ifControl.Body[3].IfControl.Body[0].FunctionReturn.Expression.Literal.Integer == 1);

            Assert.True(ifControl.Body[4].FunctionReturn.Expression.Literal.Integer == 0);
        }
        
        [Test]
        public void WhileTests()
        {
            string txt = "while(true){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.WhileControlContext whileControlContext = parser.whileControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(whileControlContext);
            WhileControl whileControl = WhileControl.Compile(whileControlContext);
            Assert.True(whileControl.Expression.Literal.Bool == true);
            Assert.True(whileControl.Body.Count == 0);


            txt = "while(true){int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            whileControlContext = parser.whileControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(whileControlContext);
            whileControl = WhileControl.Compile(whileControlContext);
            Assert.True(whileControl.Expression.Literal.Bool == true);
            Assert.True(whileControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(whileControl.Body[0].VarDeclaration.Identifier == "b");
            Assert.True(whileControl.Body[0].VarDeclaration.Expression.Literal.Integer == 0);


            txt = @"while(true)
{
    int b=0
    b = 1
    funcCall(b)
    while(false){return 1}
    return 0
}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            whileControlContext = parser.whileControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(whileControlContext);
            whileControl = WhileControl.Compile(whileControlContext);
            Assert.True(whileControl.Expression.Literal.Bool == true);
            Assert.True(whileControl.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(whileControl.Body[0].VarDeclaration.Identifier == "b");
            Assert.True(whileControl.Body[0].VarDeclaration.Expression.Literal.Integer == 0);

            Assert.True(whileControl.Body[1].VarDefinition.Identifier == "b");
            Assert.True(whileControl.Body[1].VarDefinition.Expression.Literal.Integer == 1);

            Assert.True(whileControl.Body[2].FunctionCall.Identifier == "funcCall");
            Assert.True(whileControl.Body[2].FunctionCall.Arguments[0].Identifier == "b");

            Assert.True(whileControl.Body[3].WhileControl.Expression.Literal.Bool == false);
            Assert.True(whileControl.Body[3].WhileControl.Body[0].FunctionReturn.Expression.Literal.Integer == 1);

            Assert.True(whileControl.Body[4].FunctionReturn.Expression.Literal.Integer == 0);
        }

        [Test]
        public void FunctionDefinitionTests()
        {
            string txt = "void funcName(){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.FunctionDefinitionContext functionDefinitionContext = parser.functionDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            FunctionDefinition functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
            Assert.True(functionDefinition.Identifier == "funcName");
            Assert.True(functionDefinition.ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(functionDefinition.Parameters.Count == 0);
            Assert.True(functionDefinition.Body.Count == 0);


            txt = "void funcName(int abc,string str,void lol){}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
            Assert.True(functionDefinition.Identifier == "funcName");
            Assert.True(functionDefinition.ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(functionDefinition.Parameters[0].Item1 == BogieLangType.INTEGER);
            Assert.True(functionDefinition.Parameters[0].Item2 == "abc");
            Assert.True(functionDefinition.Parameters[1].Item1 == BogieLangType.STRING);
            Assert.True(functionDefinition.Parameters[1].Item2 == "str");
            Assert.True(functionDefinition.Parameters[2].Item1 == BogieLangType.VOID);
            Assert.True(functionDefinition.Parameters[2].Item2 == "lol");
            Assert.True(functionDefinition.Body.Count == 0);


            txt = "void funcName(int abc,string str,void lol){int intvar" +
                "\nint intvar=123" +
                "\nintvar=0.1" +
                "\nfuncCall(lol)" +
                "\nreturn funcCall()" +
                "\n}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
            Assert.True(functionDefinition.Identifier == "funcName");
            Assert.True(functionDefinition.ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(functionDefinition.Parameters[0].Item1 == BogieLangType.INTEGER);
            Assert.True(functionDefinition.Parameters[0].Item2 == "abc");
            Assert.True(functionDefinition.Parameters[1].Item1 == BogieLangType.STRING);
            Assert.True(functionDefinition.Parameters[1].Item2 == "str");
            Assert.True(functionDefinition.Parameters[2].Item1 == BogieLangType.VOID);
            Assert.True(functionDefinition.Parameters[2].Item2 == "lol");

            Assert.True(functionDefinition.Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(functionDefinition.Body[0].VarDeclaration.Identifier == "intvar");
            Assert.True(functionDefinition.Body[1].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(functionDefinition.Body[1].VarDeclaration.Identifier == "intvar");
            Assert.True(functionDefinition.Body[1].VarDeclaration.Expression.Literal.Integer == 123);
            Assert.True(functionDefinition.Body[2].VarDefinition.Identifier == "intvar");
            Assert.True(functionDefinition.Body[2].VarDefinition.Expression.Literal.Real == 0.1);
            Assert.True(functionDefinition.Body[3].FunctionCall.Identifier == "funcCall");
            Assert.True(functionDefinition.Body[3].FunctionCall.Arguments[0].Identifier == "lol");
            Assert.True(functionDefinition.Body[4].FunctionReturn.Expression.FunctionCall.Identifier == "funcCall");
        }

        [Test]
        public void ProgramTests()
        {
            string txt = "void funcName(){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.ProgramContext programContext = parser.program();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(programContext);
            Program program = Program.Compile(programContext);
            Assert.True(program.Functions[0].Identifier == "funcName");
            Assert.True(program.Functions[0].ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(program.Functions[0].Parameters.Count == 0);


            txt = "void funcName(int abc,string str,void lol){}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            programContext = parser.program();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(programContext);
            program = Program.Compile(programContext);
            Assert.True(program.Functions[0].Identifier == "funcName");
            Assert.True(program.Functions[0].ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(program.Functions[0].Parameters[0].Item1 == BogieLangType.INTEGER);
            Assert.True(program.Functions[0].Parameters[0].Item2 == "abc");
            Assert.True(program.Functions[0].Parameters[1].Item1 == BogieLangType.STRING);
            Assert.True(program.Functions[0].Parameters[1].Item2 == "str");
            Assert.True(program.Functions[0].Parameters[2].Item1 == BogieLangType.VOID);
            Assert.True(program.Functions[0].Parameters[2].Item2 == "lol");
            Assert.True(program.Functions[0].Body.Count == 0);


            txt = "void funcName(){}" +
                "void funcName(int abc,string str,void lol){}" +
                "void funcName(int abc,string str,void lol){int intvar" +
                "\nint intvar=123" +
                "\nintvar=0.1" +
                "\nfuncCall(lol)" +
                "\nreturn funcCall()" +
                "\n}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            programContext = parser.program();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(programContext);
            program = Program.Compile(programContext);
            Assert.True(program.Functions[0].Identifier == "funcName");
            Assert.True(program.Functions[0].ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(program.Functions[0].Parameters.Count == 0);

            Assert.True(program.Functions[1].Identifier == "funcName");
            Assert.True(program.Functions[1].ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(program.Functions[1].Parameters[0].Item1 == BogieLangType.INTEGER);
            Assert.True(program.Functions[1].Parameters[0].Item2 == "abc");
            Assert.True(program.Functions[1].Parameters[1].Item1 == BogieLangType.STRING);
            Assert.True(program.Functions[1].Parameters[1].Item2 == "str");
            Assert.True(program.Functions[1].Parameters[2].Item1 == BogieLangType.VOID);
            Assert.True(program.Functions[1].Parameters[2].Item2 == "lol");
            Assert.True(program.Functions[1].Body.Count == 0);

            Assert.True(program.Functions[2].Identifier == "funcName");
            Assert.True(program.Functions[2].ReturnBogieLangType == BogieLangType.VOID);
            Assert.True(program.Functions[2].Parameters[0].Item1 == BogieLangType.INTEGER);
            Assert.True(program.Functions[2].Parameters[0].Item2 == "abc");
            Assert.True(program.Functions[2].Parameters[1].Item1 == BogieLangType.STRING);
            Assert.True(program.Functions[2].Parameters[1].Item2 == "str");
            Assert.True(program.Functions[2].Parameters[2].Item1 == BogieLangType.VOID);
            Assert.True(program.Functions[2].Parameters[2].Item2 == "lol");

            Assert.True(program.Functions[2].Body[0].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(program.Functions[2].Body[0].VarDeclaration.Identifier == "intvar");
            Assert.True(program.Functions[2].Body[1].VarDeclaration.BogieLangType == BogieLangType.INTEGER);
            Assert.True(program.Functions[2].Body[1].VarDeclaration.Identifier == "intvar");
            Assert.True(program.Functions[2].Body[1].VarDeclaration.Expression.Literal.Integer == 123);
            Assert.True(program.Functions[2].Body[2].VarDefinition.Identifier == "intvar");
            Assert.True(program.Functions[2].Body[2].VarDefinition.Expression.Literal.Real == 0.1);
            Assert.True(program.Functions[2].Body[3].FunctionCall.Identifier == "funcCall");
            Assert.True(program.Functions[2].Body[3].FunctionCall.Arguments[0].Identifier == "lol");
            Assert.True(program.Functions[2].Body[4].FunctionReturn.Expression.FunctionCall.Identifier == "funcCall");
        }
    }
}
