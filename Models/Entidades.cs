using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Entidades
    {
        public string NomeEntidade { get; set; }
        public List<Coluna> ListaColuna { get; set; }
        public List<Entidades> ListaRelacionamento { get; set; }

        public Entidades() { }

        public Entidades(string nomeEntidade, List<Coluna> listaColuna, List<Entidades> listaRelacionamento)
        {
            NomeEntidade = nomeEntidade;
            ListaColuna = listaColuna;
            ListaRelacionamento = listaRelacionamento;
        }
    }
}
