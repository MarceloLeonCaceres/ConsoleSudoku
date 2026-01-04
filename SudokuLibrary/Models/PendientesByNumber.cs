namespace SudokuLibrary.Model
{
    public class PendientesByNumber
    {
        public List<int> filas { get; set; }
        public List<int> cols { get; set; }
        public List<int> grupos { get; set; }

        public PendientesByNumber()
        {
            filas = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            cols = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            grupos = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }


        public PendientesByNumber CopyPending()
        {
            PendientesByNumber copia = new();
            copia.filas = new List<int>(this.filas);
            copia.cols = new List<int>(this.cols);
            copia.grupos = new List<int>(this.grupos);
            return copia;
        }

        public void ReducePor(Celda celda)
        {
            this.filas.Remove(celda.X);
            this.cols.Remove(celda.Y);
            this.grupos.Remove(SudokuAbstraction.Cuadro(celda.X, celda.Y));

        }

        public bool EsValido()
        {
            if ((this.filas.Count != this.cols.Count) || (this.filas.Count != this.grupos.Count))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
