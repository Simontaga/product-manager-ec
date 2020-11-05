using System;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;

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

            string[] menuItems = { "1. Add category", "2. List categories", "3. Go to main menu" };

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
                        MainMenu();
                        break;
                }

            }
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
                
            }
            else 
            {
                AddCategorySubMenu();
                
            }

        }

        public static void ListCategoriesSubMenu() 
        {
            
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


            if (YesOrNo())
            {
                AddArticle(articleNumber:artnumberInput,productName:productNameInput,
                productDescription:productDescriptionInput,productPrice:productPriceInputAsNumber);
            }
            else
            {
                AddArticleMenu();
            }

            ArticlesSubMenu();

        }

        public static void AddArticle(string articleNumber,string productName,string productDescription,int productPrice)
        {
            string queryString = $"INSERT INTO products values('{articleNumber}','{productName}','{productDescription}',{productPrice});";

            
            if (doesArticleExist(articleNumber))
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


                    SqlCommand check_article_exists = new SqlCommand("SELECT article_number FROM products WHERE ([article_number] = @art_num)", connection);
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


            ReturnFoundArticle(articleNumber, out articleNumber, out string productName, out string productDescription, out int productPrice);


            Console.WriteLine($"Article number: {articleNumber}");
            Console.WriteLine($"          Name: {productName}");
            Console.WriteLine($"   Description: {productDescription}");
            Console.WriteLine($"         Price: {productPrice}");



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
            


            if (YesOrNo())
            {             
                UpdateArticle(articleNumber,productNameInput,productDescriptionInput,productPriceInputAsNumber);
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
                string queryString = $"DELETE FROM products WHERE article_number='{articleNumber}';";

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

        public static void ReturnFoundArticle(string articleNumber,out string articleNumberFound,out string productNameFound,out string productDescriptionFound,out int productPriceFound)
        {

            //   string articleNumberFound;
            //  string productNameFound;
            // string productDescriptionFound;
            // int productPriceFound;
            articleNumberFound = "";
            productNameFound = "";
            productDescriptionFound = "";
            productPriceFound = 0;


            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                  "SELECT article_number, product_name,product_description,product_price FROM products WHERE ([article_number] = @art_num)",
                  connection);
                command.Parameters.AddWithValue("@art_num", articleNumber);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       
                        articleNumberFound = reader.GetString(0);
                        productNameFound = reader.GetString(1);
                        productDescriptionFound = reader.GetString(2);
                        productPriceFound = reader.GetInt32(3);


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


        public static void UpdateArticle(string articleNumber,string productName,string productDescription,int productPrice) 
        {

            string queryString = @$"UPDATE products
            SET product_name = '{productName}', product_description = '{productDescription}',product_price = {productPrice}
            WHERE article_number = '{articleNumber}'; ";

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
                   
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();

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
