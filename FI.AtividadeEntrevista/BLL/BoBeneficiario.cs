using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    /// <summary>
    /// Classe de lógica de negócio de Beneficiário
    /// </summary>
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        /// <returns>ID do beneficiário inserido</returns>
        public long Incluir(Beneficiario beneficiario)
        {
            // Validação: Nome obrigatório
            if (string.IsNullOrWhiteSpace(beneficiario.Nome))
                throw new Exception("Nome do beneficiário é obrigatório");

            // Validação: CPF obrigatório
            if (string.IsNullOrWhiteSpace(beneficiario.CPF))
                throw new Exception("CPF do beneficiário é obrigatório");

            // Remove formatação do CPF
            string cpfLimpo = CpfValidator.RemoverFormatacao(beneficiario.CPF);

            // Validação: CPF válido
            if (!CpfValidator.IsValid(cpfLimpo))
                throw new Exception("CPF do beneficiário inválido");

            // Validação: CPF duplicado para o mesmo cliente
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            if (dao.VerificarDuplicado(cpfLimpo, beneficiario.IdCliente))
                throw new Exception("Já existe um beneficiário com este CPF para este cliente");

            // Salva CPF formatado
            beneficiario.CPF = CpfValidator.Formatar(cpfLimpo);

            return dao.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário existente
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void Alterar(Beneficiario beneficiario)
        {
            // Validação: Nome obrigatório
            if (string.IsNullOrWhiteSpace(beneficiario.Nome))
                throw new Exception("Nome do beneficiário é obrigatório");

            // Validação: CPF obrigatório
            if (string.IsNullOrWhiteSpace(beneficiario.CPF))
                throw new Exception("CPF do beneficiário é obrigatório");

            // Remove formatação do CPF
            string cpfLimpo = CpfValidator.RemoverFormatacao(beneficiario.CPF);

            // Validação: CPF válido
            if (!CpfValidator.IsValid(cpfLimpo))
                throw new Exception("CPF do beneficiário inválido");

            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();

            // Consulta beneficiário atual
            var benefAtual = dao.Consultar(beneficiario.Id);
            if (benefAtual == null)
                throw new Exception("Beneficiário não encontrado");

            // Remove formatação do CPF atual
            string cpfAtualLimpo = CpfValidator.RemoverFormatacao(benefAtual.CPF);

            // Só valida duplicado se o CPF foi alterado
            if (cpfAtualLimpo != cpfLimpo)
            {
                if (dao.VerificarDuplicado(cpfLimpo, beneficiario.IdCliente, beneficiario.Id))
                    throw new Exception("Já existe um beneficiário com este CPF para este cliente");
            }

            // Salva CPF formatado
            beneficiario.CPF = CpfValidator.Formatar(cpfLimpo);

            dao.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta um beneficiário por ID
        /// </summary>
        /// <param name="id">ID do beneficiário</param>
        /// <returns>Objeto Beneficiario</returns>
        public Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            return dao.Consultar(id);
        }

        /// <summary>
        /// Lista todos os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente">ID do cliente</param>
        /// <returns>Lista de beneficiários</returns>
        public List<Beneficiario> ListarPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            return dao.ListarPorCliente(idCliente);
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        /// <param name="id">ID do beneficiário</param>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            dao.Excluir(id);
        }
    }
}