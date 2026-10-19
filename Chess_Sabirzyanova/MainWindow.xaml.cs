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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_Sabirzyanova
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public List<Chess.Classes.Pawn> Pawns = new List<Chess.Classes.Pawn>();
        public List<Chess.Classes.Rook> Rooks = new List<Chess.Classes.Rook>();
        public MainWindow()
        {
            InitializeComponent();
            MainWindow.mainWindow = this;
            Pawns.Add(new Chess.Classes.Pawn(0, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(1, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(2, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(3, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(4, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(5, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(6, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(7, 1, false));
            Pawns.Add(new Chess.Classes.Pawn(0, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(1, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(2, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(3, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(4, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(5, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(6, 6, true));
            Pawns.Add(new Chess.Classes.Pawn(7, 6, true));
            CreateFigure();
            Rooks.Add(new Chess.Classes.Rook(0, 0, false)); 
            Rooks.Add(new Chess.Classes.Rook(7, 0, false)); 
            Rooks.Add(new Chess.Classes.Rook(0, 7, true));  
            Rooks.Add(new Chess.Classes.Rook(7, 7, true));  
            CreateRooks();
        }

        public void CreateFigure()
        {
            foreach (Chess.Classes.Pawn Pawn in Pawns)
            {
                Pawn.Figure = new Grid()
                {
                    Width = 50,
                    Height = 50
                };
                if (Pawn.Black)
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn (black).png")));
                else
                    Pawn.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Pawn.png")));
                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }
        }
        public void CreateRooks()
        {
            foreach (Chess.Classes.Rook rook in Rooks)
            {
                rook.Figure = new Grid() { Width = 50, Height = 50 };

                if (rook.Black)
                    rook.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook (black).png")));
                else
                    rook.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook.png")));
                Grid.SetColumn(rook.Figure, rook.X);
                Grid.SetRow(rook.Figure, rook.Y);
                rook.Figure.MouseDown += rook.SelectFigure;
                gameBoard.Children.Add(rook.Figure);
            }
        }

        public void OnSelect(Chess.Classes.Pawn SelectPawn)
        {
            foreach (Chess.Classes.Pawn Pawn in Pawns)
            {
                if (Pawn != SelectPawn)
                    if (Pawn.Select)
                        Pawn.SelectFigure(null, null);
            }
        }

        private void SelectTile(object sender, MouseButtonEventArgs e)
        {
            Grid Tile = sender as Grid;
            int X = Grid.GetColumn(Tile);
            int Y = Grid.GetRow(Tile);
            Chess.Classes.Pawn SelectPawn = Pawns.Find(x => x.Select == true);
            Chess.Classes.Rook SelectRook = Rooks.Find(x => x.Select == true);
            if (SelectPawn != null)
            {
                SelectPawn.Transform(X, Y);
                ClearBoardColors();
            }
            else if (SelectRook != null)
            {
                SelectRook.Transform(X, Y);
                ClearBoardColors();
            }
        }

        public void HighlightMoves()
        {
            ClearBoardColors();
            Chess.Classes.Pawn selectPawn = Pawns.Find(x => x.Select == true);
            Chess.Classes.Rook selectRook = Rooks.Find(x => x.Select == true);
            if (selectPawn == null && selectRook == null) return;
            foreach (var child in gameBoard.Children)
            {
                if (child is Grid tile)
                {
                    Chess.Classes.Pawn isPawnElement = Pawns.Find(p => p.Figure == tile);
                    Chess.Classes.Rook isRookElement = Rooks.Find(r => r.Figure == tile);
                    if (isPawnElement != null || isRookElement != null) continue;
                    int tileX = Grid.GetColumn(tile);
                    int tileY = Grid.GetRow(tile);
                    bool canMove = false;
                    if (selectPawn != null)
                    {
                        if (selectPawn.X == tileX)
                        {
                            if (!selectPawn.Black && ((selectPawn.Y == 1 && selectPawn.Y + 2 == tileY) || selectPawn.Y + 1 == tileY)) canMove = true;
                            if (selectPawn.Black && ((selectPawn.Y == 6 && selectPawn.Y - 2 == tileY) || selectPawn.Y - 1 == tileY)) canMove = true;
                        }
                        if (Math.Abs(selectPawn.X - tileX) == 1)
                        {
                            if (!selectPawn.Black && selectPawn.Y + 1 == tileY) canMove = true;
                            if (selectPawn.Black && selectPawn.Y - 1 == tileY) canMove = true;
                        }
                    }
                    else if (selectRook != null)
                    {
                        if (selectRook.X == tileX || selectRook.Y == tileY) canMove = true;
                    }
                    if (canMove)
                    {
                        Chess.Classes.Pawn pawnOnTile = Pawns.Find(p => p.X == tileX && p.Y == tileY);
                        Chess.Classes.Rook rookOnTile = Rooks.Find(r => r.X == tileX && r.Y == tileY);
                        bool isBlackFigure = selectPawn != null ? selectPawn.Black : selectRook.Black;
                        if (pawnOnTile != null)
                        {
                            if (pawnOnTile.Black != isBlackFigure) tile.Background = Brushes.Red;
                        }
                        else if (rookOnTile != null)
                        {
                            if (rookOnTile.Black != isBlackFigure) tile.Background = Brushes.Red;
                        }
                        else
                        {
                            if (selectPawn != null && selectPawn.X != tileX) continue;
                            tile.Background = Brushes.Green;
                        }
                    }
                }
            }
        }

        public void ClearBoardColors()
        {
            foreach (var child in gameBoard.Children)
            {
                if (child is Grid tile)
                {
                    Chess.Classes.Pawn isPawn = Pawns.Find(p => p.Figure == tile);
                    Chess.Classes.Rook isRook = Rooks.Find(r => r.Figure == tile);
                    if (isPawn != null || isRook != null) continue;
                    int x = Grid.GetColumn(tile);
                    int y = Grid.GetRow(tile);
                    if ((x + y) % 2 == 0)
                        tile.Background = Brushes.White;
                    else
                        tile.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF9A6400");
                }
            }
        }
    }
}