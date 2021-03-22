//using PagedList;
using System.Collections.Generic;

namespace DynamicForms.Areas.SGI.Model
{
    public class ViewMedicoes
    {
        //public IPagedList<MedicoesInd> Indicadores { get; set; } encontrar PagedList para o ASP.NET Core
        public List<MedicoesInd> Indicadores { get; set; }
        public List<T_Medicoes> Medicoes { get; set; }
        public List<T_Metas> Metas { get; set; }
        public List<SP_SGI_MEDICOES_MES_Result> SP_SGI_MEDICOES_MES_Result { get; set; }
        public T_Indicadores Indicador { get; set; }
        public List<vw_SGI_PARAMETRO_RELMEDICOES> AnoAnterior { get; set; }
    }
}