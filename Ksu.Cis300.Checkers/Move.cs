/* Move.cs
 * Authors: Josh Weese and Rod Howell
 * Modified By: Gabriel Whitehair
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ksu.Cis300.Checkers
{
    public class Move // Represents a move, either a single jump or a non-jump.
    {
        /// <summary>
        /// Gets whether the move is a jump.
        /// </summary>
        public bool IsJump { get; }
        /// <summary>
        /// Gets the player making the move.
        /// </summary>
        public Player MovingPlayer { get; }
        /// <summary>
        /// Gets the player not making the move.
        /// </summary>
        public Player OtherPlayer { get; }
        /// <summary>
        /// The square from which the move begins.
        /// </summary>
        public Point From { get; }
        /// <summary>
        /// The square at which the move ends.
        /// </summary>
        public Point To { get; }
        /// <summary>
        /// Gets whether the piece is a king at the start of the move.
        /// </summary>
        public bool FromKing { get; }
        /// <summary>
        /// Gets the captured piece.
        /// </summary>
        public bool CapturedIsKing { get; }
        /// <summary>
        /// The location of the captured piece.
        /// </summary>
        public Point CapturedLocation { get; }
        /// <summary>
        /// Gets the legal moves at the time this move is made.
        /// </summary>
        public Stack<Move> LegalMoves { get; } 
        
        /// <summary>
        /// Constructor 1: Constructs a non-jump move.
        /// </summary>
        /// <param name="from">The square where the move starts.</param>
        /// <param name="to">The square where the move ends.</param>
        /// <param name="fromKing">Indicates whether the moving piece start the move as a king.</param>
        /// <param name="moving">The moving player.</param>
        /// <param name="other">The other player.</param>
        /// <param name="moves">The legal moves at the time this move is made.</param>
        public Move(Point from, Point to, bool fromKing, Player moving, Player other, Stack<Move> moves)
        {
            From = from;
            To = to;
            FromKing = fromKing;
            MovingPlayer = moving;
            OtherPlayer = other;
            LegalMoves = moves;
            IsJump = false;
        }
        /// <summary>
        /// Constructor 2: Constructs a jump move.
        /// </summary>
        /// <param name="from">The square where the move starts.</param>
        /// <param name="to">The square where the move ends.</param>
        /// <param name="fromKing">Indicates whether the moving piece start the move as a king.</param>
        /// <param name="moving">The moving player.</param>
        /// <param name="other">The other player.</param>
        /// <param name="moves">The legal moves at the time this move is made.</param>
        /// <param name="capIsKing">The captured piece is a king or not</param>
        /// <param name="capLoc">The location of the captured piece.</param>
        public Move(Point from, Point to, bool fromKing, Player moving, Player other, Stack<Move> moves, 
            bool capIsKing, Point capLoc)
        {
            From = from;
            To = to;
            FromKing = fromKing;
            MovingPlayer = moving;
            OtherPlayer = other;
            LegalMoves = moves;
            CapturedIsKing = capIsKing;
            CapturedLocation = capLoc;
            IsJump = true;
        }
        /// <summary>
        /// Override's ToString
        /// </summary>
        /// <returns>string representation</returns>
        public override string ToString() // Gets a string representation of this move.
        {
            string move = MovingPlayer.Name + "-" + From.X +"," + From.Y + " to " + To.X + "," + To.Y;
            if (!IsJump)
            {
                return move;
            }
            else
            {
                StringBuilder sb = new StringBuilder(MovingPlayer.Name + " Jump--");
                sb.Append(move);
                sb.Append("(capt: ");
                sb.Append(CapturedLocation.X + "," + CapturedLocation.Y);
                return sb.ToString();
            }
        }

        

    }
}
