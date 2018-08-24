using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CableRobot
{
    /// <summary>
    /// You like progress bars, don't you?
    /// </summary>
    public partial class ProgressDialog : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        public ProgressDialog(string progressText)
        {
            InitializeComponent();
            Text = progressText;
            label.Text = progressText;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }


    }
}
