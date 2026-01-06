using SudokuLibrary.Models;

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
        state.FrontierSet.Add(new Tablerito(root));

        try
        {
            while (state.Frontier.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = state.Frontier.Pop();
                state.FrontierSet.Remove(new Tablerito(current));

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

            var childTablero = new Tablerito(child);
            if (!state.FrontierSet.Contains(childTablero) &&
                !state.Visited.ContainsKey(childTablero))
            {
                state.Frontier.Push(child);
                state.FrontierSet.Add(childTablero);
            }
        }
    }
}
