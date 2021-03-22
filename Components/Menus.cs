using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Components
{
    public class Menus : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ViewBag.Inicial = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "INICIAL" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Engenharia = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "ENGENHARIA" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Estoque = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "ESTOQUE" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Transporte = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "TRANSPORTE" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Vendas = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "VENDAS" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Admin = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "ADMIN" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.Gestao = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "GESTAO" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            ViewBag.QA = ObjetosControlaveisSingleton.Instance.ObjetosControlaveis.Where(x => x.OBJ_GRUPO == "QA" && (x.OBJ_TIPO.Trim() == "CLASSE" || x.OBJ_TIPO.Trim() == "INTERFACE")).ToList();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                #region Máquinas
                List<Maquina> maquinas = db.Maquina.AsNoTracking()
                    .Where(f => f.CAL_ID != null)
                    .Select(m => new Maquina
                    { MAQ_ID = m.MAQ_ID, MAQ_CONTROL_IP = m.MAQ_CONTROL_IP, MAQ_DESCRICAO = m.MAQ_DESCRICAO })
                    .ToList();
                ViewBag.MaquinasMenu = maquinas;
                #endregion Máquinas


                #region Equipes

                var maquinaEquipes = db.T_MAQUINAS_EQUIPES.AsNoTracking().GroupBy(x => x.EQU_ID).ToList();
                ViewBag.MaquinaEquipes = maquinaEquipes;

                #endregion Equipes

                return View();
            }

        }

        private List<string> GetLabelClasses(List<string> namespace_classes)
        {
            List<string> label_menus = new List<string>();
            string[] vet = null;
            foreach (var namespace_classe in namespace_classes)
            {
                vet = namespace_classe.Split(".");
                label_menus.Add(vet[vet.Length - 1]);
            }

            return label_menus;
        }
    }
}
