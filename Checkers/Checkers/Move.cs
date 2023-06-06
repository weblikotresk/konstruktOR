using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Checkers
{
    public class Move
    {
        public Position oldPos;
        public Position newPos;
        public int PiecesTaken;
        public List<Position> PiecesMoved = new List<Position>();
        public List<Position> PiecesTakenPos = new List<Position>();
        public bool IsKinging;
        public bool IsEmpty;

        public Move(Position oldPosition, Position newPosition)
        {
            oldPos= oldPosition; newPos=newPosition;
        }

        public Move(List<Position> FrTo, List<Position> Taken, bool IsKingingMove)
        {
            oldPos = FrTo[0]; newPos = FrTo[1]; 
            foreach (Position pos in Taken)
            {
                PiecesTakenPos.Add(pos);
            }
            foreach (Position pos in FrTo)
            {
                PiecesMoved.Add(pos);
            }
            IsKinging = IsKingingMove;
        }

        //when move is null
        public Move()
        {

        }
        //deep move copy
        public Move(Move oldMove)
        {
            oldPos = oldMove.oldPos;
            newPos = oldMove.newPos;
            PiecesTaken = oldMove.PiecesTaken;
            PiecesMoved = oldMove.PiecesMoved; //needed deep copy?
            PiecesTakenPos = oldMove.PiecesTakenPos; //needed deep copy?
            IsKinging = oldMove.IsKinging;
        }


    }
}
