using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Manager.Domain.Models
{
    class ArticleCategoryRelation
    {
        public ArticleCategoryRelation(Article article, Category category)
        {
            Article = article;
            Category = category;
        }

        public Article Article { get; protected set; }

        public Category Category { get; protected set; }
        


    }
}
