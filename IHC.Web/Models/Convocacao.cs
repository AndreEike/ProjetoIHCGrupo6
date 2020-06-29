using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHC.Web.Models
{
    public class Convocacao
    {
        public int Id { get; set; }
        public int IdColaborador{ get; set; }
        public int IdEmpresa { get; set; }
       
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int EntradaHora { get; set; }
        public int EntradaMinuto { get; set; }
        public int SaidaHora { get; set; }
        public int SaidaMinuto { get; set; }
        public string Funcao { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public double ValorHora { get; set; }
        public DateTime ConfirmarAte { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Status { get; set; }
        

        public Convocacao()
        {
        }

        public Convocacao(int idColaborador, int idEmpresa, DateTime inicio, DateTime termino, int entradaHora, int entradaMinuto, int saidaHora, int saidaMinuto, string funcao, string rua, 
            string numero, string complemento, string bairro, string cep, string cidade, string estado, double valorHora, DateTime confirmarAte, 
            string titulo, string mensagem)
        {
            Id = 0;
            IdColaborador = idColaborador;
            IdEmpresa = idEmpresa;
           
            Inicio = inicio;
            Termino = termino;
            EntradaHora = entradaHora;
            EntradaMinuto = entradaMinuto;
            SaidaHora = saidaHora;
            SaidaMinuto = saidaMinuto;
            Funcao = funcao;
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cep;
            Cidade = cidade;
            Estado = estado;
            ValorHora = valorHora;
            ConfirmarAte = confirmarAte;
            Titulo = titulo;
            Mensagem = mensagem;
            Status = "Aguardando aceite";
        }
    }
}