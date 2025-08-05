using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Materia
    {
        public int IdMateria { get; set; }
        public string Nombre { get; set; }
        public byte Creditos { get; set; }
        public decimal Costo { get; set; }
        //propiedad de navegacion
        public ML.Semestre Semestre { get; set; }
        public byte[] Imagen {  get; set; }
        public List<object>Materias { get; set; }
    }
}
