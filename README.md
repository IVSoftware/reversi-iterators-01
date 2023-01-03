Your question states that the function you wrote to scan the surrounding cells is failing sporadically and that it's difficult to diagnose. It was easy to reproduce the failures by running your code, but I wasn't able to see an obvious way to effectively debug it.

It "might" be more effective to _improve the algorithm_ where it's more methodical in how it inspects the surrounding cells in the first place, which would also be easier to debug if necessary. One solid way to do this would be to use custom [Iterators](https://learn.microsoft.com/en-us/dotnet/csharp/iterators#enumeration-sources-with-iterator-methods) where you could use a standard `foreach` pattern to inspect virtual "lines" radiating in the eight directions. At each 'yield' you can check to see whether a determination can be made in terms of either a "legal move" or a "capture".

***
Here's a proof-of-concept grid that is intended to demonstrate how the iterators work. It _doesn't_ evaluate the cells in terms of game play in any way but you can see how it would lend itself to doing that. The idea here is to click any cell and observe the markup of U-R-D-L. It may also help to see it working so you can [clone](https://github.com/IVSoftware/reversi-iterators-01.git) this sample and set breakpoints.

[![iterating from points][1]][1]

***
Left, Right, Up, Down iterator examples are shown - diagonals would follow the same pattern. The mouse down control passes the starting cell coordinate position as a Point:


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
The demo board has been mocked like this for testing purposes:

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


  [1]: https://i.stack.imgur.com/REaTB.png