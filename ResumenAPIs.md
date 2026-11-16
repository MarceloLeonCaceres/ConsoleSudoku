# Resumen de Firmas Públicas

## Archivos procesados por proyecto
### src
- **Aplicacion**: 10 archivo(s)
- **ConsoleSudoku**: 1 archivo(s)
- **Domain**: 9 archivo(s)
- **Infrastructure**: 1 archivo(s)

### tests
- **Domain.Tests.Unit**: 1 archivo(s)
- **SudokuSolver.Tests**: 1 archivo(s)

---

## Proyecto: src/Aplicacion

```csharp
// === src/Aplicacion/DFS/ConsoleReporter.cs ===
using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;
using System.Globalization;

public sealed class ConsoleReporter : ISolverReporter
{
    public void OnProgress(Nodo nodo, int knownCells);
    public void OnElapsed(SolverState state);
    public void PrintProgress(Nodo nodo, int known);
    public void PrintElapsed(SolverState state);
    public void PrintSolution(SolveResult result);
    public void PrintNoSolution(SolveResult result);
}

// === src/Aplicacion/DFS/SolutionBuilder.cs ===
using Aplicacion.Interfaces;
using Domain.Entities;

namespace Aplicacion.DFS
{
    public sealed class SolutionBuilder : ISolutionBuilder
    {
        public List<Accion> Build(Nodo solutionNode, SortedDictionary<Tablerito, NodoSimple> visited);
    }
}

// === src/Aplicacion/DFS/SudokuSolver.cs ===
using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;
using Domain.Services;

namespace Aplicacion.DFS;
public sealed class SudokuSolver : ISudokuSolver
{
    public SudokuSolver(ISolverReporter reporter, ISolutionBuilder solutionBuilder, ISudokuValidator validator);
    public SolveResult Solve(Nodo root, CancellationToken cancellationToken = default);
}

// === src/Aplicacion/Interfaces/ISolutionBuilder.cs ===
using Domain.Entities;

namespace Aplicacion.Interfaces;
public interface ISolutionBuilder
{
    List<Accion> Build(Nodo solutionNode, SortedDictionary<Tablerito, NodoSimple> visited);
}

// === src/Aplicacion/Interfaces/ISolverReporter.cs ===
using Aplicacion.Models;
using Domain.Entities;

namespace Aplicacion.Interfaces;
public interface ISolverReporter
{
    void OnProgress(Nodo nodo, int knownCells);
    void OnElapsed(SolverState state);
    void PrintSolution(SolveResult result);
    void PrintNoSolution(SolveResult result);
}

// === src/Aplicacion/Interfaces/ISudokuPuzzleRepository.cs ===
namespace Aplicacion.Interfaces;
public interface ISudokuPuzzleRepository
{
    /// <summary>
    /// Obtiene una copia de la matriz inicial del Sudoku por su nombre.
    /// </summary>
    byte[, ] GetPuzzle(string name);
    /// <summary>
    /// Devuelve los nombres de todos los tableros disponibles.
    /// </summary>
    IEnumerable<string> GetAvailablePuzzles();
}

// === src/Aplicacion/Interfaces/ISudokuSolver.cs ===
using Aplicacion.Models;
using Domain.Entities;

namespace Aplicacion.Interfaces;
public interface ISudokuSolver
{
    SolveResult Solve(Nodo root, CancellationToken cancellationToken = default);
}

// === src/Aplicacion/Models/SolveResult.cs ===
using Domain.Entities;

namespace Aplicacion.Models;
public sealed record SolveResult(bool Found, Nodo Root, Nodo? SolutionNode, SolverState State, IReadOnlyList<Accion>? Actions)
{
    public static SolveResult Success(Nodo root, Nodo solution, SolverState state, List<Accion> actions);
    public static SolveResult Failure(Nodo root, SolverState state);
}

// === src/Aplicacion/Models/SolverState.cs ===
using Domain.Entities;
using System.Diagnostics;

namespace Aplicacion.Models;
public sealed class SolverState
{
    public Stack<Nodo> Frontier { get; }
    public HashSet<Tablerito> FrontierSet { get; }
    // Aquí es donde brillan Tablerito y NodoSimple (nuestros Snapshots) para ahorrar RAM
    public SortedDictionary<Tablerito, NodoSimple> Visited { get; }
    public Stopwatch Timer { get; }
    public int MaxKnownCells { get; set; }
    public DateTime StartTime { get; }
}

// === src/Aplicacion/SudokuAbstraction.cs ===
namespace SudokuLibrary;
public static class SudokuAbstraction
{
    public static int Cuadro(int x, int y);
    public static bool Valida(byte[, ] matriz);
    public static bool EsSubGrupoValido(int[] grupo);
    public static int[] VectorGrupo(byte[, ] matriz, int numeroGrupo);
}

```

## Proyecto: src/ConsoleSudoku

```csharp
// === src/ConsoleSudoku/ConsoleReporter.cs ===
using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;
using System.Text;

namespace ConsoleSudoku;
// === SudokuConsole/ConsoleReporter.cs ===
public sealed class ConsoleReporter : ISolverReporter
{
    public void OnProgress(Nodo nodo, int knownCells);
    public void OnElapsed(SolverState state);
    public void PrintSolution(SolveResult result);
    public void PrintNoSolution(SolveResult result);
}

```

## Proyecto: src/Domain

```csharp
// === src/Domain/Entities/Accion.cs ===
namespace Domain.Entities;
public record Accion
{
    public Celda? Celda { get; private set; }
    public byte Numero { get; private set; }

    public Accion(Celda celda, byte numero);
}

// === src/Domain/Entities/BoardsComparer.cs ===
namespace Domain.Entities;
public static class BoardsComparer
{
    // The logic you had in both classes, now in one place
    public static int CompareBoards(byte[, ]? boardA, byte[, ]? boardB, int knownA, int knownB);
    public static bool AreBoardsEqual(byte[, ]? boardA, byte[, ]? boardB);
}

// === src/Domain/Entities/Celda.cs ===
namespace Domain.Entities;
public record Celda
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public Celda(int x, int y, int z);
    public Celda(int x, int y);
}

// === src/Domain/Entities/Nodo.cs ===
using Domain.Services;

namespace Domain.Entities;
public class Nodo
{
    public Tablero Tablero { get; set; }
    public Tablero? Padre { get; set; }
    public IReadOnlyList<Accion>? Acciones { get; set; }

    public Nodo(Tablero tablero, Tablero? padre, List<Accion>? acciones);
    public List<Nodo>? Siguientes(ISudokuValidator validator);
//public void Print()
//{
//    Tablero.PrintBoard();
//}
}

// === src/Domain/Entities/NodoSimple.cs ===
namespace Domain.Entities;
public record NodoSimple
{
    public Tablerito Tablerito { get; set; }
    public Tablerito PadreDelTablerito { get; set; }
    public IReadOnlyList<Accion>? Acciones { get; set; }

    public NodoSimple(Nodo nodo);
}

// === src/Domain/Entities/PendientesByNumber.cs ===
namespace Domain.Entities;
public class PendientesByNumber
{
    public List<int> filas { get; set; }
    public List<int> cols { get; set; }
    public List<int> grupos { get; set; }

    public PendientesByNumber();
    public PendientesByNumber CopyPending();
    public void ReducePor(Celda celda);
    public bool EsValido();
}

// === src/Domain/Entities/Tablerito.cs ===
namespace Domain.Entities;
public class Tablerito : IEquatable<Tablerito>, IComparable<Tablerito>
{
    public byte[, ]? Board { get; set; }
    public int CuentaConocidas { get; set; }

    public Tablerito();
    public Tablerito(Tablero tablero);
    public Tablerito(Nodo nodo) : this(nodo.Tablero);
    public int CompareTo(Tablerito? other);
    //public override int GetHashCode()
    //{
    //    var hash = new HashCode();
    //    hash.Add(CuentaConocidas);
    //    // We don't hash every cell (too slow), but we sample the diagonals
    //    // or a few rows to create a distinct signature.
    //    // For 100% correctness, you might need to hash more, 
    //    // but for Sudoku, hashing the Count + Center + Corners is usually enough to avoid collisions.
    //    if (Board != null)
    //    {
    //        hash.Add(Board[0, 0]);
    //        hash.Add(Board[4, 4]);
    //        hash.Add(Board[8, 8]);
    //        hash.Add(Board[0, 8]);
    //        hash.Add(Board[8, 0]);
    //    }
    //    return hash.ToHashCode();
    //}
    public bool Equals(Tablerito? other);
}

// === src/Domain/Entities/Tablero.cs ===
using Domain.Services;

namespace Domain.Entities;
public class Tablero : IComparable<Tablero>
{
    public byte[, ] Board { get; set; }
    public IReadOnlySet<Celda> CeldasConocidas => _celdasConocidas;
    public IReadOnlyDictionary<int, PendientesByNumber> DicPendientes => _dicPendientes;
    public IReadOnlyDictionary<int, List<Celda>> DicCeldasPara => _dicCeldasPara;
    public bool EsValida { get; private set; }
    public bool EsViable { get; private set; }
    public bool EsSolucion { get; private set; }

    public Tablero(byte[, ] matriz, ISudokuValidator validator);
    public Tablero(Tablero padre, Accion accion, ISudokuValidator validator);
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
    public (bool sonUnicas, List<Accion>? acciones) AccionesDicSiguientes();
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
    public int CompareTo(Tablero? other);
#endregion
}

// === src/Domain/Services/SudokuValidator.cs ===
namespace Domain.Services;
public interface ISudokuValidator
{
    int ObtenerCuadro(int x, int y);
    bool EsMatrizValida(byte[, ] matriz);
    bool EsSubGrupoValido(int[] grupo);
    int[] ObtenerVectorGrupo(byte[, ] matriz, int numeroGrupo);
}

public sealed class SudokuValidator : ISudokuValidator
{
    // Aquí mueves el código interno que antes estaba en SudokuAbstraction
    public int ObtenerCuadro(int x, int y);
    public bool EsMatrizValida(byte[, ] matriz);
    public bool EsSubGrupoValido(int[] grupo);
    public int[] ObtenerVectorGrupo(byte[, ] matriz, int numeroGrupo);
}

```

## Proyecto: src/Infrastructure

```csharp
// === src/Infrastructure/Repositories/InMemorySudokuRepository.cs ===
using Aplicacion.Interfaces;

namespace Infrastructure.Repositories;
public class InMemorySudokuRepository : ISudokuPuzzleRepository
{
    public InMemorySudokuRepository();
    public byte[, ] GetPuzzle(string name);
    public IEnumerable<string> GetAvailablePuzzles();
#endregion
}

```

## Proyecto: tests/Domain.Tests.Unit

```csharp
// === tests/Domain.Tests.Unit/CeldaTests.cs ===
using Domain.Entities;

namespace Domain.Tests.Unit;
public class CeldaTests
{
    [Theory]
    // Esquinas de la matriz
    [InlineData(0, 0, 0)] // Cuadrante superior izquierdo
    [InlineData(0, 8, 2)] // Cuadrante superior derecho
    [InlineData(8, 0, 6)] // Cuadrante inferior izquierdo
    [InlineData(8, 8, 8)] // Cuadrante inferior derecho
    // Centro y casos medios
    [InlineData(4, 4, 4)] // Cuadrante central
    [InlineData(2, 5, 1)] // Fila 2 (Cuad 0 en Y), Col 5 (Cuad 1 en X) -> Cuadrante 1
    [InlineData(5, 2, 3)] // Fila 5 (Cuad 1 en Y), Col 2 (Cuad 0 en X) -> Cuadrante 3
    public void Constructor_CoordenadasValidas_CalculaCuadranteCorrecto(int x, int y, int zEsperado);
    [Theory]
    [InlineData(-1, 5)]
    [InlineData(9, 5)]
    public void Constructor_FilaInvalida_LanzaArgumentOutOfRangeException(int x, int y);
    [Theory]
    [InlineData(5, -1)]
    [InlineData(5, 9)]
    public void Constructor_ColumnaInvalida_LanzaArgumentOutOfRangeException(int x, int y);
    [Fact]
    public void Records_ConMismasCoordenadas_SeEvaluanComoIguales();
}

```

## Proyecto: tests/SudokuSolver.Tests

```csharp
// === tests/SudokuSolver.Tests/SudokuSolverTests.cs ===
using Aplicacion.DFS;
using Aplicacion.Interfaces;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Repositories;

namespace SudokuTests;
public class SudokuSolverTests
{
    public SudokuSolverTests();
    [Fact]
    public void Solve_WhenSolutionExists_ReturnsSuccess();
    [Fact]
    public void Solve_WhenNoSolution_ReturnsFailure();
    [Fact]
    public void Solve_WhenCancelled_ThrowsOperationCanceledException();
    [Theory]
    [MemberData(nameof(GetSudokuPuzzles))]
    public void Solve_ShouldFindSolution_ForValidPuzzles(byte[, ] matrix, string puzzleName);
    // Data Provider for the [Theory]
    public static IEnumerable<object[]> GetSudokuPuzzles();
}

```

