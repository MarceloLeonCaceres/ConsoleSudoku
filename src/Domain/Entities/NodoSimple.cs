namespace Domain.Entities;

public record NodoSimple
{
    public Tablerito Tablerito { get; set; }
    public Tablerito PadreDelTablerito { get; set; }
    public IReadOnlyList<Accion>? Acciones { get; set; }

    public NodoSimple(Nodo nodo)
    {
        ArgumentNullException.ThrowIfNull(nodo, nameof(nodo));
        this.Tablerito = new Tablerito(nodo);
                
        // Note: You might need to add a constructor in Tablerito that takes a Tablero
        this.PadreDelTablerito = nodo.Padre != null ? new Tablerito(nodo.Padre) : new Tablerito();

        this.Acciones = nodo.Acciones != null ? new List<Accion>(nodo.Acciones) : null;

    }
}
