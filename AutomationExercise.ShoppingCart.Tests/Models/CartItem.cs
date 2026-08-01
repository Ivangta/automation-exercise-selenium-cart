using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationExercise.ShoppingCart.Tests.Models
{
    public class CartItem
    {
        public string Name { get; }
        public string Price { get; }
        public int Quantity { get; }
        public string Total { get; }

        public CartItem(
            string name,
            string price,
            int quantity,
            string total)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Total = total;
        }
    }
}
