using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de Beneficiário
    /// </summary>
    public class Beneficiario
    {
        /// <summary>
        /// ID do beneficiário
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CPF do beneficiário (formato: 999.999.999-99)
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Nome do beneficiário
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// ID do cliente (Foreign Key)
        /// </summary>
        public long IdCliente { get; set; }
    }
}