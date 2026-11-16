using Aplicacion.DFS;
using Aplicacion.Interfaces;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Repositories;

namespace SudokuTests;

public class SudokuSolverTests
{
    private readonly ISudokuValidator _validator = new SudokuValidator();
    private readonly ISolutionBuilder _solutionBuilder = new SolutionBuilder();
    private readonly ISudokuPuzzleRepository _repository = new InMemorySudokuRepository();
    public SudokuSolverTests()
    {
    }
    [Fact]
    public void Solve_WhenSolutionExists_ReturnsSuccess()
    {
        var root = new Nodo(
            new Tablero(_repository.GetPuzzle("MATRIZ_CASI_RESUELTA"), _validator),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter(), _solutionBuilder, _validator);

        var result = solver.Solve(root);

        Assert.True(result.Found);
        Assert.NotEmpty(result.Actions);
    }

    [Fact]
    public void Solve_WhenNoSolution_ReturnsFailure()
    {
        var root = new Nodo(
            new Tablero(_repository.GetPuzzle("MATRIZ_NO_VALIDA"), _validator),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter(), _solutionBuilder, _validator);

        var result = solver.Solve(root);

        Assert.False(result.Found);
    }

    [Fact]
    public void Solve_WhenCancelled_ThrowsOperationCanceledException()
    {
        var root = new Nodo(
            new Tablero(_repository.GetPuzzle("MATRIZ_MASTER"), _validator),
            null!,
            null!);

        ISudokuSolver solver = new SudokuSolver(new NullReporter(), _solutionBuilder, _validator);
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
        var solver = new SudokuSolver(reporter, _solutionBuilder, _validator);

        var root = new Nodo(
            new Tablero(matrix, _validator),
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
        var _repository = new InMemorySudokuRepository();

        //yield return new object[] { _repository.GetPuzzle("MATRIZ_INTERMEDIA"), "Easy Puzzle MATRIZ_INTERMEDIA" };
        yield return new object[] { _repository.GetPuzzle("DE_INTERNET_EASY"), "Easy Puzzle DE_INTERNET_EASY" };
        yield return new object[] { _repository.GetPuzzle("DE_INTERNET_HARD"), "Hard Puzzle DE_INTERNET_HARD" };
        yield return new object[] { _repository.GetPuzzle("MATRIZ_CASI_RESUELTA_SIN_8_y_9"), "Sin 7, 8 y 9" };
        yield return new object[] { _repository.GetPuzzle("MATRIZ_CASI_RESUELTA_SIN_5_6_7_8_y_9"), "Sin 5, 6, 7, 8 y 9" };
        yield return new object[] { _repository.GetPuzzle("MATRIZ_H_0"), "H0" };
        yield return new object[] { _repository.GetPuzzle("DE_INTERNET_EXPERT"), "Expert" };

        // You can paste your specific matrices here or reference your existing Matrices class
        // yield return new object[] { Matrices.MATRIZ_MASTER, "Master Matrix" };
    }

}

