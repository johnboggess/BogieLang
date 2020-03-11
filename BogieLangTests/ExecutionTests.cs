﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Antlr4.Runtime;

using BogieLang;
using BogieLang.Runtime;

namespace BogieLangTests
{
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
            VariableEnvironment variableEnvironment = new VariableEnvironment();

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
            variableEnvironment.DeclareVariable(txt, BogieLangType.INTEGER);
            variableEnvironment.DefineVariable(txt, 1);
            Assert.True(expression.Execute(environment, variableEnvironment) is int);
            Assert.True((int)expression.Execute(environment, variableEnvironment) == 1);

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

            variables.DeclareVariable("var", BogieLangType.INTEGER);
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


        [Test]
        public void VarDeclarationTests()
        {
            VariableEnvironment variables = new VariableEnvironment();

            string txt = "int var";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.VarDeclarationContext varDeclarationContext = parser.varDeclaration();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            VarDeclaration varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            varDeclaration.Execute(environment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["var"].Value == null);
            Assert.True(variables["var"].Identifer == "var");

            variables.Clear();
            txt = "int var=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            varDeclaration.Execute(environment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True((int)variables["var"].Value == 123);
            Assert.True(variables["var"].Identifer == "var");

            //todo: test funcCall after execution of funcDefinitions are possible
            /*txt = "int var=funcCall()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            varDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            varDeclaration.Execute(environment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True((int)variables["var"].Value == 123);
            Assert.True(variables["var"].Identifer == "var");*/
        }

        [Test]
        public void FunctionReturnTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            variables.DeclareVariable("abc", BogieLangType.BOOL);
            variables.DefineVariable("abc", true);

            string txt = "return abc";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.FunctionReturnContext functionReturnContext = parser.functionReturn();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionReturnContext);
            FunctionReturn functionReturn = FunctionReturn.Compile(functionReturnContext);
            Assert.True((bool)functionReturn.Execute(environment, variables) == true);

            variables.Clear();
            txt = "return 10.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionReturnContext = parser.functionReturn();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionReturnContext);
            functionReturn = FunctionReturn.Compile(functionReturnContext);
            Assert.True((double)functionReturn.Execute(environment, variables) == 10.0);
        }

        [Test]
        public void BodyTests()
        {
            VariableEnvironment variables = new VariableEnvironment();

            string txt = "int b=true";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.BodyContext bodyContext = parser.body();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            Body body = Body.Compile(bodyContext);
            try
            {
                body.Execute(environment, variables);
                Assert.True(false);
            }
            catch(Exception e)
            {
                Assert.True(true);//todo: test for a specific exception types once they are created
            }

            variables.Clear();
            txt = "int b=098";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(environment, variables);
            Assert.True(variables["b"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["b"].Identifer == "b");
            Assert.True((int)variables["b"].Value == 98);

            txt = "abc=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            try
            {
                Assert.True(body.Execute(environment, variables) == null);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);//todo: test for specific exception types once they are created
            }


            variables.DeclareVariable("abc", BogieLangType.INTEGER);
            txt = "abc=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(environment, variables);
            Assert.True(variables["abc"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["abc"].Identifer == "abc");
            Assert.True((int)variables["abc"].Value == 123);

            //todo: test funcCall after execution of funcDefinitions are possible
            /*txt = "func()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(environment, variables);
            Assert.True(variables["abc"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["abc"].Identifer == "abc");
            Assert.True((int)variables["abc"].Value == 123);*/


            txt = "return 0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.Execute(environment, variables) is int);
            Assert.True((int)body.Execute(environment, variables) == 0);

            txt = "if(1){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(environment, variables);

            txt = "if(true){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            try
            {
                body.Execute(environment, variables);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
            }

            //todo: test while after execution of whiles are possible
            /*txt = "while(1){int b}";
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
            Assert.True(body.WhileControl.Body[0].VarDeclaration.Identifier == "b");*/
        }

        [Test]
        public void IfTests()
        {
            VariableEnvironment variables = new VariableEnvironment();

            string txt = "if(true){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.IfControlContext ifControlContext = parser.ifControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            IfControl ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Execute(environment, variables) == null);



            txt = "if(true){int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            ifControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Execute(environment, variables) == null);
            Assert.True(variables["b"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["b"].Identifer == "b");
            Assert.True((int)variables["b"].Value == 0);

            
            //todo: test funcCall after execution of funcDefinitions are possible
/*            txt = @"if(true)
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
            ifControl.Execute(environment, variables);
            Assert.True(variables["b"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["b"].Identifer == "b");
            Assert.True((int)variables["b"].Value == 1);*/
        }

        [Test]
        public void WhileTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            variables.DeclareVariable("loop", BogieLangType.BOOL);
            variables.DefineVariable("loop", true);

            string txt = "while(loop){loop = false}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.WhileControlContext whileControlContext = parser.whileControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(whileControlContext);
            WhileControl whileControl = WhileControl.Compile(whileControlContext);
            Assert.True(whileControl.Execute(environment, variables) == null);
            Assert.True((bool)variables["loop"].Value == false);
            


            txt = " while(loop){loop = false int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            whileControlContext = parser.whileControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(whileControlContext);
            whileControl = WhileControl.Compile(whileControlContext);
            Assert.True(whileControl.Execute(environment, variables) == null);
            Assert.True((bool)variables["loop"].Value == false);
            Assert.True(!variables.IsVariableDeclared("b"));


            //todo: test funcCall after execution of funcDefinitions are possible
            /*txt = @"while(true)
{
    int b=0
    b = 1
    bool a = false
    funcCall(b)
    while(a){return 1}
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
            Assert.True((int)whileControl.Execute(environment, variables) == 0);*/
        }
    }
}
