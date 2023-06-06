using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;


namespace Checkers
{

    //используем минимакс с альфа-бета отсечением и deepening search (insurance policy)
    public class Combinations
    {
        public Move CurrentBestMove;
        public Gameboard InitialBoard;
        public DateTime SearchStartTime;
        //public DateTime SearchEndTime = new DateTime().AddSeconds(1);
        public int ActiveSearchDepth=5;
        double Infinity = double.MaxValue;
        double NInfinity = double.MinValue;

        public Combinations(Gameboard initBoard)
        {
            InitialBoard = initBoard;
        }

        // Функция активного поиска хода запускает поиск лучшего ход в позиции

        //entry point - ActiveBestMoveSearch
        public void ActiveBestMoveSearch()
        {
            int depth = 0, startDepth = 2;
            CurrentBestMove = new Move();

            //find all possible black moves for the initial board
            InitialBoard.FindAllMoves();

            // Единственный в позиции ход делается без вычислений
            if (InitialBoard.MoveList.Count == 1)
            {
                CurrentBestMove = InitialBoard.MoveList[0];
                return;
            }

            //foreach(Move move in InitialBoard.MoveList)
            //{

            //}

            // Делаем копию доски, на которой будет проводить анализ
            // Это нужно, так как во время анализа мы будем передвигать фигуры
            Gameboard boardCopy = InitialBoard.DeepCopy();
            SearchStartTime = DateTime.Now;



            
            // entry point to computing moves
            IterativeDeepeningMinimax(boardCopy, 10000, startDepth, ActiveSearchDepth, ref CurrentBestMove, ref depth, true);

            // if the move hadn't been found after the search, it's being selected randomly out of the MoveList
            if (CurrentBestMove == new Move())
                CurrentBestMove = boardCopy.MoveList[new System.Random().Next(0, boardCopy.MoveList.Count)];
        }

        // Функция минимакса с итеративным углублением: запускает минимакс со все большей и большей глубиной,
        // при этом следя за ограничением во времени
        public void IterativeDeepeningMinimax(Gameboard board, float timeLimit, int minDepth, int maxDepth, ref Move bestMove, ref int depth, bool isWhileActiveSearch)
        {
            
            for (depth = minDepth; depth <= maxDepth; depth++)
            {
                Gameboard boardCopy = board.DeepCopy();
                (float eval, Move tempBestMove) = Minimax(boardCopy, depth, (int)NInfinity, (int)Infinity, boardCopy.isWhitesTurn, timeLimit);
                // Если успели полностью завершить итерацию, сохраняем ее результат
                if ((DateTime.Now - SearchStartTime).TotalSeconds < timeLimit && !(tempBestMove is null) && tempBestMove != new Move())
                {
                    bestMove = new Move(tempBestMove);
                }
                // Если не успели и итерация завершилась экстренно, она неполная и ее результат нам не нужен
                else
                {
                    depth -= 1;
                    break;
                }

                // Мы перестаем искать, если на какой-то итерации найдем форсированный выигрыш
                if (eval >= Infinity && board.isWhitesTurn || eval <= NInfinity && !board.isWhitesTurn)
                    break;
            }
        }
        int counter = 0;

        // Функция минимакса находит лучший ход в позиции за конкретного игрока
        // Возвращает сам ход, а также оценку позиции, которая получится, если этот ход сделать
        // depth показывает, на сколько еще итераций-рекурсий нам осталось углубиться (с каждым новым рекурсивным вызовом depth уменьшается)
        // maximizingPlayer показывает, за какого игрока мы ищем лучший ход, т.е. позицию для какого игрока мы пытаемся улучшить
        public (float, Move) Minimax(Gameboard gameboard, int depth, float alpha, float beta, bool maximizingPlayer, float timeLimit) //проблема здесь!
        {
            gameboard.isWhitesTurn = maximizingPlayer;
            gameboard.FindAllMoves();
            // Проверка времени 
            if ((DateTime.Now - SearchStartTime).TotalSeconds >= timeLimit)
                return (0, null);
            
            // Проверяем нынешнее состояние игры
            GameState state = gameboard.GetGameState();
            if (state != GameState.IN_PROCESS)
            {
                if (state == GameState.WHITE_WIN)
                    return ((float)Infinity + depth, new Move());
                if (state == GameState.BLACK_WIN)
                    return ((float)NInfinity - depth, new Move());
                else
                    return (0, new Move());
            }

            // Если это последняя итерация, просто возвращаем оценку позиции
            // Ход здесь не важен, так как лучшим станет именно ход, ведущий к позиции с наилучшей оценкой
            if (depth == 0)
            {
                float eval = gameboard.BoardStaticValue(gameboard);
                return (eval, new Move());
            }

            // Если ход единственный
            if (gameboard.MoveList.Count == 1)
            {
                Move move = gameboard.MoveList[0];

                gameboard.MakeComputerMove(gameboard, move, memoriseMove: true);

                //gameboard.OnMoveFinished(move);//??

                float eval = Minimax(gameboard, depth, alpha, beta, !maximizingPlayer, timeLimit).Item1;
                
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11здесь на выходе другая доска
                gameboard.UnmakeLastMove();
                //_transpositions.Add(new Transposition(PositionCache, eval, (int)Infinity, move));
                return (eval, move);
            }


            // Ищем лучший ход (за белых) - непосредственно реализация минимакса
            Move bestMove = new Move();
            if (maximizingPlayer)
            {
                float maxEval = (float)NInfinity;
                // Проходимся по всем ходам
                foreach (Move move in gameboard.MoveList)
                {
                    // Делаем его

                    //в первый же заход поле уже изменено до 
                    gameboard.MakeComputerMove(gameboard, move, memoriseMove: true);
                    //gameboard.OnMoveFinished(move);
                    counter++;
                    // И запускаем минимакс из полученной позиции, но со стороны ПРОТИВНИКА
                    (float eval, Move compMove) = Minimax(gameboard, depth - 1, alpha, beta, false, timeLimit);

                    // Отменяем сделанный ход

                    //gameboard.UnmakeLastMove();

                    // Проверка, что минимакс со стороны противника не завершился экстренно из-за нехватки времени
                    if (compMove == null)
                        return (0, null);
                    

                    // Проверяем, является ли этот ход лучше наилучшего найденного
                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestMove = move;
                    }
                    //альфа-отсечение
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }
                return (maxEval, bestMove);
            }
            // Аналогично за черных
            else
            {
                float minEval = (float)Infinity;
                foreach (Move move in gameboard.MoveList)
                {
                    gameboard.MakeComputerMove(gameboard, move, memoriseMove: true);
                    //gameboard.OnMoveFinished(move);

                    (float eval, Move compMove) = Minimax(gameboard, depth - 1, alpha, beta, true, timeLimit);

                    //gameboard.UnmakeLastMove();

                    if (compMove == null)
                        return (0, null);
                    if (eval < minEval)
                    {
                        minEval = eval;
                        bestMove = move;
                    }
                    //бета-отсечение
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }
                return (minEval, bestMove);
            }
        }

    }
}
