using Microsoft.Office.Interop.Excel;
using QuanLyQuanCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fFinalBill : Form
    {
        public fFinalBill()
        {
            InitializeComponent();
            
        }

        public string tableID, billID, discount, finalTotalPrice;
        
        void ShowBill(int id)
        {
            
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            Double totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvFinalBill.Items.Add(lsvItem);
            }
        }
      
        private void fFinalBill_Load_1(object sender, EventArgs e)
        {
            ShowBill(int.Parse(tableID));
            lblDiscount.Text = discount;
            Double finalPrice = Convert.ToDouble(finalTotalPrice);
            CultureInfo culture = new CultureInfo("vi-VN");
            lblTotalPrice.Text = finalPrice.ToString("c", culture);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            BillDAO.Instance.CheckOut(int.Parse(billID), int.Parse(discount), float.Parse(finalTotalPrice));
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet ws = (Worksheet)app.ActiveSheet;

                    Range rg;
                    ws.Cells.Font.Name = "Times New Roman";
                    ws.Cells.Font.Size = 12;

                    ws.Cells[1, 2] = lblRestaurantName.Text;
                    rg = ws.Cells[1, 2];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[2, 2] = lblSlogan.Text;
                    rg = ws.Cells[2, 2];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[3, 2] = lblRestaurantAddress.Text;
                    rg = ws.Cells[3, 2];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    ws.Cells[5, 1] = columnHeader1.Text;
                    rg = ws.Cells[5, 1];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[5, 2] = columnHeader2.Text;
                    rg = ws.Cells[5, 2];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[5, 3] = columnHeader3.Text;
                    rg = ws.Cells[5, 3];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[5, 4] = columnHeader4.Text;
                    rg = ws.Cells[5, 4];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    int i = 6;
                    foreach(ListViewItem item in lsvFinalBill.Items)
                    {
                        ws.Cells[i, 1] = item.SubItems[0].Text;
                        rg = ws.Cells[i, 1];
                        rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        ws.Cells[i, 2] = item.SubItems[1].Text;
                        rg = ws.Cells[i, 2];
                        rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        ws.Cells[i, 3] = item.SubItems[2].Text;
                        rg = ws.Cells[i, 3];
                        rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        ws.Cells[i, 4] = item.SubItems[3].Text;
                        rg = ws.Cells[i, 4];
                        rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        i++;

                    }
                    i++;
                    ws.Cells[i, 1] = "Giảm giá";
                    rg = ws.Cells[i, 1];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[i, 4] = lblDiscount.Text + "%";
                    rg = ws.Cells[i, 4];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    i++;
                    ws.Cells[i, 1] = "Tổng tiền";
                    rg = ws.Cells[i, 1];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.Cells[i, 4] = lblTotalPrice.Text;
                    rg = ws.Cells[i, 4];
                    rg.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    app.Columns.AutoFit();
                    //app.Cells.AutoFit();
                    app.Visible = true;
                    
        }
    }
}
