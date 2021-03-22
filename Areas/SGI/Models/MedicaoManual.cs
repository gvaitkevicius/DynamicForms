using System.Collections.Generic;

namespace DynamicForms.Areas.SGI.Model
{
    public class MedicaoManual
    {
        public int tipo { get; set; }
        public T_Metas Meta { get; set; }
        public List<T_Medicoes> Medicoes { get; set; }
    }
}