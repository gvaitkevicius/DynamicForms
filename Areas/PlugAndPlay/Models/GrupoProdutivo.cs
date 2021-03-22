﻿using System;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class GrupoProdutivo
    {

        public GrupoProdutivo(DateTime de, DateTime ate, int index)
        {
            this.Inicio = de;
            this.Fim = ate;
            this.Index = index;
        }
        public GrupoProdutivo(DateTime de, DateTime ate)
        {
            this.Inicio = de;
            this.Fim = ate;
        }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public int Index { get; set; }
        public int IndexOnduladeira { get; set; }

    }
}
