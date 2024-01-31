using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace S10257825_PRG2Assignment
{
    internal class Program
    {
        // Declare a dictionary for available flavours and toppings
        public static Dictionary<string, int> flavourDictAvailability = new Dictionary<string, int>();
        public static Dictionary<string, int> toppingDictAvailability = new Dictionary<string, int>();

        public static void InitialiseHistory(Dictionary<int, Customer> customerDict)
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

                        if (!isDuplicate)
                        {
                            flavourList.Add(new Flavour(flavour, isPremium)); // Add the flavour object to the list
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
                    iceCream = new Cup(option, scoops, flavourList, toppingList);
                }
                else if (option == "Cone")
                {
                    bool dipped = Convert.ToBoolean(info[6].ToLower());
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
                foreach (Order order in orderHistoryList)
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
        public static void Main(string[] args)
        {
            // Read flavours.csv file
            ReadFlavoursToppingsFiles(flavourDictAvailability, "flavours.csv");
            ReadFlavoursToppingsFiles(toppingDictAvailability, "toppings.csv");

            void ReadFlavoursToppingsFiles(Dictionary<string, int> dict, string path)
            {
                string[] csvLines = File.ReadAllLines(path);
                for (int i = 1; i < csvLines.Length; i++)
                {
                    string[] info = csvLines[i].Split(',');
                    int cost = (int)double.Parse(info[1]); // Convert string to double then to int to remove decimal points

                    //  Add information to the individual dictionary
                    dict.Add(info[0].ToLower(), cost);
                }
            }

            // Declare a Dictionary for customer objects
            Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();

            InitialiseHistory(customerDict);  //  Initialise customers Object and Order 

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
            display a message saying they
            4. ADVANCED FEATURES – 20% INDIVIDUAL
            (a) Process an order and checkout
             dequeue the first order in the queue
             display all the ice creams in the order
             display the total bill amount
             display the membership status & points of the customer
             check if it is the customer’s birthday, and if it is, calculate the final bill while having the
            most expensive ice cream in the order cost $0.00
             check if the customer has completed their punch card. If so, then calculate the final bill
            while having the first ice cream in their order cost $0.00 and reset their punch card back
            to 0
             check Pointcard status to determine if the customer can redeem points. If they cannot,
            skip to displaying the final bill amount
            Year 2023/24 Assignment - 7 -
            PRG2 (IT, CSF, DS) Last Update: 05/01/2023
             Official Open
             if the customer is silver tier or above, prompt user asking how many of their points they
            want to use to offset their final bill
             redeem points, if necessary
             display the final total bill amount
             prompt user to press any key to make payment
             increment the punch card for every ice cream in the order (if it goes above 10 just set it
            back down to 10)
             earn points
             while earning points, upgrade the member status accordingly
             mark the order as fulfilled with the current datetime
             add this fulfilled order object to the customer’s order history
            Display monthly charged amounts breakdown & total charged amounts for the year
             prompt the user for the year
             retrieve all order objects that were successfully fulfilled within the inputted year
             compute and display the monthly charged amounts breakdown & the total charged
            amounts for the input year
            */

            //  Create a queue for 2 type of membership
            Queue<Order> goldQueue = new Queue<Order>();
            Queue<Order> normalQueue = new Queue<Order>();


            displayMenu(customerDict, goldQueue, normalQueue);

            Console.ReadLine();
        }

        public static void displayMenu(Dictionary<int, Customer> customerDict, Queue<Order> goldQueue, Queue<Order> normalQueue)
        {
            while (true)
            {
                int option;
                while (true)
                {
                    try
                    {
                        //  Input validation
                        Console.WriteLine("============================== Welcome to Our Ice Cream Store ==============================" +
                                        "\n[1] List all customers" +
                                        "\n[2] List all current orders" +
                                        "\n[3] Register a new customer" +
                                        "\n[4] Create a customer's order" +
                                        "\n[5] Display order details of a customer" +
                                        "\n[6] Modify order details" +
                                        "\n[0] Exit program" +
                                        "\n[7] Process an order and checkout" +
                                        "\n[8] Display monthly charged amounts breakdown & total charged amounts for the year" +
                                        "\n============================================================================================");

                        Console.Write("Enter your option: ");
                        option = int.Parse(Console.ReadLine().Trim());

                        if (option < 0 || option > 7)   //  Check if the option is between 0 and 8
                        {
                            Console.WriteLine("Please enter an option between 0 and 8. Try Again\n");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid integer. Try again.\n");
                    }
                }

                if (option == 0) //  Exit the program
                {
                    Console.WriteLine("\nThank you for using our ice cream Ice Cream Management System.\nCome back again. BYE!!!");
                    break;
                }
                else if (option == 1) { option1(customerDict); }
                else if (option == 2) { option2(goldQueue,normalQueue); }
                else if (option == 3) { option3(customerDict); }
                else if (option == 4) { option4(customerDict, goldQueue, normalQueue); }
                else if (option == 5) { option5(customerDict); }
                else if (option == 6) { option6(customerDict); }
                else if (option == 7) { option7(customerDict, goldQueue, normalQueue); }
                else { option8(customerDict); }
            }
        }
        public static void option1(Dictionary<int, Customer> customerDict)
        {
            Console.WriteLine("\n=====================================================================================" +
                           $"\n| {"Member ID",-10} | {"Name",-15} | {"Date of Birth",-15} | {"Membership",-10} | {"Points",-6} | {"PunchCard",-10} |" +
                           $"\n|============|=================|=================|============|========|============|");
            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                Customer customer = kvp.Value;
                Console.WriteLine(customer.ToString());
            }
            Console.WriteLine("=====================================================================================\n");
        }

        public static void  option2(Queue<Order> goldQueue, Queue<Order> normalQueue)
        {
            Console.WriteLine();
            Console.WriteLine("--------------- Gold Members Queue ---------------");
            displayOrder(goldQueue.ToArray());
            Console.WriteLine("--------------- Normal Members Queue ---------------");
            displayOrder(normalQueue.ToArray());

            Console.WriteLine();
        }

        public static void displayOrder(Order[] orderArray)
        {
            Console.WriteLine();
            if (orderArray.Length != 0) //  Check if the queue taken is empty
            {
                foreach (Order order in orderArray)
                {
                    if (order != null) //  Check if the order object is null
                    {
                        Console.WriteLine(order.ToString());
                    }
                    else
                    {
                        Console.WriteLine("There is no order here.\n");
                        break;
                    }
                }
            }
            else
            { Console.WriteLine("There is no order here.\n"); }
        }
        
        public static void option3(Dictionary<int, Customer> customerDict)
        {
            // Declare variables
            string name;
            int memberId;
            DateTime dob = DateTime.Now;

            Console.Write("Enter customer name: ");
            name = Console.ReadLine();  // Get customer name

            memberId = getMemberId(true, customerDict);   //  Get customer Id

            while (true)    // Get customer Date of Birth
            {
                //  Validate user inputs for Date of Birth
                try
                {
                    Console.Write("Enter customer Date of Birth in this format (dd/MM/yyyy): ");
                    dob = DateTime.Parse(Console.ReadLine().Trim());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid Date of Birth in this format (dd/MM/yyyy). Try again.");
                    continue;
                }
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
        
        public static void option4(Dictionary<int, Customer> customerDict, Queue<Order> goldQueue, Queue<Order> normalQueue)
        {
            //  List the customers
            option1(customerDict);

            //  Prompt user to select a customer and retrieve the selected customer
            int memberId = getMemberId(false, customerDict);

            //  Retrieve the customer object
            Customer customer = customerDict[memberId];

            //  Check for existing order
            if (customer.CurrentOrder != null)
            {
                Console.WriteLine("You have an existing order. Please choose option 6 to modify your order\n");
                return;
            }

            Order newOrder = customer.MakeOrder();  //  Create an empty order object for CurrentOrder inside the customer object

            bool repeat = false;
            do
            {
                //  Prompt user to enter their ice cream order (option, scoops, flavours, toppings)
                string option = getOption();
                int scoops = getScoops();
                List<Flavour> flavourList = getFlavoursList(scoops);
                int numToppings = getNumToppings();
                List<Topping> toppingList = getToppingList(numToppings);

                //  Create IceCream Object and it to the list Add the ice cream list to the order object
                newOrder.AddIceCream(makeIceCream(option, scoops, flavourList, toppingList));

                // Ask if the user would like to add another ice cream to the order
                while (true)
                {
                    Console.Write("\nWould you like to add another ice cream to the order? (Y/N): ");
                    string reply = Console.ReadLine().Trim().ToLower();

                    if (reply == "n")
                    { repeat = false; break; }
                    else if (reply == "y")
                    { repeat = true; break; }
                    else
                    { Console.WriteLine("Please enter a valid input (Y/N). Try again."); }
                }

            } while (repeat);

            customer.CurrentOrder = newOrder;   //  Add the order object to the customer object

            if (customer.Rewards.Tier == "Gold")
            { goldQueue.Enqueue(newOrder); }    //  Add the order object to the gold queue
            else
            { normalQueue.Enqueue(newOrder); }  //  Add the order object to the normal queue

            Console.WriteLine("\nOrder has been made successfully.\n");
        }

        /*  For option 3, 4 and 6   */
        public static int getMemberId(bool createNewId, Dictionary<int, Customer> customerDict)
        {
            int memberId;
            while (true)
            {
                try
                {
                    if (!createNewId)
                    {
                        Console.Write("Enter the customer Id: ");
                        memberId = int.Parse(Console.ReadLine().Trim());

                        //  Check if the customer Id exist in the dictionary
                        if (!customerDict.ContainsKey(memberId))
                        { Console.WriteLine("Please enter an existing customer Id.\n"); continue; }
                    }
                    else
                    {
                        Console.Write("Enter new customer Id (6 digits): ");
                        memberId = int.Parse(Console.ReadLine().Trim());

                        //  Check for at least 6 digits
                        if (memberId < 100000 && memberId > 999999)
                        { Console.WriteLine("Please enter a Id with 6 digits.\n"); continue; }

                        //  Check if for duplicate customer Id in the customer dictionary
                        else if (customerDict.ContainsKey(memberId))
                        { Console.WriteLine("This customer Id already exist. Choose another Id.\n"); continue; }
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid integers for customer Id. Try again.");
                }
            }
            return memberId;
        }

        public static IceCream makeIceCream(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList)
        {
            /*  Creating IceCream object    */
            IceCream iceCream = null;
            if (option == "cup")
            {
                iceCream = new Cup(option, scoops, flavourList, toppingList);   //  Create an ice cream object
            }
            else if (option == "cone")
            {
                bool dipped = getDipped();
                iceCream = new Cone(option, scoops, flavourList, toppingList, dipped);  //  Create an ice cream object
            }
            else if (option == "waffle")
            {
                string waffleFlavour = getWaffleFlavour();
                iceCream = new Waffle(option, scoops, flavourList, toppingList, waffleFlavour); //  Create an ice cream object
            }
            return iceCream;    //  Return the ice cream object
        }

        public static string getOption()  /*  Input validation for options    */
        {
            string option;
            while (true)
            {
                try
                {
                    Console.Write("\nEnter the option (Cup, Cone, Waffle): ");
                    option = Console.ReadLine().Trim().ToLower();

                    if (option != "cup" && option != "cone" && option != "waffle")  //  Check if the option is an available option
                    {
                        Console.WriteLine("Please enter a valid option.");
                    }
                    else { break; }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option (Cup, Cone, Waffle).");
                }
            }
            return option;
        }

        public static int getScoops() /*  Input validation for number of scoops   */
        {
            int scoops;
            while (true)
            {
                try
                {
                    Console.Write("\nEnter the number of scoops (Max 3 scoops): ");
                    scoops = int.Parse(Console.ReadLine().Trim());

                    if (scoops >= 1 && scoops <= 3) //  Check if the number of scoops is between 1 and 3
                    { break; }
                    else
                    { Console.WriteLine("Please enter a valid number of scoops from 1 to 3."); }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number of scoops in integer.");
                }
            }
            return scoops;
        }

        public static string displayAvailableOption(Dictionary<string, int> dictOptions) //  Display available Flavours and Toppings
        {
            List<string> list = new List<string>();
            Console.WriteLine();
            foreach (string key in dictOptions.Keys)
            {
                list.Add(key);
            }
            return $"{string.Join(", ", list)}";
        }

        public static List<Flavour> getFlavoursList(int numScoops)    /*  Input validation for flavours   */
        {
            List<Flavour> flavourList = new List<Flavour>();
            for (int i = 0; i < numScoops; i++)
            {
                while (true)
                {
                    // Separate premium and non-premium flavours
                    List<string> premiumFlavours = new List<string>();
                    List<string> nonPremiumFlavours = new List<string>();

                    foreach (KeyValuePair<string, int> kvp in flavourDictAvailability)
                    {
                        if (kvp.Value == 2)
                        { premiumFlavours.Add(kvp.Key); }
                        else
                        { nonPremiumFlavours.Add(kvp.Key); }
                    }

                    //  Display available Flavours
                    Console.WriteLine($"\nNormal flavours:        [ {string.Join(", ", nonPremiumFlavours)} ]");
                    Console.WriteLine($"Premium flavours (+$2): [ {string.Join(", ", premiumFlavours)} ]\n");

                    Console.Write($"Enter ice cream flavour for scoop {i + 1} : ");
                    string flavour = Console.ReadLine().Trim().ToLower();

                    //  Check if the string is a available flavour
                    if (flavourDictAvailability.ContainsKey(flavour))
                    {
                        // Check for premium flavours
                        bool isPremium = false;
                        if (flavourDictAvailability[flavour] == 2)
                        { isPremium = true; }

                        flavourList.Add(new Flavour(flavour, isPremium));
                        break;
                    }
                    else
                    { Console.WriteLine("Please enter a valid flavour."); }
                }
            }
            return flavourList;
        }

        public static int getNumToppings() /*  Input validation for number of toppings */
        {
            int numToppings;
            while (true)
            {
                try
                {
                    Console.Write($"Available Toppings: [{displayAvailableOption(toppingDictAvailability)}]");   //  Display available Toppings

                    Console.Write("\nEnter number of toppings (Max 4 toppings): ");
                    numToppings = int.Parse(Console.ReadLine().Trim());

                    if (numToppings >= 0 && numToppings <= toppingDictAvailability.Count) //  Check if the number of toppings is between 0 and 4
                    { break; }
                    else
                    { Console.WriteLine("Please enter a valid number of toppings from 0 to 4."); }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number of toppings in integer.");
                }
            }
            return numToppings;
        }

        public static List<Topping> getToppingList(int numToppings)
        {
            List<Topping> toppingList = new List<Topping>();

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
                        Console.Write($"Available Toppings (+$1 each): [{displayAvailableOption(toppingDictAvailability)}]");   //  Display available Toppings

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
                            Console.WriteLine("You have already added this topping.");
                            continue;
                        }

                        //  Check if the topping is an available topping
                        if (toppingDictAvailability.ContainsKey(topping))
                        {
                            toppingList.Add(new Topping(topping));
                            break;
                        }
                        else
                        { Console.WriteLine("Please enter a valid topping."); }
                    }
                }
            }
            return toppingList;
        }

        public static bool getDipped()
        {
            bool dipped;
            while (true)
            {
                Console.Write("\nDo you want the cone to be dipped in chocolate with additional cost of $2? (y/n): ");
                string reply = Console.ReadLine().Trim();

                if (reply == "y")
                { dipped = true; break; }

                else if (reply == "n")
                { dipped = false; break; }

                else
                { Console.WriteLine("Please enter a valid input (y/n).\n"); } //  Input validation for dipped
            }
            return dipped;
        }

        public static string getWaffleFlavour()
        {
            string reply;
            string waffleFlavour;
            while (true)
            {
                Console.Write("\nDo you want to choose premium waffle flavours with an additional cost of $3 (y/n): ");
                reply = Console.ReadLine().Trim().ToLower();

                if (reply != "y" && reply != "n")
                { Console.WriteLine("Please enter a valid input (y/n).\n"); }
                else
                { break; }
            }

            if (reply == "y")
            {
                while (true)
                {
                    Console.WriteLine("\nAvailable premium waffle flavours: [ Red Velvet, Charcoal, Pandan ]");
                    Console.Write("Enter a waffle flavour: ");
                    waffleFlavour = Console.ReadLine().Trim().ToLower();

                    if (waffleFlavour != "red velvet" && waffleFlavour != "charcoal" && waffleFlavour != "pandan") //  Check if the string is a available flavour
                    { Console.WriteLine("Please enter a valid waffle flavour."); }
                    else
                    { break; }
                }
            }
            else { waffleFlavour = "original"; }

            return waffleFlavour;
        }

        public static void option5(Dictionary<int, Customer> customerDict)
        {
            //  List the customers
            option1(customerDict);

            //  Prompt user to select a customer and retrieve the selected customer
            int memberId = getMemberId(false, customerDict);

            //  Retrieve the order objects
            List<Order> orderHistory = customerDict[memberId].OrderHistory;
            Console.WriteLine("--------------- Order History ---------------");
            displayOrder(orderHistory.ToArray());

            List<Order> currentOrder = new List<Order> { customerDict[memberId].CurrentOrder };
            Console.WriteLine("--------------- Current Order ----------------");
            displayOrder(currentOrder.ToArray());


        }

        public static void option6(Dictionary<int,Customer> customerDict)
        {
            /*  List the customers  */
            option1(customerDict);

            /*  Prompt user to select a customer and retrieve the selected customer */
            int memberId = getMemberId(false, customerDict);  // parameter is false because we are retrieving an existing customer id

            // Check if the customer has an existing order in queue
            if (customerDict[memberId].CurrentOrder == null)
            {
                Console.WriteLine("You do not have an existing order. Please choose option 4 to create an order first.\n");
                return;
            }

            //  Retrieve the current order objects
            Order currentOrder = customerDict[memberId].CurrentOrder;

            /*  List all the ice cream objects and details contained in the current order   */
            Console.WriteLine(currentOrder.ToString());

            /*  Prompt the user to modify an existing order, add an entirely new ice cream object to the order or delete existing ice cream object from the order   */
            int reply1;
            while (true)
            {
                try
                {
                    //  Input validation
                    Console.WriteLine();
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine("[1] Modify an existing ice cream");
                    Console.WriteLine("[2] Add a new ice cream");
                    Console.WriteLine("[3] Delete an existing ice cream");
                    Console.Write("Enter your option: ");
                    reply1 = int.Parse(Console.ReadLine().Trim());

                    if (reply1 < 1 || reply1 > 3)
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

            if (reply1 == 1)
            {
                Console.WriteLine("n\n---------- Modify an existing ice cream ----------");

                //  Prompt the user to select which ice cream to modify
                int reply2;
                while (true)
                {
                    try
                    {
                        Console.Write($"Enter which ice cream you want to modify from 1 to {currentOrder.IceCreamList.Count}");
                        reply2 = int.Parse(Console.ReadLine().Trim());

                        if (reply2 < 1 || reply2 > currentOrder.IceCreamList.Count)
                        {
                            Console.WriteLine($"Please enter a option from 1 to {currentOrder.IceCreamList.Count}. Try again.");
                        }
                        else { break; }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Please enter a valid integer. Try again.");
                    }
                }

                //  Modify the ice cream from the iceCream list based on the index
                currentOrder.ModifyIceCream(reply2 - 1);

                Console.WriteLine(currentOrder.ToString()); //  List all the ice cream objects and details contained in the current order

                Console.WriteLine("\nIce cream has been modified successfully.\n");
            }

            else if (reply1 == 2)
            {
                Console.WriteLine("\n---------- Add a new ice cream ----------");

                //  Prompt user to enter their ice cream order (option, scoops, flavours, toppings)
                string option = getOption();
                int scoops = getScoops();
                List<Flavour> flavourList = getFlavoursList(scoops);
                int numToppings = getNumToppings();
                List<Topping> toppingList = getToppingList(numToppings);

                IceCream newIceCream = makeIceCream(option, scoops, flavourList, toppingList);  //  Create IceCream Object
                currentOrder.AddIceCream(newIceCream);  //  Add the ice cream object to the order object

                Console.WriteLine(currentOrder.ToString()); //  List all the ice cream objects and details contained in the current order

                Console.WriteLine("\nNew ice cream has been added to order successfully.\n");
            }
            else
            {
                Console.WriteLine("\n---------- Delete an existing ice cream ----------");

                //  Prompt the user to select which ice cream to delete
                int reply2;
                while (true)
                {
                    //  Input validation
                    try
                    {
                        Console.Write($"Enter which ice cream you want to delete from 1 to {currentOrder.IceCreamList.Count}:");
                        reply2 = int.Parse(Console.ReadLine().Trim());

                        if (reply2 < 1 || reply2 > currentOrder.IceCreamList.Count) //  Check if the reply is between 1 and the number of ice cream in the list
                        {
                            Console.WriteLine($"Please enter an ice cream number u want to delete from 1 to {currentOrder.IceCreamList.Count}:");
                        }
                        else { break; }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Please enter a valid integer. Try again.");
                    }
                }

                //  Remove the ice cream from the list based on the index
                currentOrder.DeleteIceCream(reply2 - 1);

                //  List all the ice cream objects and details contained in the current order
                Console.WriteLine(currentOrder.ToString());

                Console.WriteLine("\nIce cream has been deleted from order successfully.\n");
            }
        }

        public static void option7(Dictionary<int,Customer> customerDict,Queue<Order> goldQueue, Queue<Order> normalQueue)
        {
            Console.WriteLine("\n---------- Ice Creams in the order ----------\n");

            /*  Retreive the first order in the queue   */
            Order currentOrder;
            if (goldQueue.Count != 0) // Checks if the gold queue has order
            {
                currentOrder = goldQueue.Peek();
                goldQueue.Dequeue();
            }

            else if (normalQueue.Count != 0)   //  Checks if the normal queue has order
            {
                currentOrder = normalQueue.Peek();
                normalQueue.Dequeue();
            }

            else
            { Console.WriteLine("There is currently no order to process.\n"); return; }

            Console.WriteLine($"{currentOrder.ToString()}");    //  Display all the ice creams in the order
            Console.WriteLine("--------------------------------------------");

            //  Calculate the total bill amount
            double totalAmount = 0;
            double mostExpensive = 0;
            foreach (IceCream iceCream in currentOrder.IceCreamList)
            {
                if (iceCream.CalculatePrice() > mostExpensive)  // Find the most expensive ice cream
                {
                    mostExpensive = iceCream.CalculatePrice();
                }
                totalAmount += iceCream.CalculatePrice();
            }

            Console.WriteLine($"Total amount: {totalAmount:c2}"); //  Display the total bill amount

            //  Retrieve customer object
            Customer currentCustomer = null;
            foreach (Customer customer in customerDict.Values)
            {
                if (customer.CurrentOrder == currentOrder)
                {
                    currentCustomer = customer;
                    break;
                }
            }

            //  Display the membership status & points of the customer
            Console.WriteLine($"\n{currentCustomer.Rewards.ToString()}");

            /*  Check if it is the customer’s birthday  */
            if (currentCustomer.isBirthday())
            {
                totalAmount -= mostExpensive; // Remove the most expensive icecream from the total bill
                Console.WriteLine($"It is your birthday. You get the most expensive ice cream for free!!!!\n");
            }

            /*  Check if the customer has completed their punch card    */
            PointCard card = currentCustomer.Rewards;
            if (card.PunchCard == 10)
            {
                //  Check if the most expensive ice cream is the first ice cream
                if (currentOrder.IceCreamList[0].CalculatePrice() == mostExpensive)
                { totalAmount -= currentOrder.IceCreamList[1].CalculatePrice(); } // Remove the second icecream from the total amount

                else
                { totalAmount -= currentOrder.IceCreamList[0].CalculatePrice(); } // Remove the first icecream from the total amount

                Console.WriteLine("Your punch card has reach 10. You will get the first ice cream in your order for free!!!\n");
                card.ResetPunchCard(); //  Reset the punch card
            }

            /*  Check Pointcard status to determine if the customer can redeem points   */
            if (currentCustomer.Rewards.Tier != "Ordinary" && currentCustomer.Rewards.Points != 0)
            {
                int points;
                do
                {   //  Prompt user to enter the number of points to offset the bill
                    try
                    {
                        Console.Write($"How many points do you want to redeem (1 point = $0.02): ");
                        points = int.Parse(Console.ReadLine().Trim());

                        //  Check if the points is more than the points the customer has
                        if (points > currentCustomer.Rewards.Points)
                        {
                            Console.WriteLine($"Unable to redeem {points} because you only have {currentCustomer.Rewards.Points} points.\n");
                        }
                        else { break; }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid integer.\n");
                    }
                } while (true);

                //  Redeem points
                card.RedeemPoints(points);
                totalAmount -= points * 0.02;
            }

            Console.WriteLine($"Final amount after deduction: {totalAmount:c2}\n");   //  Display the final total amount after deduction

            //  Prompt user to make payment
            Console.WriteLine("Press any key to make payment........");
            Console.ReadKey();
            Console.WriteLine();

            /*  Increment the punch card for every ice cream in the order   */
            foreach (IceCream iceCream in currentOrder.IceCreamList)
            {
                card.Punch();
            }

            /*  Earn points */
            int pointsEarned = (int)Math.Floor(totalAmount * 0.72); //  round down to the nearest integer
            card.AddPoints(pointsEarned);

            card.UpdateTier();  //  Upgrade the member status accordingly

            currentOrder.TimeFulfilled = DateTime.Now;  //  Mark the order as fulfilled with the current datetime

            currentCustomer.CurrentOrder = null;    // Remove order from current order in customer object

            currentCustomer.OrderHistory.Add(currentOrder); //  Add this fulfilled order object to the customer’s order history
        }

        public static void option8(Dictionary<int,Customer> customerDict)
        {
            //option 8 which displays the total earnings of the ice cream store each month in the year they input
            //existing history of orders are from the csv file orders.csv and the new orders that were fulfilled in the program
            try
            {
                List<string> monthslist = new List<string>  //initialise a list to store all the months in a year
                    {
                        "January","February","March","April","May","June","July","August","September","October","November","December"
                    };

                Console.Write("Enter a year between 2010 and 2024: ");  //prompt the user for the year
                string year = Console.ReadLine();  //takes in the user input as a string
                if (int.TryParse(year, out int yearx) && yearx >= 2010 && yearx <= 2024)  //compares the user's input and checks if it is a valid integer with the try parse method and if it falls within the specified range and comtinues if it does 
                {
                    double yearlyIncome = 0;  //initialise a new variable yearlyIncome total as 0
                    List<double> monthlyIncome = new List<double>();  //create a list to store monthly income values 
                    for (int i = 0; i < 12; i++)  //loops through the list 12 times to obtain the income of 12 months
                    {
                        monthlyIncome.Add(0.0);  //initialise as 0 for first 12 iterations as the next loops will add values from completed orders 
                    }
                    foreach (var customer in customerDict.Values)  //iterate through every customer object in the dictionary created above
                    {
                        foreach (var order in customer.OrderHistory)  //interates through the order history of each customer accessing it through the customer class
                        {
                            if (order.TimeFulfilled != null && order.TimeFulfilled.Value.Year == yearx)  //checks for fulfilled orders only through the association with Order class
                            {
                                double orderTotal = order.CalculateTotal();  //the order total here uses the calculate total method from the order class to calculate completed orders
                                monthlyIncome[order.TimeFulfilled.Value.Month - 1] += orderTotal; //use the -1 here to iterate through 12 months as indexing in lists start from 0 to get monthly income from month 1 to 12 in the list 
                                yearlyIncome += orderTotal;  //create a new variable to store the values from each month to be then used to add up into the yearly income
                            }
                        }
                    }
                    Console.WriteLine($"\nMonthly Income for {year}:\n");
                    for (int i = 0; i < 12; i++)  //calculates for all 12 months in a year
                    {
                        Console.WriteLine($"{monthslist[i],-10}: ${monthlyIncome[i],-10:0.00}");  //interated through the months stored in the monthslist and shows the corresponding income for that month from the monthly income calculation accurate to the month
                    }
                    Console.WriteLine("\n--------------------------------------------------------------------------------------------\n");
                    Console.WriteLine($"Total income for the year {year}: ${yearlyIncome:0.00}\n");
                }
                else
                {
                    Console.WriteLine("\nInvalid year, please enter a year between 2010 and 2024\n");  //breaks out of the flow if the inputed year doesnt fall between the specified range
                }
            }
            catch (FormatException)  //catches format errors involving incorrect data types
            {
                Console.WriteLine("Incorrect input, please input an actual year, thank you");
            }
            catch (Exception)  //catches all other errors that have not been filtered ex is not neccessary here as program is meant for customers 
            {
                Console.WriteLine($"Incorrect input, please input an integer to represent the year, thank you");
            }
        }
    }
}
