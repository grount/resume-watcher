using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using resumeWatcher.Properties;
using System.Diagnostics;


namespace resumeWatcher
{
    public partial class MainWindow : Form
    {
        string destFile = "";
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private enum eErrorType
        {
            InvalidInput,
            Valid,
            AlreadyExists
        }

        private enum eColumnIndex
        {
            companyIndex,
            positionIndex,
            urlIndex,
            cvIndex
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddComboBoxItems();

            DateTime timeToday = DateTime.Now;
            string timeTodayModified = String.Format("{0:dd/MM/yy}", timeToday);
            string[] row = new string[] { companyComboBox.Text, positionTextBox.Text, urlTextBox.Text, destFile,  timeTodayModified };
            mainDataGridView.Rows.Add(row); // TODO: not inserting duplicates.


        }

        private void AddComboBoxItems()
        {
            eErrorType returnedEErrorType = ValidateCompanyComboBoxText();
            if (returnedEErrorType == eErrorType.Valid)
            {
                companyComboBox.Items.Add(companyComboBox.Text);
            }
            else
            {
                if (returnedEErrorType == eErrorType.InvalidInput)
                {
                    companyComboBox.Text = "";  // If wrong input clear the input.
                    MessageBox.Show("Please enter a valid input", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //MessageBox.Show("Already exists in the company list", "Duplicate Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private eErrorType ValidateCompanyComboBoxText()
        {
            if (companyComboBox.Items.Contains(companyComboBox.Text))
                // If the written name is there don't add it twice.
            {
                return eErrorType.AlreadyExists;
            }

            if (string.IsNullOrWhiteSpace(companyComboBox.Text))
            {
                return eErrorType.InvalidInput;
            }

            companyComboBox.Text = companyComboBox.Text.Trim();
            RemoveSpacesBetweenString();

            return eErrorType.Valid;
        }

        private void RemoveSpacesBetweenString()
        {
            StringBuilder newComboBoxText = new StringBuilder();
            bool isSpaceFound = false;

            for (int i = 0; i < companyComboBox.Text.Length; i++)
            {
                char letter = companyComboBox.Text[i];

                if (Char.IsWhiteSpace(letter) == false)
                {
                    newComboBoxText.Append(letter);
                    isSpaceFound = false;
                }
                else if (Char.IsWhiteSpace(letter) == true && isSpaceFound == false)
                {
                    newComboBoxText.Append(letter);
                    isSpaceFound = true;
                }
            }
            companyComboBox.Text = newComboBoxText.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            companyComboBox.Items.Clear();
            foreach (object item in Properties.Settings.Default.myList)
            {
                companyComboBox.Items.Add(item);
            }
            companyComboBox.SelectedIndex = 0;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.myList = new ArrayList();
            foreach (object item in companyComboBox.Items)
            {
                Properties.Settings.Default.myList.Add(item);
            }
        
            Properties.Settings.Default.Save();
        }

        private void mainDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string url = mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (e.ColumnIndex == (int)eColumnIndex.urlIndex)
            {
                try
                {
                    Process.Start("chrome.exe", url);
                }
                finally
                {
                    try
                    {
                        Process.Start("firefox.exe", url);
                    }
                    catch (Exception ex2)
                    {
                        throw new ArgumentException("Couldnt start firefox or chrome.\nException Message:{0}.\n", ex2);
                    }
                }
            }
            else if (e.ColumnIndex == (int)eColumnIndex.cvIndex)
            {
                string filePath = mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start("explorer.exe", filePath);
            }
        }

        private void CVButton_Click(object sender, EventArgs e)
        {
            copyFileToDirectory();
        }

        private void copyFileToDirectory()
        {
            if (openCVFileDialog.ShowDialog() == DialogResult.OK) // if you select file
            {
                string onlyFileName = ""; // TODO check whats not needed here.
                string fileName = openCVFileDialog.SafeFileName;
                string targetPath = AppDomain.CurrentDomain.BaseDirectory;
                string sourcePath = openCVFileDialog.FileName;
                string extenstion = "";

                ModifyFileName(onlyFileName, extenstion, ref fileName);

                // Use Path class to manipulate file and directory paths.
                //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                destFile = System.IO.Path.Combine(targetPath, fileName);

                System.IO.File.Copy(sourcePath, destFile, false);
            }
        }

        private void ModifyFileName(string onlyFileName, string extenstion, ref string fileName)
        {
           // if (System.IO.File.Exists(fileName)) 
           // {
            DateTime thisDay = DateTime.Now;
            string thisDayString = String.Format("{0:dd-MM-HH_HH-mm-ss}", thisDay);
                
           //onlyFileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
           // onlyFileName += "-" + thisDayString;
                extenstion = Path.GetExtension(openCVFileDialog.FileName);
            // onlyFileName += extenstion;
            thisDayString += extenstion;
            //fileName = onlyFileName;
            fileName = thisDayString;
            //}
        }
    }
}
