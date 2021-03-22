using System.Collections.Generic;

namespace DynamicForms.Models
{
    public class EstruturaQuery
    {
        public string ClassName { get; set; }
        public List<Filter> Filters { get; set; }
        public List<Operator> Operators { get; set; }
        public List<string> SubClass { get; set; }
        public List<Column> Columns { get; set; }
        public List<OrderBy> OrderBys { get; set; }

        public void RemoverEspacoEmBranco()
        {
            if (this.Filters != null && this.Filters.Count > 0)
            {// rotina para remover os espaços em branco da consulta
                foreach (var f in this.Filters)
                {
                    if (string.IsNullOrEmpty(f.Value))
                        continue;

                    bool temPercIni = f.Value.StartsWith("%");
                    bool temPercFim = f.Value.EndsWith("%");

                    // Removendo o char '%' para consultas que usam o like
                    if (temPercIni)
                        f.Value = f.Value.Remove(0, 1);
                    if (temPercFim)
                        f.Value = f.Value.Remove(f.Value.Length - 1, 1);

                    // remove os espaços em branco
                    f.Value = f.Value.Trim();

                    // Inserindo o char '%' para consultas que usam o like
                    if (temPercIni)
                        f.Value = "%" + f.Value;
                    if (temPercFim)
                        f.Value = f.Value + "%";
                }
            }
        }
    }
    public class Column
    {
        public string NameColumn { get; set; }
        public string Table { get; set; }
    }

    public class Filter
    {
        public string NameProperty { get; set; }
        public string OpRelational { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class Operator
    {
        public string TypeOperator { get; set; }
    }

    public class OrderBy
    {
        public string NameProperty { get; set; }
        public string Order { get; set; }
    }
}
