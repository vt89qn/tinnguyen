using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PokerTexas
{
    public partial class InputCaptcha : Form
    {
        #region - DECLARE -
        #endregion
        #region - PROPERTIES -
        #endregion
        #region - CONTRUCTOR -
        public InputCaptcha()
        {
            InitializeComponent();
            this.Load += new EventHandler(InputCaptcha_Load);
        }
        #endregion
        #region - EVENT -
        void InputCaptcha_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { 
                
            }
        }
        #endregion
        #region - METHOD - 
        #endregion
    }
}
