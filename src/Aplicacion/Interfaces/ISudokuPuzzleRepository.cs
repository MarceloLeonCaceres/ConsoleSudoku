namespace Aplicacion.Interfaces;

public interface ISudokuPuzzleRepository
{
    /// <summary>
    /// Obtiene una copia de la matriz inicial del Sudoku por su nombre.
    /// </summary>
    byte[,] GetPuzzle(string name);

    /// <summary>
    /// Devuelve los nombres de todos los tableros disponibles.
    /// </summary>
    IEnumerable<string> GetAvailablePuzzles();
}
