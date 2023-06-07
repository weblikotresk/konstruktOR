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
        public List<Position> PieceMovement = new List<Position>();
        public List<Position> PiecesTakenPos = new List<Position>();
        public bool IsKingingMove;
        public bool IsEmpty;

        public Move(Position oldPosition, Position newPosition)
        {
            oldPos= oldPosition; newPos=newPosition;
        }

        public Move(List<Position> FrTo, List<Position> Taken, bool IsKinging)
        {
            oldPos = FrTo[0]; newPos = FrTo[1]; 
            foreach (Position pos in Taken)
            {
                PiecesTakenPos.Add(pos);
            }
            //????????below
            foreach (Position pos in FrTo)
            {
                PieceMovement.Add(pos);
            }
            IsKingingMove = IsKinging;
        }

        //when move is null
        public Move()
        {

        }
        //deep move copy

        public Move DeepCopyMove()
        {
            int NewPiecesTaken = PiecesTaken;
            List<Position> NewPieceMovement = new List<Position>();
            List<Position> NewPiecesTakenPos = new List<Position>();



            foreach (Position pos in PieceMovement)
            {
                NewPieceMovement.Add(new Position(pos.Row, pos.Column));
            }
            foreach (Position pos in PiecesTakenPos)
            {
                NewPiecesTakenPos.Add(new Position(pos.Row, pos.Column));
            }
            Move NewMove = new Move()
            {
                oldPos = new Position(oldPos.Row, oldPos.Column),
                newPos = new Position(newPos.Row, newPos.Column),
                PiecesTaken = NewPiecesTaken,
                PieceMovement = NewPieceMovement,
                PiecesTakenPos = NewPiecesTakenPos

            };
            return NewMove;
        }
        public Move(Move oldMove)
        {
            oldPos = oldMove.oldPos;
            newPos = oldMove.newPos;
            PiecesTaken = oldMove.PiecesTaken;
            PieceMovement = oldMove.PieceMovement; //needed deep copy?
            PiecesTakenPos = oldMove.PiecesTakenPos; //needed deep copy?
            IsKingingMove = oldMove.IsKingingMove;
        }


    }
}
