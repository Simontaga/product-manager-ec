using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Product_Manager.Domain.Models;

namespace Product_Manager.Data
{
    class ProductManagerContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

    //    public DbSet<ArticleCategoryRelation> ArticleCategory {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            const string connectionString =
            "Data Source=(local);Initial Catalog=Product_Manager;"
            + "Integrated Security=true";

            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
