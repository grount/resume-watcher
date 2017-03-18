using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace resumeWatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
    }
}
