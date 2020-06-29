using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHC.Web.Models
{
    public class Colaborador
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int IdTipoContratacao { get; set; }

        public Colaborador()
        {
        }

        public Colaborador(string nome, string cpf, string telefone, string email, int idtipocontratacao)
        {
            Id = 0;
            Nome = nome;
            CPF = cpf;
            Telefone = telefone;
            Email = email;
            IdTipoContratacao = idtipocontratacao;
        }
    }
}