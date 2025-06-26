using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktop_Chess.Model;

namespace Desktop_Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point StartPoint {  get; set; } //check
        private Image? DraggedPiece { get; set; } //check
        private TranslateTransform Transform = new(); //check

        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
        }

        private void CreateChessBoard()
        {

            UniformGrid chessBoard = new()
            {
                Rows = 8,
                Columns = 8
            };

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border square = new()
                    {
                        Background =  new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/Assets/other/{((row + col) % 2 != 0 ?"ds.png":"ls.png")}"))), //(row + col) % 2 != 0 ? Brushes.Green : Brushes.Gray, //will be removed and replaced with an actual picture of a chess board!
                    };

                    if(row==1 || row == 6)
                    {
                        //Image pawn = new()
                        //{
                        //    Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row==6? "wp.png":"bp.png")}")),
                        //    Width = 50,
                        //    Height = 50,
                        //    Name = row == 6 ? "whitePawn":"blackPawn"
                        //};

                        string source = row == 6 ? "wp.png" : "bp.png";
                        char color = source[0];
                        string name = row == 6 ? "whitePawn" : "blackPawn";

                        Piece pawn = new(source, "pawn", color, name);
                        Control_Piece(pawn);
                        square.Name = "pawnSquare";
                        square.Child = pawn;
                    }

                    if((row==0 || row == 7) && (col==0 || col==7))
                    {
                        Image rook = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7? "wr.png" : "br.png")}")),
                            Width = 50,
                            Height = 50,
                            Name = row == 7 ? "whiteRook":"blackRook"
                        };
                        Control_Piece(rook); //or Attach_Controls(piece) ?
                        square.Name = "KnightSqaure";
                        square.Child = rook;
                    }

                    if ((row == 0 || row == 7) && (col == 1 || col == 6))
                    {
                        Image knight = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wn.png" : "bn.png")}")),
                            Width = 50,
                            Height = 50,
                            Name = row == 7 ? "whiteKnight":"blackKnight"

                        };

                        Control_Piece(knight);

                        square.Child = knight;
                    }

                    if ((row == 0 || row == 7) && col == 4)
                    {
                        Image king = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wk.png" : "bk.png")}")),
                            Width = 50,
                            Height = 50,
                            Name = row == 7 ? "whiteKing":"blackKing"
                        };

                        Control_Piece(king); //attaches event handlers for piece

                        square.Child = king;
                    }

                    if ((row == 0 || row == 7) && (col == 2 || col == 5))
                    {
                        Image bishop = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wb.png" : "bb.png")}")),
                            Width = 50,
                            Height = 50,
                            Name = row == 7 ? "whitebBishop":"blackBishop"
                        };

                        Control_Piece(bishop);

                        square.Child = bishop;
                    }

                    if ((row == 0 || row == 7) && col == 3)
                    {
                        Image queen = new()
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/pieces/{(row == 7 ? "wq.png" : "bq.png")}")),
                            Width = 50,
                            Height = 50,
                            Name = row == 7 ? "whiteQueen":"blackQueen"
                            
                        };


                        Control_Piece(queen);

                        square.Name = "queenSquare";
                        square.Child = queen;
                    }

                    Panel.SetZIndex(square, 0);
                    Add_Square_To_Grid(square, row, col);
                   
                }
            }
            //chessBoardGrid.ClipToBounds = true; // Ensures that the chessboard does not overflow its bounds
            //chessBoardGrid.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/other/board1.jpeg"))); // Background image for the chessboard
        }

        private void Add_Square_To_Grid(Border square, int row, int col) //will be removed when UniformGrid is used
        {
            chessBoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            chessBoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

            //species where to add the piece i.e. " <sqaure Grid.Row = row Grid.Column = column /> 
            square.SetValue(Grid.RowProperty, row);
            square.SetValue(Grid.ColumnProperty, col);
            chessBoardGrid.Children.Add(square);
        }

        private void Control_Piece(Image piece)
        {
            piece.MouseDown += Piece_MouseDown;
            piece.MouseMove += Piece_MouseMove;
            piece.MouseUp += Piece_MouseUp;
        }

        private void Piece_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = e.GetPosition(chessBoardGrid);
            DraggedPiece = (Image)sender;

            var parent = (Border)VisualTreeHelper.GetParent(DraggedPiece);
            Panel.SetZIndex(parent, 1);
            DraggedPiece.CaptureMouse();

            DraggedPiece.RenderTransform = Transform;
        }

        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && DraggedPiece != null)
            {
                Point currentPosition = e.GetPosition(null);
                Vector offset = currentPosition - StartPoint;
                Transform.X = offset.X;
                Transform.Y = offset.Y;
            }
        }

        private void Piece_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggedPiece != null) 
            {
                DraggedPiece.ReleaseMouseCapture();

                Point dropPosition = e.GetPosition(chessBoardGrid);

                foreach( UIElement element in chessBoardGrid.Children)
                {
                    if(element is Border border) //element selected by cursor is border (square)
                    {
                        Point sqaurePosition = border.TranslatePoint(new Point(0, 0), chessBoardGrid);
                        Rect sqaureRect = new(sqaurePosition.X, sqaurePosition.Y, border.ActualWidth, border.ActualHeight);
                        var capturedPiece = (Image) border.Child;
                        var parent = (Border)VisualTreeHelper.GetParent(DraggedPiece);


                        //if target sqaure is empty or contains enemy piece
                        bool canCapture = capturedPiece == null || capturedPiece.Name[0] != DraggedPiece.Name[0];

                        if (sqaureRect.Contains(dropPosition) && canCapture)
                        {
                            parent.Child = null;
                            border.Child = DraggedPiece;
                            break;
                        }
                        Panel.SetZIndex(parent, 0);
                    }
                }

                DraggedPiece.RenderTransform = null;
                DraggedPiece = null;

            }
        }
    }
}