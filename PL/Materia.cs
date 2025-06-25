using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Materia
    {
        public static void Add()
        {
            ML.Materia materia = new ML.Materia();

            Console.WriteLine("Dame el nombre");
            materia.Nombre = Console.ReadLine();

            Console.WriteLine("Dame los creditos");
            materia.Creditos = Convert.ToByte(Console.ReadLine());

            Console.WriteLine("Dame el costo");
            materia.Costo = Convert.ToDecimal(Console.ReadLine());

            string mensaje = BL.Materia.Add(materia);

            Console.WriteLine(mensaje);
        }


    }
}
