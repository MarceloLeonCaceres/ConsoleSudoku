using Domain.Entities;

namespace Aplicacion.Models;

public sealed record SolveResult(bool Found, Nodo Root, Nodo? SolutionNode, SolverState State, IReadOnlyList<Accion>? Actions)
{
    public static SolveResult Success(Nodo root, Nodo solution, SolverState state, List<Accion> actions)
        => new(true, root, solution, state, actions.AsReadOnly());

    public static SolveResult Failure(Nodo root, SolverState state)
        => new(false, root, null, state, null);
}
