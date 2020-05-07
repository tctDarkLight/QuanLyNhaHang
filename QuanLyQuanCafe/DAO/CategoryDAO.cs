using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null)instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }

            return list;
        }

        public DataTable GetListCategory1()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT id as [ID], name as [Tên danh mục] FROM dbo.FoodCategory ");
        }
        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "select * from FoodCategory where id = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT dbo.FoodCategory( name ) VALUES  ( N'{0}')",name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateCategory(int idCategory, string name)
        {
            string query = string.Format("UPDATE dbo.FoodCategory SET name = N'{0}' where id = {1}", name, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteCategory(int idCategory)
        {
            //FoodDAO.Instance.DeleteFoodByCategoryID(idCategory);
            //BillInfoDAO.Instance.DeleteBillInfoByFoodID()
            //string query = string.Format("DELETE dbo.FoodCategory where id = {0}", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_DeleteFoodCategory @idCategory", new object[] { idCategory});
            return result > 0;
        }
    }
}
