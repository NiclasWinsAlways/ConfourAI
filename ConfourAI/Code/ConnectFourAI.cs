using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfourAI.Code
{
    public class ConnectFourAI
    {
        private ConnectFourBoard board;
        private int maxDepth;

        public ConnectFourAI(ConnectFourBoard board, int maxDepth = 7)
        {
            this.board = board;
            this.maxDepth = maxDepth;
        }

        public int FindBestMove()
        {
            int bestCol = -1;
            int bestValue = int.MinValue;

            for (int col = 0; col < ConnectFourBoard.Columns; col++)
            {
                if (board.MakeMove(col, 2)) // 2 represents AI
                {
                    int moveValue = Minimax(0, int.MinValue, int.MaxValue, false);
                    board.UndoMove(col);

                    if (moveValue > bestValue)
                    {
                        bestValue = moveValue;
                        bestCol = col;
                    }
                }
            }

            return bestCol;
        }

        private int Minimax(int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            if (depth == 0 || board.CheckWinner(1) || board.CheckWinner(2)) // Assume 1 is player, 2 is AI
            {
                return EvaluateBoard();
            }

            if (isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                for (int col = 0; col < ConnectFourBoard.Columns; col++)
                {
                    if (board.MakeMove(col, 2)) // Trying a move as AI
                    {
                        int eval = Minimax(depth - 1, alpha, beta, false);
                        board.UndoMove(col);
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha)
                            break; // Alpha-Beta Pruning
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                for (int col = 0; col < ConnectFourBoard.Columns; col++)
                {
                    if (board.MakeMove(col, 1)) // Trying a move as Player
                    {
                        int eval = Minimax(depth - 1, alpha, beta, true);
                        board.UndoMove(col);
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha)
                            break; // Alpha-Beta Pruning
                    }
                }
                return minEval;
            }
        }
    }
}


