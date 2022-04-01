/* Game.cs
 * Authors: Josh Weese and Rod Howell
 * Modified By: Gabriel Whitehair
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
        /// Gets whether there is a selected square.
        /// </summary>
        private bool _squareIsSelected;
        /// <summary>
        ///  The currently-selected square. 
        /// </summary>
        private Point _selectedSquare;
        /// <summary>
        /// The player moving the white pieces.
        /// </summary>
        private Player _whitePlayer = new Player(1, "White");
        /// <summary>
        ///  The player moving the black pieces.
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
        /// base value of a loss
        /// </summary>
        private const int _loss = -200;
        /// <summary>
        /// random field
        /// </summary>
        private Random _random = new Random(0);
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
        public bool IsOver => _legalMoves.Count == 0; // 
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
            //Checker checker = _board[square.X, square.Y];
            bool whitePieceKing = false;
            bool blackPieceKing = false;
            bool foundWhite = _whitePlayer.pieces.TryGetValue(square, out whitePieceKing);
            bool foundBlack = _blackPlayer.pieces.TryGetValue(square, out blackPieceKing);
            
            if (!foundWhite && !foundBlack)
            {
                return SquareContents.None;
            }
            if (foundWhite)
            {
                if (whitePieceKing)
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
                if (blackPieceKing)
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
        /// Gets the valid directions for the current player assuming the given type of piece.
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
        /// <param name="currentPlayerKing">If the current player is a king</param>
        private void GetNonJumpsFrom(Point loc, bool currentPlayerKing)
        {
            Point[] dirs = GetValidDirections(currentPlayerKing);
            foreach (Point d in dirs)
            {
                Point target = new Point(loc.X + d.X, loc.Y + d.Y);
                if (GetContents(target) == SquareContents.None)
                {
                    _legalMoves.Push(new Move(loc, target, currentPlayerKing, _currentPlayer, _otherPlayer, _legalMoves));
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
        /// <param name="kingCaptured">If captured piece is a king</param>
        /// <returns>Whether or not the jump can be made</returns>
        private bool CanJump(Point startSquare, Point dir, out bool kingCaptured)
        {
            if (GetContents(new Point(startSquare.X + 2 * dir.X, startSquare.Y + 2 * dir.Y)) == SquareContents.None)
            {
                Point enemySquare = new Point(startSquare.X + dir.X, startSquare.Y + dir.Y);
                bool otherPlayerFound = _otherPlayer.pieces.TryGetValue(enemySquare, out kingCaptured);
                return otherPlayerFound;
            }
            kingCaptured = false;
            return false;
        }
        /// <summary>
        /// Adds to _legalMoves the jump moves from the given location.
        /// </summary>
        /// <param name="loc">The location of the piece making the move.</param>
        /// <param name="currentPlayerKing">Is the current player a king</param>
        private void GetJumpsFrom(Point loc, bool currentPlayerKing)
        {
            Point[] dirs = GetValidDirections(currentPlayerKing);
            bool kingCaptured;
            foreach (Point d in dirs)
            {
                if (CanJump(loc, d, out kingCaptured))
                {
                    Point capLoc = new Point(loc.X + d.X, loc.Y + d.Y);
                    _legalMoves.Push(new Move(loc, new Point(loc.X + 2 * d.X, loc.Y + 2 * d.Y), currentPlayerKing, _currentPlayer,
                        _otherPlayer, _legalMoves, kingCaptured, capLoc));
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
                    bool currentPlayerKing;
                    bool currentPlayerFound = _currentPlayer.pieces.TryGetValue(new Point(x,y), out currentPlayerKing);

                    if (currentPlayerFound)
                    {
                        Point loc = new Point(x, y);
                        if (jumps)
                        {
                            GetJumpsFrom(loc, currentPlayerKing);
                        }
                        else
                        {
                            GetNonJumpsFrom(loc, currentPlayerKing);
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
                            _otherPlayer.pieces.Add(new Point(x, y), false); 
                        }
                        else if (y > 4)
                        {
                            _currentPlayer.pieces.Add(new Point(x, y), false); 
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
                GetMoves(false); // doesn't let you not jump
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
        /// <returns>The move described, or null if the move described is illegal.</returns>
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
            _currentPlayer.pieces.Remove(move.From);
            bool nowKing = false;
            if (move.To.Y == _currentPlayer.KingAt)
            {
                _currentPlayer.pieces.Add(move.To, true);
                nowKing = true;
            }
            else
            {
                _currentPlayer.pieces.Add(move.To, move.FromKing);
            }

            if (move.IsJump)
            {
                _otherPlayer.pieces.Remove(move.CapturedLocation);
                
                if (move.FromKing || !(nowKing)) // doesn't cause a pawn to become a king
                {
                    _legalMoves = new Stack<Move>();
                    bool currentPlayerKing = false;
                    bool currentPlayerFound = _currentPlayer.pieces.TryGetValue(move.To, out currentPlayerKing);
                    GetJumpsFrom(move.To, currentPlayerKing);
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
                _currentPlayer.pieces.Remove(last.To);
                _currentPlayer.pieces.Add(last.From, last.FromKing);
                if (last.IsJump)
                {
                    _otherPlayer.pieces.Add(last.CapturedLocation, last.CapturedIsKing);
                }
                TrySelectSquare(last.From);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Shuffle moves from _legalMoves array
        /// </summary>
        /// <returns>Shuffled move array</returns>
        private Move[] ShuffleMoves() 
        {
            Move[] legalMovesArray = _legalMoves.ToArray(); // Step 1
            for (int i = legalMovesArray.Length - 1; i > 0 ; i--) // Step 2 // maybe add a greater than?
            {
                int randInt = _random.Next(i + 1);
                Move saved = legalMovesArray[i];
                legalMovesArray[i] = legalMovesArray[randInt];
                legalMovesArray[randInt] = saved;
            }
            return legalMovesArray;
        }
        /// <summary>
        /// Negamax algorithm and alpha beta pruning
        /// </summary>
        /// <param name="lowerBoundA">Lower bound alpha (a)</param>
        /// <param name="upperBoundB">Upper bound beta (b)</param>
        /// <param name="K">Score to subtract</param>
        /// <param name="legalMoves">Legal moves array</param>
        /// <param name="bestMove">Best move to make for the AI</param>
        /// <returns>Double for score</returns>
        private double Negamax(double lowerBoundA, double upperBoundB, int K, 
            IEnumerable<Move> legalMoves, out Move bestMove) 
        {
            bestMove = null;
            if (IsOver)
            {
                return ((_loss - K));
            }
            if (K == 0)
            {
                return (_currentPlayer.PositionValue - _otherPlayer.PositionValue) / (double)(_currentPlayer.pieces.Count + _otherPlayer.pieces.Count);
            }
            Player cur = _currentPlayer;
            foreach (Move move in legalMoves) // iterate through legalMoves
            {
                DoMove(move);
                double score;
                if (cur == _currentPlayer)
                {
                    score = Negamax(lowerBoundA, upperBoundB, K, _legalMoves, out Move q);
                }
                else
                {
                    score = -Negamax(-upperBoundB, -lowerBoundA, K-1, _legalMoves, out Move q);
                }
                Undo();
                if (score > lowerBoundA)
                {
                    lowerBoundA = score;
                    bestMove = move;
                    if (lowerBoundA >= upperBoundB)
                    {
                        return lowerBoundA;
                    }
                }
            }
            return lowerBoundA;
        }

        /// <summary>
        /// Uses above methods to make the best move for the AI
        /// </summary>
        /// <param name="turns">Depth to go - passed to negabax in K</param>
        public void MakeBestMove(int turns) 
        {
            Move m;
            if (IsOver)
            {
                throw new InvalidOperationException();
            }
            else if (turns < 1)
            {
                throw new InvalidOperationException();
            }
            else if (_legalMoves.Count == 1)
            {
                m = _legalMoves.Peek();
            }
            else
            {
                Negamax(double.NegativeInfinity, double.PositiveInfinity, turns, ShuffleMoves(), out m);
            }
            DoMove(m);
        }
    }
}