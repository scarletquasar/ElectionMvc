using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using ProjetoHubCount.Models;

namespace ProjetoHubCount.Managers
{
    public static class VotoManager
    {
        private static string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CandidatosDB;Integrated Security=True";

        public static string VisualizarVotos()
        {
            string query = "SELECT * FROM candidatos JOIN votos ON candidatos.legenda = votos.legenda";
            string res = string.Empty;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                {

                    List<VotoModel> votos = new List<VotoModel>();
                    while (reader.Read())
                    {
                        try
                        {
                            VotoModel voto = new VotoModel();
                            voto.legenda = (int)reader[0];
                            voto.data_voto = reader[5].ToString();
                            voto.nome_vice = reader[2].ToString();
                            voto.nome_candidato = reader[3].ToString();
                            votos.Add(voto);
                        }
                        catch { }
                    }
                    connection.Close();
                    res = JsonConvert.SerializeObject(votos, Formatting.Indented);
                    connection.Close();
                    return (String.Format("{0}", res));
                }
            }
        }

        public static void Votar(int legenda)
        {
            bool candidatoExiste = false;
            string query = $"SELECT * FROM candidatos WHERE legenda={legenda}";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((int)reader[0] == legenda) { candidatoExiste = true; }
                    }
                }
                connection.Close();
            }

            if (candidatoExiste)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    String insert = $"INSERT INTO votos (legenda, data_voto) VALUES ('{legenda}', '{DateTime.Now}')";

                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        connection.Open();
                        int r = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
}
