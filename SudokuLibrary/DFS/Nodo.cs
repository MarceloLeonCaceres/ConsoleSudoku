using SudokuLibrary.Model;

namespace SudokuLibrary.DFS
{
    public class Nodo : IComparable<Nodo>
    {
        public Tablero Tablero { get; set; }
        public Tablero? Padre { get; set; }
        public List<Accion>? Acciones { get; set; }

        public Nodo(Tablero tablero, Tablero padre, List<Accion> acciones)
        {
            Tablero = tablero;
            Padre = padre;
            if(acciones == null)
            {
                Acciones = null;
            }
            else
            {
                Acciones = new List<Accion>(acciones);
            }
        }

        //public Nodo(Tablero tablero, Tablero padre, Accion accion)
        //{
        //    Tablero = tablero;
        //    Padre = padre;
        //    Acciones = new List<Accion>();
        //    Acciones.Add(accion);
        //}

        public List<Nodo>? Siguientes()
        {
            
            var (sonUnicas, accionesSiguientes) = Tablero.AccionesDicSiguientes();
            if (accionesSiguientes != null && accionesSiguientes.Count > 0)
            {
                List<Nodo> nodosSiguientes = new List<Nodo>();
                if (sonUnicas)
                {
                    Tablero tableroMultiple = new Tablero(Tablero, null);
                    foreach (Accion accion in accionesSiguientes)
                    {
                        tableroMultiple = new Tablero(tableroMultiple, accion);
                    }
                    Nodo nodoMultiple = new Nodo(tableroMultiple, Tablero, accionesSiguientes);
                    nodosSiguientes.Add(nodoMultiple);
                    return nodosSiguientes;
                }
                foreach (Accion accion in accionesSiguientes)
                {
                    Nodo nodoPosible = new Nodo(new Tablero(Tablero, accion), Tablero, new List<Accion> { accion });
                    if (nodoPosible.Tablero.EsValida && nodoPosible.Tablero.EsViable)
                    {
                        nodosSiguientes.Add(nodoPosible);
                    }
                }
                return nodosSiguientes;
            }
            return null;
        }

        public void Print()
        {
            Tablero.PrintBoard();
        }

        public int CompareTo(Nodo? other)
        {
            return this.Tablero.CompareTo(other.Tablero);
        }

    }
}
