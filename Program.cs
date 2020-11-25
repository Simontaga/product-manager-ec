using System;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using Product_Manager.Domain.Models;

namespace Product_Manager
{
    class Program
    {
        const string connectionString =
            "Data Source=(local);Initial Catalog=Product_Manager;"
            + "Integrated Security=true";

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

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "INSERT INTO ProductCategoryRelation VALUES(@ProductID,@CategoryID)",
                  connection);
                command.Parameters.AddWithValue("@CategoryID", CategoryID);
                command.Parameters.AddWithValue("@ProductID", ProductID);
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();

                    Console.WriteLine("\n Product added to category");
                    Thread.Sleep(2000);
                    MainMenu();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Product is already in this category");
                    Thread.Sleep(2000);
                    MainMenu();
                }
                
                
                connection.Close();
            }
        }

        public static string GetCategoryNameByID(int CategoryID) 
        {
            Console.Clear();


            string CategoryName = "";

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "SELECT CategoryName FROM Categories WHERE ([CategoryID] = @categoryID) ",
                  connection);
                command.Parameters.AddWithValue("@categoryID",CategoryID);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();



                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        CategoryName = (string)reader["CategoryName"];

                        
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                connection.Close();
            }

            return CategoryName;
        }

        public static void AddCategorySubMenu() 
        {
            Console.Clear();

            Console.WriteLine("Name:");

            Console.SetCursorPosition(7,0);
            string categoryInput;

            categoryInput = Console.ReadLine();


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
                    using (SqlConnection connection = new SqlConnection(connectionString))

                    {

                        try
                        {
                            connection.Open();



                            string sql = "INSERT INTO Categories VALUES(@CategoryName)";


                            SqlCommand insert_category = new SqlCommand(sql, connection);


                            insert_category.Parameters.AddWithValue("@CategoryName", categoryInput);

                            insert_category.ExecuteNonQuery();

                            Console.WriteLine("\n \n   Category added");
                            Thread.Sleep(2000);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        connection.Close();

                        MainMenu();
                    }
                }

               
            }
            else 
            {
                AddCategorySubMenu();
                
            }

        }

        public static bool doesCategoryExist(string category)
        {


            bool matchFound = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();


                    SqlCommand check_category_exists = new SqlCommand("SELECT CategoryName FROM Categories WHERE ([CategoryName] = @categoryName)", connection);
                    check_category_exists.Parameters.AddWithValue("@categoryName", category);
                    SqlDataReader reader = check_category_exists.ExecuteReader();
                    if (reader.HasRows)
                    {
                        matchFound = true;
                        
                    }
                    else
                    {
                        matchFound = false;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();

                return matchFound;
            }



        }

        public static bool doesCategoryExist(int CategoryID)
        {


            bool matchFound = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();


                    SqlCommand check_category_exists = new SqlCommand("SELECT CategoryName FROM Categories WHERE ([CategoryID] = @categoryID)", connection);
                    check_category_exists.Parameters.AddWithValue("@categoryID", CategoryID);
                    SqlDataReader reader = check_category_exists.ExecuteReader();
                    if (reader.HasRows)
                    {
                        matchFound = true;

                    }
                    else
                    {
                        matchFound = false;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();

                return matchFound;
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
            string queryString = $"INSERT INTO Products values('{article.ArticleNumber}','{article.ArticleName}','{article.Description}',{article.Price});";

            
            if (doesArticleExist(article.ArticleName))
            {
                Console.WriteLine("Article already exists");
               
            }
            else
            {
                ExecuteQuery(queryString);
                Console.WriteLine("Article added");
                 
            }

            Thread.Sleep(2000);

        }

        public static bool doesArticleExist(string articleNumber)
        {

            
            bool matchFound = false;
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();


                    SqlCommand check_article_exists = new SqlCommand("SELECT article_number FROM Products WHERE ([article_number] = @art_num)", connection);
                    check_article_exists.Parameters.AddWithValue("@art_num", articleNumber);
                    SqlDataReader reader = check_article_exists.ExecuteReader();
                    if (reader.HasRows)
                    {
                        matchFound = true;
                    }
                    else
                    {
                        matchFound = false;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();

                return matchFound;
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
                string queryString = $"DELETE FROM Products WHERE article_number='{articleNumber}';";

                ExecuteQuery(queryString);

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


            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "SELECT article_number, product_name,product_description,product_price FROM Products WHERE ([article_number] = @art_num)",
                  connection);
                command.Parameters.AddWithValue("@art_num", articleNumber);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                   
                        string articleNumberFound = reader.GetString(0);
                        string productNameFound = reader.GetString(1);
                        string productDescriptionFound = reader.GetString(2);
                        int productPriceFound = reader.GetInt32(3);


                        article = new Article(ArticleNumber: articleNumberFound, ArticleName: productNameFound, Description: productDescriptionFound, Price: productPriceFound);


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


        public static void UpdateArticle(Article article) 
        {

            string queryString = @$"UPDATE Products
            SET product_name = '{article.ArticleName}', product_description = '{article.Description}',product_price = {article.Price}
            WHERE article_number = '{article.ArticleNumber}'; ";

            try
            {
                ExecuteQuery(queryString);

                Console.WriteLine("Article saved");
                Thread.Sleep(2000);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public static void Exit()
        {
            System.Environment.Exit(1);
        }

        public static void ExecuteQuery(string queryString)
        {
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {

             
                SqlCommand command = new SqlCommand(queryString, connection);
                
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
           

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }
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
