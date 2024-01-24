using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            // Declare a dictionary for available flavours and toppings
            Dictionary<string, int> flavourDictAvailability = new Dictionary<string, int>();
            Dictionary<string, int> toppingDictAvailability = new Dictionary<string, int>();

            // Read flavours.csv file
            ReadFlavoursToppingsFiles(flavourDictAvailability, "flavours.csv");
            ReadFlavoursToppingsFiles(toppingDictAvailability, "toppings.csv");

            void ReadFlavoursToppingsFiles(Dictionary<string,int> dict, string path)
            {
                string[] csvLines = File.ReadAllLines(path);
                for (int i = 1; i < csvLines.Length; i++)
                {
                    string[] info = csvLines[i].Split(',');
                    int cost = Convert.ToInt16(double.Parse(info[1])); // Convert string to double then to int

                    //  Add information to the individual dictionary
                    dict.Add(info[0].ToLower(), cost);
                }
            }

            // Declare a Dictionary for customer objects
            Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
            
            void InitialiseHistory()
            {
                /*  Read customers.csv file */
                string[] csvLines = File.ReadAllLines("customers.csv");
                for (int i = 1; i < csvLines.Length; i++)
                {
                    string[] info = csvLines[i].Split(',');

                    string name = info[0];
                    int memberId = Convert.ToInt32(info[1]);
                    DateTime dob = Convert.ToDateTime(info[2]);
                    string membershipStatus = info[3];
                    int membershipPoints = Convert.ToInt32(info[4]);
                    int punchCard = Convert.ToInt32(info[5]);

                    // Create a customer object
                    Customer customer = new Customer(info[0], Convert.ToInt32(info[1]), Convert.ToDateTime(info[2]));

                    // Add associated information(rewards) to the customer object 
                    customer.Rewards = new PointCard(membershipPoints, punchCard)
                    {
                        Tier = membershipStatus
                    };

                    // Add the customer object to the collection
                    customerDict.Add(customer.MemberId, customer);
                }

                /*  Read orders.csv file    */
                csvLines = File.ReadAllLines("orders.csv");
                for (int i = 1; i < csvLines.Length; i++)
                {
                    string[] info = csvLines[i].Split(',');

                    int orderId = Convert.ToInt32(info[0]);
                    int memberId = Convert.ToInt32(info[1]);
                    DateTime timeReceived = Convert.ToDateTime(info[2]);
                    DateTime timeFulfilled = Convert.ToDateTime(info[3]);
                    string option = info[4];
                    int scoops = Convert.ToInt32(info[5]);
                    
                    //  dipped = Convert.ToBoolean(info[6].ToLower());
                    //  waffleFlavour = info[7];
                    //  flavour1 = info[8];
                    //  flavour2 = info[9];
                    //  flavour3 = info[10];
                    //  topping1 = info[11];
                    //  topping2 = info[12];
                    //  topping3 = info[13];
                    //  topping4 = info[14];

                    // Create a temperary list for flavours
                    List<Flavour> flavourList = new List<Flavour>();
                    List<Topping> toppingList = new List<Topping>();

                    // Consolidating flavours and toppings into a list
                    for (int j = 8; j <= 14; j++)   
                    {
                        //  Check if the string is a available flavour
                        if (flavourDictAvailability.ContainsKey(info[j].ToLower()))
                        {
                            string flavour = info[j].ToLower();
                            bool isPremium = false;
                            bool isDuplicate = false;

                            // Check for premium flavours
                            int cost = flavourDictAvailability[flavour];    
                            if (cost == 2) 
                            { isPremium = true; }

                            // Check for duplicate flavours
                            foreach (Flavour f in flavourList)
                            {
                                if (f.Type == flavour)
                                {
                                    f.Quantity += 1;
                                    isDuplicate = true;
                                }
                            }

                            if (!isDuplicate)
                            { 
                                flavourList.Add(new Flavour(flavour, isPremium, 1)); // Add the flavour object to the list
                            }  
                        }

                        //  Check if the string is a available topping
                        else if (toppingDictAvailability.ContainsKey(info[j].ToLower()))
                        {
                            string topping = info[j].ToLower();
                            toppingList.Add(new Topping(topping));    // Create a topping object
                        }
                    }

                    /*  Creating IceCream object    */

                    IceCream iceCream = null;

                    if (option == "Cup")
                    {
                        iceCream = new Cup(option,scoops,flavourList,toppingList);
                    }
                    else if (option == "Cone")
                    {
                        bool dipped= Convert.ToBoolean(info[6].ToLower());
                        iceCream = new Cone(option, scoops, flavourList, toppingList, dipped);
                    }
                    else if (option == "Waffle")
                    {
                        string waffleFlavour = info[7];
                        iceCream = new Waffle(option, scoops, flavourList, toppingList, waffleFlavour);
                    }
                        
                    /*  creating Order objects  */

                    List<Order> orderHistoryList = customerDict[memberId].OrderHistory; //  Get the order history list from the customer object

                    //  Check if there if the ice cream is from the same existing order
                    bool isDuplicateOrder = false;
                    foreach(Order order in orderHistoryList)
                    {
                        if (order.Id == orderId)
                        {
                            order.AddIceCream(iceCream);    //  Add the ice cream object IceCreamList using class method
                            isDuplicateOrder = true;
                            break;
                        }
                    }

                    if (!isDuplicateOrder)
                    {
                        //  Create a new order object
                        Order order = new Order(orderId, timeReceived)
                        {
                            TimeFulfilled = timeFulfilled
                        };
                        order.AddIceCream(iceCream);    //  Add the ice cream object IceCreamList using class method
                        
                        //  Add the order object to the individual customer object
                        orderHistoryList.Add(order);
                    }
                }
            }

            InitialiseHistory();  //  Initialise customers Object and Order History

            foreach( Customer customer in customerDict.Values)
            {
                Console.WriteLine(customer.ToString());
                displayOrder(customer.OrderHistory.ToArray());
            }
            /*foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                Customer customer = kvp.Value;
                Console.WriteLine("-------------------------------------------------------------------------------------");
                Console.WriteLine(customer.ToString());
                foreach (Order order in customer.OrderHistory)
                {
                    Console.WriteLine(order.ToString());
                    for ( int j = 0; j<order.IceCreamList.Count; j++ )
                    {
                        Console.WriteLine($"{j+1}. " + order.IceCreamList[j].ToString());
                    }
                    Console.WriteLine();
                }
            }*/

            /*List all customers
             display the information of all the customers
            2) List all current orders
             display the information of all current orders in both the gold members and regular queue
            3) Register a new customer
             prompt user for the following information for the customer: name, id number, date of birth
             create a customer object with the information given
             create a Pointcard object
             assign Pointcard object to the customer
             append the customer information to the customers.csv file
             display a message to indicate registration status
            4) Create a customer’s order
             list the customers from the customers.csv
             prompt user to select a customer and retrieve the selected customer
             create an order object
             prompt user to enter their ice cream order (option, scoops, flavours, toppings)
             create the proper ice cream object with the information given
             add the ice cream object to the order
             prompt the user asking if they would like to add another ice cream to the order, repeating
            the previous three steps if [Y] or continuing to the next step if [N]
             link the new order to the customer’s current order
             if the customer has a gold-tier Pointcard, append their order to the back of the gold
            members order queue. Otherwise append the order to the back of the regular order queue
             display a message to indicate order has been made successfully
            Year 2023/24 Assignment - 6 -
            PRG2 (IT, CSF, DS) Last Update: 05/01/2023
             Official Open
            5) Display order details of a customer
             list the customers
             prompt user to select a customer and retrieve the selected customer
             retrieve all the order objects of the customer, past and current
             for each order, display all the details of the order including datetime received, datetime
            fulfilled (if applicable) and all ice cream details associated with the order
            6) Modify order details
             list the customers
             prompt user to select a customer and retrieve the selected customer’s current order
             list all the ice cream objects contained in the order
             prompt the user to either [1] choose an existing ice cream object to modify, [2] add an
            entirely new ice cream object to the order, or [3] choose an existing ice cream object to
            delete from the order
            o if [1] is selected, have the user select which ice cream to modify then prompt the user
            for the new information for the modifications they wish to make to the ice cream
            selected: option, scoops, flavours, toppings, dipped cone (if applicable), waffle flavour
            (if applicable) and update the ice cream object’s info accordingly
            o if [2] is selected prompt the user for all the required info to create a new ice cream
            object and add it to the order
            o if [3] is selected, have the user select which ice cream to delete then remove that ice
            cream object from the order. But if this is the only ice cream in the order, then simply
            display a message saying they*/

            //  Create a queue for 2 type of membership
            Queue<Order> goldQueue = new Queue<Order>();
            Queue<Order> normalQueue = new Queue<Order>();

            void displayMenu()
            {
                while (true)
                {
                    int option;
                    while (true)
                    {
                        try
                        {
                            //  Input validation
                            Console.WriteLine("---------- Welcome to Our Ice Cream Store ----------");
                            Console.WriteLine("[1] List all customers");
                            Console.WriteLine("[2] List all current orders");
                            Console.WriteLine("[3] Register a new customer");
                            Console.WriteLine("[4] Create a customer's order");
                            Console.WriteLine("[5] Display order details of a customer");
                            Console.WriteLine("[6] Modify order details");
                            Console.WriteLine("[0] Exit program");
                            Console.WriteLine("----------------------------------------------------");
                            Console.Write("Enter your option: ");
                            option = int.Parse(Console.ReadLine().Trim());

                            if (option<0 || option>6)
                            {
                                Console.WriteLine("Please enter an option between 0 and 6. Try Again");
                                continue;
                            }
                            break;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Please enter a valid integer. try again.");
                        }
                    }

                    if (option == 0) { break; } //  Exit the program
                    else if (option == 1) { option1(); }
                    else if (option == 2) { option2(); }
                    else if (option == 3) { option3(); }
                    else if (option == 4) { option4(); }
                    else if (option == 5) { option5(); }
                    else {  }
                }
            }
            displayMenu();

            void option1()
            {
                Console.WriteLine();
                foreach (KeyValuePair<int, Customer> kvp in customerDict)
                {
                    Customer customer = kvp.Value;
                    Console.WriteLine(customer.ToString());
                }
                Console.WriteLine();
            }

            void option2()
            {
                Console.WriteLine();
                Console.WriteLine("--------------- Gold Members Queue ---------------");
                displayOrder(goldQueue.ToArray());
                Console.WriteLine("--------------- Normal Members Queue ---------------");
                displayOrder(normalQueue.ToArray());

                Console.WriteLine();
            }

            void displayOrder(Order[] orderArray)
            {
                Console.WriteLine();
                if (orderArray.Length != 0) //  Check if the queue taken in is empty
                {
                    foreach (Order order in orderArray)
                    {
                        Console.WriteLine(order.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("There is no order here.\n");
                }
            }

            void option3()
            {
                string name;
                int memberId;
                DateTime dob = DateTime.Now;
                while (true)
                {
                    Console.Write("Enter customer name: ");
                    name = Console.ReadLine();

                    //  Validate user inputs
                    try
                    {
                        Console.Write("Enter customer Id: ");
                        memberId = int.Parse(Console.ReadLine().Trim());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid Member Id. Try again.");
                        continue;
                    }
                    try
                    {
                        Console.Write("Enter customer Date of Birth in this format (dd/MM/yyyy): ");
                        dob = DateTime.Parse(Console.ReadLine().Trim());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid Date of Birth in this format (dd/MM/yyyy). Try again.");
                        continue;
                    }
                    break;
                }

                //  Create new customer object
                Customer newCustomer = new Customer(name, memberId, dob);

                //  Create new PointCard object
                PointCard newPointCard = new PointCard(0, 0)
                {
                    Tier = "Ordinary"
                };

                //  Add PointCard object to the customer object
                newCustomer.Rewards = newPointCard;

                //  Add the customer object to the dictionary
                customerDict.Add(newCustomer.MemberId, newCustomer);

                //  Append the customer information to the customers.csv file
                using (StreamWriter sw = new StreamWriter("customers.csv", true))
                {
                    sw.WriteLine($"{newCustomer.Name},{newCustomer.MemberId},{newCustomer.Dob},{newCustomer.Rewards.Tier},{newCustomer.Rewards.Points},{newCustomer.Rewards.PunchCard}");
                }
                Console.WriteLine("Registration is successful.\n");
            }

            void option4()
            {
                //  List the customers
                option1();

                //  Prompt user to select a customer and retrieve the selected customer
                int memberId;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter the customer Id: ");
                        memberId = int.Parse(Console.ReadLine().Trim());
                        
                        if (customerDict.ContainsKey(memberId))
                        { break; }
                        else
                        { Console.WriteLine("Please enter a valid customer Id. Try again."); }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid customer Id in integer. Try again.");
                    }
                }

                //  Retrieve the customer object
                Customer customer = customerDict[memberId];
                List<IceCream> iceCreamList= new List<IceCream>();

                bool repeat = false;
                do
                {
                    iceCreamList.Add(makeiceCream()); //  Create IceCream Object and it to the list

                    // Ask if the user would like to add another ice cream to the order
                    while (true)
                    {
                        Console.Write("\nWould you like to add another ice cream to the order? (Y/N): ");
                        string reply= Console.ReadLine().Trim().ToLower();

                        if (reply == "n")
                        { repeat = false;  break; }
                        else if (reply == "y")
                        { repeat = true; break; }
                        else
                        { Console.WriteLine("Please enter a valid input (Y/N). Try again."); }
                    }
                    
                } while (repeat);

                Order newOrder = customer.MakeOrder();  //  Create an empty order object for CurrentOrder inside the customer object

                newOrder.IceCreamList.AddRange(iceCreamList);   //  Add the ice cream list to the order object
                customer.CurrentOrder = newOrder;   //  Add the order object to the customer object

                if (customer.Rewards.Tier == "Gold")
                {
                    goldQueue.Enqueue(newOrder);   //  Add the order object to the gold queue
                }
                else
                {
                    normalQueue.Enqueue(newOrder);  //  Add the order object to the normal queue
                }

                Console.WriteLine("\nOrder has been made successfully.\n");
            }

            IceCream makeiceCream()
            {
                string displayAvailableOption(Dictionary<string,int> dictOptions) //  Display available Flavours and Toppings
                {
                    List<string> list = new List<string>();
                    Console.WriteLine();
                    foreach(string key in dictOptions.Keys)
                    {
                        list.Add(key);
                    }
                    return $"{string.Join(", ",list)}";
                }
                
                //  Prompt user to enter their ice cream order (option, scoops, flavours, toppings)
                string option;
                int scoops;
                List<Flavour> flavourList = new List<Flavour>();
                int numToppings;
                List<Topping> toppingList = new List<Topping>();
                    
                /*  Input validation for options    */
                while (true)
                {
                    try
                    {
                        Console.Write("\nEnter the option (Cup, Cone, Waffle): ");
                        option = Console.ReadLine().Trim().ToLower();

                        if (option != "cup" && option != "cone" && option != "waffle")  //  Check if the option is an available option
                        {
                            Console.WriteLine("Please enter a valid option. Try again.");
                        }
                        else { break; }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid option (Cup, Cone, Waffle). Try again.");
                    }
                }

                /*  Input validation for number of scoops   */
                while (true)
                {
                    try
                    {
                        Console.Write("\nEnter the number of scoops (Max 3 scoops): ");
                        scoops = int.Parse(Console.ReadLine().Trim());

                        if (scoops >= 1 && scoops <= 3) //  Check if the number of scoops is between 1 and 3
                        { break; }
                        else
                        { Console.WriteLine("Please enter a valid number of scoops from 1 to 3. Try again."); }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid number of scoops in integer. Try again.");
                    }
                }

                /*  Input validation for flavours   */
                for (int i=0; i<scoops; i++)
                {
                    while (true)
                    {
                        Console.Write($"Available Flavourrs: [{displayAvailableOption(flavourDictAvailability)}]");   //  Display available Flavours
                        
                        Console.Write($"\nEnter ice cream flavour for scoop {i + 1} : ");
                        string flavour = Console.ReadLine().Trim().ToLower();

                        //  Check for duplicate flavours
                        bool isDuplicate = false;
                        foreach (Flavour f in flavourList)
                        {
                            if (f.Type == flavour)
                            {
                                f.Quantity += 1;
                                isDuplicate = true;
                            }
                        }
                        if (isDuplicate) { break; }

                        //  Check if the string is a available flavour
                        if (flavourDictAvailability.ContainsKey(flavour))
                        {
                            // Check for premium flavours
                            bool isPremium = false;
                            if (flavourDictAvailability[flavour] == 2)
                            { isPremium = true; }

                            flavourList.Add(new Flavour(flavour, isPremium, 1));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid flavour. Try again.");
                        }
                            
                    }
                }

                /*  Input validation for number of toppings */
                while (true)
                {
                    try
                    {
                        Console.Write("\nEnter number of toppings (Max 4 toppings): ");
                        numToppings = int.Parse(Console.ReadLine().Trim());

                        if(numToppings >= 0  && numToppings <= toppingDictAvailability.Count) //  Check if the number of toppings is between 0 and 4
                        { break; }
                        else 
                        { Console.WriteLine("Please enter a valid number of toppings from 0 to 4. Try again."); }
                    }
                    catch(FormatException)
                    {
                        Console.WriteLine("Please enter a valid number of toppings in integer. Try again.");
                    }       
                }

                if (numToppings == toppingDictAvailability.Count)   //  Check if the number of toppings is equal to the number of available toppings
                {
                    foreach (string key in toppingDictAvailability.Keys)
                    {
                        toppingList.Add(new Topping(key));
                    }
                }
                else
                {
                    //  Input validation for toppings
                    for (int i = 0; i < numToppings; i++)
                    {
                        while (true)
                        {
                            Console.Write($"Available Toppings: [{displayAvailableOption(toppingDictAvailability)}]");   //  Display available Toppings

                            Console.Write($"\nEnter topping {i + 1}: ");
                            string topping = Console.ReadLine().Trim().ToLower();

                            //  Check for duplicate toppings
                            bool isDuplicate = false;
                            foreach (Topping t in toppingList)
                            {
                                if (t.Type == topping)
                                { isDuplicate = true; }
                            }
                            if (isDuplicate) 
                            {
                                Console.WriteLine("You have already added this topping. Try again.");
                                continue;
                            }

                            //  Check if the topping is an available topping
                            if (toppingDictAvailability.ContainsKey(topping))
                            {
                                toppingList.Add(new Topping(topping));
                                break;
                            }
                            else
                            { Console.WriteLine("Please enter a valid topping. Try again."); }
                        }
                    }
                }

                /*  Creating IceCream object    */
                IceCream iceCream = null;
                if (option == "cup")
                {
                    iceCream = new Cup(option, scoops, flavourList, toppingList);   //  Create an ice cream object
                }
                else if (option == "cone")
                {
                    //  Input validation for dipped
                    bool dipped;
                    while (true)
                    {
                        try
                        {
                            Console.Write("\nIs the cone dipped in chocolate? (true/false): ");
                            dipped = bool.Parse(Console.ReadLine().Trim());
                            break;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Please enter a valid input (true/false). Try again.");
                        }
                    }
                    iceCream = new Cone(option, scoops, flavourList, toppingList, dipped);  //  Create an ice cream object
                }
                else if (option == "waffle")
                {
                    string waffleFlavour;
                    while (true)
                    {
                        Console.WriteLine("\nAvailable waffle flavours: [Red Velvet, Charcoal, Pandan]");
                        Console.Write("Enter the waffle flavour: ");
                        waffleFlavour = Console.ReadLine().Trim().ToLower();
                        
                        if ( waffleFlavour == "red velvet" || waffleFlavour == "charcoal" || waffleFlavour == "pandan") //  Check if the string is a available flavour
                        { break; }
                        else
                        { Console.WriteLine("Please enter a valid waffle flavour. Try again."); }
                    }
                    iceCream = new Waffle(option, scoops, flavourList, toppingList, waffleFlavour); //  Create an ice cream object
                }

                return iceCream;    //  Return the ice cream object
            }

            void option5()
            {
                //  List the customers
                option1();

                //  Prompt user to select a customer and retrieve the selected customer
                int memberId;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter the customer Id: ");
                        memberId = int.Parse(Console.ReadLine().Trim());

                        if (!customerDict.ContainsKey(memberId))
                        {
                            Console.WriteLine("Please enter a valid customer Id. Try again.");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid customer Id in integer. Try again.");
                    }
                }

                //  Retrieve the order objects
                List<Order> orderHistory = customerDict[memberId].OrderHistory;
                Console.WriteLine("--------------- Order History ---------------");
                displayOrder(orderHistory.ToArray());

                Console.WriteLine("--------------- Gold Queue ----------------");
                displayOrder(goldQueue.ToArray());

                Console.WriteLine("--------------- Normal Queue ---------------");
                displayOrder(normalQueue.ToArray());

                Console.WriteLine();
            }

            void option6()
            {
                //  List the customers
                option1();

                //  Prompt user to select a customer and retrieve the selected customer
                int memberId;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter the customer Id: ");
                        memberId = int.Parse(Console.ReadLine().Trim());

                        if (!customerDict.ContainsKey(memberId))
                        {
                            Console.WriteLine("Please enter a valid customer Id. Try again.");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid customer Id in integer. Try again.");
                    }
                }

                //  Retrieve the current order objects
                Order[] currentOrder = { customerDict[memberId].CurrentOrder };

                //  List all the ice cream objects and details contained in the current order
                

                //  Prompt the user to either [1] choose an existing ice cream object to modify, [2] add an entirely new ice cream object to the order, or [3] choose an existing ice cream object to delete from the order
                int option;
                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.WriteLine("What would you like to do?");
                        Console.WriteLine("[1] Modify an existing ice cream");
                        Console.WriteLine("[2] Add a new ice cream");
                        Console.WriteLine("[3] Delete an existing ice cream");
                        Console.Write("Enter your option: ");
                        option = int.Parse(Console.ReadLine().Trim());

                        if (option < 1 || option > 3)
                        {
                            Console.WriteLine("Please enter an option between 1 and 3. Try Again");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid integer. try again.");
                    }
                }

                if (option == 1)
                {

                }


            }


            Console.ReadLine();
        }
    }
}
