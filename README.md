You're having sporadic issues in your search algorithm and they're difficult to diagnose. The algorithm could be improved to be more methodical in how it inspects the surrouding cells. One solid way to do this is using custom [Iterators](https://learn.microsoft.com/en-us/dotnet/csharp/iterators#enumeration-sources-with-iterator-methods) capable of scanning virtual "lines" radiating in the eight directions. Using the standard `foreach` pattern, the cells are inspected at each 'yield' to see whether a determination can be made in terms of either a "legel move" or a "capture".

This grid is intended to demonstrate how the iterators work and _doesn't_ evaluate the cells in terms of game play in any way. The idea hear is to click any cell and observe the markup of U-R-D-L. It may help to see it in action and you can [clone]() this sample to try it.


The first four iterators might look like this, taking the mouse down position as the start:


    public IEnumerable<Point> CellsUp(Point point)
    {
        while (true)
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

***
The code lays the groundwork for a methodical scan outward from any given point.

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

***
Where the demo board has been mocke like this for testing purposes:
using System.Diagnostics;
using System.Net.NetworkInformation;

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
                        Margin = new Padding(1),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    board.Controls.Add(tile, col, row);
                    tile.MouseDown += legalMove;
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
        .
        .
        .
    }

