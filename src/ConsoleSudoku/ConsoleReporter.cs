using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;
using System.Text;

namespace ConsoleSudoku;

// === SudokuConsole/ConsoleReporter.cs ===
public sealed class ConsoleReporter : ISolverReporter
{
    public void OnProgress(Nodo nodo, int knownCells)
    {
        Console.WriteLine($"Progreso: {knownCells} celdas conocidas.");
    }

    public void OnElapsed(SolverState state)
    {
        Console.WriteLine($"Tiempo transcurrido: {state.Timer.ElapsedMilliseconds} ms. Nodos visitados: {state.Visited.Count}");
    }

    public void PrintSolution(SolveResult result)
    {
        Console.WriteLine("\n¡Solución encontrada!");
        PrintBoard(result.SolutionNode!.Tablero);
        Console.WriteLine($"Acciones tomadas: {result.Actions?.Count ?? 0}");
    }

    public void PrintNoSolution(SolveResult result)
    {
        Console.WriteLine("\nNo se encontró solución.");
    }

    private static void PrintBoard(Tablero tablero)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                var val = tablero.Board[i, j];
                sb.Append(val == 0 ? "_" : val.ToString());
                sb.Append(" ");
            }
            sb.AppendLine();
        }
        Console.WriteLine(sb.ToString());
    }
}
