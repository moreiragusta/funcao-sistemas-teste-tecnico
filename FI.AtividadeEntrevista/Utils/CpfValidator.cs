using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Utils
{
    /// <summary>
    /// Classe utilitária para validação de CPF
    /// </summary>
    public static class CpfValidator
    {
        /// <summary>
        /// Valida um CPF brasileiro
        /// </summary>
        /// <param name="cpf">CPF com ou sem formatação (999.999.999-99 ou 99999999999)</param>
        /// <returns>True se o CPF é válido, False caso contrário</returns>
        public static bool IsValid(string cpf)
        {
            // Remove caracteres não numéricos (pontos, traços, espaços)
            cpf = RemoverFormatacao(cpf);

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            // CPFs com todos dígitos iguais são inválidos
            if (cpf.Distinct().Count() == 1)
                return false;

            // Validação do primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Validação do segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }

        /// <summary>
        /// Remove formatação do CPF (pontos e traços)
        /// </summary>
        /// <param name="cpf">CPF formatado</param>
        /// <returns>CPF apenas com números</returns>
        public static string RemoverFormatacao(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Formata um CPF no padrão 999.999.999-99
        /// </summary>
        /// <param name="cpf">CPF sem formatação</param>
        /// <returns>CPF formatado</returns>
        public static string Formatar(string cpf)
        {
            cpf = RemoverFormatacao(cpf);

            if (cpf.Length != 11)
                return cpf;

            return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
        }
    }
}