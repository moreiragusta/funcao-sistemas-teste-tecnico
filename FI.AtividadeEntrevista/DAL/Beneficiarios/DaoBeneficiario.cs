using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiário
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        /// <returns>ID do beneficiário inserido</returns>
        internal long Incluir(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("NOME", beneficiario.Nome),
                new SqlParameter("IDCLIENTE", beneficiario.IdCliente)
            };

            DataSet ds = base.Consultar("FI_SP_IncBenef", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Altera um beneficiário existente
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        internal void Alterar(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("ID", beneficiario.Id),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("NOME", beneficiario.Nome),
                new SqlParameter("IDCLIENTE", beneficiario.IdCliente)
            };

            base.Executar("FI_SP_AltBenef", parametros);
        }

        /// <summary>
        /// Consulta um beneficiário por ID
        /// </summary>
        /// <param name="id">ID do beneficiário</param>
        /// <returns>Objeto Beneficiario ou null</returns>
        internal Beneficiario Consultar(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("ID", id)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<Beneficiario> lista = Converter(ds);

            return lista.FirstOrDefault();
        }

        /// <summary>
        /// Lista todos os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente">ID do cliente</param>
        /// <returns>Lista de beneficiários</returns>
        internal List<Beneficiario> ListarPorCliente(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("IDCLIENTE", idCliente)
            };

            DataSet ds = base.Consultar("FI_SP_ListBenefPorCliente", parametros);
            return Converter(ds);
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        /// <param name="id">ID do beneficiário</param>
        internal void Excluir(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("ID", id)
            };

            base.Executar("FI_SP_DelBenef", parametros);
        }

        /// <summary>
        /// Verifica se já existe um beneficiário com o mesmo CPF para o cliente
        /// </summary>
        /// <param name="cpf">CPF do beneficiário</param>
        /// <param name="idCliente">ID do cliente</param>
        /// <param name="id">ID do beneficiário (para ignorar na edição)</param>
        /// <returns>True se CPF duplicado, False caso contrário</returns>
        internal bool VerificarDuplicado(string cpf, long idCliente, long id = 0)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", cpf),
                new SqlParameter("IDCLIENTE", idCliente),
                new SqlParameter("ID", id)
            };

            DataSet ds = base.Consultar("FI_SP_VerificaBenefDuplicado", parametros);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                return count > 0;
            }

            return false;
        }

        /// <summary>
        /// Converte DataSet em lista de Beneficiarios
        /// </summary>
        /// <param name="ds">DataSet retornado do banco</param>
        /// <returns>Lista de beneficiários</returns>
        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario benef = new Beneficiario
                    {
                        Id = row.Field<long>("ID"),
                        CPF = row.Field<string>("CPF"),
                        Nome = row.Field<string>("NOME"),
                        IdCliente = row.Field<long>("IDCLIENTE")
                    };
                    lista.Add(benef);
                }
            }

            return lista;
        }
    }
}