namespace SudokuLibrary.Models;

public record NodoSimple
{
    public Tablerito Tablerito { get; set; }
    public Tablerito PadreDelTablerito { get; set; }
    public List<Accion> Acciones { get; set; }

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

    }

}
