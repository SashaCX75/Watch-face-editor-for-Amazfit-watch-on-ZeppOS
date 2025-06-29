using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLibrary
{
    public partial class EditToast : Form
    {
        bool updateText = false;
        string toastText = ""; 
        public bool UpdateText
        {
            get
            {
                return updateText;
            }
        }
        public string ToastText
        {
            get
            {
                return toastText;
            }
        }

        public EditToast(string toast_text)
        {
            InitializeComponent();
            toastText = toast_text;
            richTextBox_click.Text = toastText;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            updateText = true;
            toastText = richTextBox_click.Text;

            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            updateText = false;
            this.Close();
        }
    }
}
