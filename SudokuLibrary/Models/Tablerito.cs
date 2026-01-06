namespace SudokuLibrary.Models;

public class Tablerito : IComparable<Tablerito>, IEquatable<Tablerito>
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

    public Tablerito(Tablerito hijo, Accion accion) 
    {
        this.Board = (byte[,])hijo.Board.Clone();
        this.Board[accion.Celda.X, accion.Celda.Y] = 0;
        this.CuentaConocidas = hijo.CuentaConocidas - 1;
    }
    
    public int CompareTo(Tablerito? other)
    {
        if(this.CuentaConocidas < other.CuentaConocidas)
        {
            return -1;
        }
        else if (this.CuentaConocidas < other.CuentaConocidas)
        {
            return 1;
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this.Board[i, j] < other.Board[i, j])
                    {
                        return -1;

                    }
                    else if (this.Board[i, j] > other.Board[i, j])
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(CuentaConocidas);

        // We don't hash every cell (too slow), but we sample the diagonals
        // or a few rows to create a distinct signature.
        // For 100% correctness, you might need to hash more, 
        // but for Sudoku, hashing the Count + Center + Corners is usually enough to avoid collisions.
        if (Board != null)
        {
            hash.Add(Board[0, 0]);
            hash.Add(Board[4, 4]);
            hash.Add(Board[8, 8]);
            hash.Add(Board[0, 8]);
            hash.Add(Board[8, 0]);
        }
        return hash.ToHashCode();
    }
    public override bool Equals(object? obj)
    {
        return Equals(obj as Tablerito);
    }
    public bool Equals(Tablerito? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (this.CuentaConocidas != other.CuentaConocidas) return false;

        // Delegate to the shared static comparison logic to stay DRY
        return SudokuComparer.AreBoardsEqual(this.Board, other.Board);
    }
}
