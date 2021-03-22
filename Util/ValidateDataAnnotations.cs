using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DynamicForms.Util
{
    public static class ValidateDataAnnotations
    {
        private static IEnumerable<ValidationResult> GetValidationErros(object obj)
        {
            var resultadoValidacao = new List<ValidationResult>();
            var contexto = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, contexto, resultadoValidacao, true);
            return resultadoValidacao;
        }

        /// <summary>
        /// Responsavel pela Chamada da validação do Modelo
        /// </summary>
        /// <param name="obj">Modelo (Classe)</param>
        /// <returns>(true) se a validação foi feita com sucesso (""), 
        /// se encontrou erros Apresenta mensagem com o erros </returns>
        public static string ValidateModel(object obj)
        {
            var errors = GetValidationErros(obj);
            StringBuilder sb_erros = new StringBuilder();
            int i = 0;
            for (; i < errors.Count(); i++)
            {
                var error = errors.ElementAt(i);
                if (i == 0)
                    sb_erros.Append(string.Format("{0}:{1}", error.MemberNames.ElementAt(0), error.ErrorMessage));
                else
                    sb_erros.Append(string.Format(";{0}:{1}", error.MemberNames.ElementAt(0), error.ErrorMessage));
            }
            return sb_erros.ToString();
        }
    }
}
