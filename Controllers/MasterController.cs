using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicForms.Controllers
{

    public class MasterController
    {
        private JSgi _context;
        public T_Usuario UsuarioLogado { get; set; }
        public MasterController() { }

        /// <summary>
        /// Constrói a estrutura da classe, incluindo suas propriedades, métodos e relacionamentos.
        /// </summary>
        /// <param name="class_name">Namespace da classe</param>
        /// <returns>Lista com a estrutura da classe principal e das classes estrangeiras.</returns>
        public List<EstruturaObjeto> GetObjetosJSON(string class_name)
        {
            List<EstruturaObjeto> objetos_estruturados = new List<EstruturaObjeto>();
            List<string> classesEstrangeiras = new List<string>();

            //Constroi o JSON da classe principal que foi requisitada
            EstruturaObjeto obj = ContruirJSON(class_name, ref classesEstrangeiras);
            objetos_estruturados.Add(obj);

            //Controi o JSON das Sub Classes (classes estrangeiras)
            int qtd_classes = classesEstrangeiras.Count();
            for (int i = 0; i < qtd_classes; i++)
            {
                obj = ContruirJSON(classesEstrangeiras[i], ref classesEstrangeiras);
                obj.SubClasse = true;
                objetos_estruturados.Add(obj);
            }

            //Orndenar campos pelos index
            objetos_estruturados = this.SortClassProperties(objetos_estruturados);

            return objetos_estruturados;
        }

        /// <summary>
        /// Ordena a posição das propriedades das classes com base no parâmetros Index da Annotation TAB.
        /// </summary>
        /// <param name="objetos_estruturados">Objetos que serão ordenados.</param>
        /// <returns>Lista da estrutura das classes ordenadas.</returns>
        private List<EstruturaObjeto> SortClassProperties(List<EstruturaObjeto> objetos_estruturados)
        {
            foreach (EstruturaObjeto objEstruturado in objetos_estruturados)
            {
                IList<EstruturaPropriedade> listAtualEstruturaPropriedade = objEstruturado.Propriedades;
                List<EstruturaPropriedade> newListEstruturaPropriedade = new List<EstruturaPropriedade>();
                List<EstruturaPropriedade> listNotContainsIndex = new List<EstruturaPropriedade>();
                string strIndex;
                float auxIndex;
                string strIndex2;
                float auxIndex2;
                int pos;
                EstruturaPropriedade propriedade;
                EstruturaAnnotation annotationTab;
                EstruturaPropriedade propriedade2;
                EstruturaAnnotation annotationTab2;

                int count = 0;
                while (count < listAtualEstruturaPropriedade.Count)
                {
                    propriedade = listAtualEstruturaPropriedade[count];

                    annotationTab = propriedade.AnnotationsProp
                                                .Where(a => a.AttributeName == "TAB")
                                                .FirstOrDefault();
                    if (annotationTab != null)
                    {
                        strIndex = annotationTab.Parametros.Where(p => p.Name == "Index").Select(p => p.Value).FirstOrDefault();
                        auxIndex = (strIndex != null) ? float.Parse(strIndex) : -1;

                        if (auxIndex != -1)
                        {
                            if (newListEstruturaPropriedade.Count == 0)
                            {// lista vazia
                                newListEstruturaPropriedade.Add(propriedade);
                            }
                            else
                            {
                                pos = 0;
                                while (pos < newListEstruturaPropriedade.Count)
                                {
                                    propriedade2 = newListEstruturaPropriedade[pos];
                                    annotationTab2 = propriedade2.AnnotationsProp
                                                .Where(a => a.AttributeName == "TAB")
                                                .FirstOrDefault();
                                    strIndex2 = annotationTab2.Parametros.Where(p => p.Name == "Index").Select(p => p.Value).FirstOrDefault();
                                    auxIndex2 = (strIndex2 != null) ? float.Parse(strIndex2) : -1;

                                    if (auxIndex > auxIndex2)
                                        pos++; // continua para o próximo campo
                                    else
                                        break;
                                }
                                newListEstruturaPropriedade.Insert(pos, propriedade);
                            }
                        }
                        else
                        {
                            listNotContainsIndex.Add(propriedade);
                        }
                    }
                    else
                    {
                        listNotContainsIndex.Add(propriedade);
                    }
                    count++;
                }
                newListEstruturaPropriedade.AddRange(listNotContainsIndex);
                objEstruturado.Propriedades = newListEstruturaPropriedade;
            }

            return objetos_estruturados;
        }

        /// <summary>
        /// Constrói um objeto que representa toda estrutura do objeto (Nome da classe, Propriedades e 
        /// suas Annotations e sub classes extrangeiras).
        /// </summary>
        /// <param name="class_name">Namespace da classe.</param>
        /// <param name="classesEstrangeiras">Lista de classes estrangeiras da classe principal.</param>
        /// <returns>Estrutura do objeto solicitado.</returns>
        private EstruturaObjeto ContruirJSON(string class_name, ref List<string> classesEstrangeiras)
        {
            EstruturaObjeto estruturaJSON = new EstruturaObjeto();
            try
            {
                Type type_class = Type.GetType(class_name);
                estruturaJSON.AnnotationsClass = GetAnnotations(type_class.CustomAttributes);

                PropertyInfo[] propriedades = type_class.GetProperties();
                estruturaJSON.ClassName = class_name;
                List<EstruturaPropriedade> estruturaPropriedades = new List<EstruturaPropriedade>();

                estruturaJSON.Methods = VerificarMethods(type_class);

                for (int i = 0; i < propriedades.Length; i++)
                {
                    PropertyInfo propertyInfo = propriedades[i];

                    List<EstruturaAnnotation> annotations = GetAnnotations(propertyInfo.CustomAttributes);

                    EstruturaAnnotation annotationHiddenInterface = annotations
                                                .Where(a => a.AttributeName == nameof(HIDDENINTERFACE))
                                                .FirstOrDefault();

                    if (annotationHiddenInterface != null) // Esta propriedade não vai para tela
                        continue;

                    string tipo_propriedade = propertyInfo.PropertyType.ToString();

                    string foreignKeyClass = null;
                    string foreignKey = null;
                    string foreignKeyReference = null;
                    string foreignKeyNamespace = null;
                    if (tipo_propriedade.StartsWith("DynamicForms."))
                    {
                        if (!classesEstrangeiras.Any(ce => ce == tipo_propriedade))
                            classesEstrangeiras.Add(tipo_propriedade);
                        foreignKeyClass = tipo_propriedade;
                        string[] aux = tipo_propriedade.Split(".");
                        tipo_propriedade = aux[aux.Length - 1];
                    }
                    else if (tipo_propriedade.StartsWith("System.Collections.Generic.ICollection`1[DynamicForms."))
                    {
                        foreignKeyClass = tipo_propriedade.Split("System.Collections.Generic.ICollection`1[", 2)[1].Replace("]", "");
                        tipo_propriedade = "ICollection";
                        foreignKey = "1N";
                    }
                    else if (tipo_propriedade.StartsWith("System.Collections.Generic.List`1[DynamicForms."))
                    {
                        foreignKeyClass = tipo_propriedade.Split("System.Collections.Generic.List`1[", 2)[1].Replace("]", "");
                        tipo_propriedade = "List";
                    }
                    else
                    {
                        tipo_propriedade = tipo_propriedade.Replace("System.", "");
                    }

                    string nome_propriedade = propertyInfo.Name;


                    if (annotations.Count > 0)
                    {

                        #region Verificando se é um campo de pesquisa
                        EstruturaAnnotation annotation = annotations
                                                .Where(a => a.AttributeName == nameof(SEARCH_NOT_FK))
                                                .FirstOrDefault();
                        if (annotation != null)
                        {
                            EstruturaParametro estruturaParametro = annotation.Parametros
                                                                        .Where(p => p.Name == "namespaceOfForeignClass")
                                                                        .FirstOrDefault();
                            if (estruturaParametro != null)
                            {
                                foreignKey = "ForeignKey";
                                if (!classesEstrangeiras.Any(ce => ce == estruturaParametro.Value))
                                    classesEstrangeiras.Add(estruturaParametro.Value);
                                foreignKeyClass = UtilPlay.getNameClass(estruturaParametro.Value);

                                using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
                                {
                                    IEntityType entityType = db.Model.FindEntityType(Type.GetType(estruturaParametro.Value));
                                    string[] vetPrimaryKey = GetPrimaryKey(entityType);
                                    if (vetPrimaryKey.Length > 0)
                                    {
                                        foreignKeyReference = vetPrimaryKey[0].Split(".")[1];
                                        foreignKeyNamespace = entityType.Name;

                                    }
                                }
                            }
                        }
                        #endregion Verificando se é um campo de pesquisa

                        #region Verificando se é um campo obrigatório
                        annotation = annotations.Where(a => a.AttributeName == "RequiredAttribute" ||
                                                            a.AttributeName == "RangeAttribute")
                                                .FirstOrDefault();
                        if (annotation != null)
                        {
                            annotation = annotations.Where(a => a.AttributeName == "DisplayAttribute")
                                                .FirstOrDefault();
                            if (annotation != null)
                            {
                                annotation.Parametros[0].Value += " *";
                            }
                        }
                        #endregion Verificando se é um campo obrigatório
                    }

                    estruturaPropriedades.Add(
                        new EstruturaPropriedade()
                        {
                            TypeProp = tipo_propriedade,
                            Identifier = nome_propriedade,
                            AnnotationsProp = annotations,
                            ForeignKeyClass = foreignKeyClass,
                            ForeignKey = foreignKey,
                            ForeignKeyReference = foreignKeyReference,
                            ForeignKeyNamespace = foreignKeyNamespace
                        });
                }

                #region Verificacao de classe interface
                EstruturaPropriedade estruturaPropriedade = estruturaPropriedades
                    .Where(x => x.Identifier == "NamespaceOfClassMapped")
                    .FirstOrDefault();
                if (estruturaPropriedade != null)
                {// É uma interface
                    object instance = Activator.CreateInstance(type_class);
                    string namespaceClassMapped = type_class.GetProperty("NamespaceOfClassMapped")
                        .GetValue(instance).ToString();
                    if (!String.IsNullOrEmpty(namespaceClassMapped))
                    {
                        type_class = Type.GetType(namespaceClassMapped);
                    }
                }
                #endregion

                VerificarForeignKey(type_class, estruturaPropriedades);
                VerificarPrimaryKey(type_class, estruturaPropriedades);
                estruturaJSON.Propriedades = estruturaPropriedades;
            }
            catch (Exception e)
            {
                return null;
            }

            return estruturaJSON;
        }

        /// <summary>
        /// Obtém o nome de todos os métodos públicos da classe.
        /// </summary>
        /// <param name="type">Type da classe.</param>
        /// <returns>Nome de todos os métodos públicos da classe</returns>
        private List<string> VerificarMethods(Type type)
        {
            List<MethodInfo> methods = type.GetMethods()
                        .Where(m => !m.Name.Contains("get_") &&
                        !m.Name.Contains("set_") &&
                        !m.Name.Contains("ToString") &&
                        !m.Name.Contains("Equals") &&
                        !m.Name.Contains("GetHashCode") &&
                        !m.Name.Contains("GetType") &&
                        !m.Name.Contains("BeforeChanges") &&
                        !m.Name.Contains("AfterChanges"))
                        .ToList();

            methods.RemoveAll(m => m.GetCustomAttributes().Where(y => y.GetType().Name == nameof(HIDDEN)).Count() > 0);
            List<string> nameMethods = methods.Select(m => m.Name).ToList();

            return nameMethods;
        }

        /// <summary>
        /// Obtém a chave primária da classe mapeada pelo Entity.
        /// </summary>
        /// <param name="entityType">IEntityType da classe.</param>
        /// <returns>Vetor de atributos que fazem parte da chave primária da classe.</returns>
        private string[] GetPrimaryKey(IEntityType entityType)
        {
            List<IKey> primaryKeys = entityType.GetKeys().ToList();
            string[] vetPrimaryKeys = primaryKeys[0].ToString()
                .Replace("Key: ", "")
                .Replace(" PK", "")
                .Replace(" ", "")
                .Split(",");

            return vetPrimaryKeys;
        }

        /// <summary>
        /// Verifica quais propriedades fazem parte da chave primária da classe.
        /// </summary>
        /// <param name="type_class">Type da classe.</param>
        /// <param name="propriedades">Lista de propriedades da classe.</param>
        private void VerificarPrimaryKey(Type type_class, List<EstruturaPropriedade> propriedades)
        {
            using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                IEntityType entityType = _context.Model.FindEntityType(type_class);
                if (entityType != null)
                {//entidade mapeada
                    string[] vetPrimaryKeys = GetPrimaryKey(entityType);

                    string nameKey = "";
                    int i = 0;
                    for (; i < vetPrimaryKeys.Length; i++)
                    {
                        nameKey = vetPrimaryKeys[i].Split(".", 2)[1];
                        EstruturaPropriedade estruturaPropriedade = propriedades
                                                    .Where(p => p.Identifier == nameKey)
                                                    .FirstOrDefault();

                        if (estruturaPropriedade != null)
                        {
                            estruturaPropriedade.PrimaryKey = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifica quais propriedades são chaves estrangeiras.
        /// </summary>
        /// <param name="type_class">Type da classe.</param>
        /// <param name="propriedades">Lista de propriedades da classe.</param>
        private void VerificarForeignKey(Type type_class, List<EstruturaPropriedade> propriedades)
        {
            using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                IEntityType entityType = _context.Model.FindEntityType(type_class);
                if (entityType != null)
                {//entidade mapeada
                    List<IForeignKey> foreignkeys = entityType.GetForeignKeys().ToList();

                    int i = 0;
                    for (; i < foreignkeys.Count; i++)
                    {
                        string strForeignKey = foreignkeys[i].ToString().Split("{'", 2)[1].Split("'}", 2)[0];

                        string fkReference = foreignkeys[i].ToString().Split("-> ", 2)[1].Split("{'", 2)[1].Split("'}", 2)[0];

                        string[] foreignKeys = strForeignKey.Split(",");
                        string[] foreignKeysReferences = fkReference.Split(",");

                        for (int j = 0; j < foreignKeys.Length; j++)
                        {
                            foreignKeys[j] = foreignKeys[j].Replace("'", "").Trim();

                            EstruturaPropriedade estruturaPropriedade = propriedades
                                                                        .Where(p => p.Identifier == foreignKeys[j])
                                                                        .FirstOrDefault();
                            if (estruturaPropriedade != null)
                            {
                                string classeEstrangeira = "";
                                if (foreignkeys[i].PrincipalEntityType.ToString().Contains("Base:"))
                                {
                                    classeEstrangeira = foreignkeys[i].PrincipalEntityType.ToString()
                                                                        .Split(" Base: ", 2)[0];
                                    classeEstrangeira = classeEstrangeira.Split("EntityType: ", 2)[1];
                                }
                                else
                                {
                                    classeEstrangeira = foreignkeys[i].PrincipalEntityType.ToString()
                                                                        .Split("EntityType: ", 2)[1];
                                }

                                estruturaPropriedade.ForeignKeyClass = classeEstrangeira;
                                estruturaPropriedade.ForeignKey = "ForeignKey";
                                estruturaPropriedade.ForeignKeyReference = foreignKeysReferences[j].Replace("'", "").Trim();
                                estruturaPropriedade.ForeignKeyNamespace = foreignkeys[i].PrincipalEntityType.Name;

                            }
                        }

                    }
                }
                else
                {// por enquanto cai aqui quando for interface se ouverem mais opções pegaremos o parametro pelo menu
                    //estruturaPropriedade.ForeignKeyClass = classeEstrangeira;
                    //estruturaPropriedade.ForeignKey = "ForeignKey";
                }
            }
        }

        /// <summary>
        /// Retorna as estruturas das annotations da classe e seu parametros.
        /// </summary>
        /// <param name="customAttributes">Lista de atributos customizados da classe.</param>
        /// <returns>Estrutura das annotations da classe.</returns>
        private List<EstruturaAnnotation> GetAnnotations(IEnumerable<CustomAttributeData> customAttributes)
        {
            List<EstruturaAnnotation> annotations = new List<EstruturaAnnotation>();
            for (int i = 0; i < customAttributes.Count(); i++)
            {
                CustomAttributeData attribute = customAttributes.ElementAt(i);
                string name = attribute.AttributeType.Name;
                List<EstruturaParametro> parametros = GetParametros(attribute.NamedArguments);

                annotations.Add(
                    new EstruturaAnnotation()
                    {
                        AttributeName = name,
                        Parametros = parametros
                    });
            }

            return annotations;
        }

        /// <summary>
        /// Retorna as estruturas dos parametros de uma annotation (Nome e Valor).
        /// </summary>
        /// <param name="customAttributeNameds"></param>
        /// <returns>Retorna as estruturas dos parametros de uma annotation (Nome e Valor).</returns>
        private List<EstruturaParametro> GetParametros(IList<CustomAttributeNamedArgument> customAttributeNameds)
        {
            List<EstruturaParametro> parametros = new List<EstruturaParametro>();
            for (int i = 0; i < customAttributeNameds.Count; i++)
            {
                CustomAttributeNamedArgument attibute_named = customAttributeNameds[i];
                string name = attibute_named.MemberName;
                string value = attibute_named.TypedValue.Value.ToString();

                parametros.Add(
                    new EstruturaParametro()
                    {
                        Name = name,
                        Value = value
                    });
            }
            return parametros;
        }

        /// <summary>
        /// Retorna um IQueryable de uma classe mapeada no Contexto para fazer consultas no banco
        /// </summary>
        /// <param name="class_name"></param>
        /// <returns></returns>
        public IQueryable<Object> GetQueryable(string class_name, JSgi _context)
        {
            return DynamicForms.Context.DbContextExtensions.Set(_context, Type.GetType(class_name));
        }

        public object[] Pesquisar(EstruturaQuery estruturaQuery, int reg_inicial, int qtde_registros)
        {
            object[] obj_resp = new object[2];
            if (estruturaQuery != null && !String.IsNullOrEmpty(estruturaQuery.ClassName))
            {
                using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    IQueryable<Object> query = this.GetQueryable(estruturaQuery.ClassName, _context);
                    Type type = Type.GetType(estruturaQuery.ClassName);
                    IEntityType entityType = _context.Model.FindEntityType(estruturaQuery.ClassName);
                    string tableName = entityType.Relational().TableName;
                    string[] vetPrimaryKeys = GetPrimaryKey(entityType);
                    string auxKey = vetPrimaryKeys[0].Split(".", 2)[1];
                    string nameKey = entityType.FindProperty(auxKey).Relational().ColumnName;
                    StringBuilder str_query = new StringBuilder();

                    if (estruturaQuery.Filters != null && estruturaQuery.Filters.Count > 0)
                    {
                        string str_where = this.GetCondicaoWhere(estruturaQuery.Filters, estruturaQuery.Operators, entityType, type);
                        str_query.Append($"select * from {tableName} where({str_where})");
                    }
                    else
                    {
                        str_query.Append($"select * from {tableName}  ");
                    }
                    List<object> registros;
                    if (reg_inicial == -1 && qtde_registros == -1) // A pesquisa são será paginada
                    {
                        //se houver um filtro de order by, ordena por ele
                        if (estruturaQuery.OrderBys != null && estruturaQuery.OrderBys.First().NameProperty != "")
                        {
                            string propriedade = estruturaQuery.OrderBys.First().NameProperty.ToUpper();
                            string tipo_ordem = estruturaQuery.OrderBys.First().Order.ToUpper();

                            //se o tipo de ordem == 1, ordena ascendente, se nao, descendente
                            registros = tipo_ordem == "1" ?
                                query.AsNoTracking().FromSql(str_query.ToString()).OrderBy(p => p.GetType().GetProperty(propriedade).GetValue(p, null)).ToList() :
                                query.AsNoTracking().FromSql(str_query.ToString()).OrderByDescending(p => p.GetType().GetProperty(propriedade).GetValue(p, null)).ToList();
                        }
                        else
                        {
                            registros = query.AsNoTracking().FromSql(str_query.ToString()).ToList();
                        }
                    }
                    else
                    {
                        //se houver um filtro de order by, ordena por ele
                        if (estruturaQuery.OrderBys != null && estruturaQuery.OrderBys.First().NameProperty != "")
                        {
                            string propriedade = estruturaQuery.OrderBys.First().NameProperty.ToUpper();
                            string tipo_ordem = estruturaQuery.OrderBys.First().Order.ToUpper();

                            //se o tipo de ordem == 1, ordena ascendente, se nao, descendente
                            registros = tipo_ordem == "1" ?
                                query.AsNoTracking().FromSql(str_query.ToString()).OrderBy(p => p.GetType().GetProperty(propriedade).GetValue(p, null)).Skip(reg_inicial).Take(qtde_registros).ToList() :
                                query.AsNoTracking().FromSql(str_query.ToString()).OrderByDescending(p => p.GetType().GetProperty(propriedade).GetValue(p, null)).Skip(reg_inicial).Take(qtde_registros).ToList();
                        }
                        else
                        {
                            registros = query.AsNoTracking().FromSql(str_query.ToString()).Skip(reg_inicial).Take(qtde_registros).ToList();
                        }

                    }

                    int qtd_tegistros = query.AsNoTracking().FromSql(str_query.ToString()).Count();
                    obj_resp[0] = registros;
                    obj_resp[1] = qtd_tegistros;
                    return obj_resp;
                }
            }
            return null;
        }

        public string GetCondicaoWhere(List<Filter> filters, List<Operator> operators, IEntityType entityType, Type type)
        {
            StringBuilder str_where = new StringBuilder();
            Filter filter;
            IProperty property;
            string columnName;
            string value;
            Operator opt;
            Operator auxOpt = new Operator();
            if (filters.Count == 1)
            {
                filter = filters[0];
                property = entityType.FindProperty(filter.NameProperty);
                columnName = property.Relational().ColumnName;
                filter.Type = filter.Type.ToLower();
                value = RemoverEspacos(filter.Value);

                //Se o filtro for do tipo DateTime é preciso converter a data no formato correto aceitado pelo SQL Server (yyyy-MM-DD)
                if (filter.Type == "datetime")
                {
                    string[] tempData = filter.Value.Split("_");
                    DateTime dateTimeInicio = Convert.ToDateTime(tempData[0]);
                    DateTime dateTimeFim = Convert.ToDateTime(tempData[1]);

                    value = string.Format("'{0}' and '{1}'", dateTimeInicio.ToString("dd-MM-yyyy HH:mm:ss"), dateTimeFim.ToString("dd-MM-yyyy HH:mm:ss"));
                    str_where.Append(string.Format("{0} {1} {2}", columnName, filter.OpRelational, value));
                }
                else
                {
                    str_where.Append(string.Format("{0} {1} '{2}'", columnName, filter.OpRelational, value));
                }

            }
            else if (filters.Count > 1 && ValidarFiltrosDaQuery(filters, operators))
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    if (auxOpt != null)
                    {
                        str_where.Append(string.Format(" {0} ", auxOpt.TypeOperator));
                    }

                    filter = filters[i];
                    property = entityType.FindProperty(filter.NameProperty);
                    columnName = property.Relational().ColumnName;
                    //value = (filter.Type == "string" || filter.Type == "date") ? string.Format("'{0}'", filter.Value) : filter.Value;
                    if (filter.Type == "datetime")
                    {
                        string[] tempData = filter.Value.Split("_");
                        DateTime dateTimeInicio = Convert.ToDateTime(tempData[0]);
                        DateTime dateTimeFim = Convert.ToDateTime(tempData[1]);

                        value = string.Format("'{0}' and '{1}'", dateTimeInicio.ToString("dd-MM-yyyy HH:mm:ss"), dateTimeFim.ToString("dd-MM-yyyy HH:mm:ss"));
                    }
                    else
                    {
                        value = "'" + filter.Value + "'";
                    }
                    str_where.Append(string.Format("{0} {1} {2}", columnName, filter.OpRelational, value));


                    if (i < filters.Count - 1) // não é o último filtro
                    {
                        opt = operators[i];
                        str_where.Append(string.Format(" {0} ", opt.TypeOperator));
                    }
                }
            }
            return str_where.ToString();
        }

        /// <summary>
        /// Remove os espaços do começo e do final da string, levando em conta a possibilidade de haver um operador like (parecido com) na string.
        /// </summary>
        /// <param name="str">String que terá os espaços removidos.</param>
        /// <returns>String com os espaços removidos.</returns>
        private string RemoverEspacos(string str)
        {
            if (str == null || str == "")
                return "";

            bool tem_operador_de_like = false; //Operador da condição like é o % % no começo e final da string.

            //se tiver 2 '%' (um no inicio e um no fim), deve remover para adicionar depois de limpar os espaços
            if (str[0] == '%' && str[str.Length - 1] == '%')
            {
                str = str.Remove(str.IndexOf('%'), 1); //remove o primeiro caracter da string, que deve ser um '%'
                str = str.Remove(str.Length - 1, 1); //remove o ultimo caracter da string, que tambem deve ser um '%'
                tem_operador_de_like = true;
            }

            //remover os espaços do começo e final da string
            str = str.TrimStart().TrimEnd();

            //se a string originalmente tinha o operador de like, adiciona novamente no começo e final da string, se não, retorna a string normalmente.
            return tem_operador_de_like ? "%" + str + "%" : str;
        }

        /// <summary>
        /// Valida a quantidade de filtros e operadores lógicos da consulta.
        /// </summary>
        /// <param name="filters">Filtros da consulta</param>
        /// <param name="operators">Operadores lógicos da consulta</param>
        /// <returns></returns>
        private bool ValidarFiltrosDaQuery(List<Filter> filters, List<Operator> operators)
        {
            if (filters.Count > 1 && operators != null && operators.Count == filters.Count - 1)
                return true;
            return false;
        }

        /// <summary>
        /// Verifica se a classe possui o método chamado ExecuteValidation, e caso este método exista 
        /// ele é invocado.
        /// </summary>
        /// <param name="objects">Objetos que serão validados</param>
        /// <param name="class_name">Nome da classe que ao qual os objetos pertence</param>
        /// <returns>Lista de objetos que foram verificados</returns>
        private List<object> ExecuteValidation(List<object> objects, string class_name)
        {
            string str_namespace = objects[0].ToString();

            Type obj_type = Type.GetType(str_namespace);
            ConstructorInfo obj_constructor = obj_type.GetConstructor(Type.EmptyTypes);
            object class_object = obj_constructor.Invoke(new object[] { });
            MethodInfo method = obj_type.GetMethod("Validation");

            if (method != null)
            {
                object retorno = method.Invoke(class_object, new object[] { objects });
                objects = ((IEnumerable)retorno).Cast<object>().ToList();
            }

            return objects;
        }

        /// <summary>
        /// Verifica se a classe possui o método chamado ExecuteCustomValidation, e caso este método exista 
        /// ele é invocado.
        /// </summary>
        /// <param name="objects">Objetos que serão validados</param>
        /// <param name="class_name">Nome da classe que ao qual os objetos pertence</param>
        /// <returns>Lista de objetos que foram verificados</returns>
        private List<object> ExecuteCustomValidation(List<object> objects, string class_name)
        {
            objects = Custom.ValidationCustom.ExecuteCustomValidation(objects, class_name);
            return objects;
        }

        /// <summary>
        /// Verifica se a classe possui o método chamado BeforeChanges, e caso este método exista 
        /// ele é invocado.
        /// </summary>
        /// <param name="objects">Objetos que serão alterados</param>
        /// <param name="class_name">Nome da classe que ao qual os objetos pertence</param>
        /// <returns>Lista de objetos que foram alterados</returns>
        private List<object> BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, ref List<LogPlay> logs, ref int modo_insert)
        {
            object[] arguments = null;
            List<string> namespaceObjects = this.GetNamespaceObjects(objects, null);
            List<string> namespaceExecutados = new List<string>();
            while (namespaceObjects.Count > 0)
            {
                string str_namespace = namespaceObjects[0];
                Type obj_type = Type.GetType(str_namespace);
                ConstructorInfo obj_constructor = obj_type.GetConstructor(Type.EmptyTypes);
                object class_object = obj_constructor.Invoke(new object[] { });
                MethodInfo method = obj_type.GetMethod("BeforeChanges");
                if (method != null)
                {
                    arguments = new object[] { objects, cloneObjeto, logs, modo_insert };

                    UtilPlay.InjetarUsuarioLogado(ref objects, this.UsuarioLogado);

                    method.Invoke(class_object, arguments);

                    objects = ((IEnumerable)arguments[0]).Cast<object>().ToList(); // Objetos atuais utilizado no BeforeChanges da classse
                    modo_insert = Convert.ToInt32(arguments[3]);
                    // Validacoes de erros
                    if (logs != null && logs.Count > 0 || objects.Count() == 0)
                        return objects;
                    // Validacoes de erros 
                    foreach (object obj in objects)
                    {
                        dynamic objDynamic = obj;
                        if (!String.IsNullOrEmpty(objDynamic.PlayMsgErroValidacao) && !objDynamic.PlayAction.Equals("alert", StringComparison.OrdinalIgnoreCase))
                            return objects;
                    }
                }

                namespaceExecutados.Add(str_namespace);
                namespaceObjects = this.GetNamespaceObjects(objects, namespaceExecutados);
            }

            return objects;
        }

        /// <summary>
        /// Invoca o método BeforeCustomChanges da classe BeforeCustom para efetuar alteracoes 
        /// customizadas.
        /// </summary>
        /// <param name="objects">Objetos que serão alterados</param>
        /// <param name="class_name">Nome da classe que ao qual os objetos pertence</param>
        /// <returns>Lista de objetos que foram alterados</returns>
        private List<object> BeforeCustomChanges(List<object> objects, ref CloneObjeto cloneObjeto, string class_name, ref List<LogPlay> logs)
        {
            objects = Custom.BeforeCustom.BeforeCustomChanges(objects, ref cloneObjeto, class_name, ref logs);
            return objects;
        }

        /// <summary>
        /// Este método executa ações que devem ser implementadas na classe cujo namespace corresponde ao parâmetro class_name
        /// </summary>
        /// <param name="objects">Lista de objetos que serão manipulados</param>
        /// <param name="cloneObjeto">Clone dos registros dos objetos na base da dados</param>
        /// <param name="class_name">Namespace da classe que possui o método AfterChangesInTransaction implementado</param>
        /// <param name="logs">Logs dos objetos manipulados</param>
        /// <param name="modo_insert">Modo de persistência na base de dados utilizando no UpdateData</param>
        private List<object> AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, ref List<LogPlay> logs, ref int modo_insert, JSgi db)
        {
            object[] arguments;
            List<string> namespaceObjects = this.GetNamespaceObjects(objects, null);
            List<string> namespaceExecutados = new List<string>();
            while (namespaceObjects.Count > 0)
            {
                string str_namespace = namespaceObjects[0];
                Type obj_type = Type.GetType(str_namespace);
                ConstructorInfo obj_constructor = obj_type.GetConstructor(Type.EmptyTypes);
                object class_object = obj_constructor.Invoke(new object[] { });
                MethodInfo method = obj_type.GetMethod("AfterChanges");
                if (method != null)
                {
                    arguments = new object[] { objects, cloneObjeto, logs, modo_insert, db };
                    UtilPlay.InjetarUsuarioLogado(ref objects, this.UsuarioLogado);

                    method.Invoke(class_object, arguments);

                    objects = ((IEnumerable)arguments[0]).Cast<object>().ToList(); // Objetos atuais utilizado no AfterChangesInTransaction da classse
                    modo_insert = Convert.ToInt32(arguments[3]);

                    // Validacoes de erros 
                    foreach (object obj in objects)
                    {
                        dynamic objDynamic = obj;
                        if (objDynamic.PlayMsgErroValidacao != null && objDynamic.PlayMsgErroValidacao != "")
                            return objects;
                    }
                }
                namespaceExecutados.Add(str_namespace);
                namespaceObjects = this.GetNamespaceObjects(objects, namespaceExecutados);
            }
            return objects;
        }

        /// <summary>
        /// Este método executa ações que devem ser implementadas na classe cujo namespace corresponde ao parâmetro class_name
        /// </summary>
        /// <param name="objects">Lista de objetos que serão manipulados</param>
        /// <param name="cloneObjeto">Clone dos registros dos objetos na base da dados</param>
        /// <param name="class_name">Namespace da classe que possui o método AfterChangesInTransaction implementado</param>
        /// <param name="logs">Logs dos objetos manipulados</param>
        /// <param name="modo_insert">Modo de persistência na base de dados utilizando no UpdateData</param>
        /// <returns></returns>
        private List<object> AfterChangesInTransaction(List<object> objects, ref CloneObjeto cloneObjeto, ref List<LogPlay> logs, ref int modo_insert, JSgi db)
        {
            List<object> retorno = new List<object>();
            object[] arguments;
            List<string> namespaceObjects = this.GetNamespaceObjects(objects, null);
            List<string> namespaceExecutados = new List<string>();
            while (namespaceObjects.Count > 0)
            {
                string str_namespace = namespaceObjects[0];
                Type obj_type = Type.GetType(str_namespace);
                ConstructorInfo obj_constructor = obj_type.GetConstructor(Type.EmptyTypes);
                object class_object = obj_constructor.Invoke(new object[] { });
                MethodInfo method = obj_type.GetMethod("AfterChangesInTransaction");
                if (method != null)
                {
                    arguments = new object[] { objects, cloneObjeto, logs, modo_insert, db };
                    UtilPlay.InjetarUsuarioLogado(ref objects, this.UsuarioLogado);
                    method.Invoke(class_object, arguments);

                    objects = ((IEnumerable)arguments[0]).Cast<object>().ToList(); // Objetos atuais utilizado no AfterChangesInTransaction da classse

                    modo_insert = Convert.ToInt32(arguments[3]);

                    // Validacoes de erros 
                    foreach (object obj in objects)
                    {
                        dynamic objDynamic = obj;
                        if (objDynamic.PlayMsgErroValidacao != null && objDynamic.PlayMsgErroValidacao != "")
                            return objects;
                    }
                }
                namespaceExecutados.Add(str_namespace);
                namespaceObjects = this.GetNamespaceObjects(objects, namespaceExecutados);
            }

            foreach (dynamic ob in objects)
            {
                if (ob.PlayAction.StartsWith("AFTER-"))
                {
                    ob.PlayAction = ob.PlayAction.Split("AFTER-", 2)[1];
                    retorno.Add(ob);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Invoca o método AfterCustomChanges da classe AfterCustom para efetuar alteracoes 
        /// customizadas.
        /// </summary>
        /// <param name="objects">Objetos que serão alterados</param>
        /// <param name="class_name">Nome da classe que ao qual os objetos pertence</param>
        /// <returns>Lista de objetos que foram alterados</returns>
        private List<object> AfterCustomChanges(List<object> objects, string class_name)
        {
            objects = Custom.AfterCustom.AfterCustomChanges(objects, class_name);
            return objects;
        }

        /// <summary>
        /// Efetua operacões de Insert/Update/Delete em uma lista de diferentes tipos de objetos na base de dados.
        /// </summary>
        /// <param name="list_objects">Lista de objetos a serem manipulados na base de dados</param>
        /// <param name="modo_insert">
        ///     0 - Insert único (Com Transacao Global);
        ///     1 - Executa o modo '0' e depois faz o insert de cada objeto até o primeiro erro (Sem Transacao Global);
        ///     2 - Executa o modo '0' e depois faz o insert de cada objeto mesmo dando erro em alguns deles (Sem Transacao Global)
        ///     3 - Faz insert individual usando transacao global (Com Transacao Global);
        ///     4 - Faz insert individual (Sem Transacao Global);
        /// </param>
        /// <param name="trigger">true - Executa os métodos, Before, After e Custom Changes. false - Não executa os métodos</param>
        /// <param name="db">Conexão da base de dados, o valor padrão é null.</param>
        /// <param name="recursivo">Indica se é uma chamada recursiva, o valor padrão é false.</param>
        /// <returns></returns>
        public List<LogPlay> UpdateData(List<List<object>> list_objects, int modo_insert, bool trigger, JSgi db = null, bool recursivo = false)
        {
            List<LogPlay> logs_final = new List<LogPlay>();
            List<LogPlay> logs = new List<LogPlay>();
            for (int i = 0; i < list_objects.Count; i++)
            {
                List<object> objects = list_objects.ElementAt(i);
                string classe_principal = objects[0].ToString();
                if (objects == null || objects.Count == 0)
                    continue;

                CloneObjeto cloneObjeto = new CloneObjeto();

                if (trigger)
                {
                    objects = this.BeforeCustomChanges(objects, ref cloneObjeto, classe_principal, ref logs);
                    if (logs != null && logs.Count > 0 || objects.Count() == 0) // log com erro
                        return logs;

                    objects = this.BeforeChanges(objects, ref cloneObjeto, ref logs, ref modo_insert);
                    if (logs != null && logs.Count > 0 || objects.Count() == 0) // log com erro
                        return logs;
                }

                UtilPlay.DefinirValoresPadroesEntrada(ref objects);
                UtilPlay.VerificarValorForeignKey(ref objects);

                List<object> objects_insert = new List<object>();
                List<object> objects_update = new List<object>();
                List<object> objects_delete = new List<object>();
                List<object> objects_total = new List<object>();

                cloneObjeto.AtualizarClones(ref objects);
                List<T_LOGS_DATABASE> logsDatabase = new List<T_LOGS_DATABASE>();
                T_LOGS_DATABASE auxLogDatabase = new T_LOGS_DATABASE();

                bool insert_unico = true; // utilizado para controlar insert em massa ou individual
                bool fezRollback = false;
                LogPlay objComErro = null;

                if (db == null)
                    db = new ContextFactory().CreateDbContext(new string[] { });

                if (modo_insert == 3)
                {
                    insert_unico = false;

                    if (db.Database.CurrentTransaction == null)
                        db.Database.BeginTransaction();

                }
                else if (modo_insert == 4)
                {
                    insert_unico = false;
                }
                int cont = 0;
                do
                {
                    logs = new List<LogPlay>();

                    for (int j = 0; j < objects.Count; j++)
                    {
                        object item = objects[j];
                        dynamic objeto = item;

                        try
                        {
                            if (objeto.PlayMsgErroValidacao == "") { }
                        }
                        catch (Exception)
                        {
                            objComErro = new LogPlay(item, "ERRO", "Não existe campo PlayMsgErroValidacao definido na classe.");
                            logs.Add(objComErro);

                            if (!recursivo && db.Database.CurrentTransaction != null)
                            {
                                db.Database.RollbackTransaction();
                                fezRollback = true;
                                break;
                            }
                        }

                        try
                        {
                            if (objeto.PlayAction == "") { }
                        }
                        catch (Exception)
                        {
                            objComErro = new LogPlay(item, "ERRO", "Não existe campo PlayAction definido na classe.");
                            logs.Add(objComErro);

                            if (!recursivo && db.Database.CurrentTransaction != null)
                            {
                                db.Database.RollbackTransaction();
                                fezRollback = true;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(objeto.PlayMsgErroValidacao) && objeto.PlayAction != "ALERT") // Erro na validacao da classe, ou da validacao customizada
                        {// SOMENTE ENTRA EM VALIDAÇÕES CASO SEJA "INSERT""UPDATE" OU "DELETE"
                            objComErro = new LogPlay(item, "ERRO", objeto.PlayMsgErroValidacao);
                            logs.Add(objComErro);
                            //TEMPORARIAMENTE RETORNAREMOS LOGO NO PRIMEIRO ERRO 
                            // POSTERIORMENTEE LISTAREMOS TODOS OS ERROS, NAO CHAMAREMOS AFTERCHANCHES E NADA SERA SALVO NEM TENTATIVAS DE SALVAR EM CASO DE FALHA 
                            if (modo_insert != 2)
                            {// 2 SOMENTE PODE SER UTILIZADO QUANDO O ALGORITIMO GARANTE INTEGRIDADE CASSO SEJA RODADO N VESES APOS UMA FALHA
                                // FUNÇÕES PADROES EXETO INTERFACE   ÃO PODEM UTILIZAR O 2 POIS ESTARÃO SUGEITAS A FALHAS E ESTANDO SUGEITA A FALHAS PODEM INCLUIR ALGUNS REGISTROS E OUTROS NÃO 
                                if (!recursivo && db.Database.CurrentTransaction != null)
                                {
                                    db.Database.RollbackTransaction();
                                    fezRollback = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            string msg_validation = "";
                            if (objeto.PlayAction.ToUpper() == "UPDATE" || objeto.PlayAction.ToUpper() == "INSERT")
                            {
                                msg_validation = ValidateDataAnnotations.ValidateModel(item);
                            }
                            if (msg_validation != "")
                            {
                                objComErro = new LogPlay(item, "ERRO", msg_validation);
                                logs.Add(objComErro);

                                if (!recursivo && db.Database.CurrentTransaction != null)
                                {
                                    db.Database.RollbackTransaction();
                                    fezRollback = true;
                                    break;
                                }
                            }
                            else
                            {
                                try
                                {
                                    string namespaceItem = item.GetType().FullName;
                                    IQueryable<Object> query = this.GetQueryable(namespaceItem, db);
                                    //Controle Acesso
                                    if (objeto.PlayAction.ToUpper() == "INSERT" || objeto.PlayAction.ToUpper() == "UPDATE" || objeto.PlayAction.ToUpper() == "DELETE")
                                    {
                                        if (UsuarioLogado != null && !ValidacoesUsuario.ValidarOperacoesNaBaseDeDados(UsuarioLogado, objeto, namespaceItem, query))
                                        {
                                            throw new Exception("Usuário sem permissão para esta operação");
                                        }
                                    }

                                    switch (objeto.PlayAction.ToUpper())
                                    {
                                        case "INSERT":
                                            if (!insert_unico)
                                            {
                                                db.Add(item);

                                                db.SaveChanges();
                                                logs.Add(new LogPlay(item, "OK", ""));
                                            }
                                            else
                                            {
                                                objects_insert.Add(item);
                                            }
                                            break;

                                        case "UPDATE":
                                            if (!insert_unico)
                                            {
                                                db.Update(item);

                                                logsDatabase = auxLogDatabase.GerarLogsAlteracoes(new List<object> { item }, ref cloneObjeto, UsuarioLogado);
                                                db.AddRange(logsDatabase);

                                                db.SaveChanges();
                                                logs.Add(new LogPlay(item, "OK", ""));
                                            }
                                            else
                                            {
                                                objects_update.Add(item);
                                            }
                                            break;

                                        case "DELETE":
                                            if (!insert_unico)
                                            {
                                                db.Remove(item);

                                                logsDatabase = auxLogDatabase.GerarLogsAlteracoes(new List<object> { item }, ref cloneObjeto, UsuarioLogado);
                                                db.AddRange(logsDatabase);

                                                db.SaveChanges();
                                                logs.Add(new LogPlay(item, "OK", ""));
                                            }
                                            else
                                            {
                                                objects_delete.Add(item);
                                            }
                                            break;
                                        case "INTERFACE":
                                            break;
                                        case "OK":
                                            break;
                                        case "IGNORE":
                                            break;
                                        case "ALERT":
                                            logs.Add(new LogPlay(item, "ALERT", ""));
                                            break;
                                        default:
                                            throw new Exception("Action definida não é válida");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //string aux_msg = UtilPlay.getErro(ex);
                                    //string aux_msg = ex.Message;
                                    string aux_msg = UtilPlay.getErro(ex);

                                    objComErro = new LogPlay(item, "ERRO", aux_msg);
                                    logs.Add(objComErro);
                                    db.Entry(item).State = EntityState.Detached;
                                    if (modo_insert == 1)
                                        break;

                                    if (!recursivo && db.Database.CurrentTransaction != null)
                                    {
                                        db.Database.RollbackTransaction();
                                        fezRollback = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (insert_unico)
                    {
                        try
                        {
                            objects_total = new List<object>();
                            objects_total.AddRange(objects_delete);
                            objects_total.AddRange(objects_insert);
                            objects_total.AddRange(objects_update);

                            if (objects_delete.Count > 0)
                                db.RemoveRange(objects_delete);
                            if (objects_insert.Count > 0)
                                db.AddRange(objects_insert);
                            if (objects_update.Count > 0)
                                db.UpdateRange(objects_update);

                            List<object> objAlterados = new List<object>();
                            objAlterados.AddRange(objects_update);
                            objAlterados.AddRange(objects_delete);

                            logsDatabase = auxLogDatabase.GerarLogsAlteracoes(objAlterados, ref cloneObjeto, UsuarioLogado);
                            db.AddRange(logsDatabase);

                            if (objects_total.Count > 0)
                            {
                                if (db.Database.CurrentTransaction == null)
                                    db.Database.BeginTransaction();

                                db.SaveChanges();
                                for (int k = 0; k < objects_total.Count; k++)
                                {
                                    logs.Add(new LogPlay(objects_total[k], "OK", ""));
                                }

                                List<object> objectsAfterChanges = AfterChangesInTransaction(objects, ref cloneObjeto, ref logs, ref modo_insert, db);
                                List<LogPlay> logsUpdateData = new List<LogPlay>();
                                if (objectsAfterChanges.Count > 0)
                                {
                                    logsUpdateData = UpdateData(new List<List<object>> { objectsAfterChanges }, modo_insert, trigger, db, true);
                                    logs.AddRange(logsUpdateData);
                                }
                                if (!recursivo && !logsUpdateData.Any(l => l.Status.Equals("ERRO")))
                                {
                                    db.Database.CommitTransaction();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //duplicidades ESTRUTURA
                            List<EstruturaProduto> EST = new List<EstruturaProduto>();
                            foreach (var obj in objects)
                            {
                                EstruturaProduto e = (EstruturaProduto)obj;
                                EST.Add(e);
                            }
                            var ESTGrp = EST.
                            GroupBy(g => new { pro_id = g.PRO_ID_PRODUTO, comp = g.PRO_ID_COMPONENTE})
                            .Select(l => new {
                                pro_id = l.Key.pro_id,
                                comp = l.Key.comp,
                                conta = l.Count()
                            }).ToList();
                            foreach (var item in ESTGrp.Where(x=>x.conta>1))
                            {
                                Console.WriteLine(item.pro_id + " - " + item.comp);
                            }
                            //fim duplicidade estrutura

                            if (!recursivo && db.Database.CurrentTransaction != null)
                            {
                                db.Database.RollbackTransaction();
                            }

                            if (modo_insert == 1 || modo_insert == 2)
                            {
                                insert_unico = false;
                                objects_insert = null;
                                objects_update = null;
                                objects_delete = null;
                                db.DetachAllEntities();
                            }
                            else
                            {
                                string msg_erro = UtilPlay.getErro(ex);
                                int k = 0;
                                for (; k < objects_total.Count; k++)
                                {
                                    logs.Add(new LogPlay(objects_total[k], "ERRO", msg_erro));
                                }
                            }
                        }
                    }
                    else
                    {
                        cont = 2;
                        if (db.Database.CurrentTransaction != null && !fezRollback)
                        {
                            /* Está usando transação global, e não fez rollback
                             */

                            List<object> objectsAfterChanges = AfterChangesInTransaction(objects, ref cloneObjeto, ref logs, ref modo_insert, db);
                            List<LogPlay> logsUpdateData = new List<LogPlay>();
                            if (objectsAfterChanges.Count > 0)
                            {
                                logsUpdateData = UpdateData(new List<List<object>> { objectsAfterChanges }, modo_insert, trigger, db, true);
                                logs.AddRange(logsUpdateData);
                            }
                            if (!recursivo && !logsUpdateData.Any(l => l.Status.Equals("ERRO")))
                            {
                                db.Database.CommitTransaction();
                            }
                            else if (!recursivo)
                            {
                                db.Database.RollbackTransaction();
                            }
                        }
                        else if (fezRollback)
                        {
                            // Deu erro no insert de algum objeto, e está usando Transacao Global
                            // Todos os objetos deverão ficar com o status de ERRO
                            List<LogPlay> logsOK = logs.Where(x => x.Status == "OK").ToList();
                            string msgErro = $@"Erro na classe {objComErro.NomeClasse} - Chave Primáia: {objComErro.PrimaryKey} - MsgErro: {objComErro.MsgErro}";
                            foreach (var item in logsOK)
                            {
                                item.Status = "ERRO";
                                item.MsgErro = msgErro;
                            }
                        }
                    }
                    cont++;
                } while (!insert_unico && cont < 2);

                if (trigger)
                {
                    objects = this.AfterChanges(objects, ref cloneObjeto, ref logs, ref modo_insert, db);
                    objects = this.AfterCustomChanges(objects, classe_principal);
                }
            }
            logs_final.AddRange(logs);
            return logs_final;
        }

        /// <summary>
        /// Verifica quais namespaces de uma lista de objetos já foram executados.
        /// </summary>
        /// <param name="objects">Lista de objetos que estão sendo sendo processados.</param>
        /// <param name="namespaceExecutados">Namespaces que já foram executados.</param>
        /// <returns>Lista de namespaces que ainda não foram executados.</returns>
        private List<string> GetNamespaceObjects(List<object> objects, List<string> namespaceExecutados)
        {
            List<string> namespaceAux = (namespaceExecutados != null && namespaceExecutados.Count > 0) ?
                namespaceExecutados : new List<string>();

            List<string> namespaceObjects = new List<string>();

            foreach (object item in objects)
            {
                int qtd = namespaceAux.Count(x => x == item.ToString());
                if (qtd == 0)
                    namespaceObjects.Add(item.ToString());
            }

            return namespaceObjects;
        }
    }
}
