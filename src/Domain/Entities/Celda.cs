namespace Domain.Entities;

public record Celda
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public Celda(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Celda(int x, int y)
    {
        if (x < 0 || x >= 9)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "La fila debe estar entre 0 y 8");
        }
        if ( y < 0 || y >= 9)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "La columna debe estar entre 0 y 8");
        }
        X = x;
        Y = y;
        Z = ( x /3) * 3 + (y / 3);
        //Z = SudokuAbstraction.Cuadro(x, y);
    }

}
