using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using BaseDodos;
using NegocioFindFriend;
using TabelaAmigos;

namespace FindYourFriend.Controllers
{
    public class FindFriendController : ApiController
    {
        public ListaAmigos Get(Int64 id)
        {
            #region clases/variaveis
            /// <summary>
            /// Variaveis e classes utilizadas 
            /// </summary>
            string msg;
            Crypt vlToken = new Crypt();
            Dictionary<double, Amigos> lisFriendOrder = new Dictionary<double, Amigos>();
            BaseDados dbData = new BaseDados();
            RegraNegocio rn = new RegraNegocio();
            Amigos friendTime = null;
            Amigos friend = null;
            #endregion

            #region retornoGet
            /// <summary>
            ///Identifica e guarda o amigo que esta fazendo a solicitação
            /// </summary>
            try
            {
                friendTime = dbData.Busca(id);
            }
            catch
            {
                // Registrar uma linha de erro no log caso falhe o acesso pelo Token.
                msg = string.Empty;
                msg = "Token invalid: " + id;
                Log(msg, "CalculoHistoricoLog");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            /// <summary>
            /// Essa rotina ira calcular a distancia de todos amigos diferente do amigo que solicitou a busca
            /// </summary>
            int tot = dbData.totalDados();
            int i = 1;
            while (i <= tot)
            {
                try
                {
                    friend = dbData.Busca2(i);
                }
                catch
                {
                    msg = string.Empty;
                    msg = "Error at find a friend";
                    Log(msg, "CalculoHistoricoLog");
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
                if (friend.Token != vlToken.EncDec(id.ToString(), true).ToString())
                {
                    double distancia = rn.DistanciaAmigos(friendTime.latitude, friendTime.longitude, friend.latitude, friend.longitude);
                    lisFriendOrder.Add(distancia, friend);
                    msg = string.Empty;
                    msg = "Calculo de distancia entro o " + friendTime.Name + " com o " + friend.Name + ", total of : " + distancia.ToString() + " KM";
                    Log(msg, "CalculoHistoricoLog");
                }
                i++;
            }
            /// <summary>
            /// Lista que retona apenas os 3 amigos mais proximos, baseado no calculo de distancia.
            /// </summary>
            int c = 1;
            ListaAmigos liFriend = new ListaAmigos();
            liFriend.AfProximos = "Your next friends";
            foreach (KeyValuePair<double, Amigos> liRemove in lisFriendOrder.OrderBy(key => key.Key))
            {
                if (c > 3)
                {
                    lisFriendOrder.Remove(liRemove.Key);
                }
                else
                {
                    liFriend.Amigos.Add(liRemove.Value);
                }

                c++;
            }

            return liFriend;
            #endregion
        }

        #region Log
        /// <summary>
        // Metodo utilizado para passar parametro do arquivo de log que será gerar,
        // A comando utilizado para gerar o arquivo esta no metodo GravaLog.
        // parameter msg é utliza para criar a linha dentro do arquivo log
        // parameter fileName é utilizado para definir o nome do arquivo de log.
        /// </summary>
        public void Log(string msg, string fileName)
        {
            RegraNegocio rNeg = new RegraNegocio();

            string filePath = System.Configuration.ConfigurationManager.AppSettings["PathLog"];
            if (!filePath.EndsWith("\\")) filePath += "\\";
            rNeg.SaveLog(filePath, fileName, msg);
        }
        #endregion

    }
}
