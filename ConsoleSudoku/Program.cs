// See https://aka.ms/new-console-template for more information
using SudokuLibrary;
using SudokuLibrary.DFS;
using SudokuLibrary.Model;
using System.Linq;
using System.Text;


// Con Tablero
// Tablero tablero = new Tablero(Matrices.MATRIZ_H);
//Tablero tablero = new Tablero(Matrices.MATRIZ_H_2);
//Tablero tablero = new Tablero(Matrices.MATRIZ_DIAGONAL);
//tablero.PrintBoard();
//tablero.PrintPendientes();
//tablero.PrintCeldasPara();
//tablero.DetectCeldasUnicas();
//tablero.AccionesSiguientes();
//tablero.PrintResumen();

// Tablero hijo = new Tablero(tablero, new Celda(1, 8, 2), 3);
// Tablero hijo = new Tablero(tablero, new Accion(1, 8, 3));

Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H_0), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_NO_VALIDA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_6), null, null);
Stack<Nodo> frontera = new Stack<Nodo>();
List<Nodo> visitados = new List<Nodo>();

frontera.Push(raiz);
while (frontera.Count > 0)
{
    Nodo nodoActual = frontera.Pop();
    if (nodoActual.Tablero.EsSolucion) 
    {
        Console.WriteLine("Encontre la solucion!!!");
        nodoActual.Print();
        Console.WriteLine();
        while(nodoActual.Padre is not null)
        {
            Console.WriteLine("Nodo Padre:");
            nodoActual.Print();
            //nodoActual = visitados.Select();
        }
        break;
    }
    else
    {
        visitados.Add(nodoActual);
        List<Nodo> siguientes = nodoActual.Siguientes();
        if (siguientes != null)
        {
            foreach (Nodo nodo in siguientes)
            {
                if (!visitados.Contains(nodo) && !frontera.Contains(nodo))
                {
                    frontera.Push(nodo);
                    Console.WriteLine("No visitado y no en frontera");
                }
                else
                {
                    Console.WriteLine("Ya visitado o ya esta en frontera");
                }
                nodo.Print();
            }
        }
    }
}

Console.WriteLine("No hubo solucion, se terminaron los elementos de la frontera.");
