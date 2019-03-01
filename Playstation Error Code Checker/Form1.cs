using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
using System.Data;
using System.Reflection;

namespace Playstation_Error_Code_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filterField = "Error Code";
        DataSet result;

        private void Form1_Load(object sender, EventArgs e)
        {

            string path = Environment.CurrentDirectory;

            Extract("Playstation_Error_Code_Checker", path, "Resources", "Error_Code.xlsx");


            FileStream stream = File.Open(path + "\\Error_Code.xlsx", FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true } });
            comboBox1.Items.Clear();
            foreach (DataTable dt in result.Tables)
                comboBox1.Items.Add(dt.TableName);
            reader.Close();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridView1.DataSource = result.Tables[comboBox1.SelectedIndex];
            dataGridView1.ScrollBars = ScrollBars.Both;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Error_Code LIKE '%{0}%'", textBox1.Text);
        }

        private static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))

            using (BinaryReader r = new BinaryReader(s))

            using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))

            using (BinaryWriter w = new BinaryWriter(fs))

                w.Write(r.ReadBytes((int)s.Length));

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string path = Environment.CurrentDirectory;
            File.Delete("Error_Code.xlsx");
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Open and copy info"));
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                m.Show(dataGridView1, new Point(e.X, e.Y));
                if (dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    try
                    {
                        // Add the selection to the clipboard.
                        Clipboard.SetDataObject(
                            dataGridView1.GetClipboardContent());
                        // Replace the text box contents with the clipboard text. 

                        MessageBox.Show(Clipboard.GetText(), "Info");
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {

                        MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                    }
                }
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://web.facebook.com/qwerty753951");
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/pearlxcore");

        }
    }
}
