You can enter the sudoku puzzle in the Matrices file, the format is the following:

public static int[,] DE_INTERNET_MEDIUM = {
    {0, 6, 0, 9, 0, 4, 0, 0, 1 },
    {1, 8, 3, 0, 5, 7, 6, 4, 0 },
    {0, 0, 4, 6, 0, 0, 0, 0, 7 },
    {0, 2, 0, 0, 0, 6, 7, 0, 0 },
    {6, 0, 0, 0, 0, 0, 2, 1, 0 },
    {7, 0, 0, 0, 0, 0, 0, 0, 0 },
    {5, 0, 0, 0, 7, 0, 0, 0, 2 },
    {0, 0, 0, 4, 0, 9, 0, 0, 0 },
    {0, 0, 2, 5, 0, 0, 3, 0, 4 }   
};

In the Program.cs file, you must change the line 
Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM), null, null);

Then execute the program.

