using Aplicacion.Models;
using Domain.Entities;

namespace Aplicacion.Interfaces;

public interface ISolverReporter
{
    void OnProgress(Nodo nodo, int knownCells);
    void OnElapsed(SolverState state);
    void PrintSolution(SolveResult result);
    void PrintNoSolution(SolveResult result);

}
