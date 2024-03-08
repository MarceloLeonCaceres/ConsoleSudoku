using SudokuLibrary.Model;

namespace SudokuLibrary.DFS
{
    public record Accion
    {
        public Celda? Celda { get; private set; }
        public int Numero { get; private set; }

        public Accion(int X, int Y, int numero)
        {
            Celda = new Celda(X, Y);
            Numero = numero;
        }

        public Accion(Celda celda, int numero)
        {
            Celda = celda;
            Numero = numero;
        }

    }
}
