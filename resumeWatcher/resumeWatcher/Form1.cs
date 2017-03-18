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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] row = new string[]{ "Intel", "Software Developer", "Link", "www.intel.com", "www.intel.com" };
            mainDataGridView.Rows.Add(row);
        }

        private enum eErrorType
        {
            InvalidInput,
            Valid,
            AlreadyExists
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            eErrorType returnedEErrorType = ValidateCompanyComboBoxText();
            if (returnedEErrorType == eErrorType.Valid)
            {
                companyComboBox.Items.Add(companyComboBox.Text);
            }
            else
            {
                companyComboBox.Text = "";  // If wrong input clear the input.

                if (returnedEErrorType == eErrorType.InvalidInput)
                {
                    MessageBox.Show("Please enter a valid input", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Already exists in the company list", "Duplicate Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string url = "";
            url = mainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            Process.Start("chrome.exe", url);
        }
    }
}
