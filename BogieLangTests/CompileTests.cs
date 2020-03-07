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
    }
}
