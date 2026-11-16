using Aplicacion.Models;
using Domain.Entities;

namespace Aplicacion.Interfaces;

public interface ISudokuSolver
{
    SolveResult Solve(Nodo root, CancellationToken cancellationToken = default);
}
