using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfourAI.Code
{
    public class ConnectFourBoard
    {
        public const int Rows = 7;
        public const int Columns = 6;
        private int[,] board;

        public ConnectFourBoard()
        {
            board = new int[Rows, Columns];
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public bool MakeMove(int column, int player)
        {
            // Check from the bottom of the board upwards
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, column] == 0) // If cell is empty
                {
                    board[row, column] = player; // Place the player's piece
                    return true; // Move was successful
                }
            }
            return false; // Column is full
        }

        public void UndoMove(int column)
        {
            // Remove the top piece from the column
            for (int row = 0; row < Rows; row++)
            {
                if (board[row, column] != 0) // If cell is not empty
                {
                    board[row, column] = 0; // Remove the piece
                    break; // Only remove one piece
                }
            }
        }

        public bool CheckWinner(int player)
        {
            // Check for horizontal, vertical, and diagonal wins
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    // Horizontal
                    if (col <= Columns - 4 &&
                        board[row, col] == player &&
                        board[row, col + 1] == player &&
                        board[row, col + 2] == player &&
                        board[row, col + 3] == player)
                    {
                        return true;
                    }

                    // Vertical
                    if (row <= Rows - 4 &&
                        board[row, col] == player &&
                        board[row + 1, col] == player &&
                        board[row + 2, col] == player &&
                        board[row + 3, col] == player)
                    {
                        return true;
                    }

                    // Diagonal (down-right)
                    if (row <= Rows - 4 && col <= Columns - 4 &&
                        board[row, col] == player &&
                        board[row + 1, col + 1] == player &&
                        board[row + 2, col + 2] == player &&
                        board[row + 3, col + 3] == player)
                    {
                        return true;
                    }

                    // Diagonal (up-right)
                    if (row >= 3 && col <= Columns - 4 &&
                        board[row, col] == player &&
                        board[row - 1, col + 1] == player &&
                        board[row - 2, col + 2] == player &&
                        board[row - 3, col + 3] == player)
                    {
                        return true;
                    }
                }
            }
            return false; // No winner found
        }

        public void PrintBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    char piece = board[row, col] switch
                    {
                        1 => 'X',
                        2 => 'O',
                        _ => '.'
                    };
                    Console.Write(piece + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', Columns * 2)); // Print bottom border of the board
            Console.WriteLine();
        }
    }
}