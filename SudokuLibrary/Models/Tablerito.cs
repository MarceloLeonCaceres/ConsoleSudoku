namespace SudokuLibrary.Models;

public class Tablerito : IEquatable<Tablerito>, IComparable<Tablerito>
{
    public byte[,]? Board { get; set; }
    public int CuentaConocidas { get; set; }
    public Tablerito()
    {   
    }

    public Tablerito(Nodo nodo)
    {
        this.Board = (byte[,])nodo.Tablero.Board.Clone();
        this.CuentaConocidas = nodo.Tablero.CeldasConocidas.Count();
    }

    public int CompareTo(Tablerito? other)
    {
        if (other is null) return -1;
        return BoardsComparer.CompareBoards(
            this.Board, other.Board, this.CuentaConocidas, other.CuentaConocidas );

    }

    //public override int GetHashCode()
    //{
    //    var hash = new HashCode();
    //    hash.Add(CuentaConocidas);

    //    // We don't hash every cell (too slow), but we sample the diagonals
    //    // or a few rows to create a distinct signature.
    //    // For 100% correctness, you might need to hash more, 
    //    // but for Sudoku, hashing the Count + Center + Corners is usually enough to avoid collisions.
    //    if (Board != null)
    //    {
    //        hash.Add(Board[0, 0]);
    //        hash.Add(Board[4, 4]);
    //        hash.Add(Board[8, 8]);
    //        hash.Add(Board[0, 8]);
    //        hash.Add(Board[8, 0]);
    //    }
    //    return hash.ToHashCode();
    //}

    public bool Equals(Tablerito? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (this.CuentaConocidas != other.CuentaConocidas) return false;

        // Delegate to the shared static comparison logic to stay DRY
        return BoardsComparer.AreBoardsEqual(this.Board, other.Board);
    }
}
