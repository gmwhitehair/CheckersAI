/* Player.cs
 * Author: Rod Howell
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
    public class Player // Contains information about one of the players.
    {
        /// <summary>
        /// The directions a pawn can move. 2-element array, both of whose coordinates each have an absolute value of 1. The y-coordinate will be the same in both, and will depend on which player is being represented.
        /// </summary>
        public Point[] PawnDirections { get; }
        /// <summary>
        /// The y-value at which a pawn becomes a king, either 0 or 7 depending on which player is being represented.
        /// </summary>
        public int KingAt { get; }
        /// <summary>
        /// The name of this player.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// y-cord representing this player's back row
        /// </summary>
        private int _backRow;
        /// <summary>
        /// Dictionary with player's points as keys and bools representing a king as values
        /// </summary>
        public Dictionary<Point, bool> pieces { get; set; }
        /// <summary>
        /// Parameter to get postion value after iterating through pieces
        /// </summary>
        public int PositionValue 
        {
            get
            {
                int sum = 0;
                foreach (KeyValuePair<Point,bool> p in pieces) // iterate through pieces
                {
                    if (p.Value == false) // if a Pawn
                    {
                        sum += 80 + (_backRow - p.Key.Y) * (_backRow - p.Key.Y);
                    }
                    else // if a King
                    {
                        sum += 195;
                        if ((p.Key.X + p.Key.Y - 1) % 12 == 0 || (p.Key.X * p.Key.Y) == 12)
                        {
                            sum += 4;
                        }
                    }
                }
                return sum;
            }
        }
        /// <summary>
        /// Constructs a player who moves in the given direction (1 for White, -1 for Black) and has the given name ("White" or "Black").
        /// </summary>
        /// <param name="dir">The direction the player normally moves (1 for White,
        /// -1 for Black).</param>
        /// <param name="name">The name of this player ("White" or "Black").</param>
        public Player(int dir, string name)
        {
            PawnDirections = new Point[] { new Point(-1, dir), new Point(1, dir) };
            if (dir == -1) // black
            {
                KingAt = 0;
                _backRow = 7;
            }
            else // white
            {
                KingAt = 7;
                _backRow = 0; 
            }
            Name = name;
            pieces = new Dictionary<Point, bool>();
        }
    }
}
