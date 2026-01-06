namespace SudokuLibrary.Models;

public record NodoSimple // : IComparable<NodoSimple>
{
    public Tablerito Tablerito { get; set; }
    public Tablerito PadreDelTablerito { get; set; }
    public Accion? Accion { get; set; }
    public List<Accion> Acciones { get; set; }
    public int CuentaConocidas => Tablerito.CuentaConocidas;

    //public NodoSimple() { }
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
        this.Tablerito = new Tablerito(nodo);
        
        if (nodo.Padre != null)
        {
            // We construct a Tablerito from the parent directly
            // Note: You might need to add a constructor in Tablerito that takes a Tablero
            this.PadreDelTablerito = new Tablerito
            {
                Board = (byte[,])nodo.Padre.Board.Clone(),
                CuentaConocidas = nodo.Padre.CeldasConocidas.Count
            };
        }
        else
        {
            this.PadreDelTablerito = new Tablerito(); // Empty root
        }

        this.Acciones = nodo.Acciones != null ? new List<Accion>(nodo.Acciones) : null;
        //this.PadreDelTablerito = new Tablerito();            
        //this.Tablerito.Board = (byte[,])nodo.Tablero.Board.Clone();
        //this.Tablerito.CuentaConocidas = nodo.Tablero.CeldasConocidas.Count();
        //if (nodo.Padre == null)
        //{
        //    this.PadreDelTablerito.Board = null;
        //}
        //else
        //{
        //    this.PadreDelTablerito.Board = (byte[,])nodo.Padre.Board.Clone();
        //}
        //if(nodo.Acciones != null)
        //{
        //    this.PadreDelTablerito.CuentaConocidas = this.Tablerito.CuentaConocidas - nodo.Acciones.Count;
        //}
        //this.Acciones = nodo.Acciones;
        //return;
    }

    public int CompareTo(NodoSimple? other)
    {
        if(other == null)
        {
            return 1;
        }
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
