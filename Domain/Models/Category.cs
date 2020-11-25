using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Manager.Domain.Models
{
    class Category
    {
        public Category(string categoryName)
        {

            CategoryName = categoryName;
        }

        public int Id { get; protected set; }
        public string CategoryName { get; protected set; }

        
    }
}
