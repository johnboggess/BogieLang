using System;
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
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

            string txt = "1";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.ExpressionContext expressionContext = parser.expression();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Expression expression = Expression.Compile(expressionContext, null);
            Assert.True(expression.Execute(functionEnvironment, new VariableEnvironment()) is int);
            Assert.True(expression.ParentExpression == null);
            Assert.True((int)expression.Execute(functionEnvironment, new VariableEnvironment()) == 1);


            txt = "1.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext, null);
            Assert.True(expression.Execute(functionEnvironment, new VariableEnvironment()) is double);
            Assert.True(expression.ParentExpression == null);
            Assert.True((double)expression.Execute(functionEnvironment, new VariableEnvironment()) == 1.0);


            txt = "false";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext, null);
            Assert.True(expression.Execute(functionEnvironment, new VariableEnvironment()) is bool);
            Assert.True(expression.ParentExpression == null);
            Assert.True((bool)expression.Execute(functionEnvironment, new VariableEnvironment()) == false);


            txt = "\"asd899asd\"";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext, null);
            Assert.True(expression.Execute(functionEnvironment, new VariableEnvironment()) is string);
            Assert.True((string)expression.Execute(functionEnvironment, new VariableEnvironment()) == "asd899asd");


            txt = "VarName";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            expression = Expression.Compile(expressionContext, null);
            variableEnvironment.DeclareVariable(txt, BogieLangType.INTEGER);
            variableEnvironment.DefineVariable(txt, 1);
            Assert.True(expression.Execute(functionEnvironment, variableEnvironment) is int);
            Assert.True(expression.ParentExpression == null);
            Assert.True((int)expression.Execute(functionEnvironment, variableEnvironment) == 1);
        }

        [Test]
        public void VarDefinitionTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

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
            varDefinition.Execute(functionEnvironment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["var"].Value is int);
            Assert.True((int)variables["var"].Value == 123);
        }


        [Test]
        public void VarDeclarationTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

            string txt = "int var";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.VarDeclarationContext varDeclarationContext = parser.varDeclaration();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(varDeclarationContext);
            VarDeclaration varDeclaration = VarDeclaration.Compile(varDeclarationContext);
            varDeclaration.Execute(functionEnvironment, variables);
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
            varDeclaration.Execute(functionEnvironment, variables);
            Assert.True(variables["var"].BogieLangType == BogieLangType.INTEGER);
            Assert.True((int)variables["var"].Value == 123);
            Assert.True(variables["var"].Identifer == "var");
        }

        [Test]
        public void FunctionReturnTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();
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
            Assert.True((bool)functionReturn.Execute(functionEnvironment, variables) == true);

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
            Assert.True((double)functionReturn.Execute(functionEnvironment, variables) == 10.0);
        }

        [Test]
        public void BodyTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

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
                body.Execute(functionEnvironment, variables);
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
            body.Execute(functionEnvironment, variables);
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
                Assert.True(body.Execute(functionEnvironment, variables) == null);
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
            body.Execute(functionEnvironment, variables);
            Assert.True(variables["abc"].BogieLangType == BogieLangType.INTEGER);
            Assert.True(variables["abc"].Identifer == "abc");
            Assert.True((int)variables["abc"].Value == 123);


            txt = "return 0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            Assert.True(body.Execute(functionEnvironment, variables) is int);
            Assert.True((int)body.Execute(functionEnvironment, variables) == 0);

            txt = "if(1){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(functionEnvironment, variables);

            txt = "if(true){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            bodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(bodyContext);
            body = Body.Compile(bodyContext);
            body.Execute(functionEnvironment, variables);
        }

        [Test]
        public void IfTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

            string txt = "if(true){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.IfControlContext ifControlContext = parser.ifControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            IfControl ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Execute(functionEnvironment, variables) == null);



            txt = "if(true){int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            ifControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ifControlContext);
            ifControl = IfControl.Compile(ifControlContext);
            Assert.True(ifControl.Execute(functionEnvironment, variables) == null);
            Assert.True(!variables.IsVariableDeclared("b"));
        }

        [Test]
        public void WhileTests()
        {
            VariableEnvironment variables = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();
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
            Assert.True(whileControl.Execute(functionEnvironment, variables) == null);
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
            Assert.True(whileControl.Execute(functionEnvironment, variables) == null);
            Assert.True((bool)variables["loop"].Value == false);
            Assert.True(!variables.IsVariableDeclared("b"));
            
        }
        
        [Test]
        public void FunctionDefinitionTests()
        {
            VariableEnvironment variableEnvironment = new VariableEnvironment();
            FunctionEnvironment functionEnvironment = new FunctionEnvironment();

            string txt = "void funcName(){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.FunctionDefinitionContext functionDefinitionContext = parser.functionDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            FunctionDefinition functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
            functionEnvironment.FunctionDefinitions.Add("funcName", functionDefinition);
            functionDefinition.Execute(functionEnvironment, variableEnvironment);

            functionEnvironment.Clear();
            variableEnvironment.DeclaredVariables = new Dictionary<string, BogieLangTypeInstance>()
            {
                { "abc", new BogieLangTypeInstance() { BogieLangType = BogieLangType.INTEGER, Identifer="abc", Value = 1 } },
                { "str", new BogieLangTypeInstance() { BogieLangType = BogieLangType.STRING, Identifer="str", Value = "string" } },
                { "lol", new BogieLangTypeInstance() { BogieLangType = BogieLangType.VOID, Identifer="lol", Value = null } }
            };
            txt = "void funcName(int abc,string str,void lol){}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            functionDefinition = FunctionDefinition.Compile(functionDefinitionContext);
            functionEnvironment.FunctionDefinitions.Add("funcName", functionDefinition);
            functionDefinition.Execute(functionEnvironment, variableEnvironment);
            
            txt = "int funcName(int abc,string str,bool b){int intvar" +
                "\n intvar=123" +
                "\nintvar=1" +
                "\nreturn funcCall(b)" +
                "\n}";
            
            string txt2 = "\nint funcCall(bool b)" +
                "\n{" +
                "\nif(b)" +
                "\n{" +
                "\n return 100" +
                "\n}" +
                "\nreturn 0" +
                "}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            FunctionDefinition functionDefinition1 = FunctionDefinition.Compile(functionDefinitionContext);

            inputStream = new AntlrInputStream(txt2);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            functionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionDefinitionContext);
            FunctionDefinition functionDefinition2 = FunctionDefinition.Compile(functionDefinitionContext);

            variableEnvironment.Clear();
            functionEnvironment.Clear();

            variableEnvironment.DeclaredVariables = new Dictionary<string, BogieLangTypeInstance>()
            {
                { "abc", new BogieLangTypeInstance() { BogieLangType = BogieLangType.INTEGER, Identifer="abc", Value = 1 } },
                { "str", new BogieLangTypeInstance() { BogieLangType = BogieLangType.STRING, Identifer="str", Value = "string" } },
                { "b", new BogieLangTypeInstance() { BogieLangType = BogieLangType.BOOL, Identifer="b", Value = true } }
            };
            functionEnvironment.DefineFunction("funcName", functionDefinition1);
            functionEnvironment.DefineFunction("funcCall", functionDefinition2);
            Assert.True((int)functionDefinition1.Execute(functionEnvironment, variableEnvironment) == 100);
        }

        [Test]
        public void ProgramTests()
        {
            string txt = @"
bool Main()
{
    return func(true)
}

bool func(bool b)
{
    if(b){return false}
    return true
}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            BogieLangParser.ProgramContext programContext = parser.program();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(programContext);
            Program program = Program.Compile(programContext);

            Assert.True((bool)program.Execute() == false);


            txt = @"
int Main()
{
    return fibonacci(8)
}

int fibonacci(int n)
{
    if(n==0){return 0}
    if(n==1){return 1}
    return fibonacci(n-2)+fibonacci(n-1)
}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            programContext = parser.program();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(programContext);
            program = Program.Compile(programContext);
            Assert.True((int)program.Execute() == 21);
        }
    }
}
