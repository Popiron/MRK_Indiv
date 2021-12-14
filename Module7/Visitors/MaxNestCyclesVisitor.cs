using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class MaxNestCyclesVisitor : AutoVisitor
    {
        public int MaxNest = 0;
        private int currNest = 0;

        public override void VisitCycleNode(CycleNode c)
        {
            currNest++;
            c.Stat.Visit(this);
            if (currNest > MaxNest)
                MaxNest = currNest;
            currNest--;
        }
        public override void VisitBlockNode(BlockNode bl)
        {
            var Count = bl.StList.Count;

            if (Count > 0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < Count; i++)
                bl.StList[i].Visit(this);
        }

    }
}
