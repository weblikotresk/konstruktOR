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
        private List<Cell> CellList = new List<Cell>(); //список всех клеток доски
        public List <Move> MoveList = new List<Move>();//список всех ходов на данный момент

        //флаг последовательной атаки белых (белые едят 2 или более шашки за один ход)
        public bool ContinuedWhiteAttack = false; 
        //флаг хода белых
        public bool isWhitesTurn = true;


        //активная клетка на данный момент (нужна для проверки хода игрока)
        public Cell ActiveCell = new Cell();
        //последний сделанный компьютером ход (для Combinations.cs)
        public Move LastMove = new Move();



        private readonly int PieceValue = 200; // стоимость простой шашки
        private readonly int SupremeValue = 500; // стоимость дамки
        private readonly float[,] CellBonus = new float[8, 4] // бонус клетки
            {
        { 1.1f, 1.1f, 1.1f, 1.1f },
        { 1.05f, 1.1f, 1.1f, 1.05f },
        { 1.05f, 1.1f, 1.1f, 1.03f },
        { 1.0f, 1.1f, 1.05f, 1.0f },
        { 1.0f, 1.1f, 1.1f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
        { 1.0f, 1.0f, 1.0f, 1.0f },
            };

        private readonly float[] YBonus = new float[8]{ 1.0f, 1.025f, 1.05f, 1.075f, 1.1f, 1.125f, 1.15f, 1.175f}; // бонус по вертикали

        //METHODS


        //----flags----

        //полученная позиция находится внутри границ доски
        public bool IsInBounds(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }
        //полученные позиции отличаются цветом
        public bool IsDifferentColor(Position pos1, Position pos2)
        {
            bool isBlack = CellList[pos1.Index].Type == CellTypes.Black || CellList[pos1.Index].Type == CellTypes.BlackSupreme;
            bool isWhite = CellList[pos1.Index].Type == CellTypes.White || CellList[pos1.Index].Type == CellTypes.WhiteSupreme;
            if (isBlack)
            {
                return CellList[pos2.Index].Type != CellTypes.Black && CellList[pos2.Index].Type != CellTypes.BlackSupreme;
            }
            else if (isWhite)
            {
                return CellList[pos2.Index].Type != CellTypes.White && CellList[pos2.Index].Type != CellTypes.WhiteSupreme;
            }
            return false;
           
        }
        //в полученной позиции нет фигуры
        public bool IsEmpty(Position pos)
        {
            return CellList[pos.Index].Type == CellTypes.Empty;
        }
        //полученная позиция превращает фигуру в дамку
        public bool IsKinging(Position to, bool IsWhite)
        {
            return IsWhite ? to.Row == 0 : to.Row == 7;
        }

        //----interface----

        //подсвечивает возможные ходы для конкретного списка ходов
        public void HighlightMoves(List<Move> availableMovesList)
        {
            foreach (Move move in availableMovesList)
            {
                CellList[move.PieceMovement[1].Index].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                SetCellType(move.PieceMovement[1], CellTypes.Highlighted);
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
        public void RenderImage(CellTypes Type, Position pos, Position TurnSupremePos, double width)
        {
            if (TurnSupremePos!=null && TurnSupremePos.Index == pos.Index)
            {
                CellList[pos.Index].Content.Content = new Image
                {
                    Width = width,
                    Source =
                Type+3 == CellTypes.BlackSupreme ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black_supreme.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white_supreme.png"))
                };
            }
            else
            {
                if ((int)Type > 3)
                {
                    CellList[pos.Index].Content.Content = new Image
                    {
                        Width = width,
                        Source =
                    Type == CellTypes.BlackSupreme ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black_supreme.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white_supreme.png"))
                    };
                }
                else
                {
                    CellList[pos.Index].Content.Content = new Image
                    {
                        Width = width,
                        Source =
                    Type == CellTypes.Black ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
                    };
                }
                
            }
            
        }

        //уберём наполнение клетки и изменим её тип на Empty
        public void RemovePiece(int index)
        {
            CellList[index].Content.Content = null;
            CellList[index].Type = CellTypes.Empty;
        }




        // ----getters & setters----
        public CellTypes GetCellType(Position pos)
        {

            return CellList[pos.Index].Type;
        }
        public void SetCellType(Position pos, CellTypes type)
        {
            CellList[pos.Index].Type = type;
        }



        // ----board interaction----

        //рассчитаем статическое значение доски (для Combinations.cs) 
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
        //получить состояние игры
        public GameState GetGameState()
        {
            //посчитаем количество шашек на доске
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

                
            }

            //если шашек стороны нет, игра окончена
            if (blackCounter == 0)
            {
                return GameState.WhiteWon;
            }
            else if (whiteCounter == 0)
            {
                return GameState.BlackWon;
            }
            else
            //проверим возможность хода для обеих сторон
            {   //В ComputerMove() isWhitesTurn = true
                //в ActiveBestMoveSearch() isWhitesTurn = false

                FindAllMoves();
                if (MoveList.Count == 0)
                    return isWhitesTurn ? GameState.BlackWon : GameState.WhiteWon;
                
                if (MoveList.Count == 0)
                {
                    return GameState.WhiteWon;
                }

                return GameState.InProgress;
            }
            
           

            

        }
        //проверим, не закончилась ли игра
        public void AnybodyWon()
        {
            GameState state = GetGameState();
            if ((int)state > 0)
            {//если закончилась, выведем сообщение
                MessageBox.Show(state == GameState.BlackWon ? "The blacks won. Press 'OK' to start new game." : "You won! Press 'OK' to start new game.");
                System.Windows.Application.Current.Shutdown();
            }
        }
       
        public Cell MovePiece(List<Cell> CellList, Position posFrom, Position posTo, Position taken)
        {
            //уберём все съеденные фигуры
            if (taken != null)
            {
                CellList[taken.Index].Type = CellTypes.Empty;
            }

            //проверка на дамку в тернарном операторе
            CellList[posTo.Index].Type = CellList[posFrom.Index].Type + (IsKinging(posTo, CellList[posFrom.Index].Type == CellTypes.White)? 3:0);
            CellList[posFrom.Index].Type = CellTypes.Empty;

            return CellList[posTo.Index];
        }

        //установим для хода позицию, в которой шашка становится дамкой 
        public Position GetTurningSupremePos(Move move)
        {
            //если внутри данного хода шашка превращается в дамку
            if (move.IsKingingMove && (int)GetCellType(move.oldPos)<=3)
            {//пройдём по всем положениям шашки в ходе
                foreach (Position pos in move.PieceMovement)
                {   //если эта позиция в верхнем ряду, то установим её в IsKingingMove
                    if (IsKinging(pos, GetCellType(move.oldPos) == CellTypes.White)){
                        return pos;
                    }
                }
            }
            return null;
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
                takingMoves.Sort((Move a, Move b) => -a.PiecesTakenPos.Count.CompareTo(b.PiecesTakenPos.Count));

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
            if ((CellList[currPos.Index].Type == CellTypes.White || CellList[currPos.Index].Type == CellTypes.WhiteSupreme) && isWhitesTurn)
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
                takingMoves.Sort((Move a, Move b) => -a.PiecesTakenPos.Count.CompareTo(b.PiecesTakenPos.Count));
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
            //если фигура - дамка
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
                                    AddMove(cell.Position, target, pos_opp); // Косвенно рекурсивно продолжаем поиск дальнейших атак
                            }
                            else if (!IsDifferentColor(target, cell.Position)) // Если уперлись в свою шашку — то прекратим цикл
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

                        if (IsInBounds(AttackPos) && IsEmpty(AttackPos) && !(IsEmpty(JumpOverPos)) && IsDifferentColor(cell.Position, JumpOverPos))
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
                // Турецкий удар
                foreach(Position pos in exc)
                {
                    if (taken.Equals(pos))
                    {
                        return false;
                    }
                }

                // Моделируем доску, на которой этот ход сделан
                Gameboard nextBoard = DeepCopy();
                //проверяем, превращает ли этот ход фигуру в дамку
                bool isThisMoveKinging = !((int)cell.Type > 3) && IsKinging(to, cell.Type == CellTypes.White);
                //передвинем шашку на виртуальной доске
                Cell thisCell = MovePiece(nextBoard.CellList, fr, to, taken);
                
                List<Position> newExc = new List<Position>(exc){taken};
                //рекурсивно продолжим искать последующие атакующие ходы относительно новой позиции
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

        // Поиск всех простых ходов шашки
        private List<Move> GetAllSimpleMovesOfPiece(Cell cell)
        {
            List<Move> moves = new List<Move>();
            //на клетке дамка
            if ((int)(cell.Type) > 3)
            {
                for (int x = 1; x > -2; x -= 2)
                {
                    for (int y = 1; y > -2; y -= 2)
                    {
                        // Куда дамка встанет после прыжка
                        Position target = new Position(cell.Position.Row + x, cell.Position.Column + y);
                        while (IsInBounds(target)) // Функция IsInBounds проверяет, что позиция принадлежат полю
                        {
                            if (IsEmpty(target)) // Функция IsEmpty проверяет, что поле пустое
                            {
                                AddMoveToList(moves, cell.Position, target);
                            }
                            else if (!IsDifferentColor(target, cell.Position)) // Если уперлись в свою шашку — конец цикла
                                break;
                            //прибавим единичные поля и продолжим поиск
                            target = new Position(target.Row + x, target.Column + y);
                        }
                    }
                }

            }
            else
            {
                if (isWhitesTurn)
                {
                    //Используем условные обозначения для простоты понимания: NE, SE, SW, NW - северо-восточный ход и т.д.
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
            }
            return moves;

        }

        //добавить ход в общий список ходов (Для GetAllSimpleMovesOfPiece())
        private void AddMoveToList(List<Move> list, Position oldPos, Position newPos)
        {
            Move newMove = new Move(
                       oldPos,
                       newPos,
                       !((int)GetCellType(oldPos) > 3) && IsKinging(newPos, GetCellType(oldPos) == CellTypes.White),
                       null
                       );
            newMove.TurningSupremePos = GetTurningSupremePos(newMove);

            if (IsInBounds(newPos) && GetCellType(newPos) == 0)
                list.Add(newMove);
        }

        //ход компьютера на виртуальной доске (для Combinations.cs)
        public void MakeComputerMove(Gameboard board, Move move, bool memoriseMove)
        {   // запоминаем ход, если необходимо
            if (memoriseMove)
            {
                board.LastMove = move;
            }
            //если в этом ходе шашка превращается в дамку, изменим тип через тернарный оператор
            board.CellList[move.newPos.Index].Type = board.CellList[move.oldPos.Index].Type+ (move.IsKingingMove? 3:0);
            board.CellList[move.oldPos.Index].Type = CellTypes.Empty;

        }

        //отмена последнего хода на виртуальной доске (для Combinations.cs)
        public void UnmakeLastMove()
        {
            if (LastMove.IsKingingMove)
            {
                CellList[LastMove.oldPos.Index].Type = CellList[LastMove.newPos.Index].Type - 3;
                CellList[LastMove.newPos.Index].Type = CellTypes.Empty;
            }
            else
            {
                CellList[LastMove.oldPos.Index].Type = CellList[LastMove.newPos.Index].Type;
                CellList[LastMove.newPos.Index].Type = CellTypes.Empty;
            }
            
        }

        //установка флагов атаки тем клеткам, у которых есть доступная атака в списке ходов
        //возвращает 1, если есть атакующие фигуры
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
            //компьютер ходит
            if (move.PiecesTakenPos.Count !=0 && !isWhitesTurn)
            {
                //убирает все съеденные шашки
                foreach (Position pos in move.PiecesTakenPos)
                    RemovePiece(pos.Index);

                RenderImage(CellList[move.oldPos.Index].Type, move.PieceMovement.Last(), (move.IsKingingMove ? move.PieceMovement.Last() : null), 40);
                //ставит бьющую шашку на последнее место, изменяем на дамку, если необходимо
                SetCellType(move.PieceMovement.Last(), CellList[move.oldPos.Index].Type + (GetTurningSupremePos(move) != null ? 3 : 0));
                RemovePiece(move.oldPos.Index);
                
            }//атакующий ход человека
            else if (move.PiecesTakenPos.Count != 0 && isWhitesTurn)
            {
                //убирает первую съеденную шашку
                RemovePiece(move.PiecesTakenPos[0].Index);
                //уберём её из списка съеденных фигур
                move.PiecesTakenPos.Remove(move.PiecesTakenPos[0]);

                //ставит бьющую шашку на первое из атакующих место
                //проверим, не является ли этот ход дамочным
                move.IsKingingMove = IsKinging(move.PieceMovement[1], GetCellType(move.PieceMovement[0]) == CellTypes.White);
                bool TurnToSupreme = GetTurningSupremePos(move) == move.PieceMovement[1];

                RenderImage(CellList[move.PieceMovement[0].Index].Type, move.PieceMovement[1], GetTurningSupremePos(move), 40);
                SetCellType(move.PieceMovement[1], CellList[move.PieceMovement[0].Index].Type + (TurnToSupreme?3:0));
                RemovePiece(move.PieceMovement[0].Index);
                //уберём исходную позицию из листа
                move.PieceMovement.Remove(move.PieceMovement[0]);

                //если есть ещё не побитые шашки, изменим ContinuedWhiteAttack
                ContinuedWhiteAttack = move.PiecesTakenPos.Count != 0;
                if (ContinuedWhiteAttack) {
                    //изменим флаг атаки
                    CellList[move.PieceMovement[0].Index].IsToAttack = true;
                    //изменим активную клетку, относительно которой проверяются ходы в CellClicked при нажатии на подсвеченную клетку
                    ActiveCell.Position = new Position(move.PieceMovement[0].Row, move.PieceMovement[0].Column);
                } 
            }
            else
            {//обычный ход пользователя на пустую клетку

                //проверим, не является ли этот ход дамочным
                move.IsKingingMove = IsKinging(move.newPos, GetCellType(move.oldPos) == CellTypes.White);
                //считается ли следующая позиция дамочной
                bool TurnToSupreme = GetTurningSupremePos(move) == move.PieceMovement[1];
                //отрисуем новую картинку шашки
                RenderImage(CellList[move.oldPos.Index].Type, move.newPos, GetTurningSupremePos(move), 40);
                //изменим тип клетки, на которую ходит игрок
                SetCellType(move.newPos, CellList[move.oldPos.Index].Type + (TurnToSupreme ? 3 : 0));
                //уберём старую фигуру
                RemovePiece(move.oldPos.Index);
            }
            //очистим выделенные синим клетки
           foreach(Cell cell in CellList)
            {
                if(cell.Type == CellTypes.Highlighted)
                {
                    cell.Type = CellTypes.Empty;
                    cell.Content.Style = ButtonStyles.Instance.grayCellStyle;
                }
            }

        }
        //убрать флаги атак у клеток
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

        //точка входа компьютерного хода
        public void ComputersTurn(Gameboard gameboard)
        {
            //проверим, никто ли не выиграл к этому моменту
            AnybodyWon();
            Combinations computerCombos = new Combinations(gameboard.DeepCopy());
            //используя метод ActiveBestMoveSearch, найдём лучший ход для компьютера
            computerCombos.ActiveBestMoveSearch();
            //выполним лучший ход в интерфейсной доске
            MakeMoveOnBoard(computerCombos.CurrentBestMove);
            //смена хода
            isWhitesTurn = true;
        }
        //точка входа подсветки ходов игрока для конкретной фигуры
        public void PlayersTurn(Position clickedButtonPosition)
        {
            //проверим, есть ли атакующие ходы
            FindAllMoves();
            bool AnyToAttack = PiecesToAttack(MoveList);
            //позволять ход только тем шашкам, что могут бить 
            if (AnyToAttack)
            {
                if (CellList[clickedButtonPosition.Index].IsToAttack)
                {
                    HighlightMoves(FindAllMoves(clickedButtonPosition));
                }
                else
                {
                    MessageBox.Show("You must attack. Please, choose another piece.");
                }
            }//если обычный ход, просчитаем ходы и подсветим
            else HighlightMoves(FindAllMoves(clickedButtonPosition));
            //уберём флаги атаки
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
        {   //получим позицию кнопки, с которой ушёл фокус
            Button lostButton = (Button)sender;
            Position lostButtonPos = new Position(Grid.GetRow(lostButton), Grid.GetColumn(lostButton));
            //IsPieceClicked -  нажатая клетка содержит фигуру
            bool IsPieceClicked = (int)GetCellType(lostButtonPos) !=3 && (int)GetCellType(lostButtonPos) != 0;
            //применим стиль к данной кнопке
            if (IsPieceClicked) lostButton.Style = ButtonStyles.Instance.grayCellStyle;

            foreach (Cell gridButton in CellList)
            {
                //если кнопка была подсвечена, вернуть ей серый стиль
                if (GetCellType(gridButton.Position) == CellTypes.Highlighted) gridButton.Content.Style = ButtonStyles.Instance.grayCellStyle;
                //удалим непрозрачность
                if (IsPieceClicked) gridButton.Content.Opacity = 1;
            }
        }

        public void CellClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Position clickedButtonPos = new Position(Grid.GetRow(clickedButton), Grid.GetColumn(clickedButton));
            //IsPieceClicked - нажата только фигура игрока
            bool IsPieceClicked = GetCellType(clickedButtonPos) == CellTypes.White || GetCellType(clickedButtonPos) == CellTypes.WhiteSupreme; 

            if (IsPieceClicked) clickedButton.Style = ButtonStyles.Instance.clickedCellStyle;

            
            //игрок нажимает на подсвеченную клетку
            if (GetCellType(clickedButtonPos) == CellTypes.Highlighted)
            {
                //пройдём по списку ходов до тех пор, пока не найдём тот, что делает игрок
                foreach(Move move in MoveList)
                {
                    //если ход совпадает, реализовать его в интерфейсе
                    if(move.PieceMovement[0].Index == ActiveCell.Position.Index && move.PieceMovement[1].Index == clickedButtonPos.Index)
                        MakeMoveOnBoard(move.DeepCopyMove());
                }

                if (!ContinuedWhiteAttack)
                {
                    isWhitesTurn = false;
                    ComputersTurn(this);
                    AnybodyWon();
                    
                }

            }

            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    //очистим уже подсвеченные клетки
                    if (GetCellType(new Position(i,j)) == CellTypes.Highlighted) SetCellType(new Position(i, j), 0);
                    //включим непрозрачность поля
                    if (IsPieceClicked)
                        CellList[i * 8 + j].Content.Opacity = (i != clickedButtonPos.Row || j != clickedButtonPos.Column) ? 0.65 : 1;
                }
            }

            //игрок выбрал шашку
            if (IsPieceClicked && isWhitesTurn) PlayersTurn(clickedButtonPos);
        }


        // ----constructors----
        public Gameboard(){
        
        }

        //deep copy constructor
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
