using SudokuLibrary.DFS;
using SudokuLibrary.Model;

public sealed class SudokuSolver : ISudokuSolver
{
    private readonly ISolverReporter _reporter;

    public SudokuSolver(ISolverReporter reporter)
    {
        _reporter = reporter;
    }

    public SolveResult Solve(
        Nodo root,
        CancellationToken cancellationToken = default)
    {
        var state = new SolverState();
        state.Frontier.Push(root);

        try
        {
            while (state.Frontier.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = state.Frontier.Pop();
                TrackProgress(current, state);

                if (current.Tablero.EsSolucion)
                {
                    state.Timer.Stop();

                    return SolveResult.Success(
                        root,
                        current,
                        state,
                        SolutionBuilder.Build(current, state.Visited));
                }

                VisitNode(current, state, cancellationToken);
            }

            return SolveResult.Failure(state);
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

        var next = nodo.Siguientes();
        if (next == null)
            return;

        foreach (var child in next)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!state.Frontier.Contains(child) &&
                !state.Visited.ContainsKey(new Tablerito(child)))
            {
                state.Frontier.Push(child);
            }
        }
    }
}
