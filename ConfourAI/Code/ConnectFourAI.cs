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
        private int EvaluateBoard()
        {
            int score = 0;

            // Example: Scoring based on consecutive discs in rows, columns, and diagonals
            // You can assign different weights for 2, 3, or 4-in-a-line scenarios.
            // For instance: 3-in-a-line is more valuable than 2-in-a-line.

            for (int row = 0; row < ConnectFourBoard.Rows; row++)
            {
                for (int col = 0; col < ConnectFourBoard.Columns; col++)
                {
                    if (board[row, col] != 0) // If the cell is not empty
                    {
                        // Check horizontally
                        if (col + 3 < ConnectFourBoard.Columns)
                        {
                            score += EvaluateLine(row, col, 0, 1, board[row, col]);
                        }
                        // Check vertically (if applicable)
                        if (row + 3 < ConnectFourBoard.Rows)
                        {
                            score += EvaluateLine(row, col, 1, 0, board[row, col]);
                        }
                        // Check diagonally (down-right and up-right)
                        if (row + 3 < ConnectFourBoard.Rows && col + 3 < ConnectFourBoard.Columns)
                        {
                            score += EvaluateLine(row, col, 1, 1, board[row, col]);
                        }
                        if (row - 3 >= 0 && col + 3 < ConnectFourBoard.Columns)
                        {
                            score += EvaluateLine(row, col, -1, 1, board[row, col]);
                        }
                    }
                }
            }

            return score;
        }

        private int EvaluateLine(int row, int col, int deltaRow, int deltaCol, int player)
        {

            int score = 0;
            int consecutive = 0;

            for (int i = 0; i < 4; i++)
            {
                if (board[row + i * deltaRow, col + i * deltaCol] == player)
                {
                    consecutive++;
                }
                else if (board[row + i * deltaRow, col + i * deltaCol] != 0)
                {
                    consecutive = 0;
                    break;
                }
            }

            if (consecutive > 0)
            {
                score = (int)Math.Pow(10, consecutive - 1); // Example: 10 points for 2-in-a-line, 100 for 3-in-a-line
            }

            return player == 2 ? score : -score; // AI is player 2; invert score if it's the player's line
        }

    }
}

