using ConfourAI.Code;

class Program
{
    static void Main(string[] args)
    {
        ConnectFourBoard board = new ConnectFourBoard();
        ConnectFourAI ai = new ConnectFourAI(board);
        bool isGameOver = false;
        int currentPlayer = 1; // Start with the human player as '1' and AI as '2'

        // Continue playing until the game is over
        while (!isGameOver)
        {
            board.PrintBoard();
            if (currentPlayer == 1) // Human player's turn
            {
                Console.WriteLine("Human player's turn. Enter a column number (1-7):");
                if (int.TryParse(Console.ReadLine(), out int humanColumn) && humanColumn >= 1 && humanColumn <= ConnectFourBoard.Columns)
                {
                    // Adjust the column to 0-based index by subtracting 1
                    humanColumn -= 1;

                    if (board.MakeMove(humanColumn, currentPlayer))
                    {
                        // If the move was successful, check for a win or a draw
                        isGameOver = board.CheckWinner(currentPlayer);
                        if (isGameOver)
                        {
                            Console.WriteLine("Human player wins!");
                            break;
                        }
                        currentPlayer = 2; // Switch to AI player
                    }
                    else
                    {
                        Console.WriteLine("Column full or invalid. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                }
            }
            else if (currentPlayer == 2) // AI player's turn
            {
                Console.WriteLine("AI player's turn.");
                int aiColumn = ai.SelectOptimalMove();
                board.MakeMove(aiColumn, currentPlayer);
                isGameOver = board.CheckWinner(currentPlayer);
                if (isGameOver)
                {
                    Console.WriteLine("AI player wins!");
                    break;
                }
                currentPlayer = 1; // Switch back to human player
            }

            // Check if the board is full to declare a draw
            if (IsBoardFull(board))
            {
                Console.WriteLine("The game is a draw!");
                isGameOver = true;
            }
        }

        // After the game is over, print the final board state
        board.PrintBoard();
    }

    // Helper method to check if the board is full
    static bool IsBoardFull(ConnectFourBoard board)
    {
        int[,] currentBoard = board.GetBoard();
        for (int col = 0; col < ConnectFourBoard.Columns; col++)
        {
            if (currentBoard[0, col] == 0) // If top row of any column is empty, board isn't full
            {
                return false;
            }
        }
        return true; // If top row of all columns are full, board is full
    }
}
