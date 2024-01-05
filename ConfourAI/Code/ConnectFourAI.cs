using System;

namespace ConfourAI.Code
{
    public class ConnectFourAI
    {
        private ConnectFourBoard gameBoard;
        private int searchDepth;

        // Constructor: Initializes the AI with a game board and search depth for Minimax
        public ConnectFourAI(ConnectFourBoard gameBoard, int searchDepth = 7)
        {
            this.gameBoard = gameBoard;
            this.searchDepth = searchDepth;
        }

        // Selects the best move for the AI by evaluating all possible moves
        public int SelectOptimalMove()
        {
            int optimalColumn = -1;
            int highestScore = int.MinValue;

            // Iterate through each column to find the best move
            for (int col = 0; col < ConnectFourBoard.Columns; col++)
            {
                // Simulate a move and run Minimax
                if (gameBoard.MakeMove(col, 2)) // AI is player 2
                {
                    int score = RunMinimax(0, int.MinValue, int.MaxValue, false);
                    gameBoard.UndoMove(col); // Undo the simulated move

                    // Update the best move if a higher score is found
                    if (score > highestScore)
                    {
                        highestScore = score;
                        optimalColumn = col;
                    }
                }
            }

            return optimalColumn;
        }

        // Minimax algorithm with Alpha-Beta pruning for optimal decision making
        private int RunMinimax(int depth, int alpha, int beta, bool maximizingPlayer)
        {
            // Check for terminal conditions: maximum depth or a player has won
            if (depth == searchDepth || gameBoard.CheckWinner(1) || gameBoard.CheckWinner(2))
            {
                return BoardEvaluation();
            }

            // Maximizing player (AI)
            if (maximizingPlayer)
            {
                int bestEval = int.MinValue;
                for (int col = 0; col < ConnectFourBoard.Columns; col++)
                {
                    if (gameBoard.MakeMove(col, 2))// AI Move
                    {
                        int eval = RunMinimax(depth + 1, alpha, beta, false);
                        gameBoard.UndoMove(col);
                        bestEval = Math.Max(bestEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha)
                            break; // Alpha-Beta pruning
                    }
                }
                return bestEval;
            }
            else // Minimizing player (Opponent)
            {
                int worstEval = int.MaxValue;
                for (int col = 0; col < ConnectFourBoard.Columns; col++)
                {
                    if (gameBoard.MakeMove(col, 1))// Player Move
                    {
                        int eval = RunMinimax(depth + 1, alpha, beta, true);
                        gameBoard.UndoMove(col);
                        worstEval = Math.Min(worstEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha)
                            break; // Alpha-Beta pruning
                    }
                }
                return worstEval;
            }
        }

        // Evaluates the board and returns a score representing the state's favorability
        private int BoardEvaluation()
        {
            int boardScore = 0;
            int[,] currentLayout = gameBoard.GetBoard();

            // Center column preference and scoring for potential lines
            int centerColumn = ConnectFourBoard.Columns / 2;
            for (int r = 0; r < ConnectFourBoard.Rows; r++)
            {
                // Scoring based on center control and potential winning lines
                if (currentLayout[r, centerColumn] == 2) boardScore += 5; // AI control
                else if (currentLayout[r, centerColumn] == 1) boardScore -= 5; // Opponent control

                for (int c = 0; c < ConnectFourBoard.Columns; c++)
                {
                    if (currentLayout[r, c] != 0)
                    {
                        // Check and score potential lines for both players
                        boardScore += CheckLinePotential(currentLayout, r, c, 1, 0, currentLayout[r, c]); // Vertical
                        boardScore += CheckLinePotential(currentLayout, r, c, 0, 1, currentLayout[r, c]); // Horizontal
                        boardScore += CheckLinePotential(currentLayout, r, c, 1, 1, currentLayout[r, c]); // Diagonal right
                        boardScore += CheckLinePotential(currentLayout, r, c, 1, -1, currentLayout[r, c]); // Diagonal left
                    }
                }
            }

            return boardScore;
        }

        // Evaluates the potential of a line (horizontal, vertical, diagonal) for scoring
        private int CheckLinePotential(int[,] layout, int row, int col, int dRow, int dCol, int player)
        {
            int lineScore = 0;
            int count = 0;

            // Check up to 4 cells in a specified direction for a potential line
            for (int i = 0; i < 4; i++)
            {
                if (row >= ConnectFourBoard.Rows || col >= ConnectFourBoard.Columns || row < 0 || col < 0)
                    break;

                if (layout[row, col] == player) count++;
                else if (layout[row, col] != 0) break; // Line blocked by opponent

                row += dRow;
                col += dCol;
            }

            if (count > 0) lineScore = (int)Math.Pow(5, count - 1); // Score based on number of consecutive discs

            return lineScore * (player == 2 ? 1 : -1); // Adjust score based on the player
        }
    }
}
