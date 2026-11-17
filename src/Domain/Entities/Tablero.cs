using Domain.Services;

namespace Domain.Entities;

public class Tablero : IComparable<Tablero>
{
    public byte[,] Board { get; set; }

    public IReadOnlySet<Celda> CeldasConocidas => _celdasConocidas;
    public IReadOnlyDictionary<int, PendientesByNumber> DicPendientes => _dicPendientes;
    public IReadOnlyDictionary<int, List<Celda>> DicCeldasPara => _dicCeldasPara;

    private readonly HashSet<Celda> _celdasConocidas;
    private readonly Dictionary<int, PendientesByNumber> _dicPendientes;
    private readonly Dictionary<int, List<Celda>> _dicCeldasPara;

    public bool EsValida { get; private set; } = false;
    public bool EsViable { get; private set; } = true;
    public bool EsSolucion { get; private set; } = false;

    public Tablero(byte[,] matriz, ISudokuValidator validator)
    {
        ArgumentNullException.ThrowIfNull(matriz, nameof(matriz));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        Board = (byte[,])matriz.Clone();

        _celdasConocidas = new HashSet<Celda>();
        _dicPendientes = new Dictionary<int, PendientesByNumber>();
        _dicCeldasPara = new Dictionary<int, List<Celda>>();

        EsValida = validator.EsMatrizValida(Board);

        SetCeldasConocidas();
        SetPendientesPorNumero();
        ReducePendientes();
        SetCeldasPara_De_DicPendientes();
        if (this.EsViable)
        {
            SetDicViabilidad();
        }
    }

    public Tablero(Tablero padre, Accion accion, ISudokuValidator validator)
    {
        ArgumentNullException.ThrowIfNull(padre, nameof(padre));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        this.Board = (byte[,])padre.Board.Clone();
        _celdasConocidas = new HashSet<Celda>(padre.CeldasConocidas);

        if (accion != null)
        {
            this.Board[accion.Celda.X, accion.Celda.Y] = accion.Numero;
            _celdasConocidas.Add(new Celda(accion.Celda.X, accion.Celda.Y));
        }
        this.EsValida = validator.EsMatrizValida(this.Board);
        this.EsViable = padre.EsViable;
        this.EsSolucion = this.CeldasConocidas.Count == 81;

        _dicPendientes = new Dictionary<int, PendientesByNumber>();
        foreach (int num in padre.DicCeldasPara.Keys)
        {
            if (padre.DicPendientes[num].filas.Count > 0)
            {
                _dicPendientes[num] = padre.DicPendientes[num].CopyPending();
            }
        }

        _dicCeldasPara = new Dictionary<int, List<Celda>>();
        foreach (int num in _dicPendientes.Keys)
        {
            if (padre.DicCeldasPara.TryGetValue(num, out var celdas) && celdas.Count > 0)
            {
                _dicCeldasPara[num] = new List<Celda>(celdas);
            }
            else
            {
                _dicCeldasPara[num] = new List<Celda>();
            }
        }

        if (accion != null)
        {
            ReduceDicPendientes(accion.Celda);
        }
    }

    //private void ValidaBoard()
    //{
    //    bool esvalida = SudokuAbstraction.Valida(Board);
    //    if (!esvalida)
    //    {
    //        Console.WriteLine("Esta matriz no es valida");
    //        Console.WriteLine();
    //    }
    //    EsValida = esvalida;
    //}

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

    private void SetCeldasConocidas()
    {
        for (int i = 0; i < 9; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                if (Board[i, j] > 0)
                {
                    _celdasConocidas.Add(new Celda(i, j));
                }
            }
        }
    }

    private void SetPendientesPorNumero()
    {
        for (int i = 1; i <= 9; ++i)
        {
            _dicPendientes[i] = new PendientesByNumber();
        }
    }

    private void SetCeldasPara_De_DicPendientes()
    {
        _dicCeldasPara.Clear();
        foreach (int num in DicPendientes.Keys)
        {
            _dicCeldasPara[num] = new List<Celda>();
            foreach (int i in DicPendientes[num].filas)
            {
                int contador = 0;
                foreach (int j in DicPendientes[num].cols)
                {
                    foreach (int k in DicPendientes[num].grupos)
                    {
                        var celdaTemporal = new Celda(i, j);
                        if (celdaTemporal.Z == k && Board[i, j] == 0)
                        {
                            _dicCeldasPara[num].Add(celdaTemporal);
                            contador++;
                        }
                        //if (SudokuAbstraction.Cuadro(i, j) == k && Board[i, j] == 0)
                        //{
                        //    DicCeldasPara[num].Add(new Celda(i, j, k));
                        //    contador++;
                        //}
                    }
                }
                if (contador == 0)
                {
                    EsViable = false;
                    return;
                }
            }

            bool faltanFilas = DicPendientes[num].filas.Any(fila => !_dicCeldasPara[num].Any(c => c.X == fila));
            bool faltanCols = DicPendientes[num].cols.Any(col => !_dicCeldasPara[num].Any(c => c.Y == col));
            bool faltanGrupos = DicPendientes[num].grupos.Any(z => !_dicCeldasPara[num].Any(c => c.Z == z));

            if (faltanFilas || faltanCols || faltanGrupos)
            {
                EsViable = false;
                return;
            }
            //foreach (int fila in DicPendientes[num].filas)
            //{
            //    if (TieneAlgunaCelda(DicCeldasPara[num], "row", fila) == false)
            //    {
            //        this.EsViable = false;
            //        return;
            //    }
            //}
            //foreach (int col in DicPendientes[num].cols)
            //{
            //    if (TieneAlgunaCelda(DicCeldasPara[num], "col", col) == false)
            //    {
            //        this.EsViable = false;
            //        return;
            //    }
            //}
            //foreach (int z in DicPendientes[num].grupos)
            //{
            //    if (TieneAlgunaCelda(DicCeldasPara[num], "grupo", z) == false)
            //    {
            //        this.EsViable = false;
            //        return;
            //    }
            //}
        }
    }

    //private static bool TieneAlgunaCelda(List<Celda> paraNums, string rowColZ, int numRowColZ)
    //{
    //    if (rowColZ == "row")
    //    {
    //        foreach (Celda celda in paraNums)
    //        {
    //            if (celda.X == numRowColZ)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    else if (rowColZ == "col")
    //    {
    //        foreach (Celda celda in paraNums)
    //        {
    //            if (celda.Y == numRowColZ)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (Celda celda in paraNums)
    //        {
    //            if (celda.Z == numRowColZ)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    public (bool sonUnicas, List<Accion>? acciones) AccionesDicSiguientes()
    {

        List<Accion> accionesSiguientes = new List<Accion>();
        int menosInstancias = 10;
        int menosCeldasFactibles = 100;
        byte numeroConMayorIndice = 0;

        foreach (byte num in DicPendientes.Keys)
        {
            int instanciasPendientesNum = DicPendientes[num].filas.Count;
            int countCeldasFactiblesNum = DicCeldasPara.TryGetValue(num, out var celdas) ? celdas.Count : 0;

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
                var celdasGrupo = _dicCeldasPara.TryGetValue(num, out var lista) ? lista : new List<Celda>();
                int cuenta = celdasGrupo.Count(c => c.Z == k);
                if (cuenta == 1)
                {
                    Celda celdaUnicaPosible = celdasGrupo.Single(c => c.Z == k);
                    Accion siguienteAccion = new Accion(celdaUnicaPosible, num);
                    accionesSiguientes.Add(siguienteAccion);
                }
            }
        }
        if (accionesSiguientes.Count > 0)
        {
            return (true, accionesSiguientes);
        }
        if (numeroConMayorIndice == 0)
        {
            return (false, null);
        }
        // Búsqueda segura final
        if (_dicCeldasPara.TryGetValue(numeroConMayorIndice, out var celdaFactibles))
        {
            foreach (var celda in celdaFactibles)
            {
                accionesSiguientes.Add(new Accion(celda, numeroConMayorIndice));
            }
        }

        if (accionesSiguientes.Count > 0)
        {
            Console.WriteLine("Acciones Siguientes:");
            foreach (Accion accion in accionesSiguientes)
            {
                Console.WriteLine(accion);
            }
            Console.WriteLine();

            return (false, accionesSiguientes);
        }
        return (false, null);
    }

    private void ReducePendientes()
    {
        foreach (var celda in CeldasConocidas)
        {
            DicPendientes[Board[celda.X, celda.Y]].ReducePor(celda);
        }
        for (int i = 1; i <= 9; i++)
        {
            if (this.DicPendientes[i].EsValido() == false)
            {
                this.EsViable = false;
                this.EsValida = false;
                return;
            }
            if (DicPendientes[i].filas.Count == 0)
            {
                _dicPendientes.Remove(i);
            }
        }
    }

    private void ReduceDicPendientes(Celda celda)
    {
        int num = Board[celda.X, celda.Y];
        if (DicPendientes.ContainsKey(num))
        {
            this.DicPendientes[num].ReducePor(celda);

            if (this.DicPendientes[num].filas.Count != this.DicPendientes[num].cols.Count ||
                    this.DicPendientes[num].filas.Count != this.DicPendientes[num].grupos.Count)
            {
                this.EsViable = false;
                this.EsValida = false;
                return;
            }
        }
        if (this.DicCeldasPara.ContainsKey(num))
        {
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
        }

        foreach (int i in DicCeldasPara.Keys.ToList())
        {
            this.DicCeldasPara[i].Remove(celda);

            if (DicPendientes.ContainsKey(i))
            {
                if (this.DicPendientes[i].filas.Count != this.DicPendientes[i].cols.Count ||
                this.DicPendientes[i].filas.Count != this.DicPendientes[i].grupos.Count ||
                this.DicPendientes[i].filas.Count > this.DicCeldasPara[i].Count)
                {
                    this.EsViable = false;
                    this.EsValida = false;
                    return;
                }
            }
            else
            {
                _dicCeldasPara.Remove(i);
            }
        }
    }

    #region PrintEnConsola
    //public void PrintBoard()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < 9; i++)
    //    {
    //        for (int j = 0; j < 9; j++)
    //        {
    //            sb.Append(Board[i, j] == 0 ? "_" : Board[i, j]);
    //            sb.Append(" ");
    //        }
    //        sb.AppendLine();
    //    }
    //    Console.WriteLine(sb.ToString());
    //    Console.WriteLine();
    //}

    //public void PrintPendientes()
    //{
    //    for (int i = 0; i < 9; i++)
    //    {
    //        Console.WriteLine($"Pendientes {i + 1}: {string.Join(", ", DicPendientes[i].filas)}: \t {string.Join(", ", DicPendientes[i].cols)}: \t {string.Join(", ", DicPendientes[i].grupos)} ");
    //    }
    //    Console.WriteLine();
    //}
    //public void PrintCeldasPara()
    //{
    //    for (int num = 0; num < 9; num++)
    //    {
    //        Console.WriteLine($"Celdas para el numero {num + 1}:");
    //        PrintCeldas(DicCeldasPara[num]);
    //    }
    //}

    //public void PrintCeldas(List<Celda> celdas)
    //{
    //    foreach (Celda celda in celdas)
    //    {
    //        Console.WriteLine(celda);
    //    }
    //    Console.WriteLine();
    //}

    //public void PrintResumen()
    //{
    //    for (int num = 0; num < 9; num++)
    //    {
    //        int countCeldasFactibles = DicCeldasPara[num].Count;
    //        int instanciasPendientes = DicPendientes[num].filas.Count;
    //        float indice;

    //        Console.WriteLine($"Celdas para el numero {num + 1}:");
    //        if (countCeldasFactibles == 0 && instanciasPendientes > 0)
    //        {
    //            Console.WriteLine("Error, esta matriz ya no tiene solucion");
    //            break;
    //        }
    //        else if (countCeldasFactibles > 0 && instanciasPendientes > 0)
    //        {
    //            indice = (float)countCeldasFactibles / instanciasPendientes;
    //            Console.WriteLine($"Total Celdas factibles: {countCeldasFactibles} \t\t para {instanciasPendientes} instancias \t\t indice:{indice:F2}");
    //        }
    //        Console.WriteLine();
    //    }
    //}
    #endregion

    #region Comparaciones

    public int CompareTo(Tablero? other)
    {
        return BoardsComparer.CompareBoards(
            this.Board,
            other?.Board,
            this.CeldasConocidas.Count,
            other?.CeldasConocidas.Count ?? 0);
    }

    #endregion
}
