using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CommonlyUsedVarVisitor : AutoVisitor
    {
        private Dictionary<string, int> freqVars = new Dictionary<string, int>();
        public string mostCommonlyUsedVar()
        {
            if (freqVars.Count == 0)
                throw new ArgumentException("no vars");
            return freqVars.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        private void addToFreqVars(string s)
        {
            if (freqVars.ContainsKey(s))
                freqVars[s]++;
            else
                freqVars.Add(s, 1);
        }
        public override void VisitIdNode(IdNode id)
        {
            addToFreqVars(id.Name);
        }

        public override void VisitBlockNode(BlockNode bl)
        {

            var Count = bl.StList.Count;

            if (Count > 0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < Count; i++)
            {
                bl.StList[i].Visit(this);
            }
        }
        public override void VisitWriteNode(WriteNode w)
        {
            w.Expr.Visit(this);
        }
        public override void VisitVarDefNode(VarDefNode w)
        {
            for (int i = 0; i < w.vars.Count; i++)
                addToFreqVars(w.vars[i].Name);
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Visit(this);
            binop.Right.Visit(this);
        }
        public override void VisitAssignNode(AssignNode a)
        {
            a.Id.Visit(this);
            a.Expr.Visit(this);
        }
        public override void VisitCycleNode(CycleNode c)
        {
            c.Expr.Visit(this);
            c.Stat.Visit(this);
        }
    }
}

