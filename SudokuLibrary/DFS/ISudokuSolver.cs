using SudokuLibrary.DFS;

public interface ISudokuSolver
{
    SolveResult Solve(Nodo root, CancellationToken cancellationToken = default);

}