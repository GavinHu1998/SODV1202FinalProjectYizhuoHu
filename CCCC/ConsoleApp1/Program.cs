using System;


public class Game
{
    private Board board;
    private Player[] players;
    private int currentPlayerIndex;
    public Player Winner { get; private set; }

    public Game(Player player1, Player player2)
    {
        board = new Board();
        players = new Player[] { player1, player2 };
        currentPlayerIndex = 0;
        Winner = null;
    }

    private void SwitchPlayer()
    {
        currentPlayerIndex = 1 - currentPlayerIndex;
    }

    public void PlayTurn(int column)
    {
        Player currentPlayer = players[currentPlayerIndex];
        if (board.AddPiece(column, currentPlayer.Piece))
        {
            if (board.CheckWinner(currentPlayer.Piece))
            {
                Winner = currentPlayer;
            }
            else
            {
                SwitchPlayer();
            }
        }
        else
        {
            Console.WriteLine("Column is full. Try another column.");
        }
    }

    public bool IsGameOver()
    {
        return Winner != null || board.IsFull();
    }

    public void PrintBoard()
    {
        board.Print();
    }

    public Player CurrentPlayer => players[currentPlayerIndex];
}

public class Board
{
    private const int Rows = 6;
    private const int Columns = 7;
    private Piece[,] grid;

    public Board()
    {
        grid = new Piece[Rows, Columns];
    }

    public bool AddPiece(int column, Piece piece)
    {
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (grid[row, column] == null)
            {
                grid[row, column] = piece;
                return true;
            }
        }
        return false;
    }

    public bool CheckWinner(Piece piece)
    {
        return CheckHorizontal(piece) || CheckVertical(piece) || CheckDiagonal(piece);
    }

    private bool CheckHorizontal(Piece piece)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns - 3; col++)
            {
                if (grid[row, col] == piece && grid[row, col + 1] == piece &&
                    grid[row, col + 2] == piece && grid[row, col + 3] == piece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckVertical(Piece piece)
    {
        for (int col = 0; col < Columns; col++)
        {
            for (int row = 0; row < Rows - 3; row++)
            {
                if (grid[row, col] == piece && grid[row + 1, col] == piece &&
                    grid[row + 2, col] == piece && grid[row + 3, col] == piece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckDiagonal(Piece piece)
    {
        for (int row = 0; row < Rows - 3; row++)
        {
            for (int col = 0; col < Columns - 3; col++)
            {
                if (grid[row, col] == piece && grid[row + 1, col + 1] == piece &&
                    grid[row + 2, col + 2] == piece && grid[row + 3, col + 3] == piece)
                {
                    return true;
                }
            }
        }

        for (int row = 3; row < Rows; row++)
        {
            for (int col = 0; col < Columns - 3; col++)
            {
                if (grid[row, col] == piece && grid[row - 1, col + 1] == piece &&
                    grid[row - 2, col + 2] == piece && grid[row - 3, col + 3] == piece)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsFull()
    {
        for (int col = 0; col < Columns; col++)
        {
            if (grid[0, col] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void Print()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (grid[row, col] == null)
                {
                    Console.Write(".");
                }
                else
                {
                    Console.Write(grid[row, col].Color[0]);
                }
            }
            Console.WriteLine();
        }
    }
}

public class Player
{
    public string Name { get; private set; }
    public Piece Piece { get; private set; }

    public Player(string name, Piece piece)
    {
        Name = name;
        Piece = piece;
    }
}

public class Piece
{
    public string Color { get; private set; }

    public Piece(string color)
    {
        Color = color;
    }

    public override bool Equals(object obj)
    {
        if (obj is Piece piece)
        {
            return Color == piece.Color;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Color.GetHashCode();
    }
}


class Program
{
    static void Main(string[] args)
    {
        Player player1 = new Player("Player 1", new Piece("Red"));
        Player player2 = new Player("Player 2", new Piece("Yellow"));
        Game game = new Game(player1, player2);

        while (!game.IsGameOver())
        {
            game.PrintBoard();
            Player currentPlayer = game.CurrentPlayer;
            Console.Write($"{currentPlayer.Name}, choose a column (0-6): ");
            if (int.TryParse(Console.ReadLine(), out int column) && column >= 0 && column <= 6)
            {
                game.PlayTurn(column);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 6.");
            }
        }

        game.PrintBoard();

        if (game.Winner != null)
        {
            Console.WriteLine($"{game.Winner.Name} wins!");
        }
        else
        {
            Console.WriteLine("It's a draw!");
        }
    }
}


