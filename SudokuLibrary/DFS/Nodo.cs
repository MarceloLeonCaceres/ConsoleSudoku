using SudokuLibrary.Model;

namespace SudokuLibrary.DFS
{
    public class Nodo : IEquatable<Nodo>
    {
        public Tablero Tablero { get; set; }
        public Tablero? Padre { get; set; }
        public Accion? Accion { get; set; }

        public Nodo(Tablero tablero, Tablero padre, Accion accion)
        {
            Tablero = tablero;
            Padre = padre;
            Accion = accion;
        }

        public List<Nodo>? Siguientes()
        {
            List<Nodo> nodosSiguientes = new List<Nodo>();
            List<Accion> accionesSiguientes = Tablero.AccionesSiguientes();
            if(accionesSiguientes == null)
            {
                return null;
            }
            foreach(Accion accion in  accionesSiguientes)
            {
                Nodo nodoPosible = new Nodo(new Tablero(Tablero, accion), Tablero, accion);
                if (nodoPosible.Tablero.EsValida)
                {
                    nodosSiguientes.Add(nodoPosible);
                }
            }
            return nodosSiguientes;
        }

        public void Print()
        {
            Console.WriteLine(Accion);
            Tablero.PrintBoard();
        }

        public bool Equals(Nodo? other)
        {
            if(other is null) { return false; }

            if (other.Tablero.CeldasConocidas.Count != this.Tablero.CeldasConocidas.Count) return false;

            return Tablero.Equals(other.Tablero);
        }

        public static bool operator==(Nodo? nodoIzquierda, Nodo? nodoDerecha)
        {
            if(nodoIzquierda is null)
            {
                if(nodoDerecha is null)
                {
                    return true;
                }
                // Solo el lado izquierdo es null
                return false;
            }
            // Equals maneja el caso en que la derecha es null
            return nodoIzquierda.Equals(nodoDerecha);
        }
        public static bool operator !=(Nodo? nodoIzquierda, Nodo? nodoDerecha) => !(nodoIzquierda == nodoDerecha);
    }
}
