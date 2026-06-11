using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo2.DataBaseContext
{
    public partial class Product
    {
        // Итоговая цена со скидкой
        public decimal FinalPrice => Price - (Price * (Discount ?? 0) / 100);

        // Есть ли скидка
        public bool HasDiscount => Discount > 0;

        // Скидка больше 12%
        public bool IsHighDiscount => Discount > 12;

        // Нет на складе
        public bool IsOutOfStock => Quantity == 0;

        // Отображение скидки (например "15%")
        public string DiscountDisplay => Discount > 0 ? $"{Discount}%" : "";
    }
}
