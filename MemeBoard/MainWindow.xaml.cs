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

namespace MemeBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard sb => (Storyboard)this.Resources["imageRotationStoryboard"];
        private Memes memes;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            memes = new Memes();
            memes.window = this;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (memes.CurrentMeme.isRotating)
            {
                sb.Stop();
                memes.CurrentMeme.isRotating = false;
            }
            else
            {
                sb.Begin();
                memes.CurrentMeme.isRotating = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            memes.LoadMemes();
        }
    }
}
