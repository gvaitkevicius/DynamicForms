using DynamicForms.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace DynamicForms.Models
{
    public class CloneObjeto
    {
        private List<object> CloneObjetos;
        public CloneObjeto()
        {
            CloneObjetos = new List<object>();
        }

        public List<object> GetCloneObjetos()
        {
            return this.CloneObjetos;
        }

        public bool AddClone(object objAtual, object objClone)
        {
            bool retorno;
            dynamic aux = objAtual;
            if (aux.PlayAction.ToUpper() == "UPDATE" || aux.PlayAction.ToUpper() == "DELETE")
            {
                int indexClone;
                if (aux.IndexClone == null)
                {
                    this.CloneObjetos.Add(objClone);
                    indexClone = this.CloneObjetos.IndexOf(objClone);
                    aux.IndexClone = indexClone;
                    retorno = true;
                }
                else if (aux.IndexClone >= 0)
                {
                    indexClone = aux.IndexClone;
                    this.CloneObjetos[indexClone] = objClone;
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }

        public object GetClone(object obj)
        {
            object clone;
            int indexClone;
            dynamic aux = obj;
            if (aux.IndexClone != null && aux.IndexClone >= 0)
            {
                indexClone = aux.IndexClone;
                clone = this.CloneObjetos.ElementAt(indexClone);
            }
            else
            {
                using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    try
                    {
                        EntityEntry entry = db.Entry(obj);
                        clone = entry.GetDatabaseValues().ToObject();
                        this.CloneObjetos.Add(clone);
                        indexClone = this.CloneObjetos.IndexOf(clone);
                        aux.IndexClone = indexClone;
                    }
                    catch (Exception ex)
                    {
                        clone = null;
                    }
                }
            }

            return clone;
        }

        public void AtualizarClones(ref List<object> objetos)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                object clone;
                int indexClone;
                foreach (object obj in objetos)
                {
                    dynamic aux = obj;
                    if ((aux.IndexClone == null || aux.IndexClone < 0) && aux.PlayAction.ToUpper() == "UPDATE" || aux.PlayAction.ToUpper() == "DELETE")
                    {
                        try
                        {
                            EntityEntry entry = db.Entry(obj);
                            clone = entry.GetDatabaseValues().ToObject();
                            this.CloneObjetos.Add(clone);
                            indexClone = this.CloneObjetos.IndexOf(clone);
                            aux.IndexClone = indexClone;
                        }
                        catch (Exception ex)
                        {
                            clone = null;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> getChangedPoperties(object obj1, object obj2)
        {
            List<string> changedProperties = new List<string>();

            if (obj1 != null && obj2 != null)
            {
                string namespaceObj1 = obj1.GetType().FullName;
                string namespaceObj2 = obj2.GetType().FullName;

                if (namespaceObj1.StartsWith("DynamicForms") && namespaceObj2.StartsWith("DynamicForms"))
                {
                    if (namespaceObj1 == namespaceObj2)
                    {
                        PropertyInfo[] properties = obj1.GetType().GetProperties();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            PropertyInfo p = properties[i];

                            string typeProperty = p.PropertyType.ToString();
                            CustomAttributeData annotation = p.CustomAttributes.Where(x => x.AttributeType.Name == nameof(NotMappedAttribute)).FirstOrDefault();
                            if (annotation != null || typeProperty.StartsWith("System.Collections") || typeProperty.StartsWith("DynamicForms"))
                                continue;

                            string value1 = p.GetValue(obj1) == null ? null : p.GetValue(obj1).ToString().Trim();
                            string value2 = p.GetValue(obj2) == null ? null : p.GetValue(obj2).ToString().Trim();

                            if (value1 != value2)
                            {
                                changedProperties.Add(p.Name);
                            }
                        }
                    }
                }
            }

            return changedProperties;
        }
    }
}
