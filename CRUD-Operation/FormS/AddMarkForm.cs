using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FormS
{
    public partial class AddMarkForm : Form
    {
        public string Subject { get; private set; }
        public int Mark { get; private set; }

        public AddMarkForm()
        {
            InitializeComponent();
        }

        private void AddMarkForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSubject.Text) || !int.TryParse(textBoxMark.Text, out int mark))
            {
                MessageBox.Show("Please enter a valid subject and mark.");
                return;
            }

            Subject = textBoxSubject.Text;
            Mark = mark;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBoxMark_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

