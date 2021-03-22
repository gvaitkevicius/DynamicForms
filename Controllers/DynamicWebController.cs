using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace DynamicForms.Controllers
{
    [Authorize]
    public class DynamicWebController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private MasterController MasterController;
        private int page_size;

        public DynamicWebController(IHostingEnvironment hostingEnvironment) : base()
        {
            _hostingEnvironment = hostingEnvironment;
            this.MasterController = new MasterController();
        }

        public IActionResult Index()
        {
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            return View(user);
        }

        public IActionResult DynamicForm(string tipo, string url)
        {
            if (String.IsNullOrEmpty(tipo) || String.IsNullOrEmpty(url))
                return View("Index");

            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            ViewBag.Tipo = tipo;
            ViewBag.Url = url;

            return View();
        }

        public IActionResult VersaoSistema()
        {
            // A versão equivale ao ano, mes e dia
            ViewBag.Versao = "21.01.22";

            return View();
        }


        public IActionResult GetClassForm(string nome_classe, string arrayDeValoresDefault, string parametrosMetodoContrutor)
        {
            List<EstruturaObjeto> objetos_estruturados = new List<EstruturaObjeto>();
            List<object> instances = new List<object>();
            List<EstruturaObjeto> objetos_estruturados_1N = new List<EstruturaObjeto>();
            List<object> instances_1N = new List<object>();

            T_Usuario usuarioLogado = ObterUsuarioLogado();
            MasterController.UsuarioLogado = usuarioLogado;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(usuarioLogado, nome_classe))
                return StatusCode(403);

            string st = "";
            try
            {
                objetos_estruturados = MasterController.GetObjetosJSON(nome_classe);
                object[] paramsConstructor = GetParamatrosMetodoContrutor(parametrosMetodoContrutor);
                instances = this.GetInstancesClass(objetos_estruturados, paramsConstructor);

                object principalInstance = instances.Where(i => i.ToString() == nome_classe).FirstOrDefault();
                AtribuirValoresDefault(ref principalInstance, arrayDeValoresDefault, nome_classe);

                List<string> namespaceICollections = objetos_estruturados[0].Propriedades
                            .Where(p => p.TypeProp == "ICollection")
                            .Select(p => p.ForeignKeyClass)
                            .ToList();

                List<EstruturaObjeto> listEstruturasCollections;
                int count;
                foreach (var strNamespace in namespaceICollections)
                {
                    listEstruturasCollections = MasterController.GetObjetosJSON(strNamespace);

                    /* 
                     * O passo a seguir verifica se os objetos estruturados retornados anteriormente
                     * já existem na lista de estrutura dos ICollections da classe principal.
                     * objetivo é evitar que estruturas repetidas sejam retornadas.
                     */
                    foreach (var item in listEstruturasCollections)
                    {
                        count = objetos_estruturados_1N.Count(x => x.ClassName == item.ClassName);
                        if (count == 0)
                        {
                            objetos_estruturados_1N.Add(item);
                        }
                    }
                }

                instances_1N.AddRange(this.GetInstancesClass(objetos_estruturados_1N, null));

                #region Verificando se é classe interface

                Type typePrincipalClass = Type.GetType(principalInstance.ToString());
                if (typePrincipalClass.GetInterface(nameof(InterfaceDeTelas)) != null)
                {// É uma classe interface

                    EstruturaObjeto estruturaDaClassePrincipal = objetos_estruturados.Where(x => x.ClassName ==
                                                                    principalInstance.ToString()).FirstOrDefault();

                    EstruturaAnnotation nameClassMapped = estruturaDaClassePrincipal.AnnotationsClass
                                                            .Where(x => x.AttributeName == nameof(ClassMapped))
                                                            .FirstOrDefault();
                    if (nameClassMapped != null)
                    {
                        EstruturaParametro paramNameOfClassMapped = nameClassMapped.Parametros
                                                                        .Where(x => x.Name == "nameOfClassMapped")
                                                                        .FirstOrDefault();
                        if (paramNameOfClassMapped != null)
                        {
                            string strNameOfClassMapped = paramNameOfClassMapped.Value;

                            /* Begin Tratamento de chaves estrangeiras nas Collections da interface */
                            EstruturaObjeto estruturaDaCollection;
                            EstruturaPropriedade property;
                            foreach (var strNamespace in namespaceICollections)
                            {
                                estruturaDaCollection = objetos_estruturados_1N.Where(x => x.ClassName == strNamespace)
                                                            .FirstOrDefault();

                                property = estruturaDaCollection.Propriedades
                                            .Where(x => x.ForeignKeyClass == strNameOfClassMapped).FirstOrDefault();

                                if (property != null)
                                {
                                    property.ForeignKeyClass = typePrincipalClass.Name;
                                    property.AlternativeForeignKeyClass = strNameOfClassMapped;
                                }

                            }
                            /* End Tratamento de chaves estrangeiras nas Collections da interface */
                        }
                    }
                }

                #endregion Verificando se é classe interface

                #region Adicionando Parametros

                /* Serão retornados no Json as preferencias da classe principal que está sendo requisitada */
                var usuario = UsuarioSingleton.Instance.ObterUsuario(usuarioLogado.USE_ID);
                List<T_PREFERENCIAS> prefencias = new List<T_PREFERENCIAS>();
                if (usuario != null)
                {
                    prefencias = usuario
                        .T_PREFERENCIAS.Where(x => x.PRE_NAMESPACE != null && x.PRE_NAMESPACE.Equals(nome_classe, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                #endregion Adicionando Parametros



                //Validações de controle de acesso
                #region Controle de acesso por usuário e perfil
                bool flagAcesso = false;
                for (int i = 0; i < objetos_estruturados.Count; i++)
                {
                    List<T_USUARIO_OBJETO_CONTROLAVEL> listaValidacaoUse = ValidacoesUsuario.getListaUsuariosObjetos(usuarioLogado, objetos_estruturados[i].ClassName);
                    List<T_Perfil_Objeto_Controlavel> listaValidacaoPer = ValidacoesUsuario.getListaPerfisObjetos(usuarioLogado, objetos_estruturados[i].ClassName);

                    List<string> tempAcao = new List<string>();
                    List<int> tempIndice = new List<int>();

                    //Validação de usuário
                    if (listaValidacaoUse.Count > 0)
                    {
                        for (int j = 0; j < listaValidacaoUse.Count; j++)
                        {
                            string objId = listaValidacaoUse[j].OBJ_ID;
                            objId = objId.Split(".")[objId.Split(".").Length - 1]; //Exemplo DynamicForms.Models.Calendario.CAL_DESCRICAO -> Vai pegar somente o CAL_DESCRICAO
                            string className = objetos_estruturados[i].ClassName;

                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacaoUse[j].USO_ACAO =
                                listaValidacaoUse[j].USO_ACAO != null && !listaValidacaoUse[j].USO_ACAO.Equals("") ?
                                listaValidacaoUse[j].USO_ACAO.ToUpper() : listaValidacaoUse[j].USO_ACAO = "CRUD";

                            if (className == listaValidacaoUse[j].OBJ_ID)
                            {
                                if (!listaValidacaoUse[j].USO_ACAO.Contains("R"))
                                {
                                    st = "O Usuário não possui acesso para esta tela";
                                    return Json(new { st });
                                }
                                else
                                {
                                    flagAcesso = true;
                                }
                            }
                            else
                            {
                                for (int x = 0; x < objetos_estruturados[i].Propriedades.Count; x++)
                                {
                                    var prop = objetos_estruturados[i].Propriedades[x];
                                    //O objeto não pode ser visualizado pelo usuário.
                                    if (prop.Identifier.Equals(objId) && !listaValidacaoUse[j].USO_ACAO.Contains("R"))
                                    {
                                        tempAcao.Add("HIDDEN");
                                        tempIndice.Add(x);
                                    }

                                    //O objeto pode ser visualizado mas não alterado pelo usuário.
                                    if (prop.Identifier.Equals(objId) && !listaValidacaoUse[j].USO_ACAO.Contains("U"))
                                    {
                                        tempAcao.Add("READ");
                                        tempIndice.Add(x);
                                    }

                                    //Implica que o usuário pode ler e visualizar o campo, e o perfil não importa.
                                    if (prop.Identifier.Equals(objId) && listaValidacaoUse[j].USO_ACAO.Contains("U") && listaValidacaoUse[j].USO_ACAO.Contains("R"))
                                    {
                                        tempAcao.Add("OK");
                                        tempIndice.Add(x);
                                    }
                                }
                            }
                        }
                    }


                    //Validação de perfil
                    if (listaValidacaoPer.Count > 0)
                    {
                        for (int j = 0; j < listaValidacaoPer.Count; j++)
                        {
                            string objId = listaValidacaoPer[j].OBJ_ID;
                            objId = objId.Split(".")[objId.Split(".").Length - 1]; //Exemplo DynamicForms.Models.Calendario.CAL_DESCRICAO -> Vai pegar somente o CAL_DESCRICAO
                            string className = objetos_estruturados[i].ClassName;

                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacaoPer[j].PEO_ACAO =
                               listaValidacaoPer[j].PEO_ACAO != null && !listaValidacaoPer[j].PEO_ACAO.Equals("") ?
                                listaValidacaoPer[j].PEO_ACAO.ToUpper() : listaValidacaoPer[j].PEO_ACAO = "CRUD";

                            if (className == listaValidacaoPer[j].OBJ_ID)
                            {
                                if (!listaValidacaoPer[j].PEO_ACAO.Contains("R"))
                                {
                                    //Mas o usuário pode ter
                                    if (!flagAcesso)
                                    {
                                        st = "O Perfil não possui acesso para esta tela";
                                        return Json(new { st });
                                    }
                                }
                            }
                            else
                            {
                                for (int x = 0; x < objetos_estruturados[i].Propriedades.Count; x++)
                                {
                                    if (!tempIndice.Contains(x)) //Quer dizer que o usuário já possui configurações para este campo ou classe, então o objeto do usuário prevalece
                                    {
                                        var prop = objetos_estruturados[i].Propriedades[x];
                                        //O objeto não pode ser visualizado pelo perfil.
                                        if (prop.Identifier.Equals(objId) && !listaValidacaoPer[j].PEO_ACAO.Contains("R"))
                                        {
                                            tempAcao.Add("HIDDEN");
                                            tempIndice.Add(x);
                                        }

                                        //O objeto pode ser visualizado mas não alterado pelo perfil.
                                        if (prop.Identifier.Equals(objId) && !listaValidacaoPer[j].PEO_ACAO.Contains("U"))
                                        {
                                            tempAcao.Add("READ");
                                            tempIndice.Add(x);
                                        }


                                    }
                                }
                            }
                        }
                    }

                    for (int x = 0; x < tempIndice.Count; x++)
                    {
                        //Aqui realmente aplica as configurações tanto de perfil quanto de usuário
                        if (tempAcao[x] != "OK")
                        {
                            EstruturaAnnotation temp = new EstruturaAnnotation();
                            temp.AttributeName = tempAcao[x];
                            objetos_estruturados[i].Propriedades[tempIndice[x]].AnnotationsProp.Add(temp);
                        }
                    }
                }
                #endregion

                st = "OK";
                return Json(new
                {
                    st,
                    estrutura_classe = JsonConvert.SerializeObject(objetos_estruturados),
                    instances = JsonConvert.SerializeObject(instances),
                    estrutura_classe_1N = JsonConvert.SerializeObject(objetos_estruturados_1N),
                    instances_1N = JsonConvert.SerializeObject(instances_1N),
                    prefencias
                });
            }
            catch (Exception ex)
            {
                st = UtilPlay.getErro(ex);
                return Json(new
                {
                    st,
                    estrutura_classe = JsonConvert.SerializeObject(objetos_estruturados),
                    instances = JsonConvert.SerializeObject(instances),
                    estrutura_classe_1N = JsonConvert.SerializeObject(objetos_estruturados_1N),
                    instances_1N = JsonConvert.SerializeObject(instances_1N)
                });
            }

        }

        private void AtribuirValoresDefault(ref object principalInstance, string arrayDeValoresDefault, string nome_classe)
        {
            if (!String.IsNullOrEmpty(arrayDeValoresDefault))
            {
                Type type = Type.GetType(nome_classe);

                JObject jObjeto = JsonConvert.DeserializeObject<JObject>(arrayDeValoresDefault);

                foreach (JProperty jProperty in jObjeto.Properties())
                {
                    PropertyInfo property = type.GetProperty(jProperty.Name);
                    if (property != null)
                    {
                        property.SetValue(principalInstance, jProperty.Value.ToObject(property.PropertyType));
                    }
                }
            }
        }

        private List<object> GetInstancesClass(List<EstruturaObjeto> estruturaObjetos, object[] paramsConstructor)
        {
            object instance_class;
            List<object> instances = new List<object>();
            object[] params_constructor = (paramsConstructor != null) ? paramsConstructor : new object[] { };
            foreach (EstruturaObjeto item in estruturaObjetos)
            {
                Type type_class = Type.GetType(item.ClassName);

                if (!item.SubClasse)
                    instance_class = Activator.CreateInstance(type_class, params_constructor);
                else
                    instance_class = Activator.CreateInstance(type_class);

                UtilPlay.DefinirValoresPadroesSaida(instance_class);
                instances.Add(instance_class);
            }

            return instances;
        }

        private object[] GetParamatrosMetodoContrutor(string parametrosMetodoContrutor)
        {
            string[] vetParams = null; // Exemplo => "int:10|string:thiago"
            string[] vetArgument = null; // Exemplo => "int:10"
            string typeArgument;
            string valueArgument;
            List<object> listParams = new List<object>();

            if (!String.IsNullOrEmpty(parametrosMetodoContrutor))
            {
                vetParams = parametrosMetodoContrutor.Split("|");

                for (int i = 0; i < vetParams.Length; i++)
                {
                    vetArgument = vetParams[i].Split(":", 2);
                    if (vetArgument != null && vetArgument.Length == 2)
                    {// O Argumento está correto Exemplo => "int:10"
                        typeArgument = vetArgument[0];
                        valueArgument = vetArgument[1];

                        object param = this.GetValueParameter(typeArgument, valueArgument);
                        listParams.Add(param);
                    }
                    else
                    {
                        throw new Exception(@"Erro na tentativa de converter parametros do construtor da classe.\n
                                              Verifique se os tipos e os valores estao corretos.");
                    }
                }
            }

            return (listParams.Count > 0) ? listParams.ToArray() : new object[] { };
        }

        private object GetValueParameter(string typeArgument, string valueArgument)
        {
            try
            {
                typeArgument = typeArgument.ToLower();
                if (typeArgument.Contains("int"))
                {
                    return Convert.ToInt32(valueArgument);
                }
                else if (typeArgument.Contains("double"))
                {
                    return Convert.ToDouble(valueArgument);
                }
                else if (typeArgument.Contains("float")) // float
                {
                    return Convert.ToSingle(valueArgument);
                }
                else if (typeArgument.Contains("string"))
                {
                    return valueArgument;
                }
                else if (typeArgument.Contains("datetime"))
                {
                    return Convert.ToDateTime(valueArgument);
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                throw new Exception(@"Erro na tentativa de converter parametros do construtor da classe.\n
                                      Verifique se os tipos e os valores estao corretos.");
            }
        }

        public IActionResult ExportarPesquisa(string query_json, int tipoPesquisa)
        {
            string st = "";
            string urlDownload = "";
            try
            {
                EstruturaQuery estruturaQuery = JsonConvert.DeserializeObject<EstruturaQuery>(query_json);
                string namespaceClass = estruturaQuery.ClassName;
                string auxNamespaceClass = this.VerificarNamespaceClasseMapeada(estruturaQuery.ClassName);
                estruturaQuery.ClassName = (auxNamespaceClass != "") ? auxNamespaceClass : estruturaQuery.ClassName;

                MasterController.UsuarioLogado = ObterUsuarioLogado();
                object[] arrayObjects = MasterController.Pesquisar(estruturaQuery, -1, -1);
                List<object> objetosAux = ((IEnumerable)arrayObjects[0]).Cast<object>().ToList();

                if (tipoPesquisa == 2)
                {// Pesquisa individual
                    if (namespaceClass != estruturaQuery.ClassName)
                    {// É uma classe de interface, precisa converter o resultado da pesquisa em classe interface
                        objetosAux = this.ConvertClassMappedToInterface(objetosAux, estruturaQuery.ClassName, namespaceClass);
                    }
                }

                if (objetosAux.Count() > 0)
                {
                    foreach (object obj in objetosAux)
                    {
                        UtilPlay.DefinirValoresPadroesSaida(obj);
                    }
                    urlDownload = CriarArquivo(objetosAux, ref st);
                }
                else
                {
                    st = "A consulta não retornou nenhum resultado.";
                }

                if (st == "")
                    st = "OK";

                return Json(new { st, urlDownload });
            }
            catch (Exception ex)
            {
                st = UtilPlay.getErro(ex);
                urlDownload = "";
                return Json(new { st, urlDownload });
            }
        }

        private string CriarArquivo(List<object> resultadoQuery, ref string msg)
        {
            string downloadUrl = "";
            if (resultadoQuery.Count() > 0)
            {
                //https://www.c-sharpcorner.com/article/import-and-export-data-using-epplus-core/
                try
                {
                    string rootFolder = _hostingEnvironment.WebRootPath;
                    string fileName = $"ExportQuery-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);

                    FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
                    if (file.Exists)
                    {
                        file.Delete();
                        file = new FileInfo(Path.Combine(rootFolder, fileName));
                    }

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        #region Cabeçalho da planilha

                        List<PropertyInfo> properties = resultadoQuery[0].GetType().GetProperties()
                            .Where(x => x.PropertyType.Namespace != null &&
                                (x.PropertyType.Namespace != "System.Collections.Generic" && !x.PropertyType.Namespace.Contains("DynamicForms")))
                            .ToList();

                        // Remove as propriedades Not Mapped
                        properties.RemoveAll(x => x.GetCustomAttributes()
                            .Count(y => y.TypeId.ToString() == "System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute") > 0);

                        int linhaPlanilha = 1;
                        int colunaPlanilha = 0;
                        for (; colunaPlanilha < properties.Count(); colunaPlanilha++)
                        {
                            PropertyInfo property = properties.ElementAt(colunaPlanilha);

                            object name = null;
                            CustomAttributeData attribute = property.GetCustomAttributesData().FirstOrDefault(x => x.AttributeType.Name == nameof(DisplayAttribute));
                            CustomAttributeNamedArgument argument;
                            if (attribute != null)
                            {
                                argument = attribute.NamedArguments.FirstOrDefault();
                            }

                            name = (argument != null) ? argument.TypedValue.Value : property.Name;

                            worksheet.Cells[linhaPlanilha, colunaPlanilha + 1].Value = name;
                        }
                        #endregion Cabeçalho da planilha

                        #region Corpo da planilha
                        linhaPlanilha = 2;
                        for (int i = 0; i < resultadoQuery.Count(); i++)
                        {
                            object objeto = resultadoQuery[i];
                            for (colunaPlanilha = 0; colunaPlanilha < properties.Count(); colunaPlanilha++)
                            {
                                PropertyInfo property = properties.ElementAt(colunaPlanilha);
                                object valueProperty = property.GetValue(objeto);

                                string strValue = valueProperty.ToString();
                                if (!string.IsNullOrEmpty(strValue))
                                {
                                    #region Verificando a Annotation Combobox
                                    IEnumerable<CustomAttributeData> attributes = property.GetCustomAttributesData().Where(x => x.AttributeType.Name == nameof(Combobox));

                                    object valueAnnotation = attributes.Where(att => att.NamedArguments
                                                    .Any(na => (na.MemberName.Equals("Value", StringComparison.OrdinalIgnoreCase) ||
                                                        na.MemberName.Equals("ValueInt", StringComparison.OrdinalIgnoreCase)) &&
                                                        na.TypedValue.Value.ToString().Equals(strValue)))
                                                    .Select(att => att.NamedArguments
                                                        .First(na => na.MemberName.Equals("Description", StringComparison.OrdinalIgnoreCase)).TypedValue.Value)
                                                    .FirstOrDefault();

                                    if (valueAnnotation != null)
                                    {
                                        valueProperty = valueAnnotation;
                                    }

                                    #endregion Verificando a Annotation Combobox
                                }

                                if (valueProperty.GetType().Name == nameof(DateTime))
                                {
                                    DateTime dateTime = (DateTime)valueProperty;
                                    valueProperty = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                                }
                                worksheet.Cells[linhaPlanilha, colunaPlanilha + 1].Value = valueProperty;
                            }
                            linhaPlanilha++;
                        }
                        #endregion Corpo da planilha
                        package.Save();
                    }
                }
                catch (Exception ex)
                {
                    msg = UtilPlay.getErro(ex);
                }
            }

            return downloadUrl;
        }


        public IActionResult AtualizarPreferenciaUsuario(string preTipo, string preNamespace)
        {
            var usuario = ObterUsuarioLogado();
            var usuarioSingleton = UsuarioSingleton.Instance.ObterUsuario(usuario.USE_ID);
            T_PREFERENCIAS preferenciaUsuario;

            //se o namespace for diferente de nullo, coloca ele no filtro
            if (preNamespace != null)
            {
                preferenciaUsuario = usuarioSingleton.T_PREFERENCIAS
                    .Where(p => p.PRE_TIPO == preTipo && p.USE_ID == usuario.USE_ID && p.PRE_NAMESPACE.Equals(preNamespace, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            else
            {
                preferenciaUsuario = usuarioSingleton.T_PREFERENCIAS
                    .Where(p => p.PRE_TIPO == preTipo && p.USE_ID == usuario.USE_ID)
                    .FirstOrDefault();
            }

            return Json(new { preferenciaUsuario });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_json"></param>
        /// <param name="page_size"></param>
        /// <param name="index"></param>
        /// <param name="tipoPesquisa">1 - Pesquisa com paginação, 2 - Pesquisa individual</param>
        /// <returns></returns>
        public IActionResult Pesquisar(string query_json, int page_size, int index, int tipoPesquisa)
        {
            string st;
            List<object> objects = new List<object>();

            this.page_size = page_size;
            /*
                * Buscar somente o necessário.
                * page_size: É a quantidade de itens por página.
                * index: É qual o número da página.
            */
            index--; //No client o índice começa de um, no back começa de zero.
            int reg_inicial = index * page_size; //A partir de qual posição na tabela que começa a buscar

            EstruturaQuery estruturaQuery = JsonConvert.DeserializeObject<EstruturaQuery>(query_json);
            estruturaQuery.RemoverEspacoEmBranco();

            string namespaceClass = estruturaQuery.ClassName;
            string auxNamespaceClass = this.VerificarNamespaceClasseMapeada(estruturaQuery.ClassName);
            estruturaQuery.ClassName = (auxNamespaceClass != "") ? auxNamespaceClass : estruturaQuery.ClassName;

            MasterController.UsuarioLogado = ObterUsuarioLogado();
            object[] arrayObjects = MasterController.Pesquisar(estruturaQuery, reg_inicial, page_size);
            List<object> objetosAux = ((IEnumerable)arrayObjects[0]).Cast<object>().ToList();

            if (tipoPesquisa == 2)
            {// Pesquisa individual
                if (namespaceClass != estruturaQuery.ClassName)
                {// É uma classe de interface, precisa converter o resultado da pesquisa em classe interface
                    objetosAux = this.ConvertClassMappedToInterface(objetosAux, estruturaQuery.ClassName, namespaceClass);
                }
            }

            foreach (object obj in objetosAux)
            {
                UtilPlay.DefinirValoresPadroesSaida(obj);
            }

            arrayObjects[0] = objetosAux;
            objects = arrayObjects.ToList();
            st = "OK";
            return Json(new { st, objects });
        }

        private List<object> ConvertClassMappedToInterface(List<object> objects, string namespaceClassMapped,
            string namespaceClassInterface)
        {
            string nameClassInterface = Type.GetType(namespaceClassInterface).Name;
            Type type = Type.GetType(namespaceClassMapped);
            string nameClassMapped = type.Name;
            ConstructorInfo obj_constructor = type.GetConstructor(Type.EmptyTypes);
            object class_object = obj_constructor.Invoke(new object[] { });
            MethodInfo method = type.GetMethod($"{nameClassMapped}To{nameClassInterface}");
            object[] arguments;
            if (method != null)
            {
                arguments = new object[] { objects };
                method.Invoke(class_object, arguments);
                return ((IEnumerable)arguments[0]).Cast<object>().ToList();
            }

            /*
             * Caso não exista um método que faz a conversão de classe mapeada para interface,
             * será retornada a própria lista de objetos da classe mapeada e a interface do usuário
             * deverá exibir os campos da classe mapeada.
             */
            return objects;
        }

        private string VerificarNamespaceClasseMapeada(string namespaceClass)
        {
            Type type = Type.GetType(namespaceClass);
            PropertyInfo property = type.GetProperty("NamespaceOfClassMapped");
            if (property != null)
            {// é uma classe interface
                object instance_class = Activator.CreateInstance(type);
                string namespaceOfClassMapped = property.GetValue(instance_class).ToString();
                if (!String.IsNullOrEmpty(namespaceOfClassMapped))
                {// Retorna o namespace da classe mapeada.
                    return namespaceOfClassMapped;
                }
                else
                {
                    throw new ArgumentException("Não foi encontrado o namespace da classe mapeada para esta interface!");
                }
            }
            // Retorna vazio quando a classe não é mapeada
            return "";
        }

        [HttpPost]
        public IActionResult Insert(string[] listJSON, string[] class_name)
        {
            List<LogPlay> log = new List<LogPlay>();
            try
            {
                List<List<object>> list_objetos = new List<List<object>>();
                int i = 0;
                for (; i < listJSON.Length; i++)
                {
                    string classe_principal = class_name[i];
                    object objects = UtilPlay.ConvertJsonToListObjects(listJSON[i], classe_principal);
                    List<object> list_objects = ((IEnumerable)objects).Cast<object>().ToList();
                    list_objetos.Add(list_objects);
                }

                MasterController.UsuarioLogado = ObterUsuarioLogado();

                string[] Protocolo = new string[1];
                log = MasterController.UpdateData(list_objetos, 0, true);
                return Json(new { log, list_objetos });
            }
            catch (Exception ex)
            {
                string strException = UtilPlay.getErro(ex);
                log = new List<LogPlay>();
                log.Add(new LogPlay() { MsgErro = strException, Status = "ERRO" });

                return Json(new { log });
            }
        }

        [HttpPost]
        public IActionResult ExecuteMethod(string list_obj, string class_name, string name_method, string parametros)
        {
            //Exemplo de como vem os parâmetros
            //string parametros = "GMA_ID_0=020101;MAQ_ID_1=030101;MAQ_ID_2=060201"; //É necessário por o "_4" para indicar a posição correta

            List<LogPlay> log = new List<LogPlay>();
            if (ValidacoesUsuario.validarMetodoUsuarioPerfil(class_name, name_method, ObterUsuarioLogado()))
            {
                try
                {
                    Type type = Type.GetType(class_name);
                    ConstructorInfo obj_constructor = type.GetConstructor(Type.EmptyTypes);
                    object class_object = obj_constructor.Invoke(new object[] { });
                    MethodInfo method = type.GetMethod(name_method);
                    object[] arguments = null;
                    if (method != null)
                    {
                        object list_objects = UtilPlay.ConvertJsonToListObjects(list_obj, class_name);

                        List<object> objects = ((IEnumerable)list_objects).Cast<object>().ToList();

                        if (parametros != null && parametros.Length > 1)
                        {
                            object list_params = UtilPlay.ConvertParamToObjects(parametros);
                            objects.Add(list_params);
                        }

                        UtilPlay.InjetarUsuarioLogado(ref objects, ObterUsuarioLogado());
                        arguments = new object[] { objects, log };
                        method.Invoke(class_object, arguments);
                        return Json(new { log });
                    }
                    else
                    {
                        throw new Exception($"O método especificado não existe na classe: {class_name}");
                    }
                }
                catch (Exception ex)
                {
                    string strException = UtilPlay.getErro(ex);
                    log = new List<LogPlay>();
                    log.Add(new LogPlay() { MsgErro = strException, Status = "ERRO" });
                    return Json(new { log });
                }

            }
            log.Add(new LogPlay() { MsgErro = "O usuário ou perfil deste usuário não possui permissão para realizar esta operação!", Status = "ERRO" });
            return Json(new { log });
        }

        [HttpPost]
        public IActionResult ExecuteMethodWithParameters(string list_obj, string class_name, string name_method)
        {
            List<LogPlay> log = new List<LogPlay>();
            try
            {
                Type type = Type.GetType(class_name);
                ConstructorInfo obj_constructor = type.GetConstructor(Type.EmptyTypes);
                object class_object = obj_constructor.Invoke(new object[] { });
                MethodInfo method = type.GetMethod(name_method);
                object[] arguments = null;
                if (method != null)
                {
                    var definition = new { primaryKey = "", parametros = new List<string>() };
                    var objects = JsonConvert.DeserializeAnonymousType(list_obj, definition);
                    arguments = new object[] { objects, log };
                    method.Invoke(class_object, arguments);
                    return Json(new { log });
                }
                else
                {
                    throw new Exception($"O método especificado não existe na classe: {class_name}");
                }
            }
            catch (Exception ex)
            {
                string strException = UtilPlay.getErro(ex);
                log = new List<LogPlay>();
                log.Add(new LogPlay() { MsgErro = strException, Status = "ERRO" });
                return Json(new { log });
            }
        }

        public IActionResult LinkSGI(string str_namespace, string ArrayDeValoresDefault, string ParametrosMetodoConstrutor)
        {
            if (String.IsNullOrEmpty(str_namespace))
                return View("Index");

            if (ArrayDeValoresDefault != null)
            {
                int total_aspas = ArrayDeValoresDefault.Count(x => x == 39); // 39 é o caracter aspa simples

                ArrayDeValoresDefault = (total_aspas > 0 && total_aspas % 2 == 0) ? ArrayDeValoresDefault.Replace("'", "\"") : ArrayDeValoresDefault;
            }

            if (ParametrosMetodoConstrutor != null)
            {
                ParametrosMetodoConstrutor = ParametrosMetodoConstrutor.Replace("'", "");
            }

            ViewBag.NamespaceObjeto = (str_namespace == null) ? "" : str_namespace;
            ViewBag.ArrayDeValores = (ArrayDeValoresDefault == null) ? "" : ArrayDeValoresDefault;
            ViewBag.ParametrosConstrutor = (ParametrosMetodoConstrutor == null) ? "" : ParametrosMetodoConstrutor;

            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            return View();
        }

        public IActionResult ListarDiretorio(string Pro_Id)
        {
            // Read more: http://www.linhadecodigo.com.br/artigo/3684/trabalhando-com-arquivos-e-diretorios-em-csharp.aspx#ixzz6F5qHdnsL

            try
            {
                string pathInicial = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documentos\T_PRODUTOS\", Pro_Id);
                string[] pathArquivos = Directory.GetFiles(pathInicial);
                string[] nameArquivos = new string[pathArquivos.Length];

                string name = null;
                for (int i = 0; i < pathArquivos.Length; i++)
                {
                    name = new FileInfo(pathArquivos[i]).Name;
                    nameArquivos[i] = name;

                    pathArquivos[i] = string.Format("{0}{1}/{2}", @"/Documentos/T_PRODUTOS/", Pro_Id, name);
                }

                ViewBag.erro = (pathArquivos.Length == 0) ? string.Format("Não existe documentos para este Produto ({0})", Pro_Id) : null;
                ViewBag.pathArquivos = pathArquivos;
                ViewBag.nameArquivos = nameArquivos;
            }
            catch (Exception)
            {
                ViewBag.erro = string.Format("Não existe diretório para este Produto ({0})", Pro_Id);
                ViewBag.pathArquivos = null;
                ViewBag.nameArquivos = null;
            }

            return View();
        }

        public IActionResult ObterJsonUsuarioLogado()
        {
            string st = "";
            try
            {
                T_Usuario usuario = ObterUsuarioLogado();
                st = "OK";
                return Json(new { st, usuario });
            }
            catch
            {
                T_Usuario usuario = null;
                st = "Erro obtendo o usuario.";
                return Json(new { st, usuario });
            }
        }

    }

}