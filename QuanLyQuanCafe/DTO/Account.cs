using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string userName, string displayName, int type, string address, string gender, string phoneNumber, string email, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
            this.Address = address;
            this.Gender = gender;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.Type = (int)row["type"];
            this.Password = row["password"].ToString();
            this.Address = row["address"].ToString();
            this.Gender = row["gender"].ToString();
            this.PhoneNumber = row["phoneNumber"].ToString();
            this.Email = row["email"].ToString();
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }


        private string gender;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }


        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
