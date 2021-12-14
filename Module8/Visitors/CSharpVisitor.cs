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
            Text += "(";
            binop.Left.Visit(this);
            Text += " " + binop.Op + " ";
            binop.Right.Visit(this);
            Text += ")";
        }
        public override void VisitAssignNode(AssignNode a)
        {
            var tText = Text;
            Text += IndentStr();
            a.Id.Visit(this);
            Text += " = ";
            a.Expr.Visit(this);
            var v = Text.Remove(0, tText.Length);
            var t = v.Split('=');
            t[1] = t[1].Trim();
            if (t[1].First()=='(' && t[1].Last() == ')')
            {
                t[1]=t[1].Remove(0, 1);
                t[1] = t[1].Remove(t[1].Length - 1, 1);
                v = t[0] + "= " + t[1];
            }
            if (Text.Contains($"&{a.Id.Name}"))
            {
                Text = tText.Replace($"&{a.Id.Name}", v.Trim());
            } else
            {

                Text = tText + v + ";" + Environment.NewLine;
            }
            
        }
        public override void VisitCycleNode(CycleNode c)
        {
            Text += IndentStr() + "for (int i = 0; i < ";
            c.Expr.Visit(this);
            Text += "; i++) {" + Environment.NewLine;
            c.Stat.Visit(this);
            Text += IndentStr()+ "}" + Environment.NewLine;
        }
        public override void VisitBlockNode(BlockNode bl)
        {
            var currentMainBlocIsVisited = mainBlocIsVisited;
            if (!mainBlocIsVisited)
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
                bl.StList[i].Visit(this);
            }
            IndentMinus();

            if (!currentMainBlocIsVisited)
            {
                Text += Environment.NewLine + IndentStr() + "}" + Environment.NewLine + IndentStr() + "}" + Environment.NewLine + IndentStr() + "}";
            }
        }
        public override void VisitWriteNode(WriteNode w)
        {
            Text += IndentStr() + "Console.WriteLine(";
            w.Expr.Visit(this);
            Text += ");" + Environment.NewLine;
        }
        public override void VisitVarDefNode(VarDefNode w)
        {
            Text += IndentStr() + "var &" + w.vars[0].Name;
            for (int i = 1; i < w.vars.Count; i++)
                Text += ", &" + w.vars[i].Name;
            Text += ";" + Environment.NewLine;
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
            Text += ") { " + Environment.NewLine;
            IndentPlus();
            cond.ifTrue.Visit(this);
            IndentMinus();
            Text += IndentStr() + "}";
            if (null != cond.ifFalse)
            {
                Text += " else {" + Environment.NewLine;
                IndentPlus();
                cond.ifFalse.Visit(this);
                IndentMinus();
                Text += IndentStr() + "}";
            }
            Text += Environment.NewLine;
        }

        public override void VisitRepeatNode(RepeatNode rep)
        {
            Text += IndentStr() + "do {" + Environment.NewLine;
            foreach (var r in rep.StList)
            {
                r.Visit(this);
            }
            Text += IndentStr() + "}" + Environment.NewLine;
            Text += IndentStr() + "while (";
            var tempText = Text;
            rep.Expr.Visit(this);
            var x = Text.Remove(0, tempText.Length);
            if (x.Length == 1)
            {
                Text += " != 0";
            }
            Text += ");" + Environment.NewLine;
        }
    }

    
}
