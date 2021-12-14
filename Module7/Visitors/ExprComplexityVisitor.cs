using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ExprComplexityVisitor : AutoVisitor
    {
        private List<int> statsComp = new List<int>();
        private bool wip = false;

        public List<int> getComplexityList()
        {
            return statsComp;
        }
        
        public override void VisitCycleNode(CycleNode c)
        {
            if (!wip)
            {
                wip = true;
                statsComp.Add(0);
            }
            c.Expr.Visit(this);
            wip = false;
            c.Stat.Visit(this);
        }
        public override void VisitBlockNode(BlockNode bl)
        {

            var cnt = bl.StList.Count;

            if (cnt > 0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < cnt; i++)
            {
                bl.StList[i].Visit(this);
            }
        }

        public override void VisitAssignNode(AssignNode a)
        {
            if (!wip)
            {
                wip = true;
                statsComp.Add(0);
            }
            a.Expr.Visit(this);
            wip = false;
        }

        public override void VisitWriteNode(WriteNode w)
        {
            if (!wip)
            {
                wip = true;
                statsComp.Add(0);
            }
            w.Expr.Visit(this);
            wip = false;
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Visit(this);
            switch (binop.Op)
            {
                case '-':
                    {
                        statsComp[statsComp.Count - 1]++;
                        break;
                    }
                case '+':
                    {
                        statsComp[statsComp.Count - 1]++;
                        break;
                    }
                case '*':
                    {
                        statsComp[statsComp.Count - 1] += 3;
                        break;
                    }
                case '/':
                    {
                        statsComp[statsComp.Count - 1] += 3;
                        break;
                    }
            }
            binop.Right.Visit(this);
        }

    }
}
