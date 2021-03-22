using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class Coleta
    {
        public Coleta()
        {

        }

        public Coleta(string endereco, int saldoAferido, int userId, List<string> etiquetas)
        {
            Endereco = endereco;
            SaldoAferido = saldoAferido;
            UserId = userId;
            Etiquetas = etiquetas;
        }

        public string Endereco { get; set; }
        public int SaldoAferido { get; set; }
        public int UserId { get; set; }

        public List<string> Etiquetas { get; set; }

        public List<Coleta> LoadData()
        {
            return new List<Coleta>()
            {
                new Coleta()
                {
                    Endereco = "RUA 1",
                    SaldoAferido = 10,
                    UserId = 27,
                    Etiquetas = new List<string>()
                    {
                        "76775201#1#1000#1#OADM066440D ",
                        "76775201#1#1000#2#OADM066440D ",
                        "76775201#1#1000#3#OADM066440D ",
                        "76775201#1#1000#4#OADM066440D ",
                        "76775201#1#1000#5#OADM066440D ",
                        "76775201#1#1000#6#OADM066440D ",
                        "76775201#1#1000#7#OADM066440D ",
                        "76775201#1#1000#8#OADM066440D ",
                        "76775201#1#1000#9#OADM066440D ",
                        "76775201#1#1000#10#OADM066440D",
                        "76775201#1#1000#11#OADM066440D",
                        "76775201#1#1000#12#OADM066440D"
                    }
                },
                new Coleta()
                {
                    Endereco = "RUA 2",
                    SaldoAferido = 10,
                    UserId = 27,
                    Etiquetas = new List<string>()
                    {
                        "76834101#1#1000#1#OASM066560C",
                        "76834101#1#1000#2#OASM066560C",
                        "76834101#1#1000#3#OASM066560C",
                        "76834101#1#1000#4#OASM066560C",
                        "76834101#1#1000#5#OASM066560C",
                        "76834101#1#1000#6#OASM066560C",
                        "76834101#1#1000#7#OASM066560C",
                        "76834101#1#1000#8#OASM066560C",
                        "76834101#1#1000#9#OASM066560C"
                    }

                },
                new Coleta()
                {
                    Endereco = "RUA 3",
                    SaldoAferido = 10,
                    UserId = 27,
                    Etiquetas = new List<string>()
                    {
                        "76702701/062521T#1#1000#2#OATM062521T",
                        "76702701/062521T#1#1000#3#OATM062521T",
                        "76702701/062521T#1#1000#4#OATM062521T",
                        "76702701/062521T#1#1000#5#OATM062521T",
                        "76702701/062521T#1#1000#6#OATM062521T"
                    }
                }
            };
        }
    }
}

