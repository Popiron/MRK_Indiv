using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CountCyclesOpVisitor : AutoVisitor
    {
        int countParams = 0;
        int countCycles = 0;
        bool insideCycle = false;
        public int MidCount()
        {
            if (countCycles != 0)
                return countParams / countCycles;
            return 0;
        }

        private void tryIncCountParams()
        {
            if (insideCycle)
                countParams++;
        }
        public override void VisitCycleNode(CycleNode c)
        {
            countCycles++;
            tryIncCountParams();
            var t = insideCycle;
            insideCycle = true;
            c.Stat.Visit(this);
            insideCycle = t;
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

        public override void VisitIdNode(IdNode id)
        {
            tryIncCountParams();
        }
        public override void VisitIntNumNode(IntNumNode num)
        {
            tryIncCountParams();
        }
        public override void VisitBinOpNode(BinOpNode binop)
        {
            tryIncCountParams();
        }
        public override void VisitAssignNode(AssignNode a)
        {
            tryIncCountParams();
        }

        public override void VisitWriteNode(WriteNode w)
        {
            tryIncCountParams();
        }
        public override void VisitVarDefNode(VarDefNode w)
        {
            tryIncCountParams();
        }

    }
}
