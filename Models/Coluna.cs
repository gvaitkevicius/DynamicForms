using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Coluna
    {
        public Coluna(string nome, string tipo)
        {
            Nome = nome;
            Tipo = tipo;
        }

        public string Nome { get; set; }
        public string Tipo { get; set; }

    }
}
