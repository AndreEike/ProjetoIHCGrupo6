using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHC.Web.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Responsavel { get; set; }
        public string Email { get; set; }

        //public string CEP { get; set; }
        //public string Logradouro { get; set; }
        //public string Numero { get; set; }
        //public string Complemento { get; set; }
        //public string Bairro { get; set; }
        //public string Cidade { get; set; }
        //public string Estado { get; set; }
        //public string UF { get; set; }

        public Empresa()
        {
        }

        public Empresa(string razaoSocial, string cnpj, string telefone, string responsavel, string email/*, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string uf*/)
        {
            Id = 0;
            RazaoSocial = razaoSocial;
            CNPJ = cnpj;
            Telefone = telefone;
            Responsavel = responsavel;
            Email = email;
            //CEP = cep;
            //Logradouro = logradouro;
            //Numero = numero;
            //Complemento = complemento;
            //Bairro = bairro;
            //Cidade = cidade;
            //Estado = estado;
            //UF = uf;
        }
    }
}