using System.Diagnostics;
using System.Net.NetworkInformation;

// https://cardgames.io/reversi/#rules
// You do this by making a horizontal, vertical or diagonal line of pieces
namespace reversi_01
{
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
            for (int col = 0; col < board.ColumnCount; col++)
            {
                for (int row = 0; row < board.RowCount; row++)
                {
                    var tile = new Label
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Anchor = (AnchorStyles)0xF,
                        Margin = new Padding(1),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    board.Controls.Add(tile, col, row);
                    tile.MouseDown += legalMoveIterationStub;
                }
            }
        }
        const int DEMO_DELAY_MS = 100;
        private void legalMoveIterationStub(object? sender, EventArgs e)
        {
            clear();
            if(sender is Control control)
            {
                control.BackColor = Color.Blue;
                control.Refresh();
                var pos = board.GetCellPosition(control);
                var pt = new Point(pos.Column, pos.Row);
                Control ctrl;
                foreach (var point in CellsUp(pt))
                {
                    ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "U";
                    ctrl.Refresh();
                    Thread.Sleep(DEMO_DELAY_MS);
                }
                foreach (var point in CellsRight(pt, board.ColumnCount))
                {
                    ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "R";
                    ctrl.Refresh();
                    Thread.Sleep(DEMO_DELAY_MS);
                }
                foreach (var point in CellsDown(pt, board.ColumnCount))
                {
                    ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "D";
                    ctrl.Refresh();
                    Thread.Sleep(DEMO_DELAY_MS);
                }
                foreach (var point in CellsLeft(pt))
                {
                    ctrl = board.GetControlFromPosition(point.X, point.Y);
                    ctrl.Text = "L";
                    ctrl.Refresh();
                    Thread.Sleep(DEMO_DELAY_MS);
                }
            }
        }
        void clear()
        {
            foreach (Control control in board.Controls)
            {
                control.Text = string.Empty;
                control.BackColor = SystemColors.Control;
            }
            board.Refresh();
        }
        public IEnumerable<Point> CellsUp(Point point)
        {            
            while(true)
            {
                point = new Point(point.X, point.Y - 1);
                if (point.Y < 0) break;
                yield return point;
            }
        }
        public IEnumerable<Point> CellsRight(Point point, int max)
        {
            while (true)
            {
                point = new Point(point.X + 1, point.Y);
                if (point.X == max) break;
                yield return point;
            }
        }
        public IEnumerable<Point> CellsDown(Point point, int max)
        {
            while (true)
            {
                yield return point;
                point = new Point(point.X, point.Y + 1);
                if (point.Y == max) break;
            }
        }
        public IEnumerable<Point> CellsLeft(Point point)
        {
            while (true)
            {
                yield return point;
                point = new Point(point.X - 1, point.Y);
                if (point.X < 0) break;
            }
        }
    }
}