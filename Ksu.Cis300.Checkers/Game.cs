/*
 * Game.cs
 * Authors: Josh Weese and Rod Howell
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Ksu.Cis300.Checkers
{
    /// <summary>
    /// This class contains the logic for the game of Checkers
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The board.
        /// </summary>
        private Checker[,] _board = new Checker[8, 8];

        /// <summary>
        /// Gets whether there is a selected square.
        /// </summary>
        private bool _squareIsSelected;

        /// <summary>
        /// The currently-selected square. 
        /// </summary>
        private Point _selectedSquare;

        /// <summary>
        /// The player moving the white pieces.
        /// </summary>
        private Player _whitePlayer = new Player(1, "White");

        /// <summary>
        /// The player moving the black pieces.
        /// </summary>
        private Player _blackPlayer = new Player(-1, "Black");

        /// <summary>
        /// The player whose turn it currently is.
        /// </summary>
        private Player _currentPlayer;

        /// <summary>
        /// The player whose turn it is not.
        /// </summary>
        private Player _otherPlayer;

        /// <summary>
        /// The moves that are currently legal.
        /// </summary>
        private Stack<Move> _legalMoves = new Stack<Move>();

        /// <summary>
        /// The directions a king can move.
        /// </summary>
        private Point[] _allDirections = { new Point(-1, -1), new Point(-1, 1), new Point(1, -1), new Point(1, 1) };

        /// <summary>
        /// The history of moves.
        /// </summary>
        private Stack<Move> _moveHistory = new Stack<Move>();

        /// <summary>
        /// Whose turn it is.
        /// </summary>
        public string CurrentName => _currentPlayer.Name;

        /// <summary>
        /// Whose turn it is not.
        /// </summary>
        public string OtherName => _otherPlayer.Name;

        /// <summary>
        /// Gets whether the game is over.
        /// </summary>
        public bool IsOver => _legalMoves.Count == 0;

        /// <summary>
        /// Gets the contents of the given square.
        /// </summary>
        /// <param name="square">The square to check.</param>
        /// <returns>The contents of square.</returns>
        public SquareContents GetContents(Point square)
        {
            if (square.X < 0 || square.X >= 8 || square.Y < 0 || square.Y >= 8)
            {
                return SquareContents.Invalid;
            }
            Checker checker = _board[square.X, square.Y];
            if (checker == null)
            {
                return SquareContents.None;
            }
            if (checker.Owner == _whitePlayer)
            {
                if (checker.IsKing)
                {
                    return SquareContents.WhiteKing;
                }
                else
                {
                    return SquareContents.WhitePawn;
                }
            }
            else
            {
                if (checker.IsKing)
                {
                    return SquareContents.BlackKing;
                }
                else
                {
                    return SquareContents.BlackPawn;
                }
            }
        }

        /// <summary>
        /// Gets the valid directions for the current player assuming the
        /// given type of piece.
        /// </summary>
        /// <param name="isKing">Whether the piece to be moved is a king.</param>
        /// <returns>The valid directions the piece can move.</returns>
        private Point[] GetValidDirections(bool isKing)
        {
            if (isKing)
            {
                return _allDirections;
            }
            else
            {
                return _currentPlayer.PawnDirections;
            }
        }

        /// <summary>
        /// Adds to _legalMoves the non-jumps from the given location.
        /// </summary>
        /// <param name="loc">The location of the checker to move.</param>
        private void GetNonJumpsFrom(Point loc)
        {
            Checker checker = _board[loc.X, loc.Y];
            Point[] dirs = GetValidDirections(checker.IsKing);
            foreach (Point d in dirs)
            {
                Point target = new Point(loc.X + d.X, loc.Y + d.Y);
                if (GetContents(target) == SquareContents.None)
                {
                    _legalMoves.Push(new Move(loc, target, checker.IsKing, _currentPlayer, _otherPlayer, _legalMoves));
                }
            }
        }

        /// <summary>
        /// Checks whether a jump can be made from the given start square in the given
        /// direction. The only checks made are whether the first square contains an enemy
        /// piece and the second square is a valid empty square.
        /// </summary>
        /// <param name="startSquare">Where the player's piece starts</param>
        /// <param name="dir">The direction the player's piece is moving</param>
        /// <returns>Whether or not the jump can be made</returns>
        private bool CanJump(Point startSquare, Point dir)
        {
            if (GetContents(new Point(startSquare.X + 2 * dir.X, startSquare.Y + 2 * dir.Y)) ==
                SquareContents.None)
            {
                Point enemySquare = new Point(startSquare.X + dir.X, startSquare.Y + dir.Y);
                Checker enemyPiece = _board[enemySquare.X, enemySquare.Y];
                return enemyPiece != null && enemyPiece.Owner == _otherPlayer;
            }
            return false;
        }

        /// <summary>
        /// Adds to _legalMoves the jump moves from the given location.
        /// </summary>
        /// <param name="loc">The location of the piece making the move.</param>
        private void GetJumpsFrom(Point loc)
        {
            bool isKing = _board[loc.X, loc.Y].IsKing;
            Point[] dirs = GetValidDirections(isKing);
            foreach (Point d in dirs)
            {
                if (CanJump(loc, d))
                {
                    Point capLoc = new Point(loc.X + d.X, loc.Y + d.Y);
                    Checker cap = _board[capLoc.X, capLoc.Y];
                    _legalMoves.Push(new Move(loc, new Point(loc.X + 2 * d.X, loc.Y + 2 * d.Y), isKing, _currentPlayer,
                        _otherPlayer, _legalMoves, cap, capLoc));
                }
            }
        }

        /// <summary>
        /// Adds to _legalMoves the moves of the given type from all of the current player's pieces.
        /// <param name="jumps">Indicates whether jump moves are to be found.</param>
        /// </summary>
        private void GetMoves(bool jumps)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Checker checker = _board[x, y];
                    if (checker != null && checker.Owner == _currentPlayer)
                    {
                        Point loc = new Point(x, y);
                        if (jumps)
                        {
                            GetJumpsFrom(loc);
                        }
                        else
                        {
                            GetNonJumpsFrom(loc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Constructor for initializing the game
        /// </summary>
        public Game()
        {
            _currentPlayer = _blackPlayer;
            _otherPlayer = _whitePlayer;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((x + y) % 2 == 1)
                    {
                        if (y < 3)
                        {
                            _board[x, y] = new Checker(_whitePlayer);
                        }
                        else if (y > 4)
                        {
                            _board[x, y] = new Checker(_blackPlayer);
                        }
                    }
                }
            }
            GetMoves(false); // No jumps available for the first move
        }

        /// <summary>
        /// Ends the current player's turn.
        /// </summary>
        private void EndTurn()
        {
            Player temp = _currentPlayer;
            _currentPlayer = _otherPlayer;
            _otherPlayer = temp;
            _legalMoves = new Stack<Move>();
            GetMoves(true);
            if (_legalMoves.Count == 0)
            {
                GetMoves(false);
            }
            _squareIsSelected = false;
        }

        /// <summary>
        /// Determines whether the given square is selected.
        /// </summary>
        /// <param name="square">The square to check.</param>
        /// <returns>Whether square is selected.</returns>
        public bool IsSelected(Point square)
        {
            return _squareIsSelected && _selectedSquare == square;
        }

        /// <summary>
        /// Determines whether the given square is the start square of a
        /// legal move.
        /// </summary>
        /// <param name="square">The square to check.</param>
        /// <returns>Whether square is the start square of a legal move.</returns>
        private bool IsLegalStart(Point square)
        {
            foreach (Move move in _legalMoves)
            {
                if (square == move.From)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to select the piece at the given location. If the location
        /// is not the start location of a legal move, the selection is
        /// unchanged.
        /// </summary>
        /// <param name="square">The square to select.</param>
        /// <returns>Whether the square was selected.</returns>
        private bool TrySelectSquare(Point square)
        {
            if (IsLegalStart(square))
            {
                _selectedSquare = square;
                _squareIsSelected = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to get a legal move.
        /// </summary>
        /// <param name="from">The start square of the move.</param>
        /// <param name="to">The destination square of the move.</param>
        /// <returns>The move described, or null if the move described is
        /// illegal.</returns>
        private Move TryGetMove(Point from, Point to)
        {
            foreach (Move move in _legalMoves)
            {
                if (from == move.From && to == move.To)
                {
                    return move;
                }
            }
            return null;
        }

        /// <summary>
        /// Plays the given move.
        /// </summary>
        /// <param name="move">The move to play.</param>
        private void DoMove(Move move)
        {
            _moveHistory.Push(move);
            Checker moving = _board[move.From.X, move.From.Y];
            _board[move.To.X, move.To.Y] = moving;
            _board[move.From.X, move.From.Y] = null;
            if (move.To.Y == _currentPlayer.KingAt)
            {
                moving.IsKing = true;
            }
            if (move.IsJump)
            {
                _board[move.CapturedLocation.X, move.CapturedLocation.Y] = null;
                if (move.FromKing || !moving.IsKing)
                {
                    _legalMoves = new Stack<Move>();
                    GetJumpsFrom(move.To);
                    if (_legalMoves.Count != 0)
                    {
                        TrySelectSquare(move.To);
                        return;
                    }
                }
            }
            EndTurn();
        }

        /// <summary>
        /// Tries to select the given square or move the selected piece to it.
        /// </summary>
        /// <param name="target">The target square</param>
        /// <returns>Whether either a selection or a move was successfully made.</returns>
        public bool SelectOrMove(Point target)
        {
            if (TrySelectSquare(target))
            {
                return true;
            }
            else if (_squareIsSelected)
            {
                Move move = TryGetMove(_selectedSquare, target);
                if (move == null)
                {
                    return false;
                }
                else
                {
                    DoMove(move);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to undo the last move.
        /// </summary>
        /// <returns>Whether a move was undone.</returns>
        public bool Undo()
        {
            if (_moveHistory.Count > 0)
            {
                Move last = _moveHistory.Pop();
                _currentPlayer = last.MovingPlayer;
                _otherPlayer = last.OtherPlayer;
                _legalMoves = last.LegalMoves;
                Checker checker = _board[last.To.X, last.To.Y];
                _board[last.To.X, last.To.Y] = null;
                _board[last.From.X, last.From.Y] = checker;
                checker.IsKing = last.FromKing;
                if (last.IsJump)
                {
                    _board[last.CapturedLocation.X, last.CapturedLocation.Y] = last.CapturedPiece;
                }
                TrySelectSquare(last.From);
                return true;
            }
            return false;
        }
    }
}
