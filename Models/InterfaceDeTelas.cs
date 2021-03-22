using DynamicForms.Util;
using System.Collections.Generic;

namespace DynamicForms.Models
{
    public interface InterfaceDeTelas
    {
        bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert);
        //string NamespaceOfClassMapped { get; set; }
    }
}
