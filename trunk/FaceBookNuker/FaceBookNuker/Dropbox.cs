using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaceBookNuker.Controller;

namespace FaceBookNuker
{
    public partial class Dropbox : Form
    {
        public Dropbox()
        {
            InitializeComponent();
        }

        private void Dropbox_Load(object sender, EventArgs e)
        {
            DropboxController dropb = new DropboxController();
            dropb.Reg();
        }
    }
}
