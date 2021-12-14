using System;
using System.Collections.Generic;
using NUnit.Framework;
using SimpleScanner;
using SimpleParser;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestASTParser
{
    public class ASTParserTests
    {
        public static JObject Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);

            Parser parser = new Parser(scanner);

            var b = parser.Parse();
            if (!b)
                Assert.Fail("программа не распознана");
            else
            {
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                jsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonSettings.TypeNameHandling = TypeNameHandling.All;
                string output = JsonConvert.SerializeObject(parser.root, jsonSettings);
                return JObject.Parse(output);
            }

            return null;

        }
    }

    [TestFixture]
    public class WhileTests
    {

        [Test]
        public void TestWhile()
        {
            var tree = ASTParserTests.Parse("begin while 2 do a:=2 end");
            Assert.AreEqual("ProgramTree.WhileNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            Assert.AreEqual("ProgramTree.IntNumNode, SimpleLang", (string)tree["StList"]["$values"][0]["Expr"]["$type"]);
            Assert.AreEqual("2", ((string)tree["StList"]["$values"][0]["Expr"]["Num"]).Trim());
            Assert.AreEqual("ProgramTree.AssignNode, SimpleLang", (string)tree["StList"]["$values"][0]["Stat"]["$type"]);
        }
    }

    [TestFixture]
    public class RepeatTests
    {


        [Test]
        public void TestRepeat()
        {
            var tree = ASTParserTests.Parse("begin repeat a:=2 until 2 end");
            Assert.AreEqual("ProgramTree.RepeatNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            Assert.AreEqual("2", ((string)tree["StList"]["$values"][0]["Expr"]["Num"]).Trim());
            Assert.AreEqual("a", ((string)tree["StList"]["$values"][0]["StList"]["$values"][0]["StList"]["$values"][0]["Id"]["Name"]).Trim());
            Assert.AreEqual("2", ((string)tree["StList"]["$values"][0]["StList"]["$values"][0]["StList"]["$values"][0]["Expr"]["Num"]).Trim());
        }
    }

    [TestFixture]
    public class ForTests
    {

        [Test]
        public void TestFor()
        {
            var tree = ASTParserTests.Parse("begin for i:=2 to 10 do a:=2 end");
            Assert.AreEqual("ProgramTree.ForNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            Assert.AreEqual("10", ((string)tree["StList"]["$values"][0]["Expr"]["Num"]).Trim());
            Assert.AreEqual("2", ((string)tree["StList"]["$values"][0]["Stat"]["Expr"]["Num"]).Trim());
            Assert.AreEqual("a", ((string)tree["StList"]["$values"][0]["Stat"]["Id"]["Name"]).Trim());
        }
    }

    [TestFixture]
    public class WriteTests
    {

        [Test]
        public void TestWrite()
        {
            var tree = ASTParserTests.Parse("begin write(2) end");
            Assert.AreEqual("ProgramTree.WriteNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            Assert.AreEqual("2", (string)tree["StList"]["$values"][0]["Expr"]["Num"]);
        }
    }

    [TestFixture]
    public class ExtraTests
    {

        [Test]
        public void TestIf()
        {
            var tree = ASTParserTests.Parse("begin if 2 then a:= 2 else b:= 2 end");
            Assert.AreEqual("ProgramTree.IfNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            Assert.AreEqual("2", (string)tree["StList"]["$values"][0]["Expr"]["Num"]);
            Assert.AreEqual("ProgramTree.AssignNode, SimpleLang", (string)tree["StList"]["$values"][0]["ThenStat"]["$type"]);
            Assert.AreEqual("ProgramTree.AssignNode, SimpleLang", (string)tree["StList"]["$values"][0]["ElseStat"]["$type"]);
        }

        [Test]
        public void TestVarDef()
        {
            var tree = ASTParserTests.Parse("begin var a,b,d end");
            Assert.AreEqual("ProgramTree.VarDefNode, SimpleLang", (string)tree["StList"]["$values"][0]["$type"]);
            List<string> l = new List<string>();
            foreach (JObject s in tree["StList"]["$values"][0]["Ids"]["Ids"]["$values"])
            {
                l.Add(s["Name"].ToString());
            }
            Assert.IsTrue(l.Contains("a"));
            Assert.IsTrue(l.Contains("b"));
            Assert.IsTrue(l.Contains("d"));
        }

        [Test]
        public void TestBinary()
        {
            var tree = ASTParserTests.Parse("begin for i:=2-3*(s-d) to (c-3) do a:=(a-(3-3)) end");
            Assert.AreEqual("ProgramTree.BinaryNode, SimpleLang", (string)tree["StList"]["$values"][0]["Expr"]["$type"]);
            Assert.AreEqual("c", (string)tree["StList"]["$values"][0]["Expr"]["Left"]["Name"]);
            Assert.AreEqual("3", (string)tree["StList"]["$values"][0]["Expr"]["Right"]["Num"]);
            Assert.AreEqual('-', (char)tree["StList"]["$values"][0]["Expr"]["Operation"]);
            Assert.AreEqual("3", (string)tree["StList"]["$values"][0]["Stat"]["Expr"]["Right"]["Right"]["Num"]);
            Assert.AreEqual('*', (char)tree["StList"]["$values"][0]["Assign"]["Expr"]["Right"]["Operation"]);
        }
    }
}