using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chess_Sabirzyanova;

namespace Chess.Classes
{
    public class Rook
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Select = false;
        public bool Black = false;
        public Grid Figure { get; set; }

        public Rook(int X, int Y, bool Black)
        {
            this.X = X;
            this.Y = Y;
            this.Black = Black;
        }
        public void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            bool atack = false;
            Rook SelectRook = MainWindow.mainWindow.Rooks.Find(x => x.Select == true);
            Pawn SelectPawn = MainWindow.mainWindow.Pawns.Find(x => x.Select == true);
            if (SelectRook != null)
            {
                if (SelectRook.Black != this.Black && (SelectRook.X == this.X || SelectRook.Y == this.Y))
                {
                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    MainWindow.mainWindow.Rooks.Remove(this);
                    Grid.SetColumn(SelectRook.Figure, this.X);
                    Grid.SetRow(SelectRook.Figure, this.Y);
                    SelectRook.X = this.X;
                    SelectRook.Y = this.Y;
                    SelectRook.SelectFigure(null, null); 
                    atack = true;
                }
            }
            else if (SelectPawn != null)
            {
                if (SelectPawn.Black != this.Black)
                {
                    if ((SelectPawn.Black && this.Y == SelectPawn.Y - 1 && (this.X == SelectPawn.X - 1 || this.X == SelectPawn.X + 1)) ||
                        (!SelectPawn.Black && this.Y == SelectPawn.Y + 1 && (this.X == SelectPawn.X - 1 || this.X == SelectPawn.X + 1)))
                    {
                        MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                        MainWindow.mainWindow.Rooks.Remove(this);
                        Grid.SetColumn(SelectPawn.Figure, this.X);
                        Grid.SetRow(SelectPawn.Figure, this.Y);
                        SelectPawn.X = this.X;
                        SelectPawn.Y = this.Y;
                        SelectPawn.SelectFigure(null, null); 
                        atack = true;
                    }
                }
            }
            if (!atack)
            {
                MainWindow.mainWindow.OnSelect(null);
                foreach (Rook rook in MainWindow.mainWindow.Rooks)
                {
                    if (rook != this && rook.Select)
                    {
                        if (rook.Black)
                            rook.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook (black).png")));
                        else
                            rook.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook.png")));
                        rook.Select = false;
                    }
                }

                if (this.Select)
                {
                    if (this.Black)
                        this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook (black).png")));
                    else
                        this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook.png")));
                    this.Select = false;
                    MainWindow.mainWindow.ClearBoardColors();
                }
                else
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/Rook (select).png")));
                    this.Select = true;
                    MainWindow.mainWindow.HighlightMoves(); 
                }
            }
        }
        public void Transform(int X, int Y)
        {
            if (X != this.X && Y != this.Y)
            {
                SelectFigure(null, null);
                return;
            }
            Grid.SetColumn(this.Figure, X);
            Grid.SetRow(this.Figure, Y);
            this.X = X;
            this.Y = Y;
            SelectFigure(null, null);
            MainWindow.mainWindow.ClearBoardColors();
        }
    }
}
