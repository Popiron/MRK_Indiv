using System;
using System.IO;
using SimpleScanner;
using SimpleLang.Visitors;
using SimpleParser;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static Parser Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);
            Parser parser = new Parser(scanner);
            //Assert.IsTrue(parser.Parse());
            return parser;
        }
        public static void Main()
        {
            //string FileName = @"..\..\a.txt";
            try
            {
                /*string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");

                    var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");

                    var pp = new PrettyPrintVisitor();
                    parser.root.Visit(pp);
                    Console.WriteLine(pp.Text);
                    Console.WriteLine("-------------------------------");

                    var code = new GenCodeVisitor();
                    parser.root.Visit(code);
                    code.EndProgram();
                    //code.PrintCommands();
                    Console.WriteLine("-------------------------------");

                    code.RunProgram();
                }*/
                var parser = Parse(@"begin write(2) end");
                var cSharpWalker = new CSharpVisitor();
                parser.root.Visit(cSharpWalker);
                Console.WriteLine(cSharpWalker.Text);
                Console.WriteLine(cSharpWalker.Text);

            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }

            //Console.ReadLine();
        }

    }
}
