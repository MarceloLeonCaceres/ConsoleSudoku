using SudokuLibrary.DFS;
using System;
using System.Text;

namespace SudokuLibrary.Model
{
    public class Tablero : IEquatable<Tablero>
    {
        public int[,] Board { get; set; }

        public List<PendientesPorNumero> Pendientes = new List<PendientesPorNumero>();

        public List<Celda>[] CeldasPara = new List<Celda>[9];

        public HashSet<Celda> CeldasConocidas = new HashSet<Celda>();    

        public bool EsValida { get; private set; } = false;

        public bool EsViable { get; set; } = false;
        public bool EsSolucion { get; set; } = false;
        public Tablero()
        {
            Board = new int[9, 9];
            SetPendientesPorNumero();
            SetCeldasPara();
        }

        public Tablero(int[,] matriz)
        {
            Board = (int[,])matriz.Clone();
            ValidaBoard();
            SetCeldasConocidas();
            SetPendientesPorNumero();
            ReducePendientes();
            SetCeldasPara();
            SetViabilidad();
        }

        public Tablero(Tablero padre, Accion accion)
        {
            
            this.Board = (int[,])padre.Board.Clone();
            this.Board[accion.Celda.X, accion.Celda.Y] = accion.Numero;
            this.CeldasConocidas = new HashSet<Celda>(padre.CeldasConocidas);
            this.EsValida = padre.EsValida;
            this.EsViable = padre.EsViable;

            this.CeldasConocidas.Add(new Celda(accion.Celda.X, accion.Celda.Y));

            this.Pendientes = new List<PendientesPorNumero>();
            // this.CeldasPara = (List<Celda>[])padre.CeldasPara.Clone();

            for (int i = 0; i < 9; i++)
            {
                PendientesPorNumero pendientesPorNumero = padre.Pendientes[i].CopiaPendientes();
                this.Pendientes.Add(pendientesPorNumero);

                this.CeldasPara[i] =  new List<Celda>(padre.CeldasPara[i]);
            }
            //SetPendientesPorNumero();
            //ReducePendientes();
            //SetCeldasPara();

            ReducePendientes(accion.Celda);
            if (this.EsViable)
            {
                SetViabilidad();
                EstaResuelta();
            }
        }

        private void Copia(Tablero hijo, Tablero padre)
        {
            hijo.Board = (int[,])padre.Board.Clone();
            hijo.CeldasConocidas = new HashSet<Celda>(padre.CeldasConocidas);
            hijo.EsValida = padre.EsValida;
            hijo.EsViable = padre.EsViable;
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
            //EsValida = true;
        }

        private void SetViabilidad()
        {
            for(int i =0; i < 9; i++)
            {
                if (Pendientes[i].filas.Count != Pendientes[i].cols.Count || 
                    Pendientes[i].filas.Count != Pendientes[i].grupos.Count ||
                    Pendientes[i].filas.Count > CeldasPara[i].Count )
                {
                    this.EsViable = false;
                    return;
                }
                //else if(Pendientes[i].filas.Count == CeldasPara[i].Count)
                //{

                //}
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
            //ValidaBoard();
            //if(EsValida == false)
            //{
            //    EsSolucion = false;
            //    return;
            //}
        }

        private void SetCeldasConocidas()
        {
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
            Pendientes = new List<PendientesPorNumero>();
            for (int i = 1; i <= 9; ++i)
            {
                Pendientes.Add(new PendientesPorNumero(i));
            }
        }

        private void GetPendientesDelPadre()
        {
            //for(int i = 0;i <= 9; ++i)
            //{
            //    this.Pendientes[i] = this.padre.
            //}
        }

        private void SetCeldasPara()
        {
            for (int num = 0; num < 9; num++)
            {
                CeldasPara[num] = new List<Celda>();
                foreach (int i in Pendientes[num].filas)
                {
                    int contador = 0;
                    foreach (int j in Pendientes[num].cols)
                    {
                        foreach (int k in Pendientes[num].grupos)
                        {
                            if (SudokuAbstraction.Cuadro(i, j) == k && Board[i, j] == 0)
                            {
                                CeldasPara[num].Add(new Celda(i, j, k));
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
            }
        }

        public void DetectCeldasUnicas()
        {
            for(int num = 0; num < 9; num++)
            {
                foreach (int k in Pendientes[num].grupos)
                {
                    int cuenta = CeldasPara[num].Where(c => c.Z == k).Count();
                    if (cuenta == 1)
                    {
                        Celda celdaUnicaPosible = CeldasPara[num].Where(c => c.Z == k).Select(x => x).Single();
                        Console.WriteLine($"Hay que poner el numero {num + 1} en la celda {celdaUnicaPosible}");
                        Console.WriteLine();
                    }

                }
            }
        }

        public List<Accion>? AccionesSiguientes()
        {

            List<Accion> accionesSiguientes = new List<Accion>();
            int menosInstancias = 10;
            int menosCeldasFactibles = 100;
            int numeroConMayorIndice = 0;

            for (int num = 0; num < 9; num++)
            {
                int instanciasPendientesNum = Pendientes[num].filas.Count;
                int countCeldasFactiblesNum = CeldasPara[num].Count;

                if (instanciasPendientesNum > 0 && instanciasPendientesNum <= menosInstancias)
                {
                    if (instanciasPendientesNum < menosInstancias)
                    {
                        numeroConMayorIndice = num + 1;
                        menosInstancias = instanciasPendientesNum;
                        menosCeldasFactibles = countCeldasFactiblesNum;
                    }
                    else // (instanciasPendientesNum == menosInstancias)
                    {
                        if (countCeldasFactiblesNum < menosCeldasFactibles)
                        {
                            numeroConMayorIndice = num + 1;
                            menosCeldasFactibles = countCeldasFactiblesNum;
                        }
                    }
                }
                foreach (int k in Pendientes[num].grupos)
                {
                    int cuenta = CeldasPara[num].Where(c => c.Z == k).Count();
                    if (cuenta == 1)
                    {
                        Celda celdaUnicaPosible = CeldasPara[num].Where(c => c.Z == k).Select(x => x).Single();
                        Accion siguienteAccion = new Accion(celdaUnicaPosible, num+1);
                        accionesSiguientes.Add(siguienteAccion);
                    }
                }
            }
            if(accionesSiguientes.Count > 0)
            {
                return accionesSiguientes;
            }
            if(numeroConMayorIndice == 0)
            {
                return null;
            }
            
            foreach(var celda in CeldasPara[numeroConMayorIndice-1])
            {
                accionesSiguientes.Add(new Accion(celda, numeroConMayorIndice));
            }
            
            if(accionesSiguientes.Count > 0)
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
                Pendientes[Board[celda.X, celda.Y] - 1].ReducePor(celda);
            }
        }
        private void ReducePendientes(Celda celda)
        {
            int indiceNum = Board[celda.X, celda.Y] - 1;
            this.Pendientes[indiceNum].ReducePor(celda);
            for (int i = 0; i < 9; i++)
            {
                List<Celda> listaParaEliminar = new List<Celda>();

                foreach (Celda potencial in this.CeldasPara[i])
                {
                    if (potencial.X == celda.X || potencial.Y == celda.Y || potencial.Z == celda.Z)
                    {
                        listaParaEliminar.Add(potencial);
                    }
                }
                foreach(Celda cell in listaParaEliminar)
                {
                    this.CeldasPara[i].Remove(cell);
                }


                if (this.Pendientes[i].filas.Count != this.Pendientes[i].cols.Count ||
                    this.Pendientes[i].filas.Count != this.Pendientes[i].grupos.Count ||
                    this.Pendientes[i].filas.Count > this.CeldasPara[i].Count)
                {
                    this.EsViable = false;
                    return;
                }
            }
        }

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
                Console.WriteLine($"Pendientes {i + 1}: {string.Join(", ", Pendientes[i].filas)}: \t {string.Join(", ", Pendientes[i].cols)}: \t {string.Join(", ", Pendientes[i].grupos)} ");
            }
            Console.WriteLine();
        }
        public void PrintCeldasPara()
        {
            for (int num = 0; num < 9; num++)
            {
                Console.WriteLine($"Celdas para el numero {num + 1}:");
                PrintCeldas(CeldasPara[num]);
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
                int countCeldasFactibles = CeldasPara[num].Count;
                int instanciasPendientes = Pendientes[num].filas.Count;
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

        public bool Equals(Tablero? other)
        {
            if (other is null) return false;

            if(this.CeldasConocidas.Count != other.CeldasConocidas.Count) return false;

            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (this.Board[i,j] != other.Board[i,j]) return false;
                }
            }
            return true;
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
    }
}
