using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lexer
{

    public class LexerException : System.Exception
    {
        public LexerException(string msg)
            : base(msg)
        {
        }

    }

    public class Lexer
    {

        protected int position;
        protected char currentCh; // очередной считанный символ
        protected int currentCharValue; // целое значение очередного считанного символа
        protected System.IO.StringReader inputReader;
        protected string inputString;

        public Lexer(string input)
        {
            inputReader = new System.IO.StringReader(input);
            inputString = input;
        }

        public void Error()
        {
            System.Text.StringBuilder o = new System.Text.StringBuilder();
            o.Append(inputString + '\n');
            o.Append(new System.String(' ', position - 1) + "^\n");
            o.AppendFormat("Error in symbol {0}", currentCh);
            throw new LexerException(o.ToString());
        }

        protected void NextCh()
        {
            this.currentCharValue = this.inputReader.Read();
            this.currentCh = (char) currentCharValue;
            this.position += 1;
        }

        public virtual bool Parse()
        {
            return true;
        }
    }

    public class IntLexer : Lexer
    {

        protected System.Text.StringBuilder intString;
        public int parseResult = 0;

        public IntLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override bool Parse()
        {
            NextCh();
            
            if (currentCh == '+' || currentCh == '-')
            {
                intString.Append(currentCh);
                NextCh();
            }
        
            if (char.IsDigit(currentCh))
            {
                intString.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                intString.Append(currentCh);
                NextCh();
            }


            if (currentCharValue != -1)
            {
                Error();
            }
            Int32.TryParse(intString.ToString(),out parseResult);
            return true;

        }
    }
    
    public class IdentLexer : Lexer
    {
        private string parseResult;
        protected StringBuilder builder;
    
        public string ParseResult
        {
            get { return parseResult; }
        }
    
        public IdentLexer(string input) : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            NextCh();

            if (currentCharValue == -1)
            {
                Error();
            }

            while (currentCh != '.' & currentCh != '$' & (char.IsLetterOrDigit(currentCh) || currentCh=='_'))
            {
                builder.Append(currentCh);
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            parseResult = builder.ToString();
            
            return true;
        }
       
    }

    public class IntNoZeroLexer : IntLexer
    {
        public IntNoZeroLexer(string input)
            : base(input)
        {
        }

        public override bool Parse()
        {
            NextCh();

            if (currentCh == '+' || currentCh == '-')
            {
                intString.Append(currentCh);
                NextCh();
            }

            if (char.IsDigit(currentCh) & currentCh!='0')
            {
                intString.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                intString.Append(currentCh);
                NextCh();
            }


            if (currentCharValue != -1)
            {
                Error();
            }
            Int32.TryParse(intString.ToString(), out parseResult);
            return true;
        }
    }

    public class LetterDigitLexer : Lexer
    {
        protected StringBuilder builder;
        protected string parseResult;

        public string ParseResult
        {
            get { return parseResult; }
        }

        public LetterDigitLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            NextCh();
            if (char.IsLetter(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            } else
            {
                Error();
            }
            while ((char.IsLetter(builder[builder.Length-1]) & char.IsDigit(currentCh))|| (char.IsDigit(builder[builder.Length - 1]) & char.IsLetter(currentCh)))
            {
                builder.Append(currentCh);
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            parseResult = builder.ToString();

            return true;
        }
       
    }

    public class LetterListLexer : Lexer
    {
        protected List<char> parseResult;

        public List<char> ParseResult
        {
            get { return parseResult; }
        }

        public LetterListLexer(string input)
            : base(input)
        {
            parseResult = new List<char>();
        }

        public override bool Parse()
        {
            NextCh();

            if (char.IsLetter(currentCh))
            {
                parseResult.Add(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsLetter(currentCh) || currentCh == ',' || currentCh == ';')
            {
                
                if (currentCh == ',' || currentCh == ';')
                {
                    NextCh();
                }
                else
                {
                    Error();
                }

                if (char.IsLetter(currentCh))
                {
                    parseResult.Add(currentCh);
                    NextCh();
                }
                else
                {
                    Error();
                }
            }
            
            if (currentCharValue != -1)
            {
                Error();
            }

            Console.WriteLine(parseResult);

            return true;
        }
    }

    public class DigitListLexer : Lexer
    {
        protected List<int> parseResult;

        public List<int> ParseResult
        {
            get { return parseResult; }
        }

        public DigitListLexer(string input)
            : base(input)
        {
            parseResult = new List<int>();
        }

        public override bool Parse()
        {
            NextCh();

            if (char.IsDigit(currentCh))
            {
                parseResult.Add(Int32.Parse(currentCh.ToString()));
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh) || char.IsWhiteSpace(currentCh))
            {
                if (char.IsWhiteSpace(currentCh))
                {
                    NextCh();
                }
                else
                {
                    Error();
                }

                while (char.IsWhiteSpace(currentCh))
                {
                    NextCh();
                }

                if (char.IsDigit(currentCh))
                {
                    parseResult.Add(Int32.Parse(currentCh.ToString()));
                    NextCh();
                }
                else
                {
                    Error();
                }
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            Console.WriteLine(parseResult);

            return true;
        }
    }

    public class LetterDigitGroupLexer : Lexer
    {
        protected StringBuilder builder;
        protected string parseResult;

        public string ParseResult
        {
            get { return parseResult; }
        }
        
        public LetterDigitGroupLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            throw new NotImplementedException();
        }
       
    }

    public class DoubleLexer : Lexer
    {
        private StringBuilder builder;
        private double parseResult;

        public double ParseResult
        {
            get { return parseResult; }

        }

        public DoubleLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            NextCh();

            if (char.IsDigit(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }

            if (currentCh == '.')
            {
                builder.Append(currentCh);
                NextCh();
            } else
            {
                Error();
            }

            if (char.IsDigit(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            double.TryParse(builder.ToString(),out parseResult);

            return true;
        }
       
    }

    public class StringLexer : Lexer
    {
        private StringBuilder builder;
        private string parseResult;

        public string ParseResult
        {
            get { return parseResult; }

        }

        public StringLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            throw new NotImplementedException();
        }
    }

    public class CommentLexer : Lexer
    {
        private StringBuilder builder;
        private string parseResult;

        public string ParseResult
        {
            get { return parseResult; }

        }

        public CommentLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
        }

        public override bool Parse()
        {
            throw new NotImplementedException();
        }
    }

    public class IdentChainLexer : Lexer
    {
        private StringBuilder builder;
        private List<string> parseResult;

        public List<string> ParseResult
        {
            get { return parseResult; }

        }

        public IdentChainLexer(string input)
            : base(input)
        {
            builder = new StringBuilder();
            parseResult = new List<string>();
        }

        public override bool Parse()
        {
            throw new NotImplementedException();
        }
    }

    public class Program
    {
        public static void Main()
        {
            string input = "154216";
            Lexer L = new IntLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }

        }
    }
}