using System;
using System.Collections.Generic;
using TabelaAmigos;
using NegocioFindFriend;

namespace BaseDodos
{
    public class BaseDados
    {
        /// <summary>
        /// Dicionario de dados com lista de amigos 
        /// </summary>
        private static Dictionary<Int64, Amigos> banco = new Dictionary<Int64, Amigos>();
        private static Dictionary<Int64, Amigos> banco2 = new Dictionary<Int64, Amigos>();
        private static int contador = 1;

        /// <summary>
        /// Classe resposnavel em registrar amigos
        /// </summary>
        static BaseDados()
        {
            Crypt vlToken = new Crypt();
            Amigos marcel = new Amigos(1, vlToken.EncDec(6237.ToString(),true), "Marcel", -21.723859, -46.575194);
            Amigos maria = new Amigos(2, vlToken.EncDec(3460.ToString(), true), "Maria", -23.718516, -46.577254);
            Amigos joao = new Amigos(3, vlToken.EncDec(1418.ToString(), true), "Joao", -23.740058, -46.554606);
            Amigos pedro = new Amigos(4, vlToken.EncDec(2485.ToString(), true), "Pedro", -23.736542, -46.553748);
            Amigos jose = new Amigos(5, vlToken.EncDec(1616.ToString(), true), "Jose", -23.741355, -46.558855);

            banco.Add(marcel.id, marcel);
            banco.Add(maria.id, maria);
            banco.Add(joao.id, joao);
            banco.Add(pedro.id, pedro);
            banco.Add(jose.id, jose);

            banco2.Add(marcel.id, marcel);
            banco2.Add(maria.id, maria);
            banco2.Add(joao.id, joao);
            banco2.Add(pedro.id, pedro);
            banco2.Add(jose.id, jose);
        }

        /// <summary>
        /// Classe utlizada para listar amigo e validar token de acesso API
        /// </summary>
        /// <param Token de Validacao="id"></param>
        /// <returns></returns>
        public Amigos Busca(Int64 id)
        {
            Crypt vlToken = new Crypt();
            Amigos amigos = null;

            string token = id.ToString();
            string tokenBase = string.Empty;
            int tot = this.totalDados();
            int i = 1;

            while (i <= tot)
            {
                try
                {
                    amigos = this.Busca2(i);
                    tokenBase = vlToken.EncDec(amigos.Token, false);
                    if (tokenBase == token)
                    {
                        i = tot;
                        break;
                    }
                    i++;
                }
                catch
                {
                    continue;
                }
            }

            return banco[i];
        }
        public Amigos Busca2(Int64 id)
        {
            return banco2[id];
        }

        /// <summary>
        /// metodo para identificar quantos amigos estao cadastrados
        /// </summary>
        /// <returns>Total de amigos cadastrador</returns>
        public int totalDados()
        {
            return banco.Count;
        }
    }
}
