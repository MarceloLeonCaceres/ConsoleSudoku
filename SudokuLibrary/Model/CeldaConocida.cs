using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary.Model
{
    public class CeldaConocida 
    {
        Celda CConocida { get; set; }
        public int numero { get; set; }

        public CeldaConocida(int x, int y, int num) 
        {
            CConocida = new Celda(x, y);
            numero = num;
        }
    }
}
