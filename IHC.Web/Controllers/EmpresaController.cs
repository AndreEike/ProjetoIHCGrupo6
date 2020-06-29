using IHC.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;


namespace IHC.Web.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Cadastro(string razaoSocial, string cnpj, string telefone, string responsavel, string email)

        {
            Empresa empresa = new Empresa(razaoSocial, cnpj, telefone, responsavel, email);

            try
            {
                Conexao.Insert(empresa);

                TempData["InsertEmpresa"] = "true";
                return View(empresa);
            }
            catch (Exception e)
            {
                TempData["InsertEmpresa_"] = "true";
                return View(empresa);


            }


        }



        [HttpGet]
        public ActionResult Convocacao()
        {
            string query = @"
SELECT * FROM Colaborador c
INNER JOIN TipoContratacao tc on c.IdTipoContratacao = tc.Id
WHERE tc.Descricao = 'Intermitente'";

            List<Colaborador> colaborador = new List<Colaborador>();
            colaborador = Conexao.ListColaborador(query);

            ViewBag.ListColaborador = colaborador;
            return View();

        }

        [HttpPost]
        public ActionResult Convocacao(int idColaborador, DateTime inicio, DateTime termino, DateTime entrada, DateTime saida, string funcao, string rua, string numero,
            string complemento, string bairro, string cep, string cidade, string estado, int valorHora, DateTime confirmarAte, string titulo, string mensagem)
        {
            string perfil = "";
            int IdEmpresa;
            try
            {
                perfil = Session["Perfil"].ToString();
                IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString());
            }
            catch (Exception e)
            {

                return RedirectToAction("Login", "Login");
            }


            if (perfil == "adm")
            {

                TempData["ConvocacaoAdm"] = "true";
                return View();
            }

            Convocacao convocacao = new Convocacao(idColaborador, IdEmpresa, inicio, termino, entrada.Hour, entrada.Minute, saida.Hour, saida.Minute, funcao, rua, numero, complemento, bairro, cep, cidade,
                estado, valorHora, confirmarAte, titulo, mensagem);

            Conexao.Insert(convocacao);

            string query = @"SELECT * FROM Colaborador WHERE Id = {0}";
            query = string.Format(query, idColaborador);

            Colaborador colaborador = new Colaborador();
            colaborador = Conexao.Colaborador(query);
            try
            {


                titulo = titulo == "" ? "Convacação" : titulo;
                if (mensagem == "")
                {
                    mensagem = "Convocação";

                    EmailController.EnviarEmail(titulo, mensagem, colaborador.Email, null);





                    TempData["Convocacao"] = "true";


                    return View();
                }
                else
                {
                    string mensagemcorpo = @"
<p>{0}</p>
<br/>
<p><strong>Função<strong>: {1}</p>
<p><strong>Período<strong></p>
<p><strong>Início:<strong>: {2}</p>
<p><strong>Término<strong>: {3}</p>
<p><strong>Horário<strong></p>
<p><strong>Entrada<strong>: {4}</p>
<p><strong>Saída<strong>: {5}</p>
<p><strong>Valor/Hora<strong>: {6}</p>
<p><strong>Confirma até<strong>: {7}</p>
<p><strong>Endereço<strong></p>
<p><strong>Rua<strong>: {8}</p>
<p><strong>Número<strong>: {9}</p>
<p><strong>Complemento<strong>: {10}</p>
<p><strong>Bairro<strong>: {11}</p>
<p><strong>CEP<strong>: {12}</p>
<p><strong>Cidade<strong>: {13}</p>
<p><strong>Estado<strong>: {14}</p>";

                    mensagemcorpo = string.Format(mensagemcorpo, mensagem, funcao, inicio, termino, entrada, saida, valorHora, confirmarAte, rua, numero,
                        complemento, bairro, cep, cidade, estado);

                    TempData["Convocacao"] = "true";
                    EmailController.EnviarEmail(titulo, mensagemcorpo, colaborador.Email, null);

                    return View();

                }
            }
            catch (Exception e)
            {
                TempData["Convocacao_"] = "true";
                return View();
            }

        }

        [HttpGet]
        public ActionResult Contrato()
        {
            return null;
        }

        [HttpGet]
        public ActionResult ListaConvocacao(string filtroStatus, string filtroColaborador, string filtroFuncao, string filtroConfirmarAte)
        {
            string IdEmpresa = "";
            try
            {
                IdEmpresa = Session["IdEmpresa"].ToString();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }

            
            string query = "";
            List<Convocacao> convocacao = new List<Convocacao>();


            string filtroStatus_ = "";
            string filtroColaborador_ = "";
            string filtroFuncao_ = "";
            string filtroConfirmarAte_ = "";

            if (filtroStatus == null && filtroColaborador == null && filtroFuncao == null && filtroConfirmarAte == null)
            {

                query = @"SELECT * FROM Convocacao WHERE IdEmpresa = {0} AND Status = 'Aguardando aceite'";
                query = string.Format(query, IdEmpresa);
                convocacao = Conexao.ListConvocacao(query);

            }
            else
            {
                if (filtroStatus != "Todos" && filtroStatus != "")
                {
                    filtroStatus_ = "AND Status = '{0}'";
                    filtroStatus_ = string.Format(filtroStatus_, filtroStatus);
                }
                if (filtroColaborador != "Todos" && filtroColaborador != "")
                {
                    filtroColaborador_ = "AND cl.Id = {0}";
                    filtroColaborador_ = string.Format(filtroColaborador_, filtroColaborador);
                }
                if (filtroFuncao != "Todos" && filtroFuncao != "")
                {
                    filtroFuncao_ = "AND Funcao = '{0}'";
                    filtroFuncao_ = string.Format(filtroFuncao_, filtroFuncao);
                }
                if (filtroConfirmarAte != "Todos" && filtroConfirmarAte != "")
                {
                    filtroConfirmarAte_ = "AND ConfirmarAte <= '{0}'";
                    filtroConfirmarAte_ = string.Format(filtroConfirmarAte_, filtroConfirmarAte);
                }

                query = @"SELECT * FROM Convocacao cv LEFT JOIN Colaborador cl ON cv.IdColaborador = cl.Id WHERE IdEmpresa = {0} {1} {2} {3} {4}";
                query = string.Format(query, IdEmpresa, filtroStatus_, filtroColaborador_, filtroFuncao_, filtroConfirmarAte_);

                convocacao = Conexao.ListConvocacao(query);

            }

            query = @"SELECT * FROM Colaborador";
            List<Colaborador> colaborador = new List<Colaborador>();
            colaborador = Conexao.ListColaborador(query);
            ViewBag.Colaborador = colaborador;

            query = @"SELECT DISTINCT(Funcao) FROM Convocacao";
            List<Convocacao> funcao = new List<Convocacao>();
            funcao = Conexao.ComboFuncao(query);
            ViewBag.Funcao = funcao;







            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialListaConvocacao", convocacao);
            }

            return View(convocacao);


        }

        [HttpPost]
        public ActionResult ConvocarEmLote(HttpPostedFileBase file)
        {
            string perfil = "";
            int IdEmpresa;
            string result;
            try
            {
                perfil = Session["Perfil"].ToString();
                IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString());
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }


            if (perfil == "adm")
            {

                TempData["ConvocacaoAdm"] = "true";
                return RedirectToAction("Convocacao", "Empresa");
            }
            try
            {
                result = new StreamReader(file.InputStream).ReadToEnd();
            }
            catch (Exception e)
            {
                return RedirectToAction("Convocacao", "Empresa");
            }
            

            string[] resultArray = result.Split('\r', '\n');

            int y = 0;
            foreach (var x in resultArray)
            {

                if ((x != "") && (y != 0))
                {
                    string[] conteudo = x.Split(';');

                    int idColaborador = Int32.Parse(conteudo[0].ToString());
                    DateTime inicio = DateTime.Parse(conteudo[1].ToString());
                    DateTime termino = DateTime.Parse(conteudo[2].ToString());
                    int entradaHora = Int32.Parse(conteudo[3].ToString());
                    int entradaMinuto = Int32.Parse(conteudo[4].ToString());
                    int saidaHora = Int32.Parse(conteudo[5].ToString());
                    int saidaMinuto = Int32.Parse(conteudo[6].ToString());
                    string funcao = conteudo[7].ToString();
                    string rua = conteudo[8].ToString();
                    string numero = conteudo[9].ToString();
                    string complemento = conteudo[10].ToString();
                    string bairro = conteudo[11].ToString();
                    string cep = conteudo[12].ToString();
                    string cidade = conteudo[13].ToString();
                    string estado = conteudo[14].ToString();
                    double valorHora = Double.Parse(conteudo[15].ToString());
                    DateTime confirmarAte = DateTime.Parse(conteudo[16].ToString());

                    Convocacao convocacao = new Convocacao(idColaborador, IdEmpresa, inicio, termino, entradaHora,
                        entradaMinuto, saidaHora, saidaMinuto, funcao, rua, numero, complemento, bairro, cep, cidade, estado, valorHora, confirmarAte,
                        "Convocação em lote", "Convocação em lote");

                    Conexao.Insert(convocacao);

                    string query = @"SELECT * FROM Colaborador WHERE Id = {0}";
                    query = string.Format(query, idColaborador);

                    Colaborador colaborador = new Colaborador();
                    colaborador = Conexao.Colaborador(query);
                    try
                    {

                        string mensagemcorpo = @"
<p>{0}</p>
<br/>
<p><strong>Função<strong>: {1}</p>
<p><strong>Período<strong></p>
<p><strong>Início<strong>: {2}</p>
<p><strong>Término<strong>: {3}</p>
<p><strong>Horário<strong></p>
<p><strong>Entrada<strong>: {4}:{5}</p>
<p><strong>Saída<strong>: {6}:{7}</p>
<p><strong>Valor/Hora<strong>: R$ {8}</p>
<p><strong>Confirma até<strong>: {9}</p>
<p><strong>Endereço<strong></p>
<p><strong>Rua<strong>: {10}</p>
<p><strong>Número<strong>: {11}</p>
<p><strong>Complemento<strong>: {12}</p>
<p><strong>Bairro<strong>: {13}</p>
<p><strong>CEP<strong>: {14}</p>
<p><strong>Cidade<strong>: {15}</p>
<p><strong>Estado<strong>: {16}</p>";

                        string entradaHora_ = "";
                        string entradaMinuto_ = "";
                        string saidaHora_ = "";
                        string saidaMinuto_ = "";

                        if (entradaHora < 9)
                        {
                            entradaHora_ = entradaHora.ToString() + "0";
                        }
                        if (entradaMinuto < 9)
                        {
                            entradaMinuto_ = entradaMinuto.ToString() + "0";
                        }
                        if (saidaHora < 9)
                        {
                            saidaHora_ = saidaHora.ToString() + "0";
                        }
                        if (saidaMinuto < 9)
                        {
                            saidaMinuto_ = saidaMinuto.ToString() + "0";
                        }


                        mensagemcorpo = string.Format(mensagemcorpo, colaborador.Nome + " você foi convocado. Segue informações:", funcao, inicio, termino, 
                            entradaHora_, entradaMinuto_, saidaHora_, saidaMinuto_, valorHora, confirmarAte.Date, rua, numero, complemento, bairro, cep, cidade, 
                            estado);

                        TempData["Convocacao"] = "true";
                        EmailController.EnviarEmail("Convocação", mensagemcorpo, colaborador.Email, null);



                    }
                    catch (Exception e)
                    {
                        TempData["Convocacao_"] = "true";
                    }
                }
                y++;
            }
            return RedirectToAction("Convocacao", "Empresa");
        }

        public ActionResult ListaColaborador()
        {
            string query = @"SELECT * FROM Colaborador";
            List<Colaborador> colaborador = new List<Colaborador>();
            colaborador = Conexao.ListColaborador(query);

            query = "SELECT * FROM TipoContratacao";
            List<TipoContratacao> tipoContratacao = new List<TipoContratacao>();
            tipoContratacao = Conexao.ListTipoContratacao(query);
            ViewBag.tipoContratacao = tipoContratacao;

            return View(colaborador);
        }
    }
}