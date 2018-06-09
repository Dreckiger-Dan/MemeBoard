using mrousavy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MemeBoard
{

    public class Meme
    {

        public string name = "placeholder";
        public Key key;
        public ModifierKeys modifierKeys;
        public string imageSrc = "memes";
        public bool isAnimated;

        [XmlIgnore]
        public bool isRotating;

        public void Show(MainWindow window)
        {
            var img = new BitmapImage(new Uri(imageSrc));
            if (isAnimated)
                ImageBehavior.SetAnimatedSource(window.image, img);
            else
                window.image.Source = img;
        }

        public void Hide(MainWindow window)
        {
            ImageBehavior.SetAnimatedSource(window.image, null);
            window.image.Source = null;
        }
    }
}
