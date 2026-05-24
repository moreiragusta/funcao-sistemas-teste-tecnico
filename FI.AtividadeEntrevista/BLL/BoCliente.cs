using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FI.AtividadeEntrevista.Utils;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            // Validação: CPF obrigatório
            if (string.IsNullOrWhiteSpace(cliente.CPF))
                throw new Exception("CPF é obrigatório");
            // Remove formatação do CPF para validação
            string cpfLimpo = CpfValidator.RemoverFormatacao(cliente.CPF);
            // Validação: CPF válido
            if (!CpfValidator.IsValid(cpfLimpo))
                throw new Exception("CPF inválido");
            // Validação: CPF duplicado
            DAL.DaoCliente dao = new DAL.DaoCliente();
            if (dao.VerificarExistencia(cpfLimpo))
                throw new Exception("CPF já cadastrado");
            // Salva CPF formatado no banco
            cliente.CPF = CpfValidator.Formatar(cpfLimpo);

            return dao.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            // Validação: CPF obrigatório
            if (string.IsNullOrWhiteSpace(cliente.CPF))
                throw new Exception("CPF é obrigatório");
            // Remove formatação do CPF para validação
            string cpfLimpo = CpfValidator.RemoverFormatacao(cliente.CPF);
            // Validação: CPF válido
            if (!CpfValidator.IsValid(cpfLimpo))
                throw new Exception("CPF inválido");
            DAL.DaoCliente dao = new DAL.DaoCliente();
            // Consulta cliente atual no banco
            var clienteAtual = dao.Consultar(cliente.Id);
            // Remove formatação do CPF atual para comparação
            string cpfAtualLimpo = CpfValidator.RemoverFormatacao(clienteAtual.CPF);
            // Só valida CPF duplicado se o CPF foi alterado
            if (cpfAtualLimpo != cpfLimpo)
            {
                if (dao.VerificarExistencia(cpfLimpo))
                    throw new Exception("CPF já cadastrado para outro cliente");
            }
            // Salva CPF formatado no banco
            cliente.CPF = CpfValidator.Formatar(cpfLimpo);

            dao.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }
    }
}
