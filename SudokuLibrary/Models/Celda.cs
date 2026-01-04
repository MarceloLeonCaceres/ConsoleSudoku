namespace SudokuLibrary.Model
{
    public record Celda
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Celda(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Celda(int x, int y)
        {
            X = x;
            Y = y;
            Z = SudokuAbstraction.Cuadro(x, y);
        }
    }
}
