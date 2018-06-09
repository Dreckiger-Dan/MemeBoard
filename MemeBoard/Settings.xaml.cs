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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemeBoard
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private Memes memes = new Memes();
        private Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

        public Settings()
        {
            InitializeComponent();
            memes.MemesLoaded += Memes_MemesLoaded;
            dialog.Filter = "All images|*.jpeg;*.jpg;*.png;*.gif|JPEG Files (*.jpeg; *.jpg)|*.jpeg;*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            memes.LoadMemes(false);
        }

        private void Memes_MemesLoaded(object sender, EventArgs e)
        {
            memeSettings.Children.Clear();


            var panel = new WrapPanel();
            var name = new Label();
            var path = new Label();
            var animated = new Label();
            var shortcut = new Label();

            name.Content = "Name";
            path.Content = "Image";
            animated.Content = "Is Animated";
            shortcut.Content = "Shortcut";

            panel.Children.Add(name);
            panel.Children.Add(path);
            panel.Children.Add(animated);
            panel.Children.Add(shortcut);

            memeSettings.Children.Add(panel);

            foreach (Meme meme in memes)
            {
                AddMemeRow(meme);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            memes.SaveMemes();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var meme = new Meme();
            if (dialog.ShowDialog() == true)
            {
                meme.imageSrc = dialog.FileName;
                meme.name = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1).Split('.')[0];
            }
            AddMemeRow(meme);

            memes.Add(meme);
        }

        private void AddMemeRow(Meme meme)
        {
            var panel = new WrapPanel();
            var name = new TextBox();
            var path = new TextBox();
            var animated = new CheckBox();
            var shortcut = new TextBox();
            var remove = new Button();

            name.Text = meme.name;
            path.Text = meme.imageSrc;
            animated.IsChecked = meme.isAnimated;
            shortcut.Text = meme.modifierKeys + " + " + meme.key;
            remove.Content = "-";

            shortcut.IsEnabled = false;

            path.MouseDoubleClick += (s, e) =>
            {
                if (dialog.ShowDialog() == true)
                {
                    meme.imageSrc = dialog.FileName;
                    path.Text = dialog.FileName;
                }
            };

            shortcut.MouseDoubleClick += (s, e) =>
            {
                throw new NotImplementedException();
            };

            name.TextChanged += (s, e) => meme.name = name.Text;
            animated.Click += (s, e) => meme.isAnimated = animated.IsChecked == true;
            remove.Click += (s, e) =>
            {
                memeSettings.Children.Remove(panel);
                memes.Remove(meme);
            };

            panel.Children.Add(name);
            panel.Children.Add(path);
            panel.Children.Add(animated);
            panel.Children.Add(shortcut);
            panel.Children.Add(remove);

            memeSettings.Children.Add(panel);
        }
    }
}
