You can enter the sudoku puzzle in the Matrices file, the format consists of using zeros instead of blanks, as in the following example:

public static int[,] MY_CUSTOM_SUDOKU_PUZZLE = 
{
<br>    {0, 6, 0, 9, 0, 4, 0, 0, 1 },
<br>    {1, 8, 3, 0, 5, 7, 6, 4, 0 },
<br>    {0, 0, 4, 6, 0, 0, 0, 0, 7 },
<br>    {0, 2, 0, 0, 0, 6, 7, 0, 0 },
<br>    {6, 0, 0, 0, 0, 0, 2, 1, 0 },
<br>    {7, 0, 0, 0, 0, 0, 0, 0, 0 },
<br>    {5, 0, 0, 0, 7, 0, 0, 0, 2 },
<br>    {0, 0, 0, 4, 0, 9, 0, 0, 0 },
<br>    {0, 0, 2, 5, 0, 0, 3, 0, 4 }   
};

In the Program.cs file, you must change the line 
Nodo raiz = new Nodo(new Tablero(Matrices.MY_CUSTOM_SUDOKU_PUZZLE), null, null);

Then execute the program.
You can use Visual Studio Community

