using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Desktop_Chess.Model
{
    internal sealed class Piece : Image
    {
        public char Color { get; set; }
        public string Type { get; set; }

        public Piece(string imageSource, string type)
        {
            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{imageSource}"));
            Color = imageSource[0];
            Type = type;
        }
    }
}
