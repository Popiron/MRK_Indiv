using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ChangeVarIdVisitor : AutoVisitor
    {
        private string From, To;

        public ChangeVarIdVisitor(string from, string to)
        {
            From = from;
            To = to;
        }

        public override void VisitIdNode(IdNode id)
        {
            if (id.Name == From)
                id.Name = To;
        }
        
        public override void VisitBlockNode(BlockNode bl)
        {

            var cnt = bl.StList.Count;

            if (cnt > 0)
                bl.StList[0].Visit(this);

            for (var i = 1; i < cnt; i++)
                bl.StList[i].Visit(this);
        }
        
        public override void VisitVarDefNode(VarDefNode w)
        {
            for (int i = 0; i < w.vars.Count; i++)
                if (w.vars[i].Name == From)
                    w.vars[i].Name = To;
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
        public override void VisitWriteNode(WriteNode w)
        {
            w.Expr.Visit(this);
        }
    }

}
