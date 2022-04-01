/* UserInterface.cs
 * Authors: Josh Weese and Rod Howell
 * Modified By: Gabriel Whitehair
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Ksu.Cis300.Checkers
{
    /// <summary>
    /// A GUI for a checkers-playing program.
    /// </summary>
    public partial class UserInterface : Form
    {
        /// <summary>
        /// The size of an individual board square in pixels.
        /// </summary>
        private const int _squareSize = 60;
        /// <summary>
        /// The current game.
        /// </summary>
        private Game _game = new Game();
        /// <summary>
        /// Image for the white pawn checker piece
        /// </summary>
        private Image _whitePawn = Image.FromFile(@"..\..\pics\white-pawn-modified.png");
        /// <summary>
        /// Image for the white king checker piece
        /// </summary>
        private Image _whiteKing = Image.FromFile(@"..\..\pics\white-king-modified.png");
        /// <summary>
        /// Image for the black pawn checker piece
        /// </summary>
        private Image _blackPawn = Image.FromFile(@"..\..\pics\black-pawn-modified.png");
        /// <summary>
        /// Image for the black king checker piece
        /// </summary>
        private Image _blackKing = Image.FromFile(@"..\..\pics\black-king-modified.png");
        /// <summary>
        /// New game dialog 
        /// </summary>
        private uxNewGameDialog _newGameDialog = new uxNewGameDialog();
        /// <summary>
        /// Initializes the checkers game
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();
            DrawBoard();
        }
        /// <summary>
        /// Draws the initial game board.
        /// </summary>
        private void DrawBoard()
        {
            //setup the panel
            uxBoard.Controls.Clear();
            uxBoard.Width = _squareSize * 8;
            uxBoard.Height = uxBoard.Width;
            Padding p = new Padding();
            p.Left = p.Right = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Label square = new Label();
                    square.Width = square.Height = _squareSize;
                    square.Margin = p;
                    square.Text = x + "," + y;
                    square.Click += new EventHandler(BoardSquare_Click);
                    uxBoard.Controls.Add(square);
                }
            }
            RedrawBoard(500);
        }

        /// <summary>
        /// Gets the checker image to display in the square at the given location,
        /// or null if the square is to be empty.
        /// </summary>
        /// <param name="loc">The location of the square on the board.</param>
        /// <returns>The image to display in that square, or null if the square is to
        /// be empty.</returns>
        private Image GetImage(Point loc)
        {
            switch (_game.GetContents(loc))
            {
                case SquareContents.BlackKing:
                    return _blackKing;
                case SquareContents.BlackPawn:
                    return _blackPawn;
                case SquareContents.WhiteKing:
                    return _whiteKing;
                case SquareContents.WhitePawn:
                    return _whitePawn;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Updates the GUI to match the current game position.
        /// </summary>
        private void RedrawBoard(int milliseconds) // The directions say to implement this int parameter
        {
            foreach (Label square in uxBoard.Controls)
            {
                Point loc = GetPoint(square.Text);
                square.Image = GetImage(loc);
                if (_game.IsSelected(loc))
                {
                    square.BackColor = Color.Gold;
                }
                else if ((loc.X + loc.Y) % 2 == 0)
                {
                    square.BackColor = Color.Beige;
                }
                else
                {
                    square.BackColor = Color.OliveDrab;
                }
                if (_game.IsOver)
                {
                    uxStatus.Text = "Game over.";
                }
                else
                {
                    uxStatus.Text = _game.CurrentName + "'s turn.";
                }
            }
            Refresh();                
            Thread.Sleep(milliseconds);
        }
        /// <summary>
        /// Plays computers move it is the computers turn
        /// </summary>
        public void TryComputerPlay() 
        {
            if (!_game.IsOver)
            {
                while (_game.CurrentName.Equals(_newGameDialog.ComputerPlayer))
                {
                    _game.MakeBestMove(_newGameDialog.level);
                    RedrawBoard(500);
                }
            }
        }
        /// <summary>
        /// Click event handler for when a board square is clicked.  
        /// It will attempt to make a move using the selected square through the game object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoardSquare_Click(object sender, EventArgs e)
        {
            if (!_game.IsOver)
            {
                Point loc = GetPoint(((Label)sender).Text);
                if (_game.SelectOrMove(loc))
                {
                    RedrawBoard(500);
                    TryComputerPlay();
                    CheckWin();
                }
                else
                {
                    MessageBox.Show("Invalid move.");
                }
            }
        }
        /// <summary>
        /// If the game is over, displays the appropriate message.
        /// </summary>
        private void CheckWin()
        {
            if (_game.IsOver)
            {
                MessageBox.Show(_game.OtherName + " wins!");
            }
        }
        /// <summary>
        /// Converts the name of a square to a Point.
        /// </summary>
        /// <param name="squareName">The name of a square (i.e., "x,y")</param>
        /// <returns>The equivalent Point</returns>
        private Point GetPoint(string squareName)
        {
            string[] fields = squareName.Split(',');
            int x = Convert.ToInt32(fields[0]);
            int y = Convert.ToInt32(fields[1]);
            return new Point(x, y);
        }
        /// <summary>
        /// Manages the new game dialog
        /// </summary>
        /// <returns>Bool indicating whether the user started a new game</returns>
        private bool NewGame()
        {
            if (_newGameDialog.ShowDialog() == DialogResult.OK)
            {
                _game = new Game();
                Show();
                RedrawBoard(500);
                uxUndo.Enabled = _newGameDialog.ComputerPlayer.Equals("");
                TryComputerPlay();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Click event handler for the new game menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxNew_Click(object sender, EventArgs e)
        {
            NewGame();
            RedrawBoard(500);
        }
        /// <summary>
        /// Overrides the OnLoad method 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!NewGame())
            {
                Application.Exit();
            }
        }
        /// <summary>
        /// Click event handler for the undo menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxUndo_Click(object sender, EventArgs e)
        {
            if (_game.Undo())
                RedrawBoard(0);
        }
        
    }
}
