using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Globalization;

namespace Contactos.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        public static async Task<List<Persona>> ObtenerTodas(string connectionString)
        {
            var lista = new List<Persona>();

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT Id, Nombre, Telefono FROM Personas";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new Persona
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Telefono = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return lista;

        }

        public static async Task<bool> Insertar(string connectionString, Persona nuevaPersona) {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Personas (Nombre, Telefono) VALUES (@Nombre, @Telefono)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nuevaPersona.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono", nuevaPersona.Telefono);
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}


