/* Player.cs
 * Author: Rod Howell
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ksu.Cis300.Checkers
{
    /// <summary>
    /// Contains information about one of the players.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The directions a pawn can move. This will be a 2-element
        /// array, both of whose coordinates each have an absolute value
        /// of 1. The y-coordinate will be the same in both, and will
        /// depend on which player is being represented.
        /// </summary>
        public Point[] PawnDirections { get; }

        /// <summary>
        /// The y-value at which a pawn becomes a king, either 0 or 7 depending
        /// on which player is being represented.
        /// </summary>
        public int KingAt { get; }

        /// <summary>
        /// The name of this player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Concstructs a player who moves in the given direction (1 for White,
        /// -1 for Black) and has the given name ("White" or "Black").
        /// </summary>
        /// <param name="dir">The direction the player normally moves (1 for White,
        /// -1 for Black).</param>
        /// <param name="name">The name of this player ("White" or "Black").</param>
        public Player(int dir, string name)
        {
            PawnDirections = new Point[] { new Point(-1, dir), new Point(1, dir) };
            if (dir == -1)
            {
                KingAt = 0;
            }
            else
            {
                KingAt = 7;
            }
            Name = name;
        }
    }
}
