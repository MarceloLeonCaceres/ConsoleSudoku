namespace SudokuLibrary.Model
{
    public class PosiblesPorCelda
    {
        public Celda CPosibles { get; set; }
        public List<int> Posibles { get; set; } = new List<int>();
        public PosiblesPorCelda(int x, int y) 
        {
            CPosibles = new Celda(x, y);
            Posibles = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

    }
}
