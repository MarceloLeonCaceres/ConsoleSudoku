// See https://aka.ms/new-console-template for more information
using SudokuLibrary;
using SudokuLibrary.DFS;
using SudokuLibrary.Model;
using System.Linq;
using System.Text;


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
Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_MIA_1), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_NO_VALIDA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_9), null, null);
//Nodo raiz = new Nodo(new Tablero(Matrices.MATRIZ_CASI_RESUELTA_MENOS_6), null, null);
Stack<Nodo> frontera = new Stack<Nodo>();
List<Nodo> visitados = new List<Nodo>();
List<Tablero> tablerosVistos = new List<Tablero>();

bool encontroSolucion = false;
frontera.Push(raiz);
int maxConocidos = 0;
while (frontera.Count > 0)
{
    Nodo nodoActual = frontera.Pop();
    int n = nodoActual.Tablero.CeldasConocidas.Count;
    if (n > maxConocidos)
    {
        Console.WriteLine($"Conocidas: {n}");
        nodoActual.Print();
        Console.WriteLine();
        maxConocidos = n;
    }
    if (nodoActual.Tablero.EsSolucion) 
    {
        Console.Clear();
        Console.WriteLine("Encontre la solucion!!!");
        Console.WriteLine();
        raiz.Print();
        Console.WriteLine();
        nodoActual.Print();
        Console.WriteLine();
        List<Accion> acciones = new List<Accion>();
        while(nodoActual.Padre is not null)
        {
            //Console.WriteLine("Nodo Padre:");
            //nodoActual.Print();
            acciones.Add(nodoActual.Accion);
            nodoActual = visitados.Find(n => n.Tablero == nodoActual.Padre);
        }
        encontroSolucion = true;
        acciones.Reverse();

        Console.WriteLine($"Visitados: {visitados.Count}");
        Console.WriteLine($"En Frontera: {frontera.Count}");
        Console.WriteLine($"Acciones: {acciones.Count}");
        Console.WriteLine("Solucion:");


        foreach (Accion accion in acciones)
        {
            Console.WriteLine($"{accion.Celda}  => {accion.Numero}");
        }
        Console.WriteLine();
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
                if (!frontera.Contains(nodo) && !visitados.Contains(nodo))
                {
                    frontera.Push(nodo);
                    //Console.WriteLine("No visitado y no en frontera");
                }
                //else if (!visitados.Contains(nodo) && !nodo.Tablero.EsViable)
                //{
                //    //Console.WriteLine("Ya visitado o ya esta en frontera");
                //}
                //nodo.Print();
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
