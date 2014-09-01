using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Instagram
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            this.btnSignUp.Click += (objs, obje) =>
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(signUp)).Start();
            };
        }

        private void signUp()
        {
            setEnable(btnSignUp, false);
            try
            {

                while (!this.IsDisposed)
                {
                    IGController controller = new IGController();
                    if (controller.SignUp())
                    {

                    }
                    else
                    {
                        throw controller.Error;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                setEnable(btnSignUp, true);
            }
        }

        private void setEnable(Control control, bool bEnable)
        {
            if (!control.IsDisposed)
            {
                if (control.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => { control.Enabled = bEnable; }));
                }
                else
                {
                    control.Enabled = bEnable;
                }
            }
        }
    }
}
