using System;
using System.Linq;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Controllers
{
    public class BaseController : Controller
    {
        private T_Usuario ObterUsuarioLogadoDoCookie()
        {
            var identity = HttpContext.User.Identities
                .Where(x => x.AuthenticationType == "Usuario").FirstOrDefault();

            string strUserId = identity.Claims.Where(x => x.Type == "UserId")
                .Select(x => x.Value).FirstOrDefault();

            int UserId = Convert.ToInt32(strUserId);

            string userName = identity.Claims.Where(x => x.Type == "UserName")
                .Select(x => x.Value).FirstOrDefault();

            string userTurmaId = identity.Claims.Where(x => x.Type == "UserTurma")
                .Select(x => x.Value).FirstOrDefault();

            T_Usuario usuario = new T_Usuario
            {
                USE_ID = UserId,
                USE_NOME = userName,
                TURM_ID = userTurmaId
            };

            return usuario;
        }

        public T_Usuario ObterUsuarioLogado()
        {
            T_Usuario usuarioLogado = ObterUsuarioLogadoDoCookie();

            T_Usuario usuarioSingleton = UsuarioSingleton.Instance.ObterUsuario(usuarioLogado.USE_ID);
            if (usuarioSingleton != null)
                return usuarioSingleton;

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var user = db.T_Usuario.AsNoTracking()
                    //.Include(u => u.T_PREFERENCIAS)
                    .Include(u => u.T_USUARIO_OBJETO_CONTROLAVEL)
                    .Include(u => u.T_Usuario_Perfil)
                        .ThenInclude(up => up.Perfil)
                            .ThenInclude(p => p.T_PERFIL_OBJETO_CONTROLAVEL)
                    .Where(u => u.USE_ID == usuarioLogado.USE_ID)
                    .FirstOrDefault();

                var T_PREFERENCIAS = db.T_PREFERENCIAS.AsNoTracking()
                    .Where(z =>
                        (z.USE_ID.Equals(null) &&
                        z.PER_ID.Equals(null)) ||
                        z.USE_ID.Equals(user.USE_ID) ||
                        z.PER_ID.Equals(user.T_Usuario_Perfil)
                    )
                    .OrderByDescending(y => y.USE_ID)
                    .OrderByDescending(w => w.PER_ID)
                    .ToList();

                foreach (var item in T_PREFERENCIAS)
                {
                    user.T_PREFERENCIAS.Add(item);
                }
                UsuarioSingleton.Instance.InserirUsuario(user);

                return usuarioLogado;
            }
        }

        public static string ConversorTempo(double segundos, string unidade_tempo)
        {
            /*
                MONTH - MES
                WEEK - SEMANA
                DAY - DIA
                HOUR - HORA
                MINUTE - MINUTO
                SECOND - SEGUNDO
                MILLISECOND - MILISEGUNDO
             */

            double res_numerico;
            string res_string = "error";

            //Da para melhorar fazendo recursivo
            switch (unidade_tempo.ToUpper())
            {
                case "MES":
                    break;
                case "SEMANA":
                    break;
                case "DIA":
                    break;
                case "HORA":
                    res_numerico = Math.Truncate(segundos / 3600);
                    double resto = segundos % 3600;
                    res_string = res_numerico + "h";

                    res_numerico = Math.Truncate(resto / 60);
                    resto = resto % 60;
                    res_string += res_numerico + "m" + resto + "s";

                    break;
                case "MINUTO":
                    res_numerico = Math.Truncate(segundos / 60);
                    double resto_sec = segundos % 60;
                    res_string = res_numerico + "m" + resto_sec + "s";

                    break;
                case "SEGUNDO":
                    res_string = segundos + "s";
                    break;
                case "MILISEGUNDO":
                    res_numerico = segundos * 1000;
                    res_string = res_numerico + "ms";

                    break;

                default:
                    return ConversorTempo(segundos, "SEGUNDO");
                    break;
            }

            return res_string;
        }

        public static string ConversorUnidades(double segundos, string unidade)
        {
            /*
                MONTH - MES
                WEEK - SEMANA
                DAY - DIA
                HOUR - HORA
                MINUTE - MINUTO
                SECOND - SEGUNDO
                MILLISECOND - MILISEGUNDO
             */

            double res_numerico;
            string res_string = "error";

            //Da para melhorar fazendo recursivo
            switch (unidade.ToUpper())
            {
                case "MES":
                    break;
                case "SEMANA":
                    break;
                case "DIA":
                    break;
                case "HORA":
                    res_numerico = Math.Round(segundos * 60 * 24, 2);
                    res_string = res_numerico + " pulso/hora";
                    break;

                case "MINUTO":
                    res_numerico = Math.Round(segundos * 60, 2);
                    //res_numerico = segundos * 60;
                    res_string = res_numerico + " pulso/minuto";

                    break;
                case "SEGUNDO":
                    res_string = segundos + " pulso/segundo";
                    break;
                case "MILISEGUNDO":
                    res_numerico = Math.Round(segundos / 1000, 2);
                    res_string = res_numerico + " pulso/ms";

                    break;

                default:
                    return ConversorUnidades(segundos, "SEGUNDO");
                    break;
            }

            return res_string;
        }

        public static double ConversorPeso(double gramas, string unidade)
        {

            double res_numerico = gramas;
            string res_string = "error";

            switch (unidade.ToUpper())
            {
                case "MILIGRAMA":
                    res_numerico = gramas * 100;
                    break;
                case "QUILOGRAMA":
                    res_numerico = gramas / 1000;
                    break;
                case "TONELADA":
                    res_numerico = gramas / 1000000;
                    break;
                default:
                    res_numerico = gramas;
                    break;

            }


            return res_numerico;
        }
    }
}