using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace ProgramaLoja.Clss
{
    class ClsBanco
    {
        private static FbDataAdapter fda;
        private static DataTable dt;
        private static FbConnection fbConn;
        private static FbCommand fbCmd;
        private static string connectionString;

        public static FbConnection AbrirConexao()
        {
            connectionString = "User=SYSDBA;" +
                                "Password=masterkey;" +
                                "Database=C:\\Projetos-Nardy\\BaseDados\\BASE.fdb;" +
                                "DataSource=localhost;" +
                                "Port=3050;" +
                                "Dialect=3;" +
                                "Charset=NONE;" +
                                "Role=;" +
                                "Connection lifetime=15;" +
                                "Pooling=true;" +
                                "MinPoolSize=0;" +
                                "MaxPoolSize=50;" +
                                "Packet Size=8192;" +
                                "ServerType=0";
            fbConn = new FbConnection(connectionString);
            try
            {
                fbConn.Open();
            }
            catch (FbException ex)
            {
                throw ex;
            }
            return fbConn;
        }

        public static void FecharConexao()
        {
            fbConn.Close();
        }

        public static DataTable RetornaDataTable(string qry)
        {
            dt = new DataTable();
            ExecuteQry(qry);
            fda = new FbDataAdapter(qry, ClsBanco.AbrirConexao());
            fda.Fill(dt);
            return dt;
        }

        public static void ExecuteQry(string qry)
        {
            fbCmd = new FbCommand(qry, ClsBanco.AbrirConexao());
            try
            {
                fbCmd.ExecuteNonQuery();
            }
            catch (FbException e)
            {
                MessageBox.Show(e.ToString());
            }
            ClsBanco.FecharConexao();
        }

        public static bool InsertQry(string nomeTabela, List<string> valores)
        {
            //ClsBanco.AbrirConexao();
            string qryInsert = string.Empty;

            qryInsert = $"INSERT INTO {nomeTabela} ({TrazerOsCamposDaTabela(nomeTabela)}) VALUES ({ValoresParaInserirNaTabela(valores)});";

            try
            {
                ClsBanco.ExecuteQry(qryInsert);
                MessageBox.Show($"Inserção na tabela {nomeTabela} realizado com sucesso!");
                return true;
            }
            catch (FbException fbex)
            {
                MessageBox.Show("InsertQry: " + fbex);
                return false;
            }
        }

        private static string TrazerOsCamposDaTabela(string nomeTabela)
        {
            string colunas = string.Empty;

            DataTable dt = ClsBanco.RetornaDataTable($"SELECT FIRST 1 * FROM {nomeTabela}");

            for (int i = 1; i <= dt.Columns.Count - 1; i++)
                colunas += dt.Columns[i].ColumnName.ToUpper().ToString() + ", ";

            colunas = colunas.Remove(colunas.Length - 2);

            return colunas;
        }

        private static string ValoresParaInserirNaTabela(List<string> valores)
        {
            string strValores = string.Empty;

            if (!(valores is null) || !valores.Count.Equals(0))
            {
                foreach (string valor in valores)
                {
                    strValores += "'" + valor + "', ";
                }
            }

            strValores = strValores.Remove(strValores.Length - 2);

            return strValores;
        }
    }
}
