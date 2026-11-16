using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Aplicacion.DFS;

public sealed class SudokuSolver : ISudokuSolver
{
    private readonly ISolverReporter _reporter;
    private readonly ISolutionBuilder _solutionBuilder;
    private readonly ISudokuValidator _validator;
    private readonly ILogger<SudokuSolver> _logger;

    public SudokuSolver(ISolverReporter reporter, ISolutionBuilder solutionBuilder, 
        ISudokuValidator validator, ILogger<SudokuSolver> logger)
    {
        _reporter = reporter;
        _solutionBuilder = solutionBuilder;
        _validator = validator;
        _logger = logger;
    }

    public SolveResult Solve(
        Nodo root,
        CancellationToken cancellationToken = default)
    {
        var state = new SolverState();
        state.Timer.Start();

        state.Frontier.Push(root);
        state.FrontierSet.Add(new Tablerito(root));

        _logger.LogInformation("Iniciando resolución del Sudoku...");

        try
        {
            while (state.Frontier.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = state.Frontier.Pop();

                var(sonUnicas, accionesSiguientes) = current.Tablero.AccionesDicSiguientes();
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    LogEstadoIntermedio(current.Tablero, accionesSiguientes, state.Visited.Count);
                }

                state.FrontierSet.Remove(new Tablerito(current));

                TrackProgress(current, state);

                if (current.Tablero.EsSolucion)
                {
                    state.Timer.Stop();

                    return SolveResult.Success(
                        root,
                        current,
                        state,
                        _solutionBuilder.Build(current, state.Visited));
                }

                VisitNode(current, state, cancellationToken);
            }

            return SolveResult.Failure(root, state);
        }
        catch (OperationCanceledException)
        {
            state.Timer.Stop();
            throw;
        }
    }

    private void TrackProgress(Nodo nodo, SolverState state)
    {
        int known = nodo.Tablero.CeldasConocidas.Count;

        if (known > state.MaxKnownCells)
        {
            state.MaxKnownCells = known;
            _reporter.OnProgress(nodo, known);
        }
    }

    private void VisitNode(
        Nodo nodo,
        SolverState state,
        CancellationToken cancellationToken)
    {
        state.Visited[new Tablerito(nodo)] = new NodoSimple(nodo);

        if (state.Visited.Count % 100_000 == 0)
        {
            _reporter.OnElapsed(state);
        }

        if (!nodo.Tablero.EsViable)
            return;

        var next = nodo.Siguientes(_validator);
        if (next == null)
            return;

        foreach (var child in next)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var childTablero = new Tablerito(child);
            if (!state.FrontierSet.Contains(childTablero) &&
                !state.Visited.ContainsKey(childTablero))
            {
                state.Frontier.Push(child);
                state.FrontierSet.Add(childTablero);
            }
        }
    }
    private void LogEstadoIntermedio(Tablero tablero, IEnumerable<Accion>? acciones, int visitados)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Evaluando Nodo (Nodos visitados: {visitados})");

        // Recreamos PrintBoard
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                var val = tablero.Board[i, j];
                sb.Append(val == 0 ? "_ " : $"{val} ");
            }
            sb.AppendLine();
        }

        // Recreamos Print de Acciones Siguientes
        if (acciones != null)
        {
            sb.AppendLine("Acciones Siguientes:");
            foreach (var accion in acciones)
            {
                sb.AppendLine($"- Celda: ({accion.Celda.X}, {accion.Celda.Y}) -> {accion.Numero}");
            }
        }

        // Enviamos todo el string formateado al logger de una sola vez
        _logger.LogDebug(sb.ToString());
    }
}