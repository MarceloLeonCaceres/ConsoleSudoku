using SudokuLibrary.DFS;
using SudokuLibrary.Model;

public sealed class SudokuSolver
{
    private readonly ConsoleReporter _reporter;

    public SudokuSolver(ConsoleReporter reporter)
    {
        _reporter = reporter;
    }

    public SolveResult Solve(Nodo root)
    {
        var state = new SolverState();
        state.Frontier.Push(root);

        while (state.Frontier.Count > 0)
        {
            var current = state.Frontier.Pop();

            TrackProgress(current, state);

            if (current.Tablero.EsSolucion)
            {
                state.Timer.Stop();
                return SolveResult.Success(
                    root,
                    current,
                    state,
                    SolutionBuilder.Build(current, state.Visited)
                );
            }

            VisitNode(current, state);
        }

        return SolveResult.Failure(state);
    }

    public void TrackProgress(Nodo nodo, SolverState state)
    {
        int known = nodo.Tablero.CeldasConocidas.Count;

        if (known > state.MaxKnownCells)
        {
            state.MaxKnownCells = known;
            _reporter.PrintProgress(nodo, known);
        }
    }

    public void VisitNode(Nodo nodo, SolverState state)
    {
        state.Visited[new Tablerito(nodo)] = new NodoSimple(nodo);

        if (state.Visited.Count % 100_000 == 0)
        {
            _reporter.PrintElapsed(state);
        }

        if (!nodo.Tablero.EsViable)
            return;

        var next = nodo.Siguientes();
        if (next == null)
            return;

        foreach (var child in next)
        {
            if (!state.Frontier.Contains(child) &&
                !state.Visited.ContainsKey(new Tablerito(child)))
            {
                state.Frontier.Push(child);
            }
        }
    }
}
