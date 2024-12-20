using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Conexiones
{
    public class Conexion
    {
        public SqlConnection Sqlcn;
        public MySqlConnection MySqlcn;
        DataSet dsData = new DataSet();
        String strSelectCommandText = "", mensaje = "", strResul = "";

        private void ConectarMySqlBDD(String strConexion)
        {
            try
            {
                MySqlcn = new MySqlConnection(strConexion);
                //MySqlcn.Open();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //Agregar un metodo para crear logs de auditoria 
            }
        }

        private void ConectarSqlBDD()
        {
            try
            {
                String conn = ConfigurationManager.ConnectionStrings["ConnecSQL"].ToString();
                Sqlcn = new SqlConnection(new Funciones().funDesencripta(conn));
            }
            catch (Exception ex)
            {
                //new Funciones().funCrearLogAuditoria(1, "Conexion-ConectarSqlBDD", ex.ToString(), 0);
                //Console.WriteLine(ex.Message);
            }
        }

        public Conexion(int intTipoConex, String strCadenaConexion)
        {
            switch (intTipoConex)
            {
                case 1:
                    ConectarMySqlBDD(strCadenaConexion);
                    break;
                case 2:
                    ConectarSqlBDD();
                    break;
            }
        }

        public DataSet funConsultarSqls(String strSP, object[] objparam)
        {
            try
            {
                strSelectCommandText = "exec " + strSP.ToString() + " ";
                for (int i = 0; i < objparam.GetLength(0); i++)
                {
                    if (objparam[i].GetType().ToString() == "System.String")
                    {
                        if (i == 0)
                        {
                            strSelectCommandText = strSelectCommandText + "'" + objparam[i].ToString() + "'";
                        }
                        else
                        {
                            strSelectCommandText = strSelectCommandText + ",'" + objparam[i].ToString() + "'";
                        }
                    }
                    else if (objparam[i].GetType().ToString() == "System.DateTime")
                    {
                        if (i == 0)
                        {
                            strSelectCommandText = strSelectCommandText + "'" + objparam[i].ToString() + "'";
                        }
                        else
                        {
                            strSelectCommandText = strSelectCommandText + ",'" + objparam[i].ToString() + "'";
                        }
                    }
                    else if (i == 0)
                    {
                        strSelectCommandText = strSelectCommandText + objparam[i].ToString();
                    }
                    else
                    {
                        strSelectCommandText = strSelectCommandText + "," + objparam[i].ToString();
                    }
                }
                new SqlDataAdapter(strSelectCommandText, Sqlcn).Fill(dsData);
                Sqlcn.Close();
                return dsData;
            }
            catch (Exception ex)
            {
                return dsData = null;
            }
        }


    }
}