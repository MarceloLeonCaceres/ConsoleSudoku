using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary.Model
{
    public class PendientesPorNumero
    {
        public int numero { get; set; }
        public List<int> filas { get; set; }
        public List<int> cols { get; set; }
        public List<int> grupos { get; set; }

        public PendientesPorNumero(int numero)
        {
            this.numero = numero;
            filas = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            cols = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            grupos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        public void ReducePor(int i, int j)
        {
            this.filas.Remove(i);
            this.cols.Remove(j);
            this.grupos.Remove(SudokuAbstraction.Cuadro(i, j));
        }

        public void ReducePor(Celda celda)
        {
            ReducePor(celda.X, celda.Y);
        }

    }
}
