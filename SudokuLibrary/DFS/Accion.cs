using SudokuLibrary.Model;

namespace SudokuLibrary.DFS
{
    public record Accion
    {
        public Celda? Celda { get; private set; }
        public byte Numero { get; private set; }


        public Accion(Celda celda, byte numero)
        {
            Celda = celda;
            Numero = numero;
        }

    }
}
