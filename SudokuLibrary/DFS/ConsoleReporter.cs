using SudokuLibrary.DFS;
using SudokuLibrary.Model;
using System.Globalization;

public sealed class ConsoleReporter : ISolverReporter
{

    public void OnProgress(Nodo nodo, int knownCells)
    {
        Console.WriteLine($"Conocidas: {knownCells}");
        nodo.Print();
        Console.WriteLine();
    }   

    public void OnElapsed(SolverState state)
    {
        Console.WriteLine($"Visitados: {state.Visited.Count.ToString("0,0", CultureInfo.InvariantCulture)}");
        Console.WriteLine($"Tiempo: {state.Timer.Elapsed:h\\:mm\\:ss\\.fff}");
        Console.WriteLine();
    }

    public void PrintProgress(Nodo nodo, int known)
    {
        Console.WriteLine($"Conocidas: {known}");
        nodo.Print();
        Console.WriteLine();
    }

    public void PrintElapsed(SolverState state)
    {
        Console.WriteLine($"Visitados: {state.Visited.Count.ToString("0,0", CultureInfo.InvariantCulture)}");
        Console.WriteLine($"Tiempo: {state.Timer.Elapsed:h\\:mm\\:ss\\.fff}");
        Console.WriteLine($"Hora: {DateTime.Now:T}");
        Console.WriteLine();
    }

    public void PrintSolution(SolveResult result)
    {
        Console.Clear();
        Console.WriteLine("¡Solución encontrada!");
        Console.WriteLine($"Inicio: {result.State.StartTime:T}");
        Console.WriteLine($"Fin: {DateTime.Now:T}");
        Console.WriteLine($"Tiempo: {result.State.Timer.Elapsed:h\\:mm\\:ss\\.fff}");
        Console.WriteLine();

        result.Root.Print();
        Console.WriteLine();
        result.SolutionNode.Print();
        Console.WriteLine();

        PrintActions(result);
    }

    private void PrintActions(SolveResult result)
    {
        Console.WriteLine($"Visitados: {result.State.Visited.Count:N0}");
        Console.WriteLine($"En frontera: {result.State.Frontier.Count}");
        Console.WriteLine($"Acciones: {result.Actions.Count}");
        Console.WriteLine("Solución:");

        foreach (var a in result.Actions)
        {
            Console.WriteLine($"{a.Celda} => {a.Numero}");
        }

        Console.ReadLine();
    }

    public void PrintNoSolution(SolveResult result)
    {
        Console.WriteLine("No hubo solución.");
        Console.WriteLine($"Visitados: {result.State.Visited.Count}");
        Console.WriteLine($"En frontera: {result.State.Frontier.Count}");
    }
}
