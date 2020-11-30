using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public string ArticleNumber { get; protected set; }

        [Required]
        public string ArticleName { get; protected set; }

        public string Description { get; protected set; }

        [Required]
        public int Price { get; protected set; }

        public ICollection<Category> Categories { get; set; }

        public void setName(string articleName) 
        {
            this.ArticleName = articleName;
        }

        public void setDescription(string description) 
        {
            this.Description = description;
        }

        public void setPrice(int price) 
        {
            this.Price = price;
        }

    }
}
