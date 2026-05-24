using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                model.Id = bo.Incluir(new Cliente()
                {
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Nacionalidade = model.Nacionalidade,
                    CEP = model.CEP,
                    Estado = model.Estado,
                    Cidade = model.Cidade,
                    Logradouro = model.Logradouro,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                return Json("Cadastro efetuado com sucesso");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Nacionalidade = model.Nacionalidade,
                    CEP = model.CEP,
                    Estado = model.Estado,
                    Cidade = model.Cidade,
                    Logradouro = model.Logradouro,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                return Json("Cliente alterado com sucesso");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            Models.ClienteModel model = new Models.ClienteModel()
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                Nacionalidade = cliente.Nacionalidade,
                CEP = cliente.CEP,
                Estado = cliente.Estado,
                Cidade = cliente.Cidade,
                Logradouro = cliente.Logradouro,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                CPF = cliente.CPF  // TEM ESSA LINHA?
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region Beneficiários

        /// <summary>
        /// Lista beneficiários de um cliente
        /// </summary>
        [HttpGet]
        public JsonResult ListarBeneficiarios(long idCliente)
        {
            try
            {
                BoBeneficiario bo = new BoBeneficiario();
                var beneficiarios = bo.ListarPorCliente(idCliente);

                var lista = beneficiarios.Select(b => new
                {
                    Id = b.Id,
                    CPF = b.CPF,
                    Nome = b.Nome,
                    IdCliente = b.IdCliente
                }).ToList();

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        [HttpPost]
        public JsonResult IncluirBeneficiario(BeneficiarioModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var erros = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(", ", erros));
                }

                BoBeneficiario bo = new BoBeneficiario();
                long id = bo.Incluir(new Beneficiario
                {
                    CPF = model.CPF,
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });

                return Json(new { sucesso = true, id = id, mensagem = "Beneficiário incluído com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Altera um beneficiário existente
        /// </summary>
        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var erros = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(", ", erros));
                }

                BoBeneficiario bo = new BoBeneficiario();
                bo.Alterar(new Beneficiario
                {
                    Id = model.Id,
                    CPF = model.CPF,
                    Nome = model.Nome,
                    IdCliente = model.IdCliente
                });

                return Json(new { sucesso = true, mensagem = "Beneficiário alterado com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        [HttpPost]
        public JsonResult ExcluirBeneficiario(long id)
        {
            try
            {
                BoBeneficiario bo = new BoBeneficiario();
                bo.Excluir(id);

                return Json(new { sucesso = true, mensagem = "Beneficiário excluído com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        #endregion
    }
}