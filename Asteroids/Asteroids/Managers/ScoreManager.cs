using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Asteroids.Models;

namespace Asteroids.Managers
{
    public class ScoreManager
    {
        private static string fileName = "scores.xml";
        
        public List<Score> Highscores { get; private set; }
        public List<Score> Scores { get; private set; }

        public ScoreManager(List<Score> scores)
        {
            Scores = scores;
            UpdateHighscores();
        }
        
        public ScoreManager()
            : this(new List<Score>())
        {
        }

        public void Add(Score score)
        {
            Scores.Add(score);
            Scores = Scores.OrderByDescending(c => c.Value).ToList();
            UpdateHighscores();
        }

        public void UpdateHighscores()
        {
            Highscores = Scores.Take(10).ToList();
        }

        public static void Save(ScoreManager scoreManager)
        {
            using(var writer = new StreamWriter(new FileStream(fileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));
                serializer.Serialize(writer, scoreManager.Scores);
            }
        }

        public static ScoreManager Load()
        {
            if(!File.Exists(fileName))
                return new ScoreManager();

            using(var reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));
                var scores = serializer.Deserialize(reader) as List<Score>;
                return new ScoreManager(scores);
            }
        }
    }
}