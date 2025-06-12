using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
        }

        private void CreateChessBoard()
        {
            UniformGrid chessBoardUniformGrid = new()
            {
                Rows = 8,
                Columns = 8,
            };
            
            this.Content = chessBoardUniformGrid; //

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border square = new()
                    {
                        //Background = (row + col) % 2 != 0 ? Brushes.Green : Brushes.Gray, //will be removed and replaced with an actual picture of a chess board!
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black
                    };

                    if(row==1 || row == 6)
                    {
                        Image pawn = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row==6? "wp.png":"bp.png")}")),
                            Width = 50,
                            Height = 50
                        };
                        square.Child = pawn;
                    }

                    if((row==0 || row == 7) && (col==0 || col==7))
                    {
                        Image knight = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7? "wr.png" : "br.png")}")),
                            Width = 50,
                            Height = 50
                        };
                        square.Child = knight;
                    }

                    if ((row == 0 || row == 7) && (col == 1 || col == 6))
                    {
                        Image knight = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wn.png" : "bn.png")}")),
                            Width = 50,
                            Height = 50
                        };
                        square.Child = knight;
                    }

                    if ((row == 0 || row == 7) && col == 4)
                    {
                        Image king = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wk.png" : "bk.png")}")),
                            Width = 50,
                            Height = 50
                        };
                        square.Child = king;
                    }

                    if ((row == 0 || row == 7) && (col == 2 || col == 5))
                    {
                        Image bishop = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wb.png" : "bb.png")}")),
                            Width = 50,
                            Height = 50
                        };
                        square.Child = bishop;
                    }

                    if ((row == 0 || row == 7) && col == 3)
                    {
                        Image queen = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wq.png" : "bq.png")}")),
                            Width =50,
                            Height = 50
                        };
                        square.Child = queen;
                    }

                    chessBoardUniformGrid.Children.Add(square);
                }
            }
        }
    }
}