using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class CSharpVisitor : Visitor
    {
        public string Text = "";
        private int Indent = 0;
        private bool mainBlocIsVisited = false;

        private string IndentStr()
        {
            return new string(' ', Indent);
        }
        private void IndentPlus()
        {
            Indent += 2;
        }
        private void IndentMinus()
        {
            Indent -= 2;
        }
        public override void VisitIdNode(IdNode id)
        {
            Text += id.Name;
        }
        public override void VisitIntNumNode(IntNumNode num)
        {
            Text += num.Num.ToString();
        }
        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Visit(this);
            Text += binop.Op;
            binop.Right.Visit(this);
        }
        public override void VisitAssignNode(AssignNode a)
        {
            var tText = Text;
            Text += IndentStr();
            a.Id.Visit(this);
            Text += "=";
            a.Expr.Visit(this);

            if (Text.Contains($"&{a.Id.Name}"))
            {
                var v = Text.Remove(0, tText.Length);
                Text = tText.Replace($"&{a.Id.Name}", v.Trim());
            } else
            {
                Text += ";";
            }
            
        }
        public override void VisitCycleNode(CycleNode c)
        {
            Text += IndentStr() + "cycle ";
            c.Expr.Visit(this);
            Text += Environment.NewLine;
            c.Stat.Visit(this);
        }
        public override void VisitBlockNode(BlockNode bl)
        {
            var currentMainBlocIsVisited = mainBlocIsVisited;
            if (mainBlocIsVisited)
            {
                Text += Environment.NewLine + IndentStr() + "{" + Environment.NewLine;

            } else
            {
                mainBlocIsVisited = true;
                Text += IndentStr() + "using System;" + Environment.NewLine + IndentStr() + "namespace default {" + Environment.NewLine + IndentStr() + "public class MainClass {" + Environment.NewLine + IndentStr() + "public static void main(string[] args) {" + Environment.NewLine;
            }

            IndentPlus();

            var Count = bl.StList.Count;

            if (Count > 0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < Count; i++)
            {
                
                //if (!(bl.StList[i] is AssignNode || bl.StList[i] is VarDefNode))
                //    Text += Environment.NewLine;
                bl.StList[i].Visit(this);
                //if (!(bl.StList[i] is IfNode))
                //    Text += ';';
            }
            IndentMinus();

            if (currentMainBlocIsVisited)
            {
                Text += Environment.NewLine + IndentStr() + "}" + Environment.NewLine;
            } else
            {
                Text += Environment.NewLine + IndentStr() + "}" + Environment.NewLine + IndentStr() + "}" + Environment.NewLine + IndentStr() + "}";
            }
        }
        public override void VisitWriteNode(WriteNode w)
        {
            Text += IndentStr() + "Console.WriteLine(";
            w.Expr.Visit(this);
            Text += ");";
        }
        public override void VisitVarDefNode(VarDefNode w)
        {

            for (int i = 0; i < w.vars.Count; i++)
                Text += IndentStr() + "var &" + w.vars[i].Name + ";" + Environment.NewLine;
        }
        public override void VisitIfNode(IfNode cond)
        {
            Text += IndentStr() + "if (";
            var tempText = Text;
            cond.expr.Visit(this);
            var x = Text.Remove(0,tempText.Length);
            if (x.Length == 1)
            {
                Text += " != 0";
            }
            Text += ") { ";
            Text += Environment.NewLine;
            IndentPlus();
            
            cond.ifTrue.Visit(this);
            IndentMinus();
            if (null != cond.ifFalse)
            {
                Text += Environment.NewLine;
                Text += IndentStr() + "} else {";
                Text += Environment.NewLine;
                IndentPlus();
                cond.ifFalse.Visit(this);
                IndentMinus();
                Text += Environment.NewLine;
                Text += IndentStr() + "}";
            }
        }
    }
}
