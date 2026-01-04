using SudokuLibrary.DFS;
using SudokuLibrary.Model;

public sealed record SolveResult(
    bool Found,
    Nodo Root,
    Nodo SolutionNode,
    SolverState State,
    List<Accion> Actions)
{
    public static SolveResult Success(
        Nodo root,
        Nodo solution,
        SolverState state,
        List<Accion> actions)
        => new(true, root, solution, state, actions);

    public static SolveResult Failure(SolverState state)
        => new(false, null!, null!, state, []);
}
