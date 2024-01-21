using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            void Initialise()
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
                                    break;
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
            Initialise();

            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                Customer customer = kvp.Value;
                Console.WriteLine("-------------------------------------------------------------------------------------");
                Console.WriteLine(customer.ToString());
                foreach (Order order in customer.OrderHistory)
                {
                    Console.WriteLine(order.ToString());
                    foreach (IceCream iceCream in order.IceCreamList)
                    {
                        Console.WriteLine(iceCream.ToString()); 
                    }
                    Console.WriteLine();
                }
            }



            /*  Question 1  */


            /*  Question 2  */





            Console.ReadLine();

        }
    }
}
