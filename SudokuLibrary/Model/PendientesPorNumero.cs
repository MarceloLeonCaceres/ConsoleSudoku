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

        public PendientesPorNumero CopiaPendientes()
        {
            PendientesPorNumero copia = new PendientesPorNumero(this.numero);
            copia.filas = new List<int>(this.filas);
            copia.cols = new List<int>(this.cols);
            copia.grupos = new List<int>(this.grupos);
            return copia;
        }

        public void ReducePor(Celda celda)
        {
            // ReducePor(celda.X, celda.Y);

            this.filas.Remove(celda.X);
            this.cols.Remove(celda.Y);
            this.grupos.Remove(SudokuAbstraction.Cuadro(celda.X, celda.Y));
        }

    }
}
