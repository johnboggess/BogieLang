using Antlr4.Runtime;
using BogieLang;
using NUnit.Framework;
namespace Tests
{
    public class Tests
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
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.LiteralContext literalContext = parser.literal();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(literalContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
        }

        [Test]
        public void ExpressionTests()
        {
            string txt = "1";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.ExpressionContext expressionContext = parser.expression();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "1.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "false";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "\"asd899asd\"";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "VarName";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "VarName + 1+ true*0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            expressionContext = parser.expression();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(expressionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void FunctionCallTests()
        {
            string txt = "funcCall()";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.FunctionCallContext functionCallContext = parser.functionCall();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionCallContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            Assert.True(parser.NumberOfSyntaxErrors == 0);

            
            txt = " funcCall\t(10.0\n,\rfuncCall2() \n\r\t)";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            functionCallContext = parser.functionCall();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(functionCallContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void VarDefinitionTests()
        {
            string txt = "var=123";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.VarDefinitionContext VarDefinitionContext = parser.varDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "var=funcCall(\"arg\")";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            VarDefinitionContext = parser.varDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = " \t     \nvar \t     \n= \t     \nfuncCall(123) \t     \n";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            VarDefinitionContext = parser.varDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void VarDeclarationTests()
        {
            string txt = "int var";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.VarDeclarationContext VarDeclarationContext = parser.varDeclaration();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDeclarationContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "int var=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            VarDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDeclarationContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "int var=funcCall()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            VarDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDeclarationContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = " \t     \nint \t     \nvar \t     \n= \t     \nfuncCall() \t     \n";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            VarDeclarationContext = parser.varDeclaration();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(VarDeclarationContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void FunctionReturnTests()
        {
            string txt = "return abc";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.FunctionReturnContext FunctionReturnContext = parser.functionReturn();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionReturnContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "return 10.0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            FunctionReturnContext = parser.functionReturn();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionReturnContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);

            txt = " \t     \nreturn \t     \nabc \t     \n";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            FunctionReturnContext = parser.functionReturn();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionReturnContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void BodyTests()
        {
            string txt = "int b=true";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.BodyContext BodyContext = parser.body();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(BodyContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "abc=123";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(BodyContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "func()";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(BodyContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "return 0";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(BodyContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "if(1){int b}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BodyContext = parser.body();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(BodyContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void IfTests()
        {
            string txt = "if(true){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.IfControlContext IfControlContext = parser.ifControl();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(IfControlContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "if(true){int b=0}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            IfControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(IfControlContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            IfControlContext = parser.ifControl();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(IfControlContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void FunctionDefinitionTests()
        {
            string txt = "void funcName(){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.FunctionDefinitionContext FunctionDefinitionContext = parser.functionDefinition();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "void funcName(int abc,string str,void lol){}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            FunctionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "void funcName(int abc,string str,void lol){int intvar" +
                "\nint intvar=123" +
                "\nintvar=0.1" +
                "\nfuncCall(lol)" +
                "\nreturn funcCall()" +
                "\n}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            FunctionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = @"
    void  funcName      (int abc,
                    string str,  void lol)
{
    int intvar
    int intvar=123
    intvar=0.1
    funcCall(lol)
    return funcCall()
}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            FunctionDefinitionContext = parser.functionDefinition();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(FunctionDefinitionContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }

        [Test]
        public void ProgramTests()
        {
            string txt = "void funcName(){}";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            BogieLangParser.ProgramContext ProgramContext = parser.program();
            BogieLangBaseVisitor<object> visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ProgramContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


            txt = "void funcName(int abc,string str,void lol){}";
            inputStream = new AntlrInputStream(txt);
            lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            ProgramContext = parser.program();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ProgramContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);


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
            lexer.AddErrorListener(new ParserErrorHandler<int>());
            commonTokenStream = new CommonTokenStream(lexer);
            parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            ProgramContext = parser.program();
            visitor = new BogieLangBaseVisitor<object>();
            visitor.Visit(ProgramContext);
            Assert.True(parser.NumberOfSyntaxErrors == 0);
        }
    }
}