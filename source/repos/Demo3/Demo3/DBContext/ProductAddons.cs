using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo3.DBContext
{
    public partial class Product
    {
        public decimal FinalPrice => Price - (Price * (Discount ?? 0) / 100);
        public bool HasDiscount => Discount > 0;
        public bool BigDiscount => Discount >= 12;
        public bool OutOfStock => Quantity == 0;

        public string FullImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(Image)) return "pack://application:,,,/Media/Products/picture.png";

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                return System.IO.Path.Combine(baseDir, Image);
            }
        }
    }

    public partial class Order
    {
        public string Articles
        {
            get
            {
                string result = "";
                var orders = DataBaseConnection.demoEntities.Order_Products.Where(x => x.ID_Order == ID).ToList();

                foreach (var order in orders) 
                {
                    result += $"{order.Product.Article} ";
                }

                return result;
            }
        }
    }
}
