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
    public partial class UpdateMarkForm : Form
    {
        public string Subject { get; set; }
       public  int Mark { get; set; }


        public UpdateMarkForm(string subject, int mark)
        {
            InitializeComponent();
            textBoxSubject.Text = subject;
            textBoxMark.Text = mark.ToString();
            //this.Load += new EventHandler(UpdateMarkForm_Load);
        }
        private void UpdateMarkForm_Load(object sender, EventArgs e)
        {
            // Any initialization code can go here.
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxSubject.Text) && int.TryParse(textBoxMark.Text, out int newMark))
            {
                Subject = textBoxSubject.Text;
                Mark = newMark;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid subject and mark.");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
           DialogResult = DialogResult.Cancel;
            Close();
       }
        private void textBoxSubject_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add any logic here if needed when the text changes
        }
        
    }
}
