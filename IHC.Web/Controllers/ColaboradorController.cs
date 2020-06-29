using IHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IHC.Web.Controllers
{
    [Authorize]
    public class ColaboradorController : Controller
    {
        [HttpGet]
        public ActionResult Cadastro()
        {
            string query = @"SELECT * FROM TipoContratacao";
            List<TipoContratacao> tipoContratacao = new List<TipoContratacao>();
            tipoContratacao = Conexao.ListTipoContratacao(query);
            ViewBag.TipoContratacao = tipoContratacao;

            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(string nome, string cpf, string telefone, string email, int idtipocontratacao)
        {

            Colaborador colaborador = new Colaborador(nome, cpf, telefone, email, idtipocontratacao);

            try
            {
                Conexao.Insert(colaborador);
                TempData["CadastroColaborador"] = "true";
            }
            catch (Exception e)
            {
                TempData["CadastroColaborador_"] = "true";
                return View();
            }


            return View();

        }

        [HttpGet]
        public ActionResult Convocacao()
        {
            string query = "";
            string perfil = "";
            string IdColaborador = "";
            try
            {
                perfil = Session["Perfil"].ToString();
                IdColaborador = Session["IdColaborador"].ToString();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }

            if (perfil == "adm")
            {
                query = @"
SELECT *
FROM Convocacao c 
WHERE Status = 'Aguardando aceite'";
            }
            else
            {
                query = @"
SELECT *
FROM Convocacao c 
WHERE Status = 'Aguardando aceite'
AND c.IdColaborador = {0}";
                query = string.Format(query, IdColaborador);

            }


            List<Convocacao> convocacao = new List<Convocacao>();
            convocacao = Conexao.ListConvocacao(query);

            query = @"SELECT * FROM Empresa";
            List<Empresa> empresa = new List<Empresa>();
            empresa = Conexao.ListEmpresa(query);
            ViewBag.Empresa = empresa;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialConvocacao", convocacao);
            }

            return View(convocacao);
        }

        [HttpPost]
        public ActionResult AceitarConvocacao(int idConvocacao)
        {
            string perfil = "";
            string IdColaborador = "";
            try
            {
                perfil = Session["Perfil"].ToString();
                IdColaborador = Session["IdColaborador"].ToString();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }

            if (perfil == "adm")
            {
                TempData["AceitarRecusarConvocacaoAdm"] = "true";
                return RedirectToAction("Convocacao", "Colaborador");
            }


            string mensagem = "<p>Colaborador {0} aceitou a convocação para a função de {1}.</p>";
            string query = @"select *
            from contrato ct inner join convocacao cv on ct.IdConvocacao = cv.Id where ct.ativo = 1 and cv.IdColaborador = {0}";

            string update = @"UPDATE Convocacao SET Status = 'Aceita' WHERE Id = {0}";
            update = string.Format(update, idConvocacao);

            query = string.Format(query, IdColaborador);
            List<Convocacao> Listconvocacao = new List<Convocacao>();
            Listconvocacao = Conexao.ListConvocacao(query);

            string queryEmpresa = string.Format("SELECT * FROM Empresa e INNER JOIN Convocacao cv ON e.Id = cv.IdEmpresa WHERE cv.Id = {0}",
                idConvocacao);
            Empresa empresa = new Empresa();
            empresa = Conexao.Empresa(queryEmpresa);


            string queryColaborador = string.Format("SELECT * FROM Colaborador cl INNER JOIN Convocacao cv ON cl.Id = cv.IdColaborador WHERE cv.Id = {0}",
                idConvocacao);
            Colaborador colaborador = new Colaborador();
            colaborador = Conexao.Colaborador(queryColaborador);


            string query_ = @"select * from convocacao where  id = {0}";
            query_ = string.Format(query_, idConvocacao);
            Convocacao convocacao_ = new Convocacao();
            convocacao_ = Conexao.Convocacao(query_);

            mensagem = string.Format(mensagem, colaborador.Nome, convocacao_.Funcao);

            if (Listconvocacao.Count == 0)
            {

                Contrato contrato = new Contrato(idConvocacao, DateTime.Now, true, null);
                try
                {
                    Conexao.Update(update);
                    Conexao.Insert(contrato);

                    EmailController.EnviarEmail("Convocação aceita", mensagem, empresa.Email, null);
                    TempData["SucessoConvocacao"] = "true";
                    return RedirectToAction("Convocacao", "Colaborador");
                }
                catch (Exception e)
                {

                    TempData["ErroConvocacao"] = "true";
                }
            }




            foreach (Convocacao c in Listconvocacao)
            {
                if (c.Termino < convocacao_.Inicio)
                {
                    Contrato contrato = new Contrato(idConvocacao, DateTime.Now, true, null);
                    try
                    {
                        Conexao.Update(update);
                        Conexao.Insert(contrato);
                        EmailController.EnviarEmail("Convocação aceita", mensagem, empresa.Email, null);
                        TempData["SucessoConvocacao"] = "true";
                        return RedirectToAction("Convocacao", "Colaborador");
                    }
                    catch (Exception e)
                    {

                        TempData["ErroConvocacao"] = "true";
                    }
                }
                else
                {
                    if ((c.SaidaHora + 1) < convocacao_.EntradaHora)
                    {
                        Contrato contrato = new Contrato(idConvocacao, DateTime.Now, true, null);
                        try
                        {
                            Conexao.Update(update);
                            Conexao.Insert(contrato);
                            EmailController.EnviarEmail("Convocação aceita", mensagem, empresa.Email, null);
                            TempData["SucessoConvocacao"] = "true";
                            return RedirectToAction("Convocacao", "Colaborador");
                        }
                        catch (Exception e)
                        {

                            TempData["ErroConvocacao"] = "true";
                        }

                    }
                    else
                    {
                        if (c.EntradaHora > convocacao_.SaidaHora + 1)
                        {
                            Contrato contrato = new Contrato(idConvocacao, DateTime.Now, true, null);
                            try
                            {
                                Conexao.Update(update);
                                Conexao.Insert(contrato);
                                EmailController.EnviarEmail("Convocação aceita", mensagem, empresa.Email, null);
                                TempData["SucessoConvocacao"] = "true";
                                return RedirectToAction("Convocacao", "Colaborador");
                            }
                            catch (Exception e)
                            {

                                TempData["ErroConvocacao"] = "true";
                            }


                        }

                        TempData["ErroConvocacao"] = "true";
                        return RedirectToAction("Convocacao", "Colaborador");
                    }
                }
            }


            return View();
        }



        public ActionResult PartialVerificaEndereco(int Id)
        {
            string query = @"SELECT * FROM Convocacao WHERE Id = {0}";
            query = string.Format(query, Id);

            Convocacao convocacao = new Convocacao();
            convocacao = Conexao.Convocacao(query);


            return PartialView("_PartialVerificaEndereco", convocacao);
        }


        public ActionResult RecusarConvocacao(int idConvocacao)
        {
            string perfil = "";
            try
            {
                perfil = Session["Perfil"].ToString();

            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }



            if (perfil == "adm")
            {
                TempData["AceitarRecusarConvocacaoAdm"] = "true";
                return RedirectToAction("Convocacao", "Colaborador");
            }

            string mensagem = "<p>Colaborador {0} recusou a convocação para a função de {1}.</p>";


            string queryEmpresa = string.Format("SELECT * FROM Empresa e INNER JOIN Convocacao cv ON e.Id = cv.IdEmpresa WHERE cv.Id = {0}",
                idConvocacao);
            Empresa empresa = new Empresa();
            empresa = Conexao.Empresa(queryEmpresa);


            string queryColaborador = string.Format("SELECT * FROM Colaborador cl INNER JOIN Convocacao cv ON cl.Id = cv.IdColaborador WHERE cv.Id = {0}",
                idConvocacao);
            Colaborador colaborador = new Colaborador();
            colaborador = Conexao.Colaborador(queryColaborador);


            string query_ = @"select * from convocacao where  id = {0}";
            query_ = string.Format(query_, idConvocacao);
            Convocacao convocacao_ = new Convocacao();
            convocacao_ = Conexao.Convocacao(query_);


            string query = @"UPDATE Convocacao SET Status = 'Recusada' WHERE Id = {0}";
            query = string.Format(query, idConvocacao);

            mensagem = string.Format(mensagem, colaborador.Nome, convocacao_.Funcao);

            try
            {
                Conexao.Update(query);
                EmailController.EnviarEmail("Convocação recusada", mensagem, empresa.Email, null);
                TempData["SucessoRecusarConvocacao"] = "true";
                return RedirectToAction("Convocacao", "Colaborador");
            }
            catch (Exception)
            {

                TempData["ErroRecusarConvocacao"] = "true";
                return RedirectToAction("Convocacao", "Colaborador");
            }






        }

        [HttpGet]
        public ActionResult Contrato()
        {
            string query = "";
            string perfil = "";
            string IdColaborador = "";
            try
            {
                perfil = Session["Perfil"].ToString();
                IdColaborador = Session["IdColaborador"].ToString();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }


            if (perfil == "adm")
            {
                query = @"SELECT ct.* FROM Contrato ct";
            }
            else
            {
                query = @"SELECT ct.* FROM Contrato ct INNER JOIN Convocacao cv ON ct.IdConvocacao = cv.Id WHERE cv.IdColaborador = {0}";
                query = string.Format(query, IdColaborador);
            }

            List<Contrato> contrato = new List<Contrato>();
            contrato = Conexao.ListContrato(query);

            return View(contrato);
        }
    }
}