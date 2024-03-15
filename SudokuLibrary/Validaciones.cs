using SudokuLibrary.Model;

namespace SudokuLibrary
{
    public static class Validaciones
    {
        public static bool Contiene(int[,] Matriz, List<(int x, int y)> grupo, int num)
        {
            foreach ((int x, int y) position in grupo)
            {
                if (Matriz[position.x, position.y] == num)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool EstaCompleta(int[,] Matriz, List<(int x, int y)> grupo)
        {
            HashSet<int> numeros = new HashSet<int>();
            foreach ((int x, int y) position in grupo)
            {
                numeros.Add(Matriz[position.x, position.y]);
            }
            if (numeros.Count == 9)
            {
                return true;
            }
            return false;
        }


        public static bool EstaResuelto(int[,] matriz)
        {
            for (int x = 0; x < matriz.Length; x++)
            {
                for (int y = 0; y < matriz.Length; y++)
                {
                    if (matriz[x, y] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Dictionary<int, int> Dict(int[,] matriz)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (dic.ContainsKey(matriz[x, y]))
                    {
                        dic[matriz[x, y]]++;
                    }
                    else
                    {
                        dic[matriz[x, y]] = 1;
                    }
                }
            }
            return dic;
        }

        public static void Descarta(List<Celda> celdas, Celda paraEliminar)
        {
            foreach (Celda celda in celdas)
            {
                if(celda.X == paraEliminar.X || celda.Y == paraEliminar.Y || celda.Z == paraEliminar.Z)
                {
                    celdas.Remove(celda);
                }
            }
        }
    }
}
