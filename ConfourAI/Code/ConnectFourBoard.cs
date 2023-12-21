using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConnectFourBoard
{
    public const int Rows = 6;
    public const int Columns = 7;
    private int[,] board; // 0: empty, 1: player, 2: AI

    public ConnectFourBoard()
    {
        board = new int[Rows, Columns];
    }

    public bool MakeMove(int column, int player)
    {
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (board[row, column] == 0)
            {
                board[row, column] = player;
                return true;
            }
        }
        return false; // Column full
    }

    public void UndoMove(int column)
    {
        for (int row = 0; row < Rows; row++)
        {
            if (board[row, column] != 0)
            {
                board[row, column] = 0;
                break;
            }
        }
    }

    public bool CheckWinner(int player)
    {
        // Check horizontal and vertical
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (col + 3 < Columns &&
                    board[row, col] == player &&
                    board[row, col + 1] == player &&
                    board[row, col + 2] == player &&
                    board[row, col + 3] == player)
                    return true;

                if (row + 3 < Rows &&
                    board[row, col] == player &&
                    board[row + 1, col] == player &&
                    board[row + 2, col] == player &&
                    board[row + 3, col] == player)
                    return true;
            }
        }

        // Check diagonal
        for (int row = 0; row < Rows - 3; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (col + 3 < Columns &&
                    board[row, col] == player &&
                    board[row + 1, col + 1] == player &&
                    board[row + 2, col + 2] == player &&
                    board[row + 3, col + 3] == player)
                    return true;

                if (col - 3 >= 0 &&
                    board[row, col] == player &&
                    board[row + 1, col - 1] == player &&
                    board[row + 2, col - 2] == player &&
                    board[row + 3, col - 3] == player)
                    return true;
            }
        }

        return false;
    }
    public void PrintBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                char token = board[row, col] switch
                {
                    1 => 'X', // Player
                    2 => 'O', // AI
                    _ => '.'
                };
                Console.Write(token + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
