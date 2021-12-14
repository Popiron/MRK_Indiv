using System.Collections.Generic;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public class Node // базовый класс для всех узлов    
    {
    }

    public class ExprNode : Node // базовый класс для всех выражений
    {

    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
    }

    public class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
    }

    public class WhileNode : CycleNode
    {
        public WhileNode(ExprNode expr, StatementNode stat) : base(expr, stat)
        {
        }

    }

    public class RepeatNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public ExprNode Expr { get; set; }

        public RepeatNode(StatementNode stat, ExprNode expr)
        {
            Add(stat);
            Expr = expr;
        }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
    }

    public class ForNode : StatementNode
    {
        public AssignNode Assign { get; set; }
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }

        public ForNode(AssignNode assign, ExprNode expr, StatementNode stat)
        {
            Assign = assign;
            Expr = expr;
            Stat = stat;
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode ThenStat { get; set; }
        public StatementNode ElseStat { get; set; }


        public IfNode(ExprNode expr, StatementNode thenStat, StatementNode elseStat)
        {
            Expr = expr;
            ThenStat = thenStat;
            ElseStat = elseStat;
        }
    }

    public class WriteNode : StatementNode
    {
        public ExprNode Expr { get; set; }

        public WriteNode(ExprNode expr)
        {
            Expr = expr;
        }
    }

    public class IdsNode : Node
    {
        public List<IdNode> Ids = new List<IdNode>();
        public IdsNode(IdNode id)
        {
            Add(id);
        }
        public void Add(IdNode id)
        {
            Ids.Add(id);
        }
    }

    public class VarDefNode : StatementNode
    {
        public IdsNode Ids { get; set; }
        public VarDefNode(IdsNode ids)
        {
            Ids = ids;
        }
    }

    public class BinaryNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public char Operation { get; set; }

        public BinaryNode(ExprNode left, char operation, ExprNode right)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }
    }






}