using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using System.Drawing.Printing;

namespace BookStoreApp
{
    public partial class ReportViewerForm : Form
    {
        private DataGridView dgvReport;
        private Button btnPrint, btnClose;
        private PrintDocument printDoc;
        private DataTable reportTable;
        private string connectionString;

        public ReportViewerForm()
        {
            this.Text = "Books Report Viewer";
            this.Size = new Size(800, 600);
            connectionString = ConfigurationManager.ConnectionStrings["BookStoreConn"].ConnectionString;

            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            dgvReport = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 450,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            btnPrint = new Button
            {
                Text = "Print",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnPrint.Click += BtnPrint_Click;

            btnClose = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(dgvReport);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM textBook ORDER BY Title", con);
                reportTable = new DataTable();
                da.Fill(reportTable);
                dgvReport.DataSource = reportTable;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 800,
                Height = 600
            };
            preview.ShowDialog();
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 10);
            float y = e.MarginBounds.Top;
            float x = e.MarginBounds.Left;
            float lineHeight = font.GetHeight(e.Graphics) + 5;

            // Draw column headers
            for (int i = 0; i < reportTable.Columns.Count; i++)
            {
                e.Graphics.DrawString(reportTable.Columns[i].ColumnName, font, Brushes.Black, x, y);
                x += 100;
            }

            y += lineHeight;
            x = e.MarginBounds.Left;

            // Draw each row
            foreach (DataRow row in reportTable.Rows)
            {
                for (int i = 0; i < reportTable.Columns.Count; i++)
                {
                    e.Graphics.DrawString(row[i].ToString(), font, Brushes.Black, x, y);
                    x += 100;
                }

                y += lineHeight;
                x = e.MarginBounds.Left;
            }
        }
    }
}
