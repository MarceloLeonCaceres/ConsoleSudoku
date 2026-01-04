using SudokuLibrary.Model;

public interface ISudokuSolver
{
    SolveResult Solve(Nodo root, CancellationToken cancellationToken = default);

}