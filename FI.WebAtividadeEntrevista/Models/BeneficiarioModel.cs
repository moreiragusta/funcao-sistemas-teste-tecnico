using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de modelo de Beneficiário
    /// </summary>
    public class BeneficiarioModel
    {
        /// <summary>
        /// ID do beneficiário
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CPF do beneficiário
        /// </summary>
        [Required(ErrorMessage = "CPF do beneficiário é obrigatório")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve estar no formato 999.999.999-99")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome do beneficiário
        /// </summary>
        [Required(ErrorMessage = "Nome do beneficiário é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// ID do cliente (Foreign Key)
        /// </summary>
        [Required]
        public long IdCliente { get; set; }
    }
}