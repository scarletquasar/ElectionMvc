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
    public static class CandidatoManager
    {
        private static string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CandidatosDB;Integrated Security=True";

        public static string RegistrarCandidato(int legenda, string nome, string nomeVice, DateTime dataRegistro)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                String query = "INSERT INTO candidatos (legenda, data_de_registro, nome_do_vice, nome_completo)" +
                    $" VALUES ('{legenda}', '{dataRegistro}', '{nomeVice}', '{nome}');";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int r = command.ExecuteNonQuery();
                    if (r < 0) {connection.Close(); return ("Ocorreu um erro desconhecido."); }
                    else {connection.Close(); return ("Inserção realizada com sucesso");  }
                    
                }
            }
        }
        public static string ListaDeCandidatos()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM candidatos";
                string res = string.Empty;
                List<CandidatoModel> candidatos = new List<CandidatoModel>();

                connection.Open();
                using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            CandidatoModel candidato = new CandidatoModel();
                            candidato.nome_completo = reader[3].ToString();
                            candidato.nome_do_vice = reader[2].ToString();
                            candidato.data_de_registro = reader[1].ToString();
                            candidato.legenda = (int)reader[0];
                            candidatos.Add(candidato);
                        }
                        catch { }
                    }
                    connection.Close();
                    res = JsonConvert.SerializeObject(candidatos, Formatting.Indented);
                    return (String.Format("{0}", res));
                } 
            }
        }
        public static void ApagarCandidato(int legenda)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                String query = $"DELETE FROM candidatos WHERE legenda='{legenda}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int r = command.ExecuteNonQuery();
                }
            }
        }
    }
}
