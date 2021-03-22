namespace DynamicForms.Areas.SGI.Components
{
    public static class UtilsSGI
    {
        /// <summary>
        /// Função para retornar se uma meta foi atingida ou não.
        /// </summary>
        /// <param name="valorMeta">Valor da meta</param>
        /// <param name="valorAtingido">Valor medido</param>
        /// <returns>Retorna true or false</returns>
        public static bool AtingiuMeta(decimal? valorMeta, decimal? valorAtingido, string tipoComparador)
        {
            bool atingiu = false;
            switch (tipoComparador)
            {
                case "0"://Igual
                    if (valorAtingido == valorMeta)
                        atingiu = true;
                    break;


                case "1"://Maior ou igual
                    if (valorAtingido >= valorMeta)
                        atingiu = true;
                    break;

                case "2"://Menor ou igual
                    if (valorAtingido <= valorMeta)
                        atingiu = true;
                    break;
            }

            return atingiu;
        }

        /// <summary>
        /// Metódo que retorna o formato utilizado nas medições
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string GetFormatoValor(int tipo) //Transformar isso em uma componente?
        {
            string formato = "{0:N2}";
            switch (tipo)
            {
                case (int)DynamicForms.Areas.PlugAndPlay.Enums.TipoAlvo.Valor:
                    formato = "{0:N2}";
                    break;
                case (int)DynamicForms.Areas.PlugAndPlay.Enums.TipoAlvo.Percentual:
                    formato = "{0:P0}";
                    break;
                case (int)DynamicForms.Areas.PlugAndPlay.Enums.TipoAlvo.Data:
                    formato = "{0:dd/MM/yyyy}";
                    break;
                case (int)DynamicForms.Areas.PlugAndPlay.Enums.TipoAlvo.DiaUtil:
                    formato = "{0:00}";
                    break;
            }
            return formato;
        }
    }
}
