using ML;
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
        //Querys
        public static ML.Result Add(ML.Materia materiaAdd)
        {
            ML.Result result = new ML.Result();
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
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;
        }

        public static ML.Result Update(ML.Materia materia)
        {
            ML.Result result = new ML.Result();
            using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UPDATE Materia SET [Nombre] = @Nombre, [Creditos] = @Creditos, [Costo] = @Costo, [IdSemestre] = @IdSemestre, [Imagen] = @Imagen WHERE IdMateria = @IdMateria";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IdMateria", materia.IdMateria);
                    cmd.Parameters.AddWithValue("@Nombre", materia.Nombre);
                    cmd.Parameters.AddWithValue("@Creditos", materia.Creditos);
                    cmd.Parameters.AddWithValue("@Costo", materia.Costo);
                    cmd.Parameters.AddWithValue("@IdSemestre", materia.Semestre.IdSemestre);
                    cmd.Parameters.AddWithValue("@Imagen", materia.Imagen);

                    cmd.Connection = context;

                    context.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            return result;
        }

        public static ML.Result Delete(int IdMateria)
        {
            ML.Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    var cmd = new SqlCommand("Delete from Materia where IdMateria = @IdMateria", conn);
                    cmd.Parameters.AddWithValue("@IdMateria", IdMateria);
                    conn.Open();


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
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
                    result.Objects = new List<object>();

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
                            result.Objects.Add(materia);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;
        }

        public static ML.Result GetById(int IdMateria)
        {
            ML.Result result = new ML.Result();
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                     string query = "select IdMateria, Nombre, Creditos, Costo, IdSemestre, Imagen from Materia where IdMateria = @IdMateria";

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
                        materia.Semestre = new ML.Semestre();
                        materia.IdMateria = Convert.ToInt16(row[0]);
                        materia.Nombre = Convert.ToString(row[1]);
                        materia.Creditos = Convert.ToByte(row[2]);
                        materia.Costo = Convert.ToDecimal(row[3]);
                        materia.Semestre.IdSemestre = row[4] != DBNull.Value ? Convert.ToInt16(row[4]) : 0;
                        materia.Imagen = row[5] != DBNull.Value ? (byte[])row[5] : new byte[0];

                        result.Object = materia;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;

        }

        //Stored procedures
        public static ML.Result GetAllSP()
        {
            ML.Result result = new ML.Result();
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    //destruir cuando se ejecute todo
                    string query = "MateriaGetAll";

                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = context;
                    command.CommandText = query;

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    //inicializa lista
                    result.Objects = new List<object>();

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
                            result.Objects.Add(materia); //Boxing
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;
        }

        public static ML.Result AddSP(ML.Materia materiaAdd)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    string query = "MateriaAdd";

                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = context;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Nombre", materiaAdd.Nombre);
                    command.Parameters.AddWithValue("@Creditos", materiaAdd.Creditos);
                    command.Parameters.AddWithValue("@Costo", materiaAdd.Costo);
                    command.Parameters.AddWithValue("@IdSemestre", materiaAdd.Semestre.IdSemestre);
                    command.Parameters.AddWithValue("@Imagen", materiaAdd.Imagen);

                    context.Open();

                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.exception = ex;
            }
            return result;
        }

        public static ML.Result UpdateSP(ML.Materia materia)
        {
            ML.Result result = new ML.Result();
            using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "MateriaUpdate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMateria", materia.IdMateria);
                    cmd.Parameters.AddWithValue("@Nombre", materia.Nombre);
                    cmd.Parameters.AddWithValue("@Creditos", materia.Creditos);
                    cmd.Parameters.AddWithValue("@Costo", materia.Costo);
                    cmd.Parameters.AddWithValue("@IdSemestre", materia.Semestre.IdSemestre);
                    cmd.Parameters.AddWithValue("@Imagen", materia.Imagen);

                    cmd.Connection = context;

                    context.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            return result;
        }
    }
}
