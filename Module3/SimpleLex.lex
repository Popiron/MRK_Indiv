%using ScannerHelper;
%namespace SimpleScanner

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}*
DotChr [^\r\n]
OneLineCmnt  \/\/{DotChr}*

// «десь можно делать описани€ типов, переменных и методов - они попадают в класс Scanner
%{
  public int LexValueInt;
  public double LexValueDouble;
  public int LexSumInt = 0;
  public double LexSumDouble = 0.0;
  public int LexIdCount = 0;
  public int LexMinIdLength = Int32.MaxValue;
  public int LexMaxIdLength = 0;
  public double LexAvgIdLength = 0;
  public List<string> LexIdsInComment = new List<string>();
%}

%x COMMENT

%%

"{" { 
  // переход в состо€ние COMMENT
  BEGIN(COMMENT);
}

<COMMENT> "}" { 
  // переход в состо€ние INITIAL
  BEGIN(INITIAL);
}

{INTNUM} { 
  LexValueInt = int.Parse(yytext);
  LexSumInt += LexValueInt;
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
  LexSumDouble += LexValueDouble;
  return (int)Tok.RNUM;
}

begin { 
  return (int)Tok.BEGIN;
}

end {
  LexAvgIdLength/=LexIdCount;
  return (int)Tok.END;
}

cycle { 
  return (int)Tok.CYCLE;
}

<COMMENT>begin { 
 
}

<COMMENT>end { 
  
}

<COMMENT>{ID} {
  LexIdsInComment.Add(yytext);
}

{ID}  {
  LexIdCount++;
  if (yyleng < LexMinIdLength)
  {
      LexMinIdLength = yyleng;
  }
  if (yyleng > LexMaxIdLength)
  {
      LexMaxIdLength = yyleng;
  }
  LexAvgIdLength += yyleng;
  return (int)Tok.ID;
}

":" { 
  return (int)Tok.COLON;
}

":=" { 
  return (int)Tok.ASSIGN;
}

";" { 
  return (int)Tok.SEMICOLON;
}

[^ \r\n] {
	LexError();
	return 0; // конец разбора
}

{OneLineCmnt} {
  return (int)Tok.COMMENT;
}

%%

// «десь можно делать описани€ переменных и методов - они тоже попадают в класс Scanner

public void LexError()
{
	Console.WriteLine("({0},{1}): Ќеизвестный символ {2}", yyline, yycol, yytext);
}

public string TokToString(Tok tok)
{
	switch (tok)
	{
		case Tok.ID:
			return tok + " " + yytext;
		case Tok.INUM:
			return tok + " " + LexValueInt;
		case Tok.RNUM:
			return tok + " " + LexValueDouble;
		default:
			return tok + "";
	}
}

