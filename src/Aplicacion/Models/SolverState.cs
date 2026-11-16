using Domain.Entities;
using System.Diagnostics;

namespace Aplicacion.Models;

public sealed class SolverState
{
    public Stack<Nodo> Frontier { get; } = new();
    public HashSet<Tablerito> FrontierSet { get; } = new();

    // Aquí es donde brillan Tablerito y NodoSimple (nuestros Snapshots) para ahorrar RAM
    public SortedDictionary<Tablerito, NodoSimple> Visited { get; } = new();

    public Stopwatch Timer { get; } = new();
    public int MaxKnownCells { get; set; }
    public DateTime StartTime { get; } = DateTime.UtcNow;
}
