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

        public Message WaitForWndProc(Func<Message, bool> predicate)
        {
            return WaitForWndProc<Object>(predicate, (msg) => null).Item1;
        }

        public Tuple<Message, TObject> WaitForWndProc<TObject>(Func<Message, bool> predicate, Func<Message, TObject> generator)
        {
            using (AutoResetEvent wait = new AutoResetEvent(false))
            {
                Message msg = new Message();
                TObject obj = default(TObject);
                WndProcReceiving += (m) =>
                {
                    if (predicate(m))
                    {
                        msg = m;
                        obj = generator(msg);
                        wait.Set();
                    }
                };

                this.Show();
                wait.WaitOne();

                WndProcReceiving = null;
                return new Tuple<Message, TObject>(msg, obj);
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

        public static Tuple<Message, TObject> WaitForSingleWndProc<TObject>(Func<Message, bool> predicate, Func<Message, TObject> generator)
        {
            using (WndProcReceiverForm form = new WndProcReceiverForm())
            {
                form.Show();
                return form.WaitForWndProc(predicate, generator);
            }
        }
    }
}
