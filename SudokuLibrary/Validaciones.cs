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

        //public static bool EsFactible(int[,] matriz, (int x, int y) celda, int num)
        //{
        //    if (matriz[celda.x, celda.y] > 0)
        //    {
        //        return false;
        //    }
        //    if (Contiene(matriz, SudokuAbstraction.Fila(celda.x), num))
        //    {
        //        return false;
        //    }
        //    if (Contiene(matriz, SudokuAbstraction.Col(celda.y), num))
        //    {
        //        return false;
        //    }
        //    if (Contiene(matriz, SudokuAbstraction.Cuadrado(celda.x, celda.y), num))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public static bool EsValida(int[,] matriz)
        //{
        //    var celdasConocidasPorNumero = Solver.CeldasConocidasPorNumero;
        //    for(int numero = 1; numero <= 9; numero++)
        //    {
        //        int n = celdasConocidasPorNumero[numero].Count;
        //        if (n > 9)
        //        {
        //            return false;
        //        }
        //        if (n >= 2)
        //        {
        //            for(int i = 0; i < n -1; i++)
        //            {
        //                for(int j = i+1; j < n; j++)
        //                {
        //                    if (celdasConocidasPorNumero[numero][i].x == celdasConocidasPorNumero[numero][j].x)
        //                    {
        //                        return false;
        //                    }
        //                    if (celdasConocidasPorNumero[numero][i].y == celdasConocidasPorNumero[numero][j].y)
        //                    {
        //                        return false;
        //                    }
        //                    if (SudokuAbstraction.Cuadro(celdasConocidasPorNumero[numero][i].x, celdasConocidasPorNumero[numero][i].y) 
        //                        == SudokuAbstraction.Cuadro(celdasConocidasPorNumero[numero][j].x, celdasConocidasPorNumero[numero][j].y ))
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}
    }
}
