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

        public int Id { get; protected set; }
        public string ArticleNumber { get; protected set; }

        public string ArticleName { get; protected set; }

        public string Description { get; protected set; }

        public int Price { get; protected set; }

    }
}
