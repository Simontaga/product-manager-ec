using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Manager.Domain.Models
{
    class Article
    {

        public Article(string ArticleNumber,string ArticleName,string Description,int Price) 
        {
            this.ArticleNumber = ArticleNumber;
            this.ArticleName = ArticleName;
            this.Description = Description;
            this.Price = Price;
        }

        public string ArticleNumber { get; }

        public string ArticleName { get; }

        public string Description { get; }

        public int Price { get; }

    }
}
