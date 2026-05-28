namespace ConnectFour;

public class GameState
{
    public enum WinState
    {
        No_Winner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    // Board is 7 columns x 6 rows (row 0 = bottom)
    public const int Columns = 7;
    public const int Rows = 6;

    private readonly int[,] _board = new int[Rows, Columns];

    private bool _winRecorded;

    public int PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; }

    // Consecutive wins tracking (additional feature)
    public int Player1ConsecutiveWins { get; private set; }
    public int Player2ConsecutiveWins { get; private set; }

    public int PlayPiece(int column)
    {
        if (column < 0 || column >= Columns)
            throw new ArgumentException("Invalid column");

        // Find the lowest empty row in the column (row 0 = bottom)
        for (int row = 0; row < Rows; row++)
        {
            if (_board[row, column] == 0)
            {
                _board[row, column] = PlayerTurn;
                CurrentTurn++;
                PlayerTurn = PlayerTurn == 1 ? 2 : 1;
                // Return 1-indexed row for CSS drop animation (drop1 through drop6)
                return Rows - row;
            }
        }

        throw new ArgumentException("Column is full");
    }

    public WinState CheckForWin()
    {
        // Check for a winner
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (_board[row, col] == 0) continue;
                int player = _board[row, col];

                // Check horizontal
                if (col + 3 < Columns &&
                    _board[row, col + 1] == player &&
                    _board[row, col + 2] == player &&
                    _board[row, col + 3] == player)
                    return SetWinner(player);

                // Check vertical
                if (row + 3 < Rows &&
                    _board[row + 1, col] == player &&
                    _board[row + 2, col] == player &&
                    _board[row + 3, col] == player)
                    return SetWinner(player);

                // Check diagonal up-right
                if (row + 3 < Rows && col + 3 < Columns &&
                    _board[row + 1, col + 1] == player &&
                    _board[row + 2, col + 2] == player &&
                    _board[row + 3, col + 3] == player)
                    return SetWinner(player);

                // Check diagonal up-left
                if (row + 3 < Rows && col - 3 >= 0 &&
                    _board[row + 1, col - 1] == player &&
                    _board[row + 2, col - 2] == player &&
                    _board[row + 3, col - 3] == player)
                    return SetWinner(player);
            }
        }

        // Check for tie (board full)
        if (CurrentTurn == Rows * Columns)
            return WinState.Tie;

        return WinState.No_Winner;
    }

    private WinState SetWinner(int player)
    {
        // Only update streak once per game (when winner is first detected)
        if (!_winRecorded)
        {
            _winRecorded = true;
            if (player == 1)
            {
                Player1ConsecutiveWins++;
                Player2ConsecutiveWins = 0;
            }
            else
            {
                Player2ConsecutiveWins++;
                Player1ConsecutiveWins = 0;
            }
        }
        return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;
    }

    public void ResetBoard()
    {
        Array.Clear(_board);
        PlayerTurn = 1;
        CurrentTurn = 0;
        _winRecorded = false;
    }
}
