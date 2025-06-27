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
    internal class Piece : Image
    {
        public char PieceColor { get; set; }
        public string Type { get; set; }

        public Piece(string imageSource, string type)
        {
            this.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{imageSource}"));
            PieceColor = imageSource[0];
            Type = type;
        }
    }
}
