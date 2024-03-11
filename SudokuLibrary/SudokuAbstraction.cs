using SudokuLibrary.Model;
using System;

namespace SudokuLibrary
{
    public static class SudokuAbstraction
    {

        public static List<Celda> Fila(int i)
        {
            List<Celda> celdas = new List<Celda>();
            for (int k = 0; k < 9; k++)
            {
                celdas.Add(new Celda(i, k));
            }
            return celdas;
        }

        public static List<Celda> Col(int i)
        {
            List<Celda> celdas = new List<Celda>();
            for (int k = 0; k < 9; k++)
            {
                celdas.Add(new Celda(k, i));
            }
            return celdas;
        }

        public static List<Celda>? Cuadrado(int z)
        {
            List<Celda> celdas = new List<Celda>();
            List<int> indicesFila = new List<int>();
            List<int> indicesColumna = new List<int>();

            if (z < 0 || z >= 9)
            {
                return null;
            }
            // indicesFila segun el cuadrado
            if (z == 0 || z == 1 || z == 2) {
                indicesFila = new List<int> { 0, 1, 2 };
            }
            else if (z == 3 || z == 4 || z == 5)
            {
                indicesFila = new List<int> { 3, 4, 5 };
            }
            else // (z == 6 || z == 7 || z == 8)
            {
                indicesFila = new List<int> { 6, 7, 8 };
            }

            // indicesColumna segun el cuadrado
            if (z == 0 || z == 3 || z == 6)
            {
                indicesColumna = new List<int> { 0, 1, 2 };
            }
            else if (z == 1 || z == 4 || z == 7)
            {
                indicesColumna = new List<int> { 3, 4, 5 };
            }
            else // (z == 2 || z == 5 || z == 8)
            {
                indicesColumna = new List<int> { 6, 7, 8 };
            }

            foreach (int i in indicesFila)
            {
                foreach (int j in indicesColumna)
                {
                    celdas.Add(new Celda(i, j, z));
                }
            }
            return celdas;
        }

        public static int Cuadro(int x, int y)
        {
            if (x < 0 || x >= 9 || y < 0 || y >= 9)
            {
                return -1;
            }
            if (x == 0 || x == 1 || x == 2)
            {
                if (y == 0 || y == 1 || y == 2)
                {
                    return 0;
                }
                else if (y == 3 || y == 4 || y == 5)
                {
                    return 1;
                }
                else // (y == 6 || y == 7 || y == 8)
                {
                    return 2;
                }
            }
            else if (x == 3 || x == 4 || x == 5)
            {
                if (y == 0 || y == 1 || y == 2)
                {
                    return 3;
                }
                else if (y == 3 || y == 4 || y == 5)
                {
                    return 4;
                }
                else // (y == 6 || y == 7 || y == 8)
                {
                    return 5;
                }
            }
            else // (x == 6 || x == 7 || x == 8)
            {
                if (y == 0 || y == 1 || y == 2)
                {
                    return 6;
                }
                else if (y == 3 || y == 4 || y == 5)
                {
                    return 7;
                }
                else // (y == 6 || y == 7 || y == 8)
                {
                    return 8;
                }
            }
        }

        public static bool Valida(int[,] matriz)
        {
            for (int i = 0; i < 9; i++)
            {
                int[] fila = new int[9];
                int[] columna = new int[9];
                int[] grupo = new int[9];
                for (int k = 0; k < 9; k++)
                {
                    fila[k] = matriz[i, k];
                    columna[k] = matriz[k, i];
                }
                int zAux = 0;
                for (int filaAux = 0; filaAux < 9; filaAux++)
                {
                    for (int colAux = 0; colAux < 9; colAux++)
                    {
                        if (Cuadro(filaAux, colAux) == i)
                        {
                            grupo[zAux] = matriz[filaAux, colAux];
                            zAux++;
                        }
                    }
                }
                if (EsSubGrupoValido(fila) == false || EsSubGrupoValido(columna) == false || EsSubGrupoValido(grupo) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EsSubGrupoValido(int[] grupo)
        {
            for (int i = 1; i <= 9; i++)
            {
                int cuenta = grupo.Where(c => c == i).Count();
                if (cuenta > 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
