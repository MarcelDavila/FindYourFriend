using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TabelaAmigos
{
    public class Amigos
    {
        /// <summary>
        /// Pripriedades Classe
        /// </summary>
        public Int64 id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public Amigos()
        {

        }

        public Amigos(int id, string cod, string nome, double lat, double longi)
        {
            this.id = id;
            this.Token = cod;
            this.Name = nome;
            this.latitude = lat;
            this.longitude = longi;
        }
    }

}
