using System;
using System.Collections.Generic;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace BogieLang
{
    public class ParserErrorHandler<T> : IAntlrErrorListener<T>
    {
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] T offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw e;
        }
    }
}
