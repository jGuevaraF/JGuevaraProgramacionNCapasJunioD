using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Materia
    {
        public static string Add(ML.Materia materiaAdd)
        {
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    //destruir cuando se ejecute todo
                    string query = "INSERT INTO Materia VALUES (@Nombre, @Creditos, @Costo)";

                    SqlCommand command = new SqlCommand();
                    command.Connection = context;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Nombre", materiaAdd.Nombre);
                    command.Parameters.AddWithValue("@Creditos", materiaAdd.Creditos);
                    command.Parameters.AddWithValue("@Costo", materiaAdd.Costo);

                    context.Open();

                    int filasAfectadas = command.ExecuteNonQuery();

                    if(filasAfectadas > 0)
                    {
                        return "El registro se inserto correctamente";
                    } else
                    {
                        return "El registro no se inserto correctamente";
                    }

                }

            } catch (Exception ex)
            {
                //saber el error
                //error de conexion
                return "Hay un error";
            }
        }
    }
}
