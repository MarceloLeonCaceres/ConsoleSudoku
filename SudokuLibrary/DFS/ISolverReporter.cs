using SudokuLibrary.Model;

public interface ISolverReporter
{
    void OnProgress(Nodo nodo, int knownCells);
    void OnElapsed(SolverState state);
}

