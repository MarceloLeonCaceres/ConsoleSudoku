using Aplicacion.DFS;
using Aplicacion.Interfaces;
using ConsoleSudoku;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal static class Program
{
    static void Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        using var serviceProvider = services.BuildServiceProvider();

        // 2. Resolver Dependencias
        var solver = serviceProvider.GetRequiredService<ISudokuSolver>();
        var reporter = serviceProvider.GetRequiredService<ISolverReporter>();
        var repository = serviceProvider.GetRequiredService<ISudokuPuzzleRepository>();
        var validator = serviceProvider.GetRequiredService<ISudokuValidator>();

        // 3. Obtener matriz y construir nodo raíz
        var matrizInicial = repository.GetPuzzle("DE_INTERNET_EASY");
        var tableroRaiz = new Tablero(matrizInicial, validator);
        var nodoRaiz = new Nodo(tableroRaiz, null!, null!);

        var cts = new CancellationTokenSource();

        try
        {
            var result = solver.Solve(nodoRaiz, cts.Token);

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
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        // Dominio
        services.AddSingleton<ISudokuValidator, SudokuValidator>();

        // Infraestructura
        services.AddSingleton<ISudokuPuzzleRepository, InMemorySudokuRepository>();

        // Aplicación
        services.AddTransient<ISolutionBuilder, SolutionBuilder>();
        services.AddTransient<ISudokuSolver, SudokuSolver>();

        // Presentación (UI)
        services.AddSingleton<ISolverReporter, ConsoleReporter>();
    }
    //private static Nodo CreateRootNode()
    //{
    //    return new Nodo(
    //        new Tablero(Matrices.JUEGO_ARTO_INKALA),
    //        //new Tablero(Matrices.MATRIZ_MASTER_2),
    //        null!,
    //        null!
    //    );
    //}
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
