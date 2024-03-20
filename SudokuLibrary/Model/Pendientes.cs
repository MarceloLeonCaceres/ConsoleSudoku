using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary.Model
{
    public class Pendientes
    {
        public Pendientes() {
            Numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9};
        }

        public List<int> Numeros { get; set; }
    }
}
