using Domain.Entities;

namespace Aplicacion.Interfaces;

public interface ISolutionBuilder
{
    List<Accion> Build(Nodo solutionNode, SortedDictionary<Tablerito, NodoSimple> visited);
}