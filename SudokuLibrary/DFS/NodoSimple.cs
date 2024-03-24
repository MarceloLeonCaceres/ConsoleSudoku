using SudokuLibrary.Model;

namespace SudokuLibrary.DFS
{
    public class NodoSimple : IComparable<NodoSimple>
    {
        public Tablerito Tablerito { get; set; }
        public Tablerito PadreDelTablerito { get; set; }
        public Accion? Accion { get; set; }
        public List<Accion> Acciones { get; set; }
        public int CuentaConocidas { get; set; }

        public NodoSimple() { }
        public NodoSimple(Tablerito board, Tablerito padre, Accion accion)
        {
            Tablerito = board;
            PadreDelTablerito = padre;
            Accion = accion;
        }
        public NodoSimple(Tablerito board, Tablerito padre, List<Accion> acciones)
        {
            Tablerito = board;
            PadreDelTablerito = padre;
            Acciones = new List<Accion>(acciones);
        }
        public NodoSimple(Nodo nodo)
        {
            this.Tablerito = new Tablerito();
            this.PadreDelTablerito = new Tablerito();            
            this.Tablerito.Board = (byte[,])nodo.Tablero.Board.Clone();
            this.Tablerito.CuentaConocidas = nodo.Tablero.CeldasConocidas.Count();
            if(nodo.Padre == null)
            {
                this.PadreDelTablerito.Board = null;
            }
            else
            {
                this.PadreDelTablerito.Board = (byte[,])nodo.Padre.Board.Clone();
            }
            if(nodo.Acciones != null)
            {
                this.PadreDelTablerito.CuentaConocidas = this.Tablerito.CuentaConocidas - nodo.Acciones.Count;
            }
            this.Acciones = nodo.Acciones;
            return;
        }
        //public NodoSimple(byte[,] padre, Accion accion)
        //{
        //    Tablerito = (byte[,])padre.Clone();
        //    Tablerito[accion.Celda.X, accion.Celda.Y] = accion.Numero;
        //    PadreDelTablerito = padre;
        //    Accion = accion;
        //}

        public int CompareTo(NodoSimple? other)
        {
            if (this.CuentaConocidas < other.CuentaConocidas)
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
                        if (this.Tablerito.Board[i, j] < other.Tablerito.Board[i, j])
                        {
                            return -1;

                        }
                        else if (this.Tablerito.Board[i, j] > other.Tablerito.Board[i, j])
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
