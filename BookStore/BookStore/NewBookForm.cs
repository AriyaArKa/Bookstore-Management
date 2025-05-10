using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BookStoreApp
{
    public partial class NewBookForm : Form
    {
        private string connectionString;

        public NewBookForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["BookStoreConn"].ConnectionString;
            SetupForm();
        }

        private void SetupForm()
        {
            Label[] labels = new Label[]
            {
                new Label { Text = "Title", Top = 10, Left = 10 },
                new Label { Text = "Author", Top = 40, Left = 10 },
                new Label { Text = "Publisher", Top = 70, Left = 10 },
                new Label { Text = "Year Published", Top = 100, Left = 10 },
                new Label { Text = "Price", Top = 130, Left = 10 },
            };

            TextBox txtTitle = new TextBox { Name = "txtTitle", Top = 10, Left = 120 };
            TextBox txtAuthor = new TextBox { Name = "txtAuthor", Top = 40, Left = 120 };
            TextBox txtPublisher = new TextBox { Name = "txtPublisher", Top = 70, Left = 120 };
            TextBox txtYear = new TextBox { Name = "txtYear", Top = 100, Left = 120 };
            TextBox txtPrice = new TextBox { Name = "txtPrice", Top = 130, Left = 120 };

            CheckBox chkHardcopy = new CheckBox { Text = "Hardcopy", Top = 160, Left = 120, Name = "chkHardcopy" };
            CheckBox chkEcopy = new CheckBox { Text = "eCopy", Top = 190, Left = 120, Name = "chkEcopy" };

            Button btnInsert = new Button { Text = "Insert", Top = 220, Left = 120 };
            btnInsert.Click += BtnInsert_Click;

            this.Controls.AddRange(labels);
            this.Controls.AddRange(new Control[] { txtTitle, txtAuthor, txtPublisher, txtYear, txtPrice, chkHardcopy, chkEcopy, btnInsert });
        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            var title = Controls["txtTitle"] as TextBox;
            var author = Controls["txtAuthor"] as TextBox;
            var publisher = Controls["txtPublisher"] as TextBox;
            var year = Controls["txtYear"] as TextBox;
            var price = Controls["txtPrice"] as TextBox;
            var hardcopy = Controls["chkHardcopy"] as CheckBox;
            var ecopy = Controls["chkEcopy"] as CheckBox;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO textBook (Title, Author, Publisher, Year_Published, Price, Hardcopy, eCopy) " +
                    "VALUES (@Title, @Author, @Publisher, @Year, @Price, @Hardcopy, @eCopy)", con);

                cmd.Parameters.AddWithValue("@Title", title.Text);
                cmd.Parameters.AddWithValue("@Author", author.Text);
                cmd.Parameters.AddWithValue("@Publisher", publisher.Text);
                cmd.Parameters.AddWithValue("@Year", Convert.ToInt32(year.Text));
                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(price.Text));
                cmd.Parameters.AddWithValue("@Hardcopy", hardcopy.Checked);
                cmd.Parameters.AddWithValue("@eCopy", ecopy.Checked);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Book inserted!");
            this.Close();
        }
    }
}
