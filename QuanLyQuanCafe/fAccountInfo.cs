using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountInfo : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        //public fAccountInfo()
        //{
        //    InitializeComponent();
        //}
        
        public fAccountInfo(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbName.Text = LoginAccount.DisplayName;
            txbGender.Text = LoginAccount.Gender;
            txbPhoneNumber.Text = LoginAccount.PhoneNumber;
            txbEmail.Text = LoginAccount.Email;
            txbAddress.Text = LoginAccount.Address;
        }


        void UpdateAccountInfo()
        {
            string username = txbUserName.Text;
            string name = txbName.Text;
            string password = txbPassword.Text;
            string address = txbAddress.Text;
            string gender = txbGender.Text;
            string phoneNumber = txbPhoneNumber.Text;
            string email = txbEmail.Text;

            string hashPass = hasPass(password);
            
            if (AccountDAO.Instance.UpdateAccountInfo(username,name,hashPass,gender,address,phoneNumber,email))
            {
                MessageBox.Show("Cập nhật thành công!");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đúng mật khẩu!");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }


        private string hasPass(string str)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(str);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            return hasPass;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
