using SudokuLibrary.Models;

internal sealed class NullReporter : ISolverReporter
{
    public void OnProgress(Nodo nodo, int knownCells) { }
    public void OnElapsed(SolverState state) { }

}

