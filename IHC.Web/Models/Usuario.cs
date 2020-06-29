using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHC.Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
        public string Permissao { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdEmpresa { get; set; }

        public Usuario()
        {
        }

        public Usuario(string login, string senha, string perfil, int idColaborador, int idEmpresa)
        {
            Id = 0;
            Login = login;
            Senha = senha;
            Perfil = perfil;
            Permissao = "";
            if (idColaborador == 0) { IdColaborador = null; } else { IdColaborador = idColaborador; }
            //IdColaborador = idColaborador == 0 ? null : idColaborador;
            if (idEmpresa == 0) { IdEmpresa = null; } else { IdEmpresa = idEmpresa; }
            //            IdEmpresa = idEmpresa == 0 ? null : IdEmpresa;

        }


    }
}