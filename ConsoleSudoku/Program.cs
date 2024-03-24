// See https://aka.ms/new-console-template for more information
using SudokuLibrary;
using SudokuLibrary.DFS;
using SudokuLibrary.Model;
using System.Diagnostics;
using System.Globalization;


//Nodo raiz = new Nodo(new Tablero(TablaSeguimiento.MATRIZ_71), null, null);
//Nodo raiz = new Nodo(new Tablero(TablaSeguimiento.MATRIZ_71), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_SIN_5_6_7_8_y_9), null, null);   //  1s973
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
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM), null, null);   //  3s801     // 2s206     // 0s412 4,068 visitados
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_MEDIUM_2), null, null);   //  0s307   //  59 visitados
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD), null, null);       //  0s221   //  53 visitados
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_HARD_2), null, null);     //  2m24s295    2m15s   672,550 visitados
//Nodo raiz = new Nodo(new Tablero(Matrices.DE_INTERNET_EXPERT), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_1), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_2), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_NO_VALIDA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA), null, null);
// Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_6), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER), null, null);      //  28m // 4s234
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_2), null, null);      //  4s349     //  1s800   1768 visitados
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_3), null, null);      //  4s349     //  1s800   1768 visitados
Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MASTER_4), null, null);      //  6s210     //  3377 visitados


var timer = new Stopwatch();
timer.Start();
DateTime inicio = DateTime.Now;
DateTime fin;
Stack<Nodo> frontera = new Stack<Nodo>();

SortedDictionary<Tablerito, NodoSimple> visitados = new SortedDictionary<Tablerito, NodoSimple>();

Nodo nodoActual;
List<Accion> acciones;

NodoSimple nodoSimpleActual;
List<Accion> nuevasAcciones;

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
        timer.Stop();
        fin = DateTime.Now;
        encontroSolucion = true;
        PrintSolucion();
        acciones = new List<Accion>();
        nodoSimpleActual = new NodoSimple(nodoActual);

        while(nodoSimpleActual.Acciones is not null)
        {
            foreach(Accion ax in nodoSimpleActual.Acciones)
            {
                acciones.Add(ax);
            }
            nodoSimpleActual = visitados[nodoSimpleActual.PadreDelTablerito];
        }
        acciones.Reverse();

        PrintAcciones();
        break;
    }
    else
    {
        visitados[new Tablerito(nodoActual)] = new NodoSimple(nodoActual);

        if(visitados.Count % 100000 == 0)
        {
            PrintTiempoTranscurrido(timer, visitados.Count);
        }
        if (nodoActual.Tablero.EsViable)
        {
            List<Nodo>? siguientes = nodoActual.Siguientes();
            if (siguientes != null)
            {
                foreach (Nodo nodo in siguientes)
                {
                    if (!frontera.Contains(nodo) && !visitados.ContainsKey(new Tablerito(nodo)))
                    {
                        frontera.Push(nodo);
                    }
                }
            }
        }
    }
}

if (encontroSolucion == false)
{
    Console.WriteLine();
    Console.WriteLine("No hubo solucion, se terminaron los elementos de la frontera.");
    Console.WriteLine($"Inicio {inicio.ToLongTimeString()}");
    Console.WriteLine($"Fin {DateTime.Now.ToLongTimeString()}");
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
    Console.WriteLine($"Inicio {inicio.ToLongTimeString()}");
    Console.WriteLine($"Fin {fin.ToLongTimeString()}");
    Console.WriteLine($"Tiempo transcurrido: {timer.Elapsed.ToString(@"h\:mm\:ss\.fff")}");
    Console.WriteLine();
    raiz.Print();
    Console.WriteLine();
    nodoActual.Print();
    Console.WriteLine();
}

void PrintAcciones()
{
    Console.WriteLine($"Visitados: {visitados.Count.ToString("0,0", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"En Frontera: {frontera.Count}");
    Console.WriteLine($"Acciones: {acciones.Count}");
    Console.WriteLine("Solucion:");

    foreach (Accion accion in acciones)
    {
        Console.WriteLine($"{accion.Celda}  => {accion.Numero}");
    }
    Console.WriteLine();
    Console.ReadLine();
}

static void PrintTiempoTranscurrido(Stopwatch timer, int conteoVisitados)
{
    Console.WriteLine($"Visitados: {conteoVisitados.ToString("0,0", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Tiempo transcurrido: {timer.Elapsed.ToString(@"h\:mm\:ss\.fff")}");
    Console.WriteLine($"Hora: {DateTime.Now.ToLongTimeString()}");
    Console.WriteLine();
}