using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetMah.Views
{
    public partial class FinishedControl : UserControl
    {
        public FinishedControl()
        {
            InitializeComponent();
        }

        public void Configure(int width, int height)
        {
            ClientSize = new Size(
                32 * width,
                32 * height + 32);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("YOU WIN", new Font("Arial", 32), Brushes.Green, 0, 0);
        }
    }
}
