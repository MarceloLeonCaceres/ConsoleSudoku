using SudokuLibrary.Model;

public static class SolutionBuilder
{
    public static List<Accion> Build(
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
