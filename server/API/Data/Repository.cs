using System;
using Microsoft.Data.SqlClient;
using Flashcards.Models;

namespace Flashcards.Data
{
    public class Repository : IRepository
    {
        private readonly string connectionString;
        private readonly ILogger<Repository> _logger;

        public Repository(string connectionString, ILogger<Repository> logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<Flashcard>> ReadAllFlashcards()
        {
            List<Flashcard> allCards = new List<Flashcard>();

            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string cmdText = "SELECT * FROM cards";
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            Flashcard card;
            while (reader.Read())
            {
                card = new Flashcard(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetString(5));

                allCards.Add(card);
            }

            await connection.CloseAsync();

            _logger.LogInformation("Fetched all cards. Count: {count}", allCards.Count);

            return allCards;
        }

        public async Task InsertFlashcard(Flashcard newCard)
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

