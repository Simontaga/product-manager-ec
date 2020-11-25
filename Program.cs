using System;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using Product_Manager.Data;
using Product_Manager.Domain.Models;
using System.Linq;

namespace Product_Manager
{
    class Program
    {

        const string connectionString =
        "Data Source=(local);Initial Catalog=Product_Manager;"
        + "Integrated Security=true";

        static ProductManagerContext Context = new ProductManagerContext();
        static void Main(string[] args)
        {
            bool applicationRunning = true;

            while (applicationRunning)
            {


                MainMenu();

            }
        }

        public static void MainMenu()
        {

            Console.Clear();

            string[] menuItems = { "1. Categories", "2. Articles", "3. Exit" };

            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(menuItem);
            }

            bool validInput = false;
            ConsoleKeyInfo input;
            while (!validInput)
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        CategoriesSubMenu();
                        break;

                    case ConsoleKey.D2:
                        ArticlesSubMenu();
                        break;

                    case ConsoleKey.D3:
                        Exit();
                        break;
                }

            }

        }

        public static void CategoriesSubMenu()
        {
            Console.Clear();

            string[] menuItems = { "1. Add category", "2. List categories", "3. Add product to category","4. Go to main menu" };

            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(menuItem);
            }

            bool validInput = false;
            ConsoleKeyInfo input;
            while (!validInput)
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        AddCategorySubMenu();
                        break;

                    case ConsoleKey.D2:
                        ListCategoriesSubMenu();
                        break;

                    case ConsoleKey.D3:
                        AddProductToCategoryMenu();
                        break;

                    case ConsoleKey.D4:
                        MainMenu();
                        break;
                  
                }

            }
        }

        public static void AddProductToCategoryMenu()
        {
            Console.Clear();

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "SELECT * FROM Categories",
                  connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();



                if (reader.HasRows)
                {
                    Console.WriteLine("ID   Category                     Total products");
                    Console.WriteLine("------------------------------------------------------------");


                    while (reader.Read())
                    {
                        Console.WriteLine($"{ reader["CategoryID"]}    {reader["CategoryName"]}");
                    }

                    Console.Write("\nSelected ID >");
                    string selectedIdInput = Console.ReadLine();
                    int selectedIdNumber;

                    if (Int32.TryParse(selectedIdInput, out selectedIdNumber))
                    {
                        if (doesCategoryExist(selectedIdNumber))
                        {
                            AddCategoryToProduct(selectedIdNumber);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                        Thread.Sleep(2000);
                        CategoriesSubMenu();
                    }







                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                connection.Close();
            }
        }

       


        public static int productCountCategory(string categoryID) 
        {
            int categoryIdNumber;
        

            categoryIdNumber = Int32.Parse(categoryID);

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {

                string query = $"SELECT COUNT(*) AS count FROM ProductCategoryRelation WHERE CategoryID = {categoryIdNumber}";

               
                SqlCommand command = new SqlCommand(
                  query,
                  connection);



         

                connection.Open();

                Int32 count = (Int32)command.ExecuteScalar();

                connection.Close();

                return count;
            }
        }


        

        public static void AddCategoryToProduct(int CategoryID)
        {
            Console.Clear();

            string categoryName = GetCategoryNameByID(CategoryID);

            Console.WriteLine($"Name: {categoryName}");


            Console.WriteLine("\n [A] Add product [ESC] GO back to category list");

            bool validInput = false;
            ConsoleKeyInfo input;
            while (!validInput)
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.A:
                        SearchProductAddToCategory(CategoryID);
                        break;

                    case ConsoleKey.Escape:
                        AddProductToCategoryMenu();
                        break;
                }

            }


        }
        public static void SearchProductAddToCategory(int CategoryID)
        {
            Console.Clear();

            Console.Write("Search product by name: ");

            string productToSearch = Console.ReadLine();

            // Article article = Context.Articles.Where(x => x.ArticleName.Contains(productToSearch));
           // Article article = (Article)Context.Articles.Where(x => x.ArticleName.Contains(productToSearch));

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(



                  $"SELECT * FROM Products WHERE product_name LIKE '%{productToSearch}%'; ",
                  connection);



                //Fungerade ej.
                //   command.Parameters.AddWithValue("@productToSearch", productToSearch);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();



                if (reader.HasRows)
                {
                    Console.WriteLine("ID   Name");
                    Console.WriteLine("-----------------------------------------------");

                    while (reader.Read())
                    {

                        Console.WriteLine($"{reader["ProductID"]}   {reader["product_name"]}");

                    }

                    Console.Write("Product ID>");
                    string chosenProductID = Console.ReadLine();

                    int chosenProductIDNumber;

                     if (Int32.TryParse(chosenProductID, out chosenProductIDNumber))
                    {
                        AddProductToCategory(ProductID:chosenProductIDNumber,CategoryID:CategoryID);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                        Thread.Sleep(2000);
                        SearchProductAddToCategory(CategoryID);
                    }



                   

                   
                }
                else
                {

                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                connection.Close();
            }



        }

        public static void AddProductToCategory (int ProductID,int CategoryID)
        {

            ArticleCategoryRelation artCatRelation;
  
            Article article = Context.Articles.FirstOrDefault(x => x.Id == ProductID);
            Category category = Context.Categories.FirstOrDefault(x => x.Id == CategoryID);

            artCatRelation = new ArticleCategoryRelation(article,category);

            Context.Add(artCatRelation);
            Context.SaveChanges();

            
        }

        public static string GetCategoryNameByID(int CategoryID) 
        {
            Console.Clear();


            string CategoryName = "";

            Category category;

            category = Context.Categories.FirstOrDefault(x => x.Id == CategoryID);



            return category.CategoryName;
        }

        public static void AddCategorySubMenu() 
        {
            Console.Clear();

            Console.WriteLine("Name:");

            Console.SetCursorPosition(7,0);
            string categoryInput;

            categoryInput = Console.ReadLine();

            Category category = new Category(categoryInput);

            if (YesOrNo()) 
            {

                if (doesCategoryExist(categoryInput))
                {
                    Console.WriteLine("\n \n   Category already exists.");
                    Thread.Sleep(2000);
                    CategoriesSubMenu();
                }
                else 
                {

                    Context.Add(category);
                    Context.SaveChanges();

                    Console.WriteLine("\n \n   Category added");
                    Thread.Sleep(2000);

                }

                MainMenu();

              
            }
            else
            {
                AddCategorySubMenu();

            }
        }

        public static bool doesCategoryExist(string category)
        {


            if (Context.Categories.FirstOrDefault(x => x.CategoryName == category) != null)
            {
                return true;

            }
            else
            {
                return false;
            }


        }

        public static bool doesCategoryExist(int CategoryID)
        {

            if (Context.Categories.FirstOrDefault(x => x.Id == CategoryID) != null) 
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public static void ListCategoriesSubMenu() 
        {

            Console.Clear();

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "SELECT * FROM Categories",
                  connection);
                
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();



                if (reader.HasRows)
                {
                    Console.WriteLine("Category                     Total products ");
                    Console.WriteLine("------------------------------------------------------------");


                    while (reader.Read())
                    {

                     int productCount = productCountCategory(reader["CategoryID"].ToString());


                        
                        Console.Write(reader["CategoryName"]);
                        Console.SetCursorPosition(30,Console.CursorTop);
                        Console.Write(productCount);
                        Console.WriteLine("");

                    }

                    Console.WriteLine(" \n [ESC] Return to categories menu");

                    bool validInput = false;
                    ConsoleKeyInfo input;
                    while (!validInput)
                    {
                        input = Console.ReadKey(true);

                        switch (input.Key)
                        {
                            case ConsoleKey.Escape:
                                CategoriesSubMenu();
                                break;

                        
                        }

                    }


                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                connection.Close();
            }
        }
        public static void ArticlesSubMenu()
        {
            Console.Clear();

            string[] menuItems = { "1. Add article", "2. Search Article","3. Go to main menu"};

            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(menuItem);
            }

            bool validInput = false;
            ConsoleKeyInfo input;
            while (!validInput) 
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        AddArticleMenu();
                        break;

                    case ConsoleKey.D2:
                        SearchArticleMenu();
                        break;

                   case ConsoleKey.D3:
                        MainMenu();
                       break;
                }

            }

        }


        public static void AddArticleMenu() 
        {
            Console.Clear();

            Console.WriteLine(@"Article number: ");
            Console.WriteLine(@"          name:");
            Console.WriteLine(@"   Description:");
            Console.WriteLine(@"         Price:");

            Console.SetCursorPosition(16,0);
            string artnumberInput = Console.ReadLine();

            Console.SetCursorPosition(16, 1);
            string productNameInput = Console.ReadLine();

            Console.SetCursorPosition(16, 2);
            string productDescriptionInput = Console.ReadLine();

            Console.SetCursorPosition(16, 3);
            string productPriceInput = Console.ReadLine();

            int productPriceInputAsNumber;

           

            if (Int32.TryParse(productPriceInput, out productPriceInputAsNumber))
            {
               
            }
            else
            {
                throw new ArgumentException("Invalid price, must be a number");
            }

            Article article = new Article(ArticleNumber: artnumberInput, ArticleName: productNameInput, Description: productDescriptionInput, Price: productPriceInputAsNumber);
                

            if (YesOrNo())
            {

                AddArticle(article);
            }
            else
            {
                AddArticleMenu();
            }

            ArticlesSubMenu();

        }

        public static void AddArticle(Article article)
        {
          
            if (Context.Articles.FirstOrDefault(x=> x.ArticleNumber ==  article.ArticleNumber) != null) 
            {
                Console.WriteLine("Article already exists");

            }
            else 
            {
                Context.Add(article);
                Context.SaveChanges();
            }

            Thread.Sleep(2000);



        }

        public static bool doesArticleExist(string articleNumber)
        {

            if (Context.Articles.FirstOrDefault(x => x.ArticleNumber == articleNumber) != null)
            {
                return true;

            }
            else
            {
                return false;
            }


        }

        public static void SearchArticleMenu()
        {
            Console.Clear();

            Console.WriteLine($"Article number:");

            Console.SetCursorPosition(16,0);

            string articleNumberInput = Console.ReadLine();

            if (doesArticleExist(articleNumberInput))
            {
                SearchArticleFoundMenu(articleNumberInput);
                
            }
            else
            {
                Console.WriteLine("Article not found");
                Thread.Sleep(2000);
            }

            ArticlesSubMenu();
        }

        public static void SearchArticleFoundMenu(string articleNumber) 
        {
            Console.Clear();


            ReturnFoundArticle(articleNumber, out Article article);


            Console.WriteLine($"Article number: {articleNumber}");
            Console.WriteLine($"          Name: {article.ArticleName}");
            Console.WriteLine($"   Description: {article.Description}");
            Console.WriteLine($"         Price: {article.Price}");



            Console.WriteLine("\n\n");
            
            Console.WriteLine(" [E] Edit  [D] Delete  [Esc] Main menu");

            bool validInput = false;
            ConsoleKeyInfo input;
            while (!validInput)
            {
                input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.Escape:
                        ArticlesSubMenu();
                        break;

                    case ConsoleKey.E:
                        EditArticleMenu(articleNumber);
                        break;
                    case ConsoleKey.D:
                        DeleteArticle(articleNumber);
                        break;
                }



            }




        }

        public static void EditArticleMenu(string articleNumber)
        {
            Console.Clear();

            Console.WriteLine($"Article number: {articleNumber} ");
            Console.WriteLine(@"          name:");
            Console.WriteLine(@"   Description:");
            Console.WriteLine(@"         Price:");

            Console.SetCursorPosition(16, 1);
            string productNameInput = Console.ReadLine();

            Console.SetCursorPosition(16, 2);
            string productDescriptionInput = Console.ReadLine();

            Console.SetCursorPosition(16, 3);
            string productPriceInput = Console.ReadLine();

            int productPriceInputAsNumber;

            if (!Int32.TryParse(productPriceInput, out productPriceInputAsNumber))
            {
                throw new ArgumentException("Invalid price, must be a number");
            }


            Article article = new Article(ArticleNumber: articleNumber, ArticleName: productNameInput, Description: productDescriptionInput, Price: productPriceInputAsNumber);


            if (YesOrNo())
            {             
                UpdateArticle(article);
            }
            else
            {
                EditArticleMenu(articleNumber);
            }

            ArticlesSubMenu();


        }

        public static void DeleteArticle(string articleNumber)
        {
            if (YesOrNo("Delete this article? (Y)es (N)o"))
            {

                Article article = Context.Articles.FirstOrDefault(x => x.ArticleNumber == articleNumber);

                Context.Remove(article);
                Console.WriteLine("\nArticle deleted");
                Thread.Sleep(2000);
                ArticlesSubMenu();




            }
            else
            {
                SearchArticleFoundMenu(articleNumber);
            }
        }

        public static void ReturnFoundArticle(string articleNumber,out Article article)
        {

            //Value needs to be assigned due to using out param
            article = null;


            article = Context.Articles.FirstOrDefault(x => x.ArticleNumber == articleNumber);


        }


        public static void UpdateArticle(Article article) 
        {

            
            try
            {
                Context.Update(article);
                Context.SaveChanges();

                Console.WriteLine("Article saved");
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Thread.Sleep(2000);



        }

        public static void Exit()
        {
            System.Environment.Exit(1);
        }

    
        public static bool YesOrNo(string text = "\nIs this correct? (Y)es (N)o")
        {
            Console.WriteLine("\n" + text);

            bool validInput = false;

            bool userChoice = false;

            while (!validInput)
            {
                var userInput = Console.ReadKey(true);

                switch (userInput.Key)
                {
                    case ConsoleKey.Y:
                        validInput = true;
                        userChoice = true;
                        break;
                    case ConsoleKey.N:
                        validInput = true;
                        userChoice = false;
                        break;

                    default:
                        break;


                }
            }

            return userChoice;

        }
    }
}
