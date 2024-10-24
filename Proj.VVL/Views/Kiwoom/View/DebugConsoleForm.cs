using Proj.VVL.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj.VVL.Views.Kiwoom.View
{
    public partial class DebugConsoleForm : Form
    {
        public DebugConsolePanel debugConsole = new DebugConsolePanel();
        public DebugConsoleForm()
        {
            InitializeComponent();
            panel1.Controls.Add(debugConsole);
        }
    }
}
