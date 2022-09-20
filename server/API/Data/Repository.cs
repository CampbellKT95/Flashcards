using System;
using Microsoft.Data.SqlClient;

namespace API.Data
{
    public class Repository
    {
        private readonly string? connectionString;
        private readonly ILogger<Repository> _logger;

        public Repository(ILogger<Repository> logger)
        {
            connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");
            _logger = logger;
        }

        public async Task InsertFlashcard()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string cmdText = "INSERT INTO cards (Word, Furigana, Definition, Example, Difficulty, Last_Reviewed) VALUES (@Word, @Furigana, @Definition, @Example, @Difficulty, @Last_Reviewed)";
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            // giving default values for now
            cmd.Parameters.AddWithValue("@Word", "魚");
            cmd.Parameters.AddWithValue("@Furigana", "さかな");
            cmd.Parameters.AddWithValue("@Definition", "Fish");
            cmd.Parameters.AddWithValue("@Example", "これは魚ですか？");
            cmd.Parameters.AddWithValue("@Difficulty", 1);
            cmd.Parameters.AddWithValue("@Last_Reviewed", "09/20/2022");

            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            _logger.LogInformation("Inserted new card");
        }
    }
}

