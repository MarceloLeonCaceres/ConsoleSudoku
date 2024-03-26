using SudokuLibrary.Model;
using System;

namespace SudokuLibrary
{
    public static class SudokuAbstraction
    {

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

        public static bool Valida(byte[,] matriz)
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
                grupo = VectorGrupo(matriz, i);
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

        public static int[] VectorGrupo(byte[,] matriz, int numeroGrupo)
        {
            int[] vector = new int[9];
            switch (numeroGrupo)
            {
                case 0:
                    vector[0] = matriz[0, 0];
                    vector[1] = matriz[0, 1];
                    vector[2] = matriz[0, 2];
                    vector[3] = matriz[1, 0];
                    vector[4] = matriz[1, 1];
                    vector[5] = matriz[1, 2];
                    vector[6] = matriz[2, 0];
                    vector[7] = matriz[2, 1];
                    vector[8] = matriz[2, 2];
                    return vector;

                case 1:
                    vector[0] = matriz[0, 3];
                    vector[1] = matriz[0, 4];
                    vector[2] = matriz[0, 5];
                    vector[3] = matriz[1, 3];
                    vector[4] = matriz[1, 4];
                    vector[5] = matriz[1, 5];
                    vector[6] = matriz[2, 3];
                    vector[7] = matriz[2, 4];
                    vector[8] = matriz[2, 5];
                    return vector;

                case 2:
                    vector[0] = matriz[0, 6];
                    vector[1] = matriz[0, 7];
                    vector[2] = matriz[0, 8];
                    vector[3] = matriz[1, 6];
                    vector[4] = matriz[1, 7];
                    vector[5] = matriz[1, 8];
                    vector[6] = matriz[2, 6];
                    vector[7] = matriz[2, 7];
                    vector[8] = matriz[2, 8];
                    return vector;

                case 3:
                    vector[0] = matriz[3, 0];
                    vector[1] = matriz[3, 1];
                    vector[2] = matriz[3, 2];
                    vector[3] = matriz[4, 0];
                    vector[4] = matriz[4, 1];
                    vector[5] = matriz[4, 2];
                    vector[6] = matriz[5, 0];
                    vector[7] = matriz[5, 1];
                    vector[8] = matriz[5, 2];
                    return vector;

                case 4:
                    vector[0] = matriz[3, 3];
                    vector[1] = matriz[3, 4];
                    vector[2] = matriz[3, 5];
                    vector[3] = matriz[4, 3];
                    vector[4] = matriz[4, 4];
                    vector[5] = matriz[4, 5];
                    vector[6] = matriz[5, 3];
                    vector[7] = matriz[5, 4];
                    vector[8] = matriz[5, 5];
                    return vector;

                case 5:
                    vector[0] = matriz[3, 6];
                    vector[1] = matriz[3, 7];
                    vector[2] = matriz[3, 8];
                    vector[3] = matriz[4, 6];
                    vector[4] = matriz[4, 7];
                    vector[5] = matriz[4, 8];
                    vector[6] = matriz[5, 6];
                    vector[7] = matriz[5, 7];
                    vector[8] = matriz[5, 8];
                    return vector;

                case 6:
                    vector[0] = matriz[6, 0];
                    vector[1] = matriz[6, 1];
                    vector[2] = matriz[6, 2];
                    vector[3] = matriz[7, 0];
                    vector[4] = matriz[7, 1];
                    vector[5] = matriz[7, 2];
                    vector[6] = matriz[8, 0];
                    vector[7] = matriz[8, 1];
                    vector[8] = matriz[8, 2];
                    return vector;

                case 7:
                    vector[0] = matriz[6, 3];
                    vector[1] = matriz[6, 4];
                    vector[2] = matriz[6, 5];
                    vector[3] = matriz[7, 3];
                    vector[4] = matriz[7, 4];
                    vector[5] = matriz[7, 5];
                    vector[6] = matriz[8, 3];
                    vector[7] = matriz[8, 4];
                    vector[8] = matriz[8, 5];
                    return vector;

                case 8:
                    vector[0] = matriz[6, 6];
                    vector[1] = matriz[6, 7];
                    vector[2] = matriz[6, 8];
                    vector[3] = matriz[7, 6];
                    vector[4] = matriz[7, 7];
                    vector[5] = matriz[7, 8];
                    vector[6] = matriz[8, 6];
                    vector[7] = matriz[8, 7];
                    vector[8] = matriz[8, 8];
                    return vector;
                default:
                    return vector;

            }
        }
    }
}
