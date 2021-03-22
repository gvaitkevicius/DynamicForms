using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DynamicForms.Areas.PlugAndPlay.Models.Transporte
{
    public class RotaOSM
    {
        public string GEO_ID { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public string NAME { get; set; }
        public string LSAD { get; set; }
        public string CENSUSAREA { get; set; }
        public string COORDENADAS { get; set; }

        public RotaOSM()
        {

        }
        public List<RotaOSM> LoadFileToList(string jSonFilePath)
        {
            string _msg = "OK";
            List<RotaOSM> lstPontos = new List<RotaOSM>();
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(jSonFilePath);
                string content = file.ReadToEnd();
                file.Close();

                dynamic deserialized = JsonConvert.DeserializeObject(content);

                foreach (var item in deserialized.features)
                {
                    lstPontos.Add(new RotaOSM()
                    {
                        GEO_ID = item.properties.GEO_ID,
                        LSAD = item.properties.LSAD,
                        NAME = item.properties.NAME,
                        STATE = item.properties.STATE,
                        COUNTY = item.properties.COUNTY,
                        CENSUSAREA = item.properties.CENSUSAREA,
                        COORDENADAS = item.geometry.coordinates.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                _msg = "Erro:" + ex.Message + "\n";
                _msg += (ex.InnerException != null) ? "" : ex.InnerException.Message;
            }
            return lstPontos;
        }
    }
}

