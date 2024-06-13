using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FormS
{
    public partial class AddStudentForm : Form
    {
        private MySqlConnection connection;

        public AddStudentForm()
        {
            InitializeComponent();
            connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='stud';username=root;password=");
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            string studentName = textBoxStudentName.Text;

            if (string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("Please enter a valid student name.");
                return;
            }

            try
            {
                string insertQuery = "INSERT INTO student_mark (stud_name) VALUES (@studentName)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@studentName", studentName);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Student added successfully!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                connection.Close();
            }
        }

        private void AddStudentForm_Load(object sender, EventArgs e)
        {
            // Optional: Any additional initialization code
        }
    }
}
