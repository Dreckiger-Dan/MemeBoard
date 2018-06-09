using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace MemeBoard
{
    public class Memes : List<Meme>
    {
        private const string memeSettings = "memes.xml";
        private Meme currentMeme;

        private XmlSerializer serializer = new XmlSerializer(typeof(Memes));

        [XmlIgnore]
        public MainWindow window { get; set; }
        [XmlIgnore]
        public Meme CurrentMeme { get { return currentMeme; } }

        public void ChangeMeme(Meme meme)
        {
            if (currentMeme == meme)
            {
                currentMeme.Hide(window);
                window.WindowState = WindowState.Minimized;
                currentMeme = null;
            }
            else
            {
                if (currentMeme != null)
                    currentMeme.Hide(window);

                currentMeme = meme;
                currentMeme.Show(window);
                window.WindowState = WindowState.Maximized;
                window.Activate();
            }

        }

        public void LoadMemes()
        {
            if (!File.Exists(memeSettings))
                CreateSettingsFile();

            var file = File.OpenRead(memeSettings);
            Clear();
            Memes fileMemes = (Memes)serializer.Deserialize(file);
            AddRange(fileMemes);

            foreach (Meme meme in this)
            {
                new mrousavy.HotKey(meme.modifierKeys, meme.key, window,
                    _ => ChangeMeme(meme));
            }
        }

        public void SaveMemes()
        {
            var file = File.OpenWrite(memeSettings);

            serializer.Serialize(file, this);
            file.Close();
        }

        private void CreateSettingsFile()
        {
            Add(new Meme());
            SaveMemes();
        }
    }
}
