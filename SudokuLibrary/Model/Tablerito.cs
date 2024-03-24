using SudokuLibrary.DFS;

namespace SudokuLibrary.Model
{
    public class Tablerito : IComparable<Tablerito>
    {
        public byte[,] Board { get; set; }
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
    }
}
