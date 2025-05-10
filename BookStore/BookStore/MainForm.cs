using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace BookStoreApp
{
    public partial class MainForm : Form
    {
        TextBox txtTitle, txtAuthor, txtPublisher, txtYear, txtPrice;
        CheckBox chkHardcopy, chkEcopy;
        Button btnInsert, btnLoad, btnPrint;
        DataGridView dataGridView;

        string connectionString;

        public MainForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["BookStoreConn"].ConnectionString;
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Text = "Book Store";
            this.Size = new System.Drawing.Size(800, 600);

            Label[] labels = new Label[7];
            string[] labelTexts = { "Title", "Author", "Publisher", "Year", "Price", "Hardcopy", "E-Copy" };
            int top = 20;

            for (int i = 0; i < labelTexts.Length; i++)
            {
                labels[i] = new Label { Text = labelTexts[i], Left = 20, Top = top, Width = 80 };
                this.Controls.Add(labels[i]);
                top += 30;
            }

            txtTitle = new TextBox { Left = 120, Top = 20, Width = 200 };
            txtAuthor = new TextBox { Left = 120, Top = 50, Width = 200 };
            txtPublisher = new TextBox { Left = 120, Top = 80, Width = 200 };
            txtYear = new TextBox { Left = 120, Top = 110, Width = 200 };
            txtPrice = new TextBox { Left = 120, Top = 140, Width = 200 };
            chkHardcopy = new CheckBox { Left = 120, Top = 170 };
            chkEcopy = new CheckBox { Left = 120, Top = 200 };

            this.Controls.AddRange(new Control[] { txtTitle, txtAuthor, txtPublisher, txtYear, txtPrice, chkHardcopy, chkEcopy });

            btnInsert = new Button { Text = "Insert Book", Left = 120, Top = 240, Width = 100 };
            btnInsert.Click += BtnInsert_Click;
            this.Controls.Add(btnInsert);

            btnLoad = new Button { Text = "Load Books", Left = 230, Top = 240, Width = 100 };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            btnPrint = new Button { Text = "Print Books", Left = 340, Top = 240, Width = 100 };
            btnPrint.Click += BtnPrint_Click;
            this.Controls.Add(btnPrint);

            dataGridView = new DataGridView { Left = 20, Top = 290, Width = 740, Height = 250, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            this.Controls.Add(dataGridView);
        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO textBook (Title, Author, Publisher, Year_Published, Price, Hardcopy, eCopy) VALUES (@Title, @Author, @Publisher, @Year, @Price, @Hardcopy, @eCopy)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                cmd.Parameters.AddWithValue("@Publisher", txtPublisher.Text);
                cmd.Parameters.AddWithValue("@Year", Convert.ToInt32(txtYear.Text));
                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@Hardcopy", chkHardcopy.Checked);
                cmd.Parameters.AddWithValue("@eCopy", chkEcopy.Checked);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book inserted successfully!");
                con.Close();
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM textBook", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ReportViewerForm printForm = new ReportViewerForm();
            printForm.ShowDialog();
        }
    }
}