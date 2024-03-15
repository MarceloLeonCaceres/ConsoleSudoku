namespace SudokuLibrary.Model
{
    public class CeldaConocida
    {
        Celda CConocida { get; set; }
        public int numero { get; set; }

        public CeldaConocida(int x, int y, int num)
        {
            CConocida = new Celda(x, y);
            numero = num;
        }
    }
}
