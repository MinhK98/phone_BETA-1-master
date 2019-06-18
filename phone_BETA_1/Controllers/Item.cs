using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phone_BETA_1.Models;

namespace phone_BETA_1.Controllers
{
    public class Item
    {
        private SANPHAM sanpham = new SANPHAM();
        private int quantity;

        public Item() { }

        public Item(SANPHAM sanpham, int quantity)
        {
            this.sanpham = sanpham;
            this.quantity = quantity;
        }

        public int Quantity { get => quantity; set => quantity = value; }
        public SANPHAM Sanpham { get => sanpham; set => sanpham = value; }
    }
}