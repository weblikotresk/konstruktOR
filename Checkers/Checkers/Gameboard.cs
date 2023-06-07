using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
namespace Checkers
{ 
    public class Gameboard 
    {

        private List<Cell> CellList = new List<Cell>();
        private List <Move> MoveList = new List<Move>();
        private List<Position> PiecesToBeEaten = new List<Position>();

        public bool ContinuedWhiteAttack = false;
        public bool isWhitesTurn = true;



        public Cell ActiveCell = new Cell();
        public Move LastMove = new Move();

        public int IndexToBeEaten { get; set; } //we need to remove this somehow
        public CellTypes TypeToBeEaten { get; set; }


        private readonly int PieceValue = 100; // стоимость простой шашки
        private readonly int SupremeValue = 250; // стоимость дамки
        private readonly float[,] CellBonus = new float[8, 4] // бонус клетки
            {
        { 1.2f, 1.2f, 1.2f, 1.2f },
        { 1.15f, 1.2f, 1.2f, 1.15f },
        { 1.15f, 1.2f, 1.2f, 1.13f },
        { 1.0f, 1.2f, 1.15f, 1.0f },
        { 1.0f, 1.2f, 1.2f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
            };

        private readonly float[] YBonus = new float[8]{ 1.0f, 1.025f, 1.05f, 1.075f, 1.1f, 1.125f, 1.15f, 1.175f}; // Y-бонус
        //private readonly float[] YBonus = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f}; // Y-бонус



        //METHODS


        //----flags----
        public bool IsInBounds(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }
        public bool IsDifferentColor(Position pos1, Position pos2)
        {
            return CellList[pos1.Index].Type != CellList[pos2.Index].Type;
        }
        public bool IsEmpty(Position pos)
        {
            return CellList[pos.Index].Type == CellTypes.Empty;
        }
        public bool IsKinging(Position to, bool IsWhite)
        {
            return IsWhite ? to.Row == 0 : to.Row == 8;
        }

        //----interface----

        //highlights availabale moves on the board
        public void HighlightMoves(List<Move> availableMovesList)
        {
            foreach (Move move in availableMovesList)
            {
                CellList[move.newPos.Index].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                SetType(move.newPos, CellTypes.Highlighted);
            }
        }

        public void RenderImage(bool isBlack, int index, double width)
        {
            CellList[index].Content.Content = new Image
            {
                Width = width,
                Source =
                isBlack ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
            };
        }
        public void RenderImage(CellTypes Type, Position pos, double width)
        {
            CellList[pos.Index].Content.Content = new Image
            {
                Width = width,
                Source =
                Type == CellTypes.Black ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
            };
        }

        //remove image and piece from CellList
        public void RemovePiece(int index)
        {
            CellList[index].Content.Content = null;
            CellList[index].Type = CellTypes.Empty;
        }




        // ----getters & setters----
        public CellTypes GetType(Position pos)
        {
            return CellList[pos.Index].Type;
        }
        public void SetType(Position pos, CellTypes type)
        {
            CellList[pos.Index].Type = type;
        }

        public List<Move> GetMoveList()
        {
            return MoveList;
        }


        // ----board interaction----

        public float BoardStaticValue(Gameboard gameboard)
        {
            float eval = 0;
            // Рассчитываем качество каждой шашки
            foreach (Cell piece in gameboard.CellList)
            {
                switch (piece.Type)
                {
                    case CellTypes.White:
                        eval += PieceValue * YBonus[piece.Position.Column] * CellBonus[piece.Position.Row, piece.Position.Column/2];
                        break;
                    case CellTypes.Black:
                        eval -= PieceValue * YBonus[7 - piece.Position.Column] * CellBonus[7-piece.Position.Row, (7 - piece.Position.Column)/2];
                        break;
                    case CellTypes.WhiteSupreme:
                        eval += SupremeValue;
                        break;
                    case CellTypes.BlackSupreme:
                        eval -= SupremeValue;
                        break;
                }
            }
            return eval;
        }
        public GameState GetGameState()
        {
            short blackCounter = 0, whiteCounter = 0;
            

            foreach (Cell cell in CellList)
            {
                if (cell.Type == CellTypes.Black || cell.Type == CellTypes.BlackSupreme)
                {
                    blackCounter++;
                };
                if (cell.Type == CellTypes.White || cell.Type == CellTypes.WhiteSupreme)
                {
                    whiteCounter++;
                };

                //пока без отслеживания отсутствия ходов чёрных

            }
            if (blackCounter == 0 )
            {
                return GameState.WHITE_WIN;
            }
            else if (whiteCounter == 0 || MoveList.Count ==0)
            {
                return GameState.BLACK_WIN;
            }
            else
            {
                return GameState.IN_PROCESS;
            }

        }
       
        public Cell MovePiece(List<Cell> CellList, Position posFrom, Position posTo, Position taken)
        {
            if (taken != null)
            {
                CellList[taken.Index].Type = CellTypes.Empty;
            }
            CellList[posTo.Index].Type = CellList[posFrom.Index].Type;
            CellList[posFrom.Index].Type = CellTypes.Empty;
            return CellList[posTo.Index];
        }


        // ----move finding----

        public void FindAllMoves()
        {
            List<Move> takingMoves = new List<Move>(); // взятия
            List<Move> simpleMoves = new List<Move>(); // простые ходы
            foreach (Cell cell in CellList)
            {
                bool searchForWhite = (cell.Type == CellTypes.White || cell.Type == CellTypes.WhiteSupreme) && isWhitesTurn;
                bool searchForBlack = (cell.Type == CellTypes.Black || cell.Type == CellTypes.BlackSupreme) && !isWhitesTurn;
                if (searchForWhite || searchForBlack)
                {
                    // Для каждой фигуры сначала ищем все взятия
                    takingMoves.AddRange(GetAllTakingMovesOfPiece(cell));
                    // Если взятий нет, ищем простые ходы
                    if (takingMoves.Count == 0)
                        simpleMoves.AddRange(GetAllSimpleMovesOfPiece(cell));
                }
            }

            // Если есть взятия, отбрасываем простые ходы; иначе есть только простые ходы
            if (takingMoves.Count > 0)
            {
                // Взятия сортируем по убыванию количеству побитых шашек, чтобы сначала шли самые лучшие
                // Это поможет нам более эффективно искать сильнейшие ходы, оценивая потенциально лучшие первыми
                takingMoves.Sort((Move a, Move b) => -a.PiecesTaken.CompareTo(b.PiecesTaken));

                MoveList  = takingMoves;
            }
            else
                MoveList  = simpleMoves;
        }
        public List<Move> FindAllMoves(Position currPos) //перегрузка для одной шашки
        {
            ActiveCell = CellList[currPos.Index];
            List<Move> takingMoves = new List<Move>(); // взятия
            List<Move> simpleMoves = new List<Move>(); // простые ходы
            if (CellList[currPos.Index].Type == CellTypes.White && isWhitesTurn)
            {
                // Для фигуры сначала ищем все взятия
                takingMoves.AddRange(GetAllTakingMovesOfPiece(CellList[currPos.Index]));
                // Если взятий нет, ищем простые ходы
                if (takingMoves.Count == 0)
                    simpleMoves.AddRange(GetAllSimpleMovesOfPiece(CellList[currPos.Index]));
            }

            // Если есть взятия, отбрасываем простые ходы; иначе есть только простые ходы
            if (takingMoves.Count > 0)
            {
                // Взятия сортируем по убыванию количеству побитых шашек, чтобы сначала шли самые лучшие
                // Это поможет нам более эффективно искать сильнейшие ходы, оценивая потенциально лучшие первыми
                takingMoves.Sort((Move a, Move b) => -a.PiecesTaken.CompareTo(b.PiecesTaken));
                return takingMoves;
            }
            else return simpleMoves;
        }


        // Функция поиска взятий фигуры
        // В массиве exc хранятся позиции, шашки на которых мы уже побили, так как в русских шашках,
        // согласно турецкому правилу, шашки снимаются с доски уже после хода
        private List<Move> GetAllTakingMovesOfPiece(Cell cell, List<Position> exc = null)
        {
            if (exc == null)
                exc = new List<Position>();

            List<Move> moves = new List<Move>(); // все взятия
            List<Move> movesWithFollowingTake = new List<Move>(); // взятия, после которых можно побить еще

            if ((int)cell.Type > 3)
            {
                // Перебираем все 4 направления движения
                for (int x = 1; x > -2; x -= 2)
                {
                    for (int y = 1; y > -2; y -= 2)
                    {
                        bool opp_found = false;
                        Position pos_opp = new Position(0, 0);

                        // Куда дамка встанет после прыжка
                        Position target = new Position(cell.Position.Row + x, cell.Position.Column + y);
                        while (IsInBounds(target)) // Функция IsInBounds проверяет, что позиция принадлежат полю
                        {
                            if (IsEmpty(target)) // Функция IsEmpty проверяет, что поле пустое
                            {
                                if (opp_found) // Если, прыгнув на клетку target мы перепрыгнем шашку соперника, то это взятие
                                    AddMove(cell.Position, target, pos_opp); // Косвенно рекурсивно продолжаем поиск дальнейших прыжков со взятием
                            }
                            else if (IsDifferentColor(target, cell.Position)) // Если уперлись в свою шашку — то усё
                                break;
                            else
                            {
                                if (!opp_found) // Если уперлись в шашку соперника, запоминаем это
                                {
                                    opp_found = true;
                                    pos_opp = target;
                                }
                                else // Если уткнулись во 2-ю шашку соперника, то дальше прыгнуть не получится
                                    break;
                            }
                            target = new Position(target.Row + x, target.Column + y);
                        }
                    }
                }
            }
            else
            {
                // Тут перебираем все 4 варианта взятия обычной шашки
                // AttackPos - поле куда приземлимся, JumpOverPos - поле, которое перепрыгнем.

                for (int x = 1; x > -2; x -= 2)
                {
                    for (int y = 1; y > -2; y -= 2)
                    {
                        Position AttackPos = new Position(cell.Position.Row + x + (x > 0 ? 1 : -1), cell.Position.Column + y + (y > 0 ? 1 : -1));
                        Position JumpOverPos = new Position(cell.Position.Row + x, cell.Position.Column + y);

                        if (IsInBounds(AttackPos) && IsEmpty(AttackPos) && !IsEmpty(JumpOverPos) && IsDifferentColor(cell.Position, JumpOverPos))
                        {//получившийся ход атаки рассмотрим подробнее, учитывая дальнейшие атаки подряд
                            AddMove(cell.Position, AttackPos, JumpOverPos);
                        }
                           
                    }
                }

            }

            if (movesWithFollowingTake.Count > 0)
                return movesWithFollowingTake;
            return moves;



            bool AddMove(Position fr, Position to, Position taken)
            {
                // Турецкий удар (см. в комментарии ниже)
                foreach(Position pos in exc)
                {
                    if (taken.Equals(pos))
                    {
                        return false;
                    }
                }
                // Моделируем доску, на которой этот ход сделан

                Gameboard nextBoard = DeepCopy();

                Cell thisCell = MovePiece(nextBoard.CellList, fr, to, taken);

                List<Position> newExc = new List<Position>(exc){taken};

                // Проверяем, не превратилась ли наша шашка в дамку этим ходов
                bool isThisMoveKinging = !((int)cell.Type > 3) && IsKinging(to, cell.Type == CellTypes.White);

                List<Move> nextTakes = nextBoard.GetAllTakingMovesOfPiece(thisCell, newExc);
                
                if (nextTakes.Count == 0) //если последующих взятий нет, заносим ход атаки в moves
                {
                    moves.Add(new Move(new List<Position>() { fr, to }, new List<Position>() { taken }, isThisMoveKinging));
                    return false;
                }
                else
                {
                    foreach (Move nextTake in nextTakes)
                    {
                        List<Position> pos = nextTake.PieceMovement;
                        pos.Insert(0, fr);
                        List<Position> takes = nextTake.PiecesTakenPos;
                        takes.Insert(0, taken);
                        //создадим ход, в котором запишем все прошедшие шашкой клетки и все съеденные шашки
                        movesWithFollowingTake.Add(new Move(pos, takes, isThisMoveKinging || nextTake.IsKingingMove));
                    }
                    return true;
                }
            }
        }

        // Эта функция ищет все простые ходы шашки. Она очень простая и не представляет особого интереса
        private List<Move> GetAllSimpleMovesOfPiece(Cell cell)
        {
            List<Move> moves = new List<Move>();
            if (isWhitesTurn)
            {
                //We're using ordinal directions relative to current piece position: NE, SE, SW, NW
                Position NW = new Position(cell.Position.Row - 1, cell.Position.Column - 1);
                Position NE = new Position(cell.Position.Row - 1, cell.Position.Column + 1);

                AddMoveToList(moves, cell.Position, NW);
                AddMoveToList(moves, cell.Position, NE);
            }
            else
            {
                Position SW = new Position(cell.Position.Row + 1, cell.Position.Column - 1);
                Position SE = new Position(cell.Position.Row + 1, cell.Position.Column + 1);
                AddMoveToList(moves, cell.Position, SW);
                AddMoveToList(moves, cell.Position, SE);
            }

            return moves;

        }

        private void AddMoveToList(List<Move> list, Position oldPos, Position newPos)
        {
            if (IsInBounds(newPos) && GetType(newPos) == 0)
            {
                list.Add(new Move(oldPos, newPos));
            }
        }


        public void MakeComputerMove(Gameboard board, Move move, bool memoriseMove)
        {
            if (memoriseMove)
            {
                board.LastMove = move;
            }
            board.CellList[move.newPos.Index].Type = board.CellList[move.oldPos.Index].Type;
            board.CellList[move.oldPos.Index].Type = CellTypes.Empty;
        }


        public void UnmakeLastMove()
        {
            CellList[LastMove.oldPos.Index].Type = CellList[LastMove.newPos.Index].Type;
            CellList[LastMove.newPos.Index].Type = CellTypes.Empty;
        }

        //set IsToAttack to true to those cells that can attack
        public bool PiecesToAttack(List<Move> moves)
        {
            bool AnyToAttack = false;
            foreach (Move move in moves)
            {
                if (move.PiecesTakenPos.Count != 0)
                {
                    AnyToAttack = true;
                    CellList[move.oldPos.Index].IsToAttack = true;
                }
            }
            return AnyToAttack;
        }
        
        public void MakeMoveOnBoard(Move move)
        {

            if (move.PiecesTakenPos.Count !=0 && !isWhitesTurn)
            {
                //убирает все съеденные шашки
                foreach (Position pos in move.PiecesTakenPos)
                {
                    RemovePiece(pos.Index);
                }
                //ставит бьющую шашку на последнее место
                RenderImage(CellList[move.oldPos.Index].Type, move.PieceMovement.Last(), 40);
                SetType(move.PieceMovement.Last(), CellList[move.oldPos.Index].Type);
                RemovePiece(move.oldPos.Index);
                
            }else if (move.PiecesTakenPos.Count != 0 && isWhitesTurn)
            {
                //убирает первую съеденную шашку
                RemovePiece(move.PiecesTakenPos[0].Index);
                //уберём её из листа
                move.PiecesTakenPos.Remove(move.PiecesTakenPos[0]);

                //ставит бьющую шашку на первое из атакующих место
                RenderImage(CellList[move.oldPos.Index].Type, move.PieceMovement[1], 40);
                SetType(move.PieceMovement[1], CellList[move.oldPos.Index].Type);
                RemovePiece(move.oldPos.Index);
                //уберём исходную позицию из листа
                move.PieceMovement.Remove(move.PieceMovement[0]);

                //если есть ещё не побитые шашки, изменим ContinuedWhiteAttack
                ContinuedWhiteAttack = move.PiecesTakenPos.Count != 0;
            }
            else
            {
                //обычный ход на пустую клетку
                RenderImage(CellList[move.oldPos.Index].Type, move.newPos, 40);
                SetType(move.newPos, CellList[move.oldPos.Index].Type);
                RemovePiece(move.oldPos.Index);

            }
           foreach(Cell cell in CellList)
            {
                if(cell.Type == CellTypes.Highlighted)
                {
                    cell.Type = CellTypes.Empty;
                }
            }

        }
        public void RemoveIsToAttackFlags()
        {
            foreach(Cell cell in CellList)
            {
                if (cell.IsToAttack)
                {
                    cell.IsToAttack = false;
                }
            }
        }

        public void ComputersTurn(Gameboard gameboard)
        {
            Combinations computerCombos = new Combinations(gameboard.DeepCopy());
            computerCombos.ActiveBestMoveSearch();
            //CommitChangesToBoard();
            MakeMoveOnBoard(computerCombos.CurrentBestMove);
            isWhitesTurn = true;
            //PlayersTurn();
        }

        public void PlayersTurn(Position clickedButtonPosition)
        {
            //проверим, есть ли атакующие ходы
            FindAllMoves();
            bool AnyToAttack = PiecesToAttack(MoveList);
            //позволять ход только тем шашкам, что могут бить 
            if(AnyToAttack)
            {
                if (CellList[clickedButtonPosition.Index].IsToAttack)
                {
                    HighlightMoves(FindAllMoves(clickedButtonPosition));
                }
                else
                {
                    MessageBox.Show("Gotta attack, dummy");
                }
            }//если обычный ход, просчитаем ходы и подсветим
            else HighlightMoves(FindAllMoves(clickedButtonPosition));
            RemoveIsToAttackFlags();
        }



        public Grid CreateGrid()
        {
            Grid myGrid = new Grid()
            {
                Width = 400,
                Height = 400,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,

            };
            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {
                    Cell gridButton = new Cell
                    {
                        Position = new Position(i, j),
                        Content = new Button()
                    };
                    gridButton.Content.Style = ((i + j) % 2 == 0) ? ButtonStyles.Instance.whiteCellStyle : ButtonStyles.Instance.grayCellStyle;
                    gridButton.Content.Click += new RoutedEventHandler(CellClicked);
                    gridButton.Content.LostFocus += new RoutedEventHandler(PieceDefocused);

                    CellList.Add(gridButton);
                    if ((i + j) % 2 != 0 && i <= 2)
                    {
                        RenderImage(true, i * 8 + j, myGrid.Width / 10);
                        gridButton.Type = CellTypes.Black;

                    }
                    else if ((i + j) % 2 != 0 && i > 4 && i <= 7)
                    {
                        RenderImage(false, i * 8 + j, myGrid.Width / 10);
                        gridButton.Type = CellTypes.White;

                    }

                    myGrid.Children.Add(gridButton.Content);
                    Grid.SetColumn(gridButton.Content, j);
                    Grid.SetRow(gridButton.Content, i);
                }
            }
            return myGrid;
        }

        // ----event handlers----
        public void PieceDefocused(object sender, RoutedEventArgs e)
        {
            Button lostButton = (Button)sender;
            Position lostButtonPos = new Position(Grid.GetRow(lostButton), Grid.GetColumn(lostButton));

            bool IsPieceClicked = GetType(lostButtonPos) == CellTypes.Black ||
                                    GetType(lostButtonPos) == CellTypes.White;

            if (IsPieceClicked) lostButton.Style = ButtonStyles.Instance.grayCellStyle;

            foreach (Cell gridButton in CellList)
            {
                //if button is highlighted
                if (GetType(gridButton.Position) == CellTypes.Highlighted) gridButton.Content.Style = ButtonStyles.Instance.grayCellStyle;
                //удалим непрозрачность
                if (IsPieceClicked) gridButton.Content.Opacity = 1;
            }
        }

        public void CellClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Position clickedButtonPos = new Position(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));

            bool IsPieceClicked = GetType(clickedButtonPos) == CellTypes.White; //accept only player's pieces click

            if (IsPieceClicked) clickedButton.Style = ButtonStyles.Instance.clickedCellStyle;

            //MessageBox.Show(Convert.ToString(CellList));
            //the player moves onto highlighted cell
            if (GetType(clickedButtonPos) == CellTypes.Highlighted)
            {
                
                foreach(Move move in MoveList)
                {
                    if(move.oldPos.Index == ActiveCell.Position.Index && move.newPos.Index == clickedButtonPos.Index)
                    {
                        MakeMoveOnBoard(move.DeepCopyMove());
                    }
                }

                if (!ContinuedWhiteAttack)
                {
                    isWhitesTurn = false;
                    //computer move
                    //MessageBox.Show(Convert.ToString(CellList));
                    ComputersTurn(this);
                    
                }

            }

            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    //очистим уже подсвеченные клетки
                    if (GetType(new Position(i,j)) == CellTypes.Highlighted) SetType(new Position(i, j), 0);
                    //включим непрозрачность поля
                    if (IsPieceClicked)
                        CellList[i * 8 + j].Content.Opacity = (i != clickedButtonPos.Row || j != clickedButtonPos.Column) ? 0.65 : 1;
                }
            }

            //highlight moves for white player and find em all
            if (IsPieceClicked && isWhitesTurn) PlayersTurn(clickedButtonPos);
        }


        // ----constructors----
        public Gameboard(){
        
        }

        //deep MOVES&CELL copy method
        public Gameboard DeepCopy()
        {
            bool newIsWhitesTurn = isWhitesTurn;
            List<Move> newMoveList = new List<Move>();
            List<Cell> newCellList = new List<Cell>();

            for(short i = 0; i < 8; i++)
            {
                for(short j = 0; j < 8; j++)
                {
                    newCellList.Add(new Cell()
                    {
                        Position = new Position(i, j),
                        Type = (CellTypes)(int)CellList[i * 8 + j].Type
                    });
                }
            }

            foreach (Move move in MoveList)
            {
                List<Position> newPiecesMoved = new List<Position>();
                foreach(Position pos in move.PieceMovement)
                {
                    newPiecesMoved.Add(new Position(pos.Row, pos.Column));
                }
                List<Position> newPiecesTakenPos = new List<Position>();
                foreach (Position pos in move.PiecesTakenPos)
                {
                    newPiecesTakenPos.Add(new Position(pos.Row, pos.Column));
                }

                newMoveList.Add(new Move()
                {
                    oldPos = new Position(move.oldPos.Row, move.oldPos.Column),
                    newPos = new Position(move.newPos.Row, move.newPos.Column),
                    PiecesTaken = move.PiecesTaken,
                    PieceMovement = new List<Position>(newPiecesMoved),
                    PiecesTakenPos = new List<Position>(newPiecesTakenPos),
                    IsKingingMove = move.IsKingingMove
                });
            }

            Gameboard gameboard = new Gameboard() {
                isWhitesTurn = newIsWhitesTurn,
                CellList = newCellList,
                MoveList = newMoveList,
                ActiveCell = new Cell()
            };
            return gameboard;

        }

    }
}
