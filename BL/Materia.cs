using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

                    if (filasAfectadas > 0)
                    {
                        return "El registro se inserto correctamente";
                    }
                    else
                    {
                        return "El registro no se inserto correctamente";
                    }

                }

            }
            catch (Exception ex)
            {
                //saber el error
                //error de conexion
                return "Hay un error";
            }
        }

        public static List<object> GetAll()
        {
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    //destruir cuando se ejecute todo
                    string query = "select IdMateria, Nombre, Creditos, Costo from Materia";

                    SqlCommand command = new SqlCommand();
                    command.Connection = context;
                    command.CommandText = query;

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    //inicializa lista
                    List<object> materias = new List<object>();

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Materia materia = new ML.Materia();
                            materia.IdMateria = Convert.ToInt16(row[0]);
                            materia.Nombre = Convert.ToString(row[1]);
                            materia.Creditos = Convert.ToByte(row[2]);
                            materia.Costo = Convert.ToDecimal(row[3]);

                            //agregamos materia a la lista
                            materias.Add(materia);
                        }
                        return materias;
                    }
                    else
                    {
                        return new List<object>();
                    }

                }

            }
            catch (Exception ex)
            {
                //saber el error
                //error de conexion
                return new List<object>();
            }

        }

        public static ML.Materia GetById(int IdMateria)
        {
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                     string query = "select IdMateria, Nombre, Creditos, Costo from Materia where IdMateria = @IdMateria";

                    SqlCommand command = new SqlCommand();
                    command.Connection = context;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@IdMateria", IdMateria);


                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);


                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];

                        ML.Materia materia = new ML.Materia();
                        materia.IdMateria = Convert.ToInt16(row[0]);
                        materia.Nombre = Convert.ToString(row[1]);
                        materia.Creditos = Convert.ToByte(row[2]);
                        materia.Costo = Convert.ToDecimal(row[3]);

                        return materia;
                    }
                    else
                    {
                        return null;
                    }

                }

            }
            catch (Exception ex)
            {
                //saber el error
                //error de conexion
                return null;
            }

        }



    }
}
