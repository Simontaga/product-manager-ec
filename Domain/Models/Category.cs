using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Product_Manager.Domain.Models
{
    class Category
    {
        public Category(string categoryName)
        {

            CategoryName = categoryName;

            Articles = new List<Article>();
        }

        public int Id { get; protected set; }

        [Required]
        public string CategoryName { get; protected set; }

        [Required]
        public List<Article> Articles { get; set; }

        public void addArticle(Article article)
            
        {
            Articles.Add(article);
        }

    }
}
