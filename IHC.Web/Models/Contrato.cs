using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHC.Web.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public int IdConvocacao { get; set; }
        public DateTime DataAceite { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataFim { get; set; }

        public Contrato()
        {
        }

        public Contrato(int idConvocacao, DateTime dataAceite, bool ativo, DateTime? dataFim)
        {
            Id = 0;
            IdConvocacao = idConvocacao;
            DataAceite = dataAceite;
            Ativo = ativo;
            DataFim = dataFim;
        }
    }
}