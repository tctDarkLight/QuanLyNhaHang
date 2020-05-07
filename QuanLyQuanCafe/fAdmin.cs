using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource categoryList = new BindingSource();

        BindingSource tableList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        #region methods

        DataTable SearchFoodByName(string name)
        {
            DataTable listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadListCategory();
            LoadListTable();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Loại tài khoản", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            try
            {
                dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
                CultureInfo culture = new CultureInfo("vi-VN");
                Double revenue = Convert.ToDouble(DataProvider.Instance.ExecuteScalar("EXEC USP_GetRevenueByDate @fromDate , @toDate", new object[] { checkIn, checkOut }));
                Convert.ToDouble(DataProvider.Instance.ExecuteScalar("EXEC USP_GetRevenueByDate @fromDate , @toDate", new object[] { checkIn, checkOut }));
                txbRevenue.Text = revenue.ToString("c", culture);
            }
            catch { }
            
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Tên", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbPrice.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Giá", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Đây là tài khoản bạn đang dùng để đăng nhập! Không thể xóa","Thông báo");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory1();
        }
        void AddCategoryBinding()
        {
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Tên danh mục", true, DataSourceUpdateMode.Never));
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.GetTableList();
        }

        void AddTableBinding()
        {
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Tên bàn", true, DataSourceUpdateMode.Never));
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Trạng thái", true, DataSourceUpdateMode.Never));
        }



        #endregion

        #region events
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)numericUpDown1.Value;
                try
                {
                    AddAccount(userName, displayName, type);
                }
                catch
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại", "Thông báo");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi khi tạo tài khoản, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK);
            }
            
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;

                DeleteAccount(userName);
            }
            catch
            {
                MessageBox.Show("Không có gì để xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)numericUpDown1.Value;

                EditAccount(userName, displayName, type);
            }
            catch
            {
                MessageBox.Show("Không có gì để sửa!", "Thông báo", MessageBoxButtons.OK);
            }
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;

                ResetPass(userName);
            }
            catch
            {
                MessageBox.Show("Không có gì để thực hiện!", "Thông báo", MessageBoxButtons.OK);
            }
        }


        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["Loại"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbFoodName.Text;
                int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
                float price = (float)Convert.ToDouble(txbPrice.Text);

                if (FoodDAO.Instance.InsertFood(name, categoryID, price))
                {
                    MessageBox.Show("Thêm món thành công");
                    LoadListFood();
                    if (insertFood != null)
                        insertFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm thức ăn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để thêm!","Thông báo",MessageBoxButtons.OK);
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbFoodName.Text;
                int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
                float price = (float)Convert.ToDouble(txbPrice.Text);
                int id = Convert.ToInt32(txbFoodID.Text);

                if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
                {
                    MessageBox.Show("Sửa món thành công");
                    LoadListFood();
                    if (updateFood != null)
                        updateFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa thức ăn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để sửa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txbFoodID.Text);

                if (FoodDAO.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công");
                    LoadListFood();
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa thức ăn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
            LoadListCategory();
        }
       
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnFristBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbCategoryName.Text;

                if(name == "")
                {
                    MessageBox.Show("Không có gì để thêm!", "Thông báo", MessageBoxButtons.OK);
                }

                else if (CategoryDAO.Instance.InsertCategory(name))
                {
                    MessageBox.Show("Thêm danh mục thành công");
                    LoadListCategory();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm danh mục");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để thêm!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbCategoryName.Text;
                int id = Convert.ToInt32(txbCategoryID.Text);
                if (CategoryDAO.Instance.UpdateCategory(id, name))
                {
                    MessageBox.Show("Sửa danh mục thành công");
                    LoadListCategory();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa danh mục");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để sửa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txbCategoryID.Text);
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa danh mục thành công");
                    LoadListCategory();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa danh mục");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để xóa!");
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbTableName.Text;
                string status = txbTableStatus.Text;


                if (TableDAO.Instance.InsertTable(name, status))
                {
                    MessageBox.Show("Thêm bàn thành công");
                    LoadListTable();
                    if (insertTable != null)
                        insertTable(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm bàn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để thêm!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txbTableID.Text);

                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadListTable();
                    if (deleteTable != null)
                        deleteTable(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txbTableName.Text;
                string status = txbTableStatus.Text;
                int id = Convert.ToInt32(txbTableID.Text);

                if (TableDAO.Instance.UpdateTable(id, name, status))
                {
                    MessageBox.Show("Sửa bàn thành công");
                    LoadListTable();
                    if (updateTable != null)
                        updateTable(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa bàn");
                }
            }
            catch
            {
                MessageBox.Show("Không có gì để sửa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private void btnExportListBill_Click(object sender, EventArgs e)
        {
            if (dtgvBill.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Application.Workbooks.Add(Type.Missing);
                for (int i = 1; i < dtgvBill.Columns.Count + 1; i++)
                {
                    app.Cells[1, i] = dtgvBill.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dtgvBill.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dtgvBill.Columns.Count; j++)
                    {
                        app.Cells[i + 2, j + 1] = dtgvBill.Rows[i].Cells[j].Value.ToString();
                    }
                }
                app.Columns.AutoFit();
                app.Visible = true;
            }
        }
        #endregion              



        
    }
}
