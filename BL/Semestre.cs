using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Semestre
    {

        //Store procedure
        public static ML.Result GetAllSP()
        {
            ML.Result result = new ML.Result();
            try
            {
                //intenta
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    //destruir cuando se ejecute todo
                    string query = "SemestreGetAll";

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
                            ML.Semestre semestre = new ML.Semestre();

                            semestre.IdSemestre = Convert.ToInt16(row[0]);
                            semestre.Nombre = Convert.ToString(row[1]);

                            //agregamos materia a la lista
                            result.Objects.Add(semestre); //Boxing
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


        //LINQ
        public static ML.Result GetAllLinq()
        {
            ML.Result result = new ML.Result();
            try
            {
                //intenta
                using (DL_EF.JGuevaraProgramacionNCapasJunioEntities context = new DL_EF.JGuevaraProgramacionNCapasJunioEntities())
                {
                    var listaSemestres = (
                        from semestre in context.Semestes
                        select new
                        {
                            semestre.IdSemestre,
                            semestre.Nombre
                        }).ToList();

                    //inicializa lista
                    result.Objects = new List<object>();

                    if (listaSemestres.Count > 0)
                    {
                        foreach(var semestre in listaSemestres)
                        {
                            ML.Semestre semestreItem = new ML.Semestre();
                            semestreItem.IdSemestre = semestre.IdSemestre;
                            semestreItem.Nombre = semestre.Nombre;

                            result.Objects.Add(semestreItem);
                        }
                        result.Correct = true;
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
    }
}
