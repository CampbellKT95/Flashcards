using System;
using System.Net.Http;

namespace User.Tools
{
    public class WebScrapper
    {
        private readonly string jishoUrl = "https://jisho.org/search/";

        public async Task<string[]> ScrapSentence(string word)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync($"{jishoUrl}/{word}%23sentences");

            string htmlContent = await response.Content.ReadAsStringAsync();

            // find first three instances of japanese_sentence, which indicates the beginning of a jp sentence
            int[] allSentenceStarts = new int[3];
            int lastStartPosition = 0;

            for (int i = 0; i < 3; i++)
            {
                // search for the next sentence beginning from the last found sentence
                int sentenceStart = htmlContent.IndexOf("japanese_sentence", lastStartPosition + 1);
                lastStartPosition = sentenceStart;

                allSentenceStarts[i] = sentenceStart;
            }

            // find the first three instances of english_sentence, which indicates the beginning of an EN sentence (and thus the end of the coorsponding jp sentence)
            int[] allSentenceEnds = new int[3];
            int lastEndPosition = 0;
            for (int i = 0; i < 3; i++)
            {
                int sentenceEnd = htmlContent.IndexOf("english_sentence", lastEndPosition + 1);
                lastEndPosition = sentenceEnd;

                allSentenceEnds[i] = sentenceEnd;
            }

            // break into smaller snippts
            string[] allSentences = new string[3];
            for (int i = 0; i < 3; i++)
            {
                int snippetLength = allSentenceEnds[i] - allSentenceStarts[i];
                string snippet = htmlContent.Substring(allSentenceStarts[i], snippetLength);

                allSentences[i] = snippet;
            }

            // all kanji are fixed in a <span class="unlinked">
            string[] parsedSentences = new string[3];
            for (int i = 0; i < 3; i++)
            {
                string sentence = "";

                string[] dividedSentence = allSentences[i].Split(">");
                // immediately following "unlinked", grab everything up until '<' in the following iteration
                bool willBeKanji = false;
                foreach(string s in dividedSentence)
                {
                    if (willBeKanji)
                    {
                        int openTagIndex = s.IndexOf("<");
                        string kanji = s.Substring(0, openTagIndex);

                        sentence += kanji;

                        willBeKanji = false;
                    }

                    if (s.Contains("unlinked"))
                    {
                        willBeKanji = true;
                    }
                }

                parsedSentences[i] = sentence;
            }

            return parsedSentences;
        }
    }
}

