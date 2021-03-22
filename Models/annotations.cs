using System;

namespace DynamicForms.Models
{

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Combobox : Attribute
    {
        public string Description;
        public string Value;
        public int ValueInt;
        public bool Disabled;
    }

    // CRUD
    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class READ : Attribute
    {

    }
    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class HIDDEN : Attribute
    {

    }

    /// <summary>
    /// Esta Annotation deve ser utilizada para definir quais atributos da classe não serão 
    /// incluídos na estrutura da classe que é exibida para o usuário.
    /// </summary>
    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class HIDDENINTERFACE : Attribute
    {

    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class TAB : Attribute
    {
        public string Value;
        public float Index;
    }


    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class AUTOINCREMENT : Attribute
    {

    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class DEFALTVALUE : Attribute
    {
        public string value;
    }

    // UTILIZADO EM PESQUISAS NOS CAMPOS FOREIGN KEY AO PESQUISAR O SISTEMA INCLUIRÁ ESTE CAMPO NA CLAUSULA WHERE BUSCANDO COM CAMPO_FK = 'CONTEUDO DIGITADO' OR  SEARCH_FK = 'CONTEUDO DIGITADO'
    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class SEARCH : Attribute
    {

    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class SEARCH_NOT_FK : Attribute
    {
        /* Utilizado nos campos da classe que não estão mapeados como FK.
         * O motivo de criação dessa annotation é para que o campo GRP_ID_COMPOSICAO da InterfaceTelaToProdutoCaixa
         * possa ser utilizado como campo de pesquisa.
         */
        public string namespaceOfForeignClass;
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class GRID : Attribute
    {
        //Utilizado para aparecer nas consultas, mas não ser utilizado como coluna na hora de pesquisar.
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Checkbox : Attribute
    {
        public string Description { get; set; }
        public string TargetValue { get; set; }
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Password : Attribute
    { // Mostra o campo somente na tela de criar no registro.
      // Na tela de Update o campo não é exibido.

    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Email : Attribute
    {
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class TextArea : Attribute
    {
        //O campo é do tipo TextArea
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Persistent : Attribute
    {
        //Informações que persistem na tela mesmo após apertar o botão salvar.
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class ClassMapped : Attribute
    {
        /* Criada para dizer qual é o nome da classe mapeada das interfaces.
         * Ela é necessária para poder utilizar o recurso de listagem dos registros das Collections.
         */
        public string nameOfClassMapped;
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class EDIT : Attribute
    {
        //O campo obrigatóriamente é editável
    }

    [AttributeUsage(System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class Mask : Attribute
    {
        //Aplica a máscara contida an variável Value
        public string Value { get; set; }
    }
}



