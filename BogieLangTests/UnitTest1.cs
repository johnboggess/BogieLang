using NUnit.Framework;
using Antlr4.Runtime;

using BogieLang;
namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            string txt = "";
            AntlrInputStream inputStream = new AntlrInputStream(txt);
            BogieLangLexer lexer = new BogieLangLexer(inputStream);
            lexer.AddErrorListener(new ParserErrorHandler<int>());

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BogieLangParser parser = new BogieLangParser(commonTokenStream);
            parser.AddErrorListener(new ParserErrorHandler<object>());
            
        }
    }
}