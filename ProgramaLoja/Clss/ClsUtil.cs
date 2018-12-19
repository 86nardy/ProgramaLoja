using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProgramaLoja.Clss
{
    class ClsUtil
    {
        public static string RemoverAcentosEUpperCase(TextBox nomeTextBox)
        {
            var normalizedString = nomeTextBox.Text.Normalize(NormalizationForm.FormD);
           
            nomeTextBox.Text = "";
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    nomeTextBox.Text += c.ToString();
                }
            }

            nomeTextBox.Text = nomeTextBox.Text.ToUpper();
            for (int i = 0; i<=nomeTextBox.Text.Length-1;i++)
            {
                if (!(Regex.IsMatch(nomeTextBox.Text[i].ToString(), "^[a-zA-Z ]")))
                    nomeTextBox.Text = nomeTextBox.Text.Replace(nomeTextBox.Text[i].ToString(), nomeTextBox.Text[i+1].ToString());
            }
            return nomeTextBox.Text;
        }
        public static bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(",","") ;

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
        public static bool ValidarDataNascimento(DateTime data)
        {
            if (data > DateTime.Now)
            {
                MessageBox.Show("Data informada é maior que a atual!");
                return false;
            }
            else
                return true;
        }
    }


    
}
