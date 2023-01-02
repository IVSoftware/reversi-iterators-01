using System.Diagnostics;
using System.Net.NetworkInformation;

namespace reversi_01
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
            for (int col = 0; col < board.ColumnCount; col++)
            {
                for (int row = 0; row < board.ColumnCount; row++)
                {
                    var tile = new Label
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Anchor = (AnchorStyles)0xF,
                        Margin = new Padding(1)
                    };
                    board.Controls.Add(tile, col, row);
                    tile.MouseDown += legalMove;
                }
            }
        }
        private void legalMove(object? sender, EventArgs e)
        {
            if(sender is Control control)
            {
                var pos = board.GetCellPosition(control);
                var pt = new Point(pos.Column, pos.Row);

                foreach (var point in Up(pt))
                {
                    var ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "U";
                    ctrl.Refresh();
                }
                Thread.Sleep(100);
                foreach (var point in Right(pt, board.ColumnCount))
                {
                    var ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "R";
                    ctrl.Refresh();
                }
            }
        }
        public IEnumerable<Point> Up(Point point)
        {            
            while(true)
            {
                yield return point;
                point = new Point(point.X, point.Y - 1);
                if (point.Y < 0) break;
            }
        }
        public IEnumerable<Point> Right(Point point, int max)
        {
            while (true)
            {
                yield return point;
                point = new Point(point.X + 1, point.Y);
                if (point.X == max) break;
            }
        }
    }
}