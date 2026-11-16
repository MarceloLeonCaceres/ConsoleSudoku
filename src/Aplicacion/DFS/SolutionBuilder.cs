using Aplicacion.Interfaces;
using Domain.Entities;

namespace Aplicacion.DFS
{
    public sealed class SolutionBuilder : ISolutionBuilder
    {
        public List<Accion> Build(
            Nodo solutionNode,
            SortedDictionary<Tablerito, NodoSimple> visited)
        {
            var actions = new List<Accion>();
            var current = new NodoSimple(solutionNode);

            while (current.Acciones is not null)
            {
                actions.AddRange(current.Acciones);
                current = visited[current.PadreDelTablerito];
            }

            actions.Reverse();
            return actions;
        }
    }
}