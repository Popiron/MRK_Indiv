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
            string FileName = @"..\..\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");
                    Console.WriteLine("---------------Psevdo---------------");
                    Console.Write(Text);

                    Console.WriteLine("\n---------------C#---------------");
                    var cSharp = new CSharpVisitor();
                    parser.root.Visit(cSharp);
                    Console.Write(cSharp.Text);
                    Console.WriteLine("\n--------------------------------");
                }
            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }

            Console.ReadLine();
        }

    }
}
