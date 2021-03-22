//using PagedList;
using P.Pager;
using System.Collections.Generic;

namespace DynamicForms.Areas.SGI.Model
{
    public class Graficos
    {
        public IPager<MedicoesInd> Indicadores { get; set; }
        public List<vw_SGI_PARAMETRO_RELMEDICOES> Medicoes { get; set; }
        public List<vw_SGI_PARAMETRO_RELMEDICOES> AnoAnterior { get; set; }
        public List<T_Informacoes_Complementares> Complementares { get; set; }
        public List<T_PlanoAcao> PlanoAcoes { get; set; }
        public List<T_Favoritos> Favoritos { get; set; }
    }
}