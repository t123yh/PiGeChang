using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiGeChang
{
    class WndProcReceiverForm : Form
    {
        public WndProcReceiverForm()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += (sender, e) => this.Size = new System.Drawing.Size(1, 1);
            this.Activated += (sender, e) => this.Hide();
        }

        public event Action<Message> WndProcReceiving;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WndProcReceiving != null)
            {
                WndProcReceiving(m);
            }
        }
    }
}
