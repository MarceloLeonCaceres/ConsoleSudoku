using SudokuLibrary.Model;
using System.Diagnostics;

public sealed class SolverState
{
    public Stack<Nodo> Frontier { get; } = new();
    public SortedDictionary<Tablerito, NodoSimple> Visited { get; } = new();

    public Stopwatch Timer { get; } = Stopwatch.StartNew();

    public int MaxKnownCells { get; set; }
    public DateTime StartTime { get; } = DateTime.Now;
}

