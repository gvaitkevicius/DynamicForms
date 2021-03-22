using System.Collections.Generic;

namespace DynamicForms.Models
{
    public class EstruturaObjeto
    {
        public string ClassName { get; set; }
        public bool SubClasse { get; set; }
        public string Namespace { get; set; }
        public List<string> Methods { get; set; }
        public IList<EstruturaAnnotation> AnnotationsClass { get; set; }
        public IList<EstruturaPropriedade> Propriedades { get; set; }
    }

    public class EstruturaPropriedade
    {
        public string TypeProp { get; set; }
        public string Identifier { get; set; }
        public string Value { get; set; }
        public bool PrimaryKey { get; set; }
        public string ForeignKeyClass { get; set; }
        public string AlternativeForeignKeyClass { get; set; }
        public string ForeignKey { get; set; }
        public string ForeignKeyReference { get; set; }
        public string ForeignKeyNamespace { get; set; }
        public IList<EstruturaAnnotation> AnnotationsProp { get; set; }
    }

    public class EstruturaAnnotation
    {
        public string AttributeName { get; set; }
        public IList<EstruturaParametro> Parametros { get; set; }
    }

    public class EstruturaParametro
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
