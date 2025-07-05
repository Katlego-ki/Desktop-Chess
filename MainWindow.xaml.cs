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
using Desktop_Chess.View;

namespace Desktop_Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point StartPoint {  get; set; } //check
        private Piece? DraggedPiece { get; set; } //check
        private readonly TranslateTransform Transform = new(); //check
        readonly static UniformGrid chessBoard = new()
        {
            Rows = 8,
            Columns = 8,
        };

        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
        }

        private void CreateChessBoard()
        {
            this.Content = chessBoard;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border square = new()
                    {
                        Background =  new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/Assets/other/{((row + col) % 2 != 0 ?"ds.png":"ls.png")}")))
                    };

                    var piece = Add_Piece(row, col);

                    //if a piece was added
                    if (piece != null)
                    {
                        Control_Piece(piece);
                        square.Child = piece;
                    }

                    Panel.SetZIndex(square, 0);
                    chessBoard.Children.Add(square);
                }
            }
        }

        static Piece?  Add_Piece(int row, int col)
        {
            string? type = null;
            string? source = null;

            // ======== PAWN ========
            if (row == 1 || row == 6)
            {
                type = "pawn";
                source = row == 6 ? "wp.png" : "bp.png";
            }

            // ======== ROOK ========
            if ((row == 0 || row == 7) && (col == 0 || col == 7))
            {
                type = "rook";
                source = row == 7 ? "wr.png" : "br.png";
            }

            // ======== KNIGHT ========
            if ((row == 0 || row == 7) && (col == 1 || col == 6))
            {
                type = "knight";
                source = row == 7 ? "wn.png" : "bn.png";
            }

            // ======== KING ========
            if ((row == 0 || row == 7) && col == 4)
            {
                type = "king";
                source = row == 7 ? "wk.png" : "bk.png";
            }

            // ======== BISHOP ========
            if ((row == 0 || row == 7) && (col == 2 || col == 5))
            {
                type = "bishop";
                source = row == 7 ? "wb.png" : "bb.png";
            }

            // ======== QUEEN ========
            if ((row == 0 || row == 7) && col == 3)
            {
                type = "queen";
                source = row == 7 ? "wq.png" : "bq.png";
            }

            //If there is a piece to be added
            return (source != null && type != null)? new(source, type) : null;
        }

        private void Control_Piece(Piece piece)
        {
            piece.MouseDown += Piece_MouseDown;
            piece.MouseMove += Piece_MouseMove;
            piece.MouseUp += Piece_MouseUp;
        }

        private void Piece_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = e.GetPosition(chessBoard);
            DraggedPiece = (Piece)sender;

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

                Point dropPosition = e.GetPosition(chessBoard);

                foreach( UIElement element in chessBoard.Children)
                {
                    if(element is Border border) //element selected by cursor is border (square)
                    {
                        Point sqaurePosition = border.TranslatePoint(new Point(0, 0), chessBoard);
                        Rect sqaureRect = new(sqaurePosition.X, sqaurePosition.Y, border.ActualWidth, border.ActualHeight);
                        var capturedPiece = (Piece) border.Child;
                        var parent = (Border)VisualTreeHelper.GetParent(DraggedPiece);


                        //if target sqaure is empty or contains enemy piece
                        bool canCapture = capturedPiece == null || capturedPiece.Color != DraggedPiece.Color;

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