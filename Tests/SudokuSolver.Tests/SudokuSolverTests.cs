using SudokuLibrary;
using SudokuLibrary.Model;
using Xunit;

namespace SudokuTests;

public class SudokuSolverTests
{
    [Fact]
    public void Solve_WhenSolutionExists_ReturnsSuccess()
    {
        var root = new Nodo(
            new Tablero(Matrices.MATRIZ_CASI_RESUELTA),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter());

        var result = solver.Solve(root);

        Assert.True(result.Found);
        Assert.NotEmpty(result.Actions);
    }

    [Fact]
    public void Solve_WhenNoSolution_ReturnsFailure()
    {
        var root = new Nodo(
            new Tablero(Matrices.MATRIZ_NO_VALIDA),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter());

        var result = solver.Solve(root);

        Assert.False(result.Found);
    }

    [Fact]
    public void Solve_WhenCancelled_ThrowsOperationCanceledException()
    {
        var root = new Nodo(
            new Tablero(Matrices.MATRIZ_MASTER),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter());
        using var cts = new CancellationTokenSource();

        cts.Cancel();

        Assert.Throws<OperationCanceledException>(() =>
            solver.Solve(root, cts.Token));
    }


}

