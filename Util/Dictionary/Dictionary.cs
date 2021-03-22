using System;
using System.Collections.Generic;
using System.Data;
using DynamicForms.Context;
using DynamicForms.Areas.SGI.Model;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DynamicForms.Util
{
    public class Dictionary
    {
        public string Version { get; set; }
        public List<Entity> ActualEntities { get; set; }    //  Versão atual
        private List<Entity> Entities { get; set; }         // Todas as entidades de todas as versões

        public static Dictionary instance;

        public static Dictionary GetInstance()
        {
            if (Dictionary.instance == null)
                Dictionary.instance = new Dictionary();

            return Dictionary.instance;
        }

        private Dictionary()
        {
            this.Version = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            this.Entities = new List<Entity>();
            if (StaticRead.read == false)
                this.Load();
            else
            {
                Dictionary dic = StaticRead.GetDictionary();
                this.Version = dic.Version;
                this.Entities = dic.Entities;
            }
            GetActualEntities();
        }

        public Dictionary(List<Entity> entities)
        {
            this.Version = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            this.Entities = entities;
            GetActualEntities();
        }

        public void GetActualEntities()
        {
            List<Entity> ents = new List<Entity>();
            foreach (Entity e in this.Entities)
            {
                if (!e.Version.Contains("-"))
                    ents.Add(e);
            }
            this.ActualEntities = ents;
        }

        public void Load()
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Resultquery queryResult = new Resultquery();
                List<Parametro> param = new List<Parametro>();
                string sql = "";
                string msg = "";

                using (DbCommand cmd = db.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE ='BASE TABLE' Order By Table_Name";
                        cmd.CommandType = CommandType.Text;
                        db.Database.OpenConnection();
                        using (DbDataReader result = cmd.ExecuteReader())
                        {
                            while (result.Read())
                                this.Entities.Add(new Entity(this.Version, result.GetValue(result.GetOrdinal("TABLE_NAME")).ToString().ToUpper()));
                        }
                        //
                        for (int i = 0; i < this.Entities.Count; i++)
                        {
                            cmd.CommandText = @"SELECT COLUMN_NAME,TABLE_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE '" + this.Entities[i].Name + "'";
                            cmd.CommandType = CommandType.Text;
                            using (DbDataReader r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    string name = r.GetValue(r.GetOrdinal("COLUMN_NAME")).ToString().ToUpper();
                                    string type = r.GetValue(r.GetOrdinal("DATA_TYPE")).ToString().ToUpper();
                                    this.Entities[i].AddColumn(this.Version, name, type);
                                }
                            }
                            cmd.CommandText = @"EXEC sp_fkeys @pktable_name = '" + this.Entities[i].Name + "', @pktable_owner = 'dbo'";
                            cmd.CommandType = CommandType.Text;
                            using (DbDataReader r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    this.Entities[i].AddRelation(this.Version, this.GetEntityByName(r.GetValue(r.GetOrdinal("FKTABLE_NAME")).ToString().ToUpper()));
                                }
                            }
                        }
                        this.SaveFile();
                    }
                    catch (Exception)
                    {
                        msg = $"Error: { sql }";
                    }
                }
            }
        }

        public void AddEntity(string version, string name, List<Column> columns = null, List<Relation> relations = null)
        {
            Entity e;
            if (columns != null && relations != null)
                e = new Entity(version, name, columns, relations);
            else
                e = new Entity(version, name);
            e.Version += "+";
            this.Entities.Add(e);
            e.Version.Replace("+", string.Empty);
            this.ActualEntities.Add(e);
        }

        public void DropEntity(string name)
        {
            int index = GetIndexByName(this.Entities, name);
            this.Entities[index].Version += "-";
            int index2 = GetIndexByName(this.ActualEntities, name);
            this.ActualEntities.RemoveAt(index2);
        }

        public Entity GetEntityByName(string name)
        {
            Entity t = null;
            foreach (Entity item in this.Entities)
            {
                if (item.Name == name)
                    t = item;
            }
            return t;
        }

        public Entity GetActualEntityByName(string name)
        {
            Entity t = null;
            foreach (Entity item in this.ActualEntities)
            {
                if (item.Name == name)
                    t = item;
            }
            return t;
        }

        public int GetIndexByName(List<Entity> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return i;
            }
            return -1;
        }

        public void FilterVersion(string version)
        {
            List<Entity> ents = new List<Entity>();
            if (version != this.Version)
            {
                int y = Convert.ToInt32(version.Substring(0, 4));
                int m = Convert.ToInt32(version.Substring(4, 2));
                int d = Convert.ToInt32(version.Substring(6, 2));
                DateTime ver = new DateTime(y, m, d);
                for (int i = 0; i < this.Entities.Count; i++)
                {
                    string v = this.Entities[i].Version.Replace("-", string.Empty).Replace("+", string.Empty);
                    int ye = Convert.ToInt32(v.Substring(0, 4));
                    int me = Convert.ToInt32(v.Substring(4, 2));
                    int de = Convert.ToInt32(v.Substring(6, 2));
                    DateTime dte = new DateTime(ye, me, de);
                    if (ver >= dte)
                    {
                        for (int j = 0; j < this.Entities[i].Columns.Count; j++)
                        {
                            string vx = this.Entities[i].Columns[j].Version.Replace("-", string.Empty).Replace("+", string.Empty);
                            int yx = Convert.ToInt32(vx.Substring(0, 4));
                            int mx = Convert.ToInt32(vx.Substring(4, 2));
                            int dx = Convert.ToInt32(vx.Substring(6, 2));
                            DateTime dtx = new DateTime(yx, mx, dx);
                            if (ver < dtx)
                                this.Entities[i].DropColumn(this.Entities[i].Columns[j].Name);
                        }
                        for (int j = 0; j < this.Entities[i].Relations.Count; j++)
                        {
                            string vx = this.Entities[i].Relations[j].Version.Replace("-", string.Empty).Replace("+", string.Empty);
                            int yx = Convert.ToInt32(vx.Substring(0, 4));
                            int mx = Convert.ToInt32(vx.Substring(4, 2));
                            int dx = Convert.ToInt32(vx.Substring(6, 2));
                            DateTime dtx = new DateTime(yx, mx, dx);
                            if (ver < dtx)
                                this.Entities[i].DropRelation(this.Entities[i].Relations[j].Entity.Name);
                        }
                        ents.Add(this.Entities[i]);
                    }
                }
                this.ActualEntities = ents;
            }
        }

        public void SaveFile()
        {
            string tab = "        StaticRead.entities = new List<Entity>();\n";
            for (int i = 0; i < this.Entities.Count; i++)
            {
                tab += "        StaticRead.entities.Add(new Entity(\"" + this.Entities[i].Version + "\", \"" + this.Entities[i].Name + "\"));\n";
            }
            for (int i = 0; i < this.Entities.Count; i++)
            {
                for (int j = 0; j < this.Entities[i].Columns.Count; j++)
                {
                    tab += "        StaticRead.entities[StaticRead.GetEntity(\"" + this.Entities[i].Name + "\")].AddColumn(\"" + this.Entities[i].Columns[j].Version + "\", \"" + this.Entities[i].Columns[j].Name + "\",\"" + this.Entities[i].Columns[j].Type + "\");\n";
                }
                for (int j = 0; j < this.Entities[i].Relations.Count; j++)
                {
                    tab += "        StaticRead.entities[StaticRead.GetEntity(\"" + this.Entities[i].Name + "\")].AddRelation(\"" + this.Entities[i].Relations[j].Version + "\", StaticRead.entities[StaticRead.GetEntity(\"" + this.Entities[i].Relations[j].Entity.Name + "\")]);\n";
                }
            }
            string cs = "" +
                "using DynamicForms.Util; \n" +
                "using System.Collections.Generic;\n\n" +
                "namespace DynamicForms.Util\n" +
                "{\n" +
                "   public static class StaticRead\n" +
                "   {\n" +
                "       public static readonly bool read = true;\n" +
               $"       public static readonly string version = \"{ this.Version }\";\n\n" +
                "       public static List<Entity> entities;" +
                "       public static Dictionary GetDictionary()\n" +
                "       {\n" + tab +
                "           return new Dictionary(StaticRead.entities);\n" +
                "       }\n\n" +
                "       public static int GetEntity(string name)\n" +
                "       {\n" +
                "           for(int i = 0; i < StaticRead.entities.Count; i++)\n" +
                "           {\n" +
                "               if(StaticRead.entities[i].Name == name)\n" +
                "                   return i;\n" +
                "           }\n" +
                "           return -1;\n" +
                "       }\n" +
                "   }\n" +
                "}";
            File.WriteAllText("./Util/Dictionary/StaticRead.cs", cs);
        }
    }

    public class Relation
    {
        public Entity Entity { get; set; }
        public string Version { get; set; }

        public Relation(string version, Entity ent)
        {
            this.Version = version;
            this.Entity = ent;
        }
    }

    public class Entity
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<Column> ActualColumns { get; set; }
        public List<Relation> Relations { get; set; }
        public List<Relation> ActualRelations { get; set; }
        public string Version { get; set; }

        public Entity(string version)
        {
            this.Version = version;
            this.Name = "";
            this.Columns = new List<Column>();
            this.Relations = new List<Relation>();
            this.ActualColumns = new List<Column>();
            this.ActualRelations = new List<Relation>();
        }

        public Entity(string version, string name)
        {
            this.Version = version;
            this.Name = name;
            this.Columns = new List<Column>();
            this.Relations = new List<Relation>();
            this.ActualColumns = new List<Column>();
            this.ActualRelations = new List<Relation>();
        }

        public Entity(string version, string name, List<Column> columns, List<Relation> entities)
        {
            this.Version = version;
            this.Name = name;
            this.Columns = columns;
            this.Relations = entities;
            this.ActualColumns = columns;
            this.ActualRelations = entities;
        }

        public void AddColumn(string version, string name, string type)
        {
            Column c = new Column(version, name, type);
            c.Version += "+";
            this.Columns.Add(c);
            c.Version.Replace("+", string.Empty);
            this.ActualColumns.Add(c);
        }

        public void AddRelation(string version, Entity entity)
        {
            Relation r = new Relation(version, entity);
            r.Version += "+";
            this.Relations.Add(r);
            r.Version.Replace("+", string.Empty);
            this.ActualRelations.Add(r);
        }

        public void DropColumn(string col)
        {
            int index = GetColumnByName(this.Columns, col);
            this.Columns[index].Version += "-";
            int index2 = GetColumnByName(this.ActualColumns, col);
            this.ActualColumns.RemoveAt(index2);
        }

        public void DropRelation(string rel)
        {
            int index = GetRelationByName(this.Relations, rel);
            this.Relations[index].Version += "-";
            int index2 = GetRelationByName(this.ActualRelations, rel);
            this.ActualRelations.RemoveAt(index2);
        }

        public int GetColumnByName(List<Column> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return i;
            }
            return -1;
        }

        public int GetRelationByName(List<Relation> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Entity.Name == name)
                    return i;
            }
            return -1;
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }

        public Column(string version)
        {
            this.Version = version;
            this.Name = "";
            this.Type = "";
        }

        public Column(string version, string name, string type)
        {
            this.Version = version;
            this.Name = name;
            this.Type = type;
        }
    }
}