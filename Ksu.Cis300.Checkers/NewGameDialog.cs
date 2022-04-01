using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.Checkers
{
    public partial class uxNewGameDialog : Form
    {
        /// <summary>
        /// Computer player property for the RadioButtons
        /// </summary>
        public string ComputerPlayer
        {
            get
            {
                if (uxComputerBlack.Checked == true)
                {
                    return "Black";
                }
                else if (uxComputerWhite.Checked == true)
                {
                    return "White";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// Property for level in updown box
        /// </summary>
        public int level => (int) uxLevel.Value;
        /// <summary>
        /// Initializes new game
        /// </summary>
        public uxNewGameDialog()
        {
            InitializeComponent();
        }

    }
}
