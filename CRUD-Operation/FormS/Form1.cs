using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace FormS
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='stud';username=root;password=");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table;
        private List<string> studentList = new List<string>();
        public Form1()
        {
            InitializeComponent();
            //this.Load += new EventHandler(Form1_Load);
            //this.Load += new EventHandler(Form1_Load);

            // Attach the event handler
            //this.Load += new EventHandler(Form1_Load);

            // Attach the event handler
            buttonSearch.Click += new EventHandler(buttonSearch_Click);
            comboBox1.TextChanged += new EventHandler(comboBox1_TextChanged);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            AddUpdateButtonToDataGridView();
            AddDeleteButtonColumnToDataGridView();
            AddDisplayButtonColumnToDataGridView();
            


        }
        private void AddDisplayButtonColumnToDataGridView()
        {
            DataGridViewButtonColumn displayButtonColumn = new DataGridViewButtonColumn
            {
                Name = "displayButton",
                HeaderText = "",
                Text = "Display",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(displayButtonColumn);
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string filterParam = comboBox1.Text;
            comboBox1.Items.Clear();

            foreach (var item in studentList)
            {
                if (item.ToLower().Contains(filterParam.ToLower()))
                {
                    comboBox1.Items.Add(item);
                }
            }

            comboBox1.DroppedDown = true;
            comboBox1.SelectionStart = filterParam.Length;
            comboBox1.SelectionLength = 0;
        }


        private void AddUpdateButtonToDataGridView()
        {
            DataGridViewButtonColumn updateButtonColumn = new DataGridViewButtonColumn
            {
                Name = "updateButton",
                HeaderText = "",
                Text = "Update",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(updateButtonColumn);
        }
        private void AddDeleteButtonColumnToDataGridView()
        {
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "deleteButton",
                HeaderText = "",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(deleteButtonColumn);
        }

        private void LoadComboBoxData()
        {
            try
            {
                connection.Open();
                string query = "SELECT DISTINCT stud_name, stud_id FROM student_mark";
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string student = $"{reader.GetString("stud_name")} ({reader.GetString("stud_id")})";
                    studentList.Add(student);
                    comboBox1.Items.Add(student);
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterParam = comboBox1.Text;
            comboBox1.Items.Clear();

            foreach (var item in studentList)
            {
                if (item.ToLower().Contains(filterParam.ToLower()))
                {
                    comboBox1.Items.Add(item);
                }
            }
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;


            comboBox1.DroppedDown = true;
            comboBox1.SelectionStart = filterParam.Length;
            comboBox1.SelectionLength = 0;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["deleteButton"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Retrieve the ID or any unique identifier of the record to be deleted
                    string id = dataGridView1.Rows[e.RowIndex].Cells["stud_id"].Value.ToString();

                    // Delete the record from the database

                    // Remove the row from the DataGridView
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }




            if (e.ColumnIndex == dataGridView1.Columns["updateButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string stud_id = selectedRow.Cells["stud_id"].Value.ToString();
                string subject = selectedRow.Cells["subject"].Value.ToString();
                int currentMark = Convert.ToInt32(selectedRow.Cells["marks"].Value);

                using (UpdateMarkForm updateMarkForm = new UpdateMarkForm(subject, currentMark))
                {
                    if (updateMarkForm.ShowDialog() == DialogResult.OK)
                    {
                        string newSubject = updateMarkForm.Subject;
                        int newMark = updateMarkForm.Mark;

                        try
                        {
                            string updateQuery = "UPDATE marks SET subject=@newSubject, marks=@newMark WHERE stud_id=@stud_id AND subject=@oldSubject";
                            command = new MySqlCommand(updateQuery, connection);
                            command.Parameters.AddWithValue("@newSubject", newSubject);
                            command.Parameters.AddWithValue("@newMark", newMark);
                            command.Parameters.AddWithValue("@stud_id", stud_id);
                            command.Parameters.AddWithValue("@oldSubject", subject);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            MessageBox.Show("Mark updated successfully!");

                            comboBox1_SelectedIndexChanged(null, null);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred: " + ex.Message);
                            connection.Close();
                        }
                    }
                }
            }
            if (e.ColumnIndex == dataGridView1.Columns["displayButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string stud_id = selectedRow.Cells["stud_id"].Value.ToString();
                string subject = selectedRow.Cells["subject"].Value.ToString();
                int marks = Convert.ToInt32(selectedRow.Cells["marks"].Value);

                MessageBox.Show($"Student ID: {stud_id}\nSubject: {subject}\nMarks: {marks}");
            }
        }


        private void totalInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void buttonAddStudent_Click_1(object sender, EventArgs e)
        {
            using (AddStudentForm addStudentForm = new AddStudentForm())
            {
                if (addStudentForm.ShowDialog() == DialogResult.OK)
                {
                    comboBox1.Items.Clear();
                    LoadComboBoxData();
                    if (comboBox1.SelectedItem != null)
                    {
                        comboBox1_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void buttonAddMark_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a student first.");
                return;
            }

            using (AddMarkForm addMarkForm = new AddMarkForm())
            {
                if (addMarkForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedValue = comboBox1.SelectedItem.ToString();
                    string stud_id = selectedValue.Split('(', ')')[1];
                    string subject = addMarkForm.Subject;
                    int marks = addMarkForm.Mark;

                    try
                    {
                        string insertQuery = "INSERT INTO marks (stud_id, subject, marks) VALUES (@stud_id, @subject, @marks)";
                        command = new MySqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@stud_id", stud_id);
                        command.Parameters.AddWithValue("@subject", subject);
                        command.Parameters.AddWithValue("@marks", marks);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Mark added successfully!");

                        comboBox1_SelectedIndexChanged(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                        connection.Close();
                    }
                }
            }
        }

        private void buttonDeleteStudent_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            string selectedValue = comboBox1.SelectedItem.ToString();
            string stud_id = selectedValue.Split('(', ')')[1];

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = "DELETE FROM student_mark WHERE stud_id=@stud_id";
                    command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@stud_id", stud_id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Student deleted successfully!");

                    comboBox1.Items.Clear();
                    LoadComboBoxData();
                    dataGridView1.DataSource = null;
                    totalInput.Text = "0";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    connection.Close();
                }
            }
        }

        private void CellContentClick(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string searchValue = textBoxSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Please enter a student name to search.");
                return;
            }

            string query = $"SELECT * FROM marks WHERE stud_id IN (SELECT stud_id FROM student_mark WHERE stud_name LIKE '%{searchValue}%')";

            try
            {
                connection.Open();
                adapter = new MySqlDataAdapter(query, connection);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                string totalQuery = $"SELECT SUM(marks) FROM marks WHERE stud_id IN (SELECT stud_id FROM student_mark WHERE stud_name LIKE '%{searchValue}%')";
                command = new MySqlCommand(totalQuery, connection);
                object result = command.ExecuteScalar();
                totalInput.Text = result != null ? result.ToString() : "0";

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                connection.Close();
            }
        }
    }
}
