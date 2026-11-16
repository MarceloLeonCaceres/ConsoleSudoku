using Domain.Services;

namespace Domain.Entities;

public class Nodo
{
    public Tablero Tablero { get; set; }
    public Tablero? Padre { get; set; }
    public IReadOnlyList<Accion>? Acciones { get; set; }

    public Nodo(Tablero tablero, Tablero? padre, List<Accion>? acciones)
    {
        Tablero = tablero ?? throw new ArgumentNullException(nameof(tablero));
        Padre = padre;
        Acciones = acciones != null ? new List<Accion>(acciones).AsReadOnly() : null;
    }

    public List<Nodo>? Siguientes(ISudokuValidator validator)
    {

        var (sonUnicas, accionesSiguientes) = Tablero.AccionesDicSiguientes();
        if (accionesSiguientes != null && accionesSiguientes.Count > 0)
        {
            List<Nodo> nodosSiguientes = new List<Nodo>();
            if (sonUnicas)
            {
                Tablero tableroMultiple = new Tablero(Tablero, null!, validator);
                foreach (Accion accion in accionesSiguientes)
                {
                    tableroMultiple = new Tablero(tableroMultiple, accion, validator);
                }
                Nodo nodoMultiple = new Nodo(tableroMultiple, Tablero, accionesSiguientes);
                nodosSiguientes.Add(nodoMultiple);
                return nodosSiguientes;
            }
            foreach (Accion accion in accionesSiguientes)
            {
                Nodo nodoPosible = new Nodo(new Tablero(Tablero, accion, validator), Tablero, new List<Accion> { accion });
                if (nodoPosible.Tablero.EsValida && nodoPosible.Tablero.EsViable)
                {
                    nodosSiguientes.Add(nodoPosible);
                }
            }
            return nodosSiguientes;
        }
        return null;
    }

    //public void Print()
    //{
    //    Tablero.PrintBoard();
    //}

}
