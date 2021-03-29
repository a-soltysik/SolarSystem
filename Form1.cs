using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GravityCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartAnim();
        }

        private void StartAnim() => gamePanel.Start();

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            gamePanel.Size = new Size(Width, Height);
        }


    }
}
