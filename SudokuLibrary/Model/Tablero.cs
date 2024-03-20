using SudokuLibrary.DFS;
using System;
using System.Text;

namespace SudokuLibrary.Model
{
    public class Tablero : IEquatable<Tablero>, IComparable<Tablero>
    {
        public int[,] Board { get; set; }
        public HashSet<Celda> CeldasConocidas { get; set; }
        public Dictionary<int, PendientesByNumber> DicPendientes { get; set; }
        public Dictionary<int, List<Celda>> DicCeldasPara { get; set; }
        public bool EsValida { get; private set; } = false;
        public bool EsViable { get; set; } = true;
        public bool EsSolucion { get; set; } = false;


        public Tablero(int[,] matriz)
        {
            Board = (int[,])matriz.Clone();
            ValidaBoard();
            SetCeldasConocidas();
            SetPendientesPorNumero();
            ReducePendientes();
            SetCeldasPara_De_DicPendientes();
            if (this.EsViable)
            {
                SetDicViabilidad();
            }
        }

        public Tablero(Tablero padre, Accion accion)
        {
            
            this.Board = (int[,])padre.Board.Clone();
            this.Board[accion.Celda.X, accion.Celda.Y] = accion.Numero;
            this.CeldasConocidas = new HashSet<Celda>(padre.CeldasConocidas);
            this.EsValida = padre.EsValida;
            this.EsViable = padre.EsViable;

            this.CeldasConocidas.Add(new Celda(accion.Celda.X, accion.Celda.Y));
          
            this.DicCeldasPara = new Dictionary<int, List<Celda>>();
            foreach(int num in padre.DicCeldasPara.Keys)
            {
                this.DicCeldasPara[num] = new List<Celda>(padre.DicCeldasPara[num]);
            }

            this.DicPendientes = new Dictionary<int, PendientesByNumber>();
            foreach (int num in padre.DicPendientes.Keys)
            {
                this.DicPendientes[num] = padre.DicPendientes[num].CopyPending();
            }
            ReduceDicPendientes(accion.Celda);

            if (this.EsViable)
            {
                SetDicViabilidad();
                EstaResuelta();
            }
        }

        private void ValidaBoard()
        {
            bool esvalida = SudokuAbstraction.Valida(Board);
            if (!esvalida)
            {
                Console.WriteLine("Esta matriz no es valida");
                Console.WriteLine();
            }
            EsValida = esvalida;
        }

        private void SetDicViabilidad()
        {
            foreach (int i in DicPendientes.Keys)
            {
                if (DicPendientes[i].filas.Count != DicPendientes[i].cols.Count ||
                    DicPendientes[i].filas.Count != DicPendientes[i].grupos.Count ||
                    DicPendientes[i].filas.Count > DicCeldasPara[i].Count)
                {
                    this.EsViable = false;
                    return;
                }
            }
            this.EsViable = true;
        }

        private void EstaResuelta()
        {
            if (CeldasConocidas.Count < 81)
            {
                EsSolucion = false;
                return;
            }
            else
            {
                EsSolucion = true;
            }
        }

        private void SetCeldasConocidas()
        {
            CeldasConocidas = new HashSet<Celda>();
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (Board[i, j] > 0)
                    {
                        CeldasConocidas.Add(new Celda(i, j));
                    }
                }
            }
        }

        private void SetPendientesPorNumero()
        {
            DicPendientes = new Dictionary<int, PendientesByNumber>();
            for (int i = 1; i <= 9; ++i)
            {
                DicPendientes[i] = new PendientesByNumber();
            }
        }        

        private void SetCeldasPara_De_DicPendientes()
        {
            DicCeldasPara = new Dictionary<int, List<Celda>>();
            foreach(int num in DicPendientes.Keys)
            {
                DicCeldasPara[num] = new List<Celda>();
                foreach (int i in DicPendientes[num].filas)
                {
                    int contador = 0;
                    foreach (int j in DicPendientes[num].cols)
                    {
                        foreach (int k in DicPendientes[num].grupos)
                        {
                            if (SudokuAbstraction.Cuadro(i, j) == k && Board[i, j] == 0)
                            {
                                DicCeldasPara[num].Add(new Celda(i, j, k));
                                contador++;
                            }
                        }
                    }
                    if (contador == 0)
                    {
                        EsViable = false;
                        return;
                    }
                }
                foreach (int fila in DicPendientes[num].filas)
                {
                    if (TieneAlgunaCelda(DicCeldasPara[num], "row", fila) == false)
                    {
                        this.EsViable = false;
                        return;
                    }
                }
                foreach (int col in DicPendientes[num].cols)
                {
                    if (TieneAlgunaCelda(DicCeldasPara[num], "col", col) == false)
                    {
                        this.EsViable = false;
                        return;
                    }
                }
                foreach (int z in DicPendientes[num].grupos)
                {
                    if (TieneAlgunaCelda(DicCeldasPara[num], "grupo", z) == false)
                    {
                        this.EsViable = false;
                        return;
                    }
                }
            }
        }

        private static bool TieneAlgunaCelda(List<Celda> paraNums, string rowColZ, int numRowColZ)
        {
            if(rowColZ == "row")
            {
                foreach(Celda celda in paraNums)
                {
                    if(celda.X == numRowColZ)
                    {
                        return true;
                    }
                }
            }
            else if(rowColZ == "col")
            {
                foreach (Celda celda in paraNums)
                {
                    if (celda.Y == numRowColZ)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (Celda celda in paraNums)
                {
                    if (celda.Z == numRowColZ)
                    {
                        return true;
                    }
                }
            }
            return false;
        }        

        public List<Accion>? AccionesDicSiguientes()
        {

            List<Accion> accionesSiguientes = new List<Accion>();
            int menosInstancias = 10;
            int menosCeldasFactibles = 100;
            int numeroConMayorIndice = 0;

            //for (int num = 0; num < 9; num++)
            foreach(int num in DicPendientes.Keys)
            {
                int instanciasPendientesNum = DicPendientes[num].filas.Count;
                int countCeldasFactiblesNum = DicCeldasPara[num].Count;

                if (instanciasPendientesNum > 0 && instanciasPendientesNum <= menosInstancias)
                {
                    if (instanciasPendientesNum < menosInstancias)
                    {
                        numeroConMayorIndice = num;
                        menosInstancias = instanciasPendientesNum;
                        menosCeldasFactibles = countCeldasFactiblesNum;
                    }
                    else // (instanciasPendientesNum == menosInstancias)
                    {
                        if (countCeldasFactiblesNum < menosCeldasFactibles)
                        {
                            numeroConMayorIndice = num;
                            menosCeldasFactibles = countCeldasFactiblesNum;
                        }
                    }
                }
                foreach (int k in DicPendientes[num].grupos)
                {
                    int cuenta = DicCeldasPara[num].Where(c => c.Z == k).Count();
                    if (cuenta == 1)
                    {
                        Celda celdaUnicaPosible = DicCeldasPara[num].Where(c => c.Z == k).Select(x => x).Single();
                        Accion siguienteAccion = new Accion(celdaUnicaPosible, num);
                        accionesSiguientes.Add(siguienteAccion);
                    }
                }
            }
            if (accionesSiguientes.Count > 0)
            {
                return accionesSiguientes;
            }
            if (numeroConMayorIndice == 0)
            {
                return null;
            }

            foreach (var celda in DicCeldasPara[numeroConMayorIndice])
            {
                accionesSiguientes.Add(new Accion(celda, numeroConMayorIndice));
            }

            if (accionesSiguientes.Count > 0)
            {
                Console.WriteLine("Acciones Siguientes:");
                foreach (Accion accion in accionesSiguientes)
                {
                    Console.WriteLine(accion);
                }
                Console.WriteLine();
            }
            return accionesSiguientes;
        }


        private void ReducePendientes()
        {
            foreach (var celda in CeldasConocidas)
            {
                DicPendientes[Board[celda.X, celda.Y]].ReducePor(celda);
            }
            for(int i = 1; i <=9; i++)
            {
                if (this.DicPendientes[i].EsValido() == false)
                {
                    this.EsViable = false;
                    this.EsValida = false;
                    return;
                }
                if (this.DicPendientes[i].filas.Count == 0)
                {
                    this.DicPendientes.Remove(i);
                }
            }
        }

        private void ReduceDicPendientes(Celda celda)
        {
            int num = Board[celda.X, celda.Y];
            this.DicPendientes[num].ReducePor(celda);

            List<Celda> listaParaEliminar = new List<Celda>();
            foreach (Celda potencial in this.DicCeldasPara[num])
            {
                if (potencial.X == celda.X || potencial.Y == celda.Y || potencial.Z == celda.Z)
                {
                    listaParaEliminar.Add(potencial);
                }
            }
            foreach (Celda cell in listaParaEliminar)
            {
                this.DicCeldasPara[num].Remove(cell);
            }

            foreach (int i in DicCeldasPara.Keys)
            {
                this.DicCeldasPara[i].Remove(celda);

                if (this.DicPendientes[i].filas.Count != this.DicPendientes[i].cols.Count ||
                    this.DicPendientes[i].filas.Count != this.DicPendientes[i].grupos.Count ||
                    this.DicPendientes[i].filas.Count > this.DicCeldasPara[i].Count)
                {
                    this.EsViable = false;
                    this.EsValida = false;
                    return;
                }
            }

        }
                

        #region PrintEnConsola
        public void PrintBoard()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sb.Append(Board[i, j] == 0 ? "_" : Board[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
        }

        public void PrintPendientes()
        {
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine($"Pendientes {i + 1}: {string.Join(", ", DicPendientes[i].filas)}: \t {string.Join(", ", DicPendientes[i].cols)}: \t {string.Join(", ", DicPendientes[i].grupos)} ");
            }
            Console.WriteLine();
        }
        public void PrintCeldasPara()
        {
            for (int num = 0; num < 9; num++)
            {
                Console.WriteLine($"Celdas para el numero {num + 1}:");
                PrintCeldas(DicCeldasPara[num]);
            }
        }

        public void PrintCeldas(List<Celda> celdas)
        {
            foreach (Celda celda in celdas)
            {
                Console.WriteLine(celda);
            }
            Console.WriteLine();
        }

        public void PrintResumen()
        {
            for (int num = 0; num < 9; num++)
            {
                int countCeldasFactibles = DicCeldasPara[num].Count;
                int instanciasPendientes = DicPendientes[num].filas.Count;
                float indice;

                Console.WriteLine($"Celdas para el numero {num + 1}:");
                if (countCeldasFactibles == 0 && instanciasPendientes > 0)
                {
                    Console.WriteLine("Error, esta matriz ya no tiene solucion");
                    break;
                }
                else if (countCeldasFactibles > 0 && instanciasPendientes > 0)
                {
                    indice = (float)countCeldasFactibles / instanciasPendientes;
                    Console.WriteLine($"Total Celdas factibles: {countCeldasFactibles} \t\t para {instanciasPendientes} instancias \t\t indice:{indice:F2}");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Comparaciones
        public bool Equals(Tablero? other)
        {
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (this.Board[i,j] != other.Board[i,j]) return false;
                }
            }
            return true;
        }

        public int CompareTo(Tablero? other)
        {
            if(this.CeldasConocidas.Count < other.CeldasConocidas.Count)
            {
                return -1;
            }
            else if (this.CeldasConocidas.Count > other.CeldasConocidas.Count)
            {
                return 1;
            }
            for(int i = 0; i < 9; i++)
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

        public static bool operator==(Tablero? leftTablero, Tablero? rightTablero)
        {
            if(leftTablero is null)
            {
                if(rightTablero is null)
                {
                    return true;
                }
                // Solo el lado izquierdo es null
                return false;
            }
            // Equals maneja el caso en que la derecha es null
            return leftTablero.Equals(rightTablero);
        }
        public static bool operator !=(Tablero tableroIzquierda, Tablero tableroDerecha) => !(tableroIzquierda == tableroDerecha);
        #endregion
    }
}
