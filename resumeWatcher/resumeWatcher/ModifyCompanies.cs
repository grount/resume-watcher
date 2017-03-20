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
    public partial class ModifyCompanies : Form
    {
        private MainWindow mainWindow;

        public ModifyCompanies()
        {
            InitializeComponent();
        }

        public ModifyCompanies(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            comboBox1 = mainWindow.companyComboBox;
        }
    }
}
