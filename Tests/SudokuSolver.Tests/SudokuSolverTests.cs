using SudokuLibrary;
using SudokuLibrary.Models;
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

    [Theory]
    [MemberData(nameof(GetSudokuPuzzles))]
    public void Solve_ShouldFindSolution_ForValidPuzzles(byte[,] matrix, string puzzleName)
    {
        // Arrange
        // We use the real concrete reporter (integration style test)
        var reporter = new ConsoleReporter();
        var solver = new SudokuSolver(reporter);

        var root = new Nodo(
            new Tablero(matrix),
            null!,
            null!
        );

        // Act
        var result = solver.Solve(root);

        // Assert
        Assert.True(result.Found, $"Failed to solve puzzle: {puzzleName}");
        Assert.NotNull(result.SolutionNode);
        Assert.True(result.SolutionNode.Tablero.EsSolucion, $"Solution for {puzzleName} is invalid");

        // Optional: Verify the board is actually full (no zeros)
        var solutionBoard = result.SolutionNode.Tablero.Board;
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Assert.NotEqual<byte>(0, solutionBoard[i, j]);
    }

    // Data Provider for the [Theory]
    public static IEnumerable<object[]> GetSudokuPuzzles()
    {
        // Format: { Matrix, "Name for debugging" }

        yield return new object[] { Matrices.DE_INTERNET_EASY, "Easy Puzzle" };
        yield return new object[] { Matrices.DE_INTERNET_HARD, "Hard Puzzle" };
        yield return new object[] { Matrices.MATRIZ_CASI_RESUELTA_SIN_8_y_9, "Sin 7, 8 y 9" };
        yield return new object[] { Matrices.MATRIZ_CASI_RESUELTA_SIN_5_6_7_8_y_9, "Sin 5, 6, 7, 8 y 9" };
        yield return new object[] { Matrices.MATRIZ_H_0, "H0" };
        yield return new object[] { Matrices.DE_INTERNET_EXPERT, "Expert" };

        // You can paste your specific matrices here or reference your existing Matrices class
        // yield return new object[] { Matrices.MATRIZ_MASTER, "Master Matrix" };
    }

}

