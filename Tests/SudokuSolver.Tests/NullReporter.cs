using Aplicacion.Interfaces;
using Aplicacion.Models;
using Domain.Entities;

internal sealed class NullReporter : ISolverReporter
{
    public void OnProgress(Nodo nodo, int knownCells) { }
    public void OnElapsed(SolverState state) { }

    public void PrintSolution(SolveResult result)
    {
        throw new NotImplementedException();
    }

    public void PrintNoSolution(SolveResult result)
    {
        throw new NotImplementedException();
    }
}

