using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Models
{
    public sealed class ObjetosControlaveisSingleton
    {
        private static ObjetosControlaveisSingleton _instance = null;
        private static int _contInstances;

        public List<T_Objeto_Controlavel> ObjetosControlaveis { get; private set; }
        public List<T_Perfil> Perfis { get; private set; }

        private ObjetosControlaveisSingleton()
        {
            _contInstances = 1;
            this.ObjetosControlaveis = new List<T_Objeto_Controlavel>();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                this.ObjetosControlaveis = db.T_Objeto_Controlavel.AsNoTracking()
                                            .Include(x => x.T_PERFIL_OBJETO_CONTROLAVEL).ToList();

                this.SynchronizeObjControlaveis(db);

                this.Perfis = db.T_Perfil.AsNoTracking()
                                    .Include(p => p.T_PERFIL_OBJETO_CONTROLAVEL)
                                    .ToList();
            }

        }
        public static ObjetosControlaveisSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(ObjetosControlaveisSingleton))
                        _instance = new ObjetosControlaveisSingleton();
                }
                else
                {
                    _contInstances++;
                }
                return _instance;
            }
        }
        public int ContInstances => _contInstances;

        private void SynchronizeObjControlaveis(JSgi db)
        {
            List<T_Objeto_Controlavel> objetos_controlaveis = new DatabaseVersion().ObjetosControlaveis;
            List<T_Objeto_Controlavel> objetos_insert = new List<T_Objeto_Controlavel>();
            List<T_Objeto_Controlavel> objetos_update = new List<T_Objeto_Controlavel>();

            foreach (var obj in objetos_controlaveis)
            {
                /* 
                 * Comparacao entre o dicionário de dados do projeto com os registros na base de dados.
                 * Os objetos que estão no dicionário de dados do projeto mas não estão na base de dados
                 * serão inseridos na base de dados.
                 */

                T_Objeto_Controlavel objeto_controlavel = this.ObjetosControlaveis.Where(o => o.OBJ_ID == obj.OBJ_ID).FirstOrDefault();

                if (objeto_controlavel == null)
                {
                    objetos_insert.Add(obj);
                }
                else
                {
                    /*
                     * Verifica se alguma propriedade do objeto foi modificada no dicionário de dados do projeto.
                     * Caso tenha sido alterado, o objeto será atualizado na base de dados.
                     */
                    if (!obj.Equals(objeto_controlavel))
                        objetos_update.Add(obj);
                }
            }

            if (objetos_insert.Count > 0 || objetos_update.Count > 0)
            {

                if (objetos_insert.Count > 0)
                    db.T_Objeto_Controlavel.AddRange(objetos_insert);
                if (objetos_update.Count > 0)
                    db.T_Objeto_Controlavel.UpdateRange(objetos_update);

                db.SaveChanges();
            }
        }
    }
}
