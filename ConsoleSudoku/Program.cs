// See https://aka.ms/new-console-template for more information
using SudokuLibrary;
using SudokuLibrary.DFS;
using SudokuLibrary.Model;
using System.Linq;
using System.Text;


//Nodo raiz = new Nodo(new Tablero(TablaSeguimiento.MATRIZ_71), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_5_6_7_8_y_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_6_7_8_y_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_7_8_y_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_INTERMEDIA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_8_y_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_8_y_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_1), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H_Sin_Solucion), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_H_0), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_EASY), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM_2), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD_2), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_EXPERT), null, null);
Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_1), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_NO_VALIDA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA), null, null);
// Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_6), null, null);

Stack<Nodo> frontera = new Stack<Nodo>();
List<Nodo> visitados = new List<Nodo>();

//SortedDictionary<int[,], Nodo> dicVisitados = new SortedDictionary<int[,], Nodo>();
SortedDictionary<Tablero, Nodo> dicVisitados = new SortedDictionary<Tablero, Nodo>();

Nodo nodoActual;
List<Accion> acciones;

bool encontroSolucion = false;
frontera.Push(raiz);
int maxConocidos = 0;
while (frontera.Count > 0)
{
    nodoActual = frontera.Pop();
    int n = nodoActual.Tablero.CeldasConocidas.Count;
    if (n > maxConocidos)
    {
        PrintNodo(nodoActual, n);
        maxConocidos = n;
    }
    if (nodoActual.Tablero.EsSolucion)
    {
        encontroSolucion = true;
        PrintSolucion();
        acciones = new List<Accion>();
        while(nodoActual.Padre is not null)
        {
            acciones.Add(nodoActual.Accion);
            //nodoActual = visitados.Find(n => n.Tablero == nodoActual.Padre);
            nodoActual = dicVisitados[nodoActual.Padre];
        }
        acciones.Reverse();
        PrintAcciones();
        break;
    }
    else
    {
        //visitados.Add(nodoActual);
        dicVisitados[nodoActual.Tablero] = nodoActual;
        if(nodoActual.Tablero.EsViable)
        {
            List<Nodo> siguientes = nodoActual.Siguientes();
            if (siguientes != null)
            {
                foreach (Nodo nodo in siguientes)
                {
                    //if (!frontera.Contains(nodo) && !visitados.Contains(nodo))
                    if (!frontera.Contains(nodo) && !dicVisitados.ContainsKey(nodo.Tablero))
                    {
                        frontera.Push(nodo);
                    }
                }
            }
        }
    }
}

if(encontroSolucion == false)
{
    Console.WriteLine("No hubo solucion, se terminaron los elementos de la frontera.");
    Console.WriteLine($"Visitados: {visitados.Count}");
    Console.WriteLine($"En Frontera: {frontera.Count}");
}

void PrintNodo(Nodo nodo, int n)
{
    Console.WriteLine($"Conocidas: {n}");
    nodoActual.Print();
    Console.WriteLine();
}

void PrintSolucion()
{
    Console.Clear();
    Console.WriteLine("Encontre la solucion!!!");
    Console.WriteLine();
    raiz.Print();
    Console.WriteLine();
    nodoActual.Print();
    Console.WriteLine();
}

void PrintAcciones()
{
    Console.WriteLine($"Visitados: {dicVisitados.Count}");
    Console.WriteLine($"En Frontera: {frontera.Count}");
    Console.WriteLine($"Acciones: {acciones.Count}");
    Console.WriteLine("Solucion:");

    foreach (Accion accion in acciones)
    {
        Console.WriteLine($"{accion.Celda}  => {accion.Numero}");
    }
    Console.WriteLine();
}