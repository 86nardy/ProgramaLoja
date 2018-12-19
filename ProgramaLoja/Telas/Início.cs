using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using ProgramaLoja.Clss;

namespace ProgramaLoja
{
    public partial class Início : Form
    {

        public Início()
        {
            InitializeComponent();
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = ClsUtil.RemoverAcentosEUpperCase(txtNome);
            string telefone = mtbTelefone.Text;
            string data = dtNasc.Value.ToShortDateString().Replace("/", ".");
            string cpf = mtbCPF.Text.Replace(",", "").Replace(".", "").Replace("-", "");

            List<string> valores = new List<string>();
            valores.Add(nome);
            valores.Add(telefone);
            valores.Add(data);
            valores.Add(cpf);

            if (nome != "" && data != "" && telefone != "" && cpf != "")
            {
                if (!ClsUtil.ValidarDataNascimento(dtNasc.Value))
                {
                    return;
                }
                else if (ClsUtil.ValidarCpf(cpf))
                {
                    string qry = $"SELECT * FROM USUARIOS WHERE usuarios.us_cpf = {cpf}";
                    DataTable dt = ClsBanco.RetornaDataTable(qry);
                    if (dt.Rows.Count > 0)
                        MessageBox.Show("Já existe um usuário cadastrado com esse CPF");
                    else
                    {
                        if (ClsBanco.InsertQry("USUARIOS", valores) == true)
                        {
                            limparCampos();
                        }
                    }

                }
                else
                    MessageBox.Show("CPF Incorreto!");

            }
            else
                MessageBox.Show("Há campos vazios");



        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            ClsUtil.RemoverAcentosEUpperCase(txtNome);
        }

        private void limparCampos()
        {
            txtNome.Text = "";
            dtNasc.Value = DateTime.Now;
            mtbCPF.Text = "";
            mtbTelefone.Text = "";
        }

    }
}
