using IHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHC.Web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Cadastro()
        {
            string query = @"SELECT * FROM Empresa";
            List<Empresa> empresa = new List<Empresa>();
            empresa = Conexao.ListEmpresa(query);
            ViewBag.Empresa = empresa;

            string query_ = @"SELECT * FROM Colaborador";
            List<Colaborador> colaborador = new List<Colaborador>();
            colaborador = Conexao.ListColaborador(query_);
            ViewBag.Colaborador = colaborador;

            return View();


            
        }
        [HttpPost]
        public ActionResult Cadastro(string login, string senha, string perfil, int idColaborador, int idEmpresa)
        {
            bool verificaLogin = Conexao.VerificaLogin(login);
            bool verificaLoginColaborador = Conexao.VerificaLoginColaborador(idColaborador);
            if (verificaLogin)
            {
                TempData["VerificaLogin"] = "true";
                return View();
            }
            else
            {
                if (verificaLoginColaborador)
                {
                    TempData["VerificaLoginColaborador"] = "true";
                    return View();
                }
            }

            Usuario usuario = new Usuario(login, senha, perfil, idColaborador, idEmpresa);
            try
            {
                Conexao.Insert(usuario);

                TempData["CadastroSucesso"] = "true";
                return View();
            }
            catch (Exception e)
            {
                TempData["CadastroErro"] = "true";
                return View();
            }
            


        }
    }
}