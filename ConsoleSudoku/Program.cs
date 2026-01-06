using SudokuLibrary;
using SudokuLibrary.Models;

internal static class Program
{
    static void Main()
    {
        var cts = new CancellationTokenSource(); 

        var root = CreateRootNode();

        var reporter = new ConsoleReporter();

        ISudokuSolver solver = new SudokuSolver(reporter);
        try
        {
            var result = solver.Solve(root, cts.Token);

            if (!result.Found)
            {
                reporter.PrintNoSolution(result);
                return;
            }

            reporter.PrintSolution(result);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Solving was canceled por el usuario.");
        }
    }

    private static Nodo CreateRootNode()
    {
        return new Nodo(
            new Tablero(Matrices.MATRIZ_MIA_1),
            null!,
            null!
        );
    }
}


////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_71), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_71), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_5_6_7_8_y_9), null, null);   //  1s973
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_6_7_8_y_9), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_7_8_y_9), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_INTERMEDIA), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_8_y_9), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_8_y_9), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_1), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H_Sin_Solucion), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H_0), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_EASY), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM), null, null);   //  3s801     // 2s206     // 0s412 4,068 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM_2), null, null);   //  0s307   //  59 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD), null, null);       //  0s221   //  53 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD_2), null!, null!);     //  2m24s295    2m15s   672,550 visitados   //  0s961   1,414 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_EXPERT), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_1), null!, null!);     // 28 min   2.239.781   // 0s371    752 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_2), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_NO_VALIDA), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA), null, null);
//// Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_9), null, null);
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_6), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER), null!, null!);      //  28m // 4s234    //  2s904   // 1454 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_2), null, null);      //  3s971     //   1768 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_3), null, null);      //  3s966     //    1986 visitados
////Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_4), null, null);      //  6s210     //  3377 visitados
