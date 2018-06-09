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

        private Meme currentMeme { get; set; }
        private XmlSerializer serializer = new XmlSerializer(typeof(Memes));

        public event EventHandler MemesLoaded;

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
            LoadMemes(true);
        }

        public void LoadMemes(bool registerHotkeys)
        {
            if (!File.Exists(memeSettings))
                CreateSettingsFile();

            var file = File.OpenRead(memeSettings);
            Clear();

            try
            {
                Memes fileMemes = (Memes)serializer.Deserialize(file);
                AddRange(fileMemes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"memes.xml contains errors. Please delete it or correct the errors.\n{ex.Message}");
            }

            if (registerHotkeys)
            {
                foreach (Meme meme in this)
                {
                    new mrousavy.HotKey(meme.modifierKeys, meme.key, window,
                        _ => ChangeMeme(meme));
                }
            }

            MemesLoaded.Invoke(this, new EventArgs());
        }

        public void SaveMemes()
        {
            var file = File.Open(memeSettings, FileMode.Create);

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
