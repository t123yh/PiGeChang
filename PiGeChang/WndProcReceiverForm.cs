using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private event Action<Message> WndProcReceiving;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WndProcReceiving != null)
            {
                WndProcReceiving(m);
            }
        }

        public Message WaitForWndProc()
        {
            using (AutoResetEvent wait = new AutoResetEvent(false))
            {
                Message msg = new Message();
                WndProcReceiving += (m) =>
                {
                    msg = m;
                    wait.Set();
                };
                wait.WaitOne();
                return msg;
            }
        }

        public Message WaitForWndProc(Func<Message, bool> predicate)
        {
            using (AutoResetEvent wait = new AutoResetEvent(false))
            {
                Message msg = new Message();
                WndProcReceiving += (m) =>
                {
                    msg = m;
                    wait.Set();
                };

                this.Show();

                do
                {
                    wait.WaitOne();
                }
                while (!predicate(msg));

                WndProcReceiving = null;
                return msg;
            }
        }

        public static Message WaitForSingleWndProc()
        {
            using (WndProcReceiverForm form = new WndProcReceiverForm())
            {
                return form.WaitForWndProc();
            }
        }

        public static Message WaitForSingleWndProc(Func<Message, bool> predicate)
        {
            using (WndProcReceiverForm form = new WndProcReceiverForm())
            {
                form.Show();
                return form.WaitForWndProc(predicate);
            }
        }
    }
}
