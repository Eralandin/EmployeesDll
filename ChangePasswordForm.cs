using Employees.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employees
{
    public partial class ChangePasswordForm : Form
    {
        private long _currentUserId;
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != PasswordCheckTextBox.Text)
            {
                MessageBox.Show("Введённые пароли не совпадают!");
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
