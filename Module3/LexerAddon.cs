using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;
using System.Collections.Generic;

namespace  GeneratedLexer
{
    
    public class LexerAddon
    {
        public Scanner myScanner;
        private byte[] inputText = new byte[255];

        public int idCount = 0;
        public int minIdLength = Int32.MaxValue;
        public double avgIdLength = 0;
        public int maxIdLength = 0;
        public int sumInt = 0;
        public double sumDouble = 0.0;
        public List<string> idsInComment = new List<string>();
        

        public LexerAddon(string programText)
        {
            
            using (StreamWriter writer = new StreamWriter(new MemoryStream(inputText)))
            {
                writer.Write(programText);
                writer.Flush();
            }
            
            MemoryStream inputStream = new MemoryStream(inputText);
            
            myScanner = new Scanner(inputStream);
        }

        public void Lex()
        {
            // ????? ???????????? ????? ?????????????? ? ???????????? ? ??????? 3.14 (? ?? 3,14 ??? ? ??????? Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            int tok = 0;
            do {
                tok = myScanner.yylex();
                
                if (tok == (int)Tok.EOF)
                {
                    idCount = myScanner.LexIdCount;
                    minIdLength = myScanner.LexMinIdLength;
                    avgIdLength = myScanner.LexAvgIdLength/idCount;
                    maxIdLength = myScanner.LexMaxIdLength;
                    sumInt = myScanner.LexSumInt;
                    sumDouble = myScanner.LexSumDouble;
                    idsInComment = myScanner.LexIdsInComment;
                    break;
                }
            } while (true);
        }
    }
}

