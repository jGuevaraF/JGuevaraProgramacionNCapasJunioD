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

            ML.Result result = BL.Materia.Add(materia);

            Console.WriteLine(result);
        }

        
        public static void GetAll()
        {
            ML.Result result = BL.Materia.GetAll();
            if(result.Correct)
            {
                foreach (ML.Materia materia in result.Objects)
                {
                    Console.WriteLine("*************");
                    Console.WriteLine(materia.IdMateria);
                    Console.WriteLine(materia.Nombre);
                    Console.WriteLine(materia.Creditos);
                    Console.WriteLine(materia.Costo);
                    Console.WriteLine("*************");
                }
            }
            else
            {
                Console.WriteLine("No hay registros");
            }
        }

        public static void GetById()
        {
            Console.WriteLine("Dame el Id a buscar");
            int IdMateria = Convert.ToInt16(Console.ReadLine());

            ML.Result result = BL.Materia.GetById(IdMateria);

            if(result.Correct)
            {
                ML.Materia materia = (ML.Materia)result.Object;
                Console.WriteLine("*************");
                Console.WriteLine(materia.IdMateria);
                Console.WriteLine(materia.Nombre);
                Console.WriteLine(materia.Creditos);
                Console.WriteLine(materia.Costo);
                Console.WriteLine("*************");
            }
            else
            {
                Console.WriteLine("No se encontro materia");
            }

        }

    }
}
