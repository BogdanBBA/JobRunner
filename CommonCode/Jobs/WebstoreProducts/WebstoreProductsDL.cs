using CommonCode.DataLayer;
using CommonCode.Utils;
using Jobs.WebstoreProducts.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.WebstoreProducts
{
    public enum WebstoreProductsTables
    {
        Products,
        Prices
    }

    public class WebstoreProductsDL : BaseDL<WebstoreProductsTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_WEBSTORE_PRODUCTS);

        protected override void InitializeTableOverview()
        {
            AddTable(WebstoreProductsTables.Products, "ID INTEGER, Active INTEGER, ShortName TEXT, FullName TEXT, URL TEXT, Frequency INTEGER, LastRun DATETIME", "ID, Active, ShortName, FullName, URL, Frequency, LastRun");
            AddTable(WebstoreProductsTables.Prices, "Moment DATETIME, ProductID INTEGER, Currency TEXT, CurrentPrice REAL, PreviousPrice REAL", "Moment, ProductID, Currency, CurrentPrice, PreviousPrice");
        }

        private string JobsToRunQuery
            => $"SELECT {Tables[WebstoreProductsTables.Products].ColumnList} FROM Products WHERE Active = 1 AND LastRun <= DATETIME('now', '-' || Frequency || ' seconds')";

        public bool ThereAreJobsToRunNow
            => (long)ExecuteSQLScalar($"SELECT EXISTS({JobsToRunQuery})") == 1;

        public List<ProductDTO> SelectJobsToRunNow()
        {
            return ProductDTO.ParseList(ExecuteSQLReader(JobsToRunQuery));
        }

        public void UpdateProductAndInsertPrice(ProductDTO product, PriceDTO price)
        {
            product.LastRun = price.Moment;
            ExecuteSQL($"UPDATE Products SET FullName = '{product.FullName.Replace("'", "''")}', LastRun = {product.LastRun.ToSqlDateTime()} WHERE ID = {product.ID}");
            InsertData(WebstoreProductsTables.Prices, price.ToSQL);
        }

        public Last2ProductPricesDTO SelectLast2PricesForProduct(ProductDTO product)
        {
            string sql = $"SELECT pri.* FROM Prices pri JOIN Products pro ON pro.ID = {product.ID} AND pro.ID = pri.ProductID ORDER BY pri.Moment DESC LIMIT 2";
            return Last2ProductPricesDTO.ParseSingleDTOFromList(product, ExecuteSQLReader(sql));
        }
    }
}
