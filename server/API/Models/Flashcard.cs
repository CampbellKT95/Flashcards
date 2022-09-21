using System;

namespace Flashcards.Models
{
    public class Flashcard
    {
        public string Word { get; set; }
        public string Furigana { get; set; }
        public string Definition { get; set; }
        public string Example { get; set; }
        public int Difficulty { get; set; }
        public string Last_Reviewed { get; set; }

        public Flashcard(string Word, string Furigana, string Definition, string Example, int Difficulty, string Last_Reviewed)
        {
            this.Word = Word;
            this.Furigana = Furigana;
            this.Definition = Definition;
            this.Example = Example;
            this.Difficulty = Difficulty;
            this.Last_Reviewed = Last_Reviewed;
        }
    }
}

