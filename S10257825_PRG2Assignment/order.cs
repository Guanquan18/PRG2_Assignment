//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System.Collections.Generic;
using System;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Linq;

namespace S10257825_PRG2Assignment
{
    public class Order
    {
        // Properties
        private int _id;
        private DateTime _timeReceived;
        private DateTime? _timeFulfilled = null;
        private List<IceCream> _iceCreamList = new List<IceCream>();

        // Attributes
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public DateTime TimeReceived
        {
            get { return _timeReceived; }
            set { _timeReceived = value; }
        }
        public DateTime? TimeFulfilled
        {
            get { return _timeFulfilled; }
            set { _timeFulfilled = value; }
        }
        public List<IceCream> IceCreamList
        {
            get { return _iceCreamList; }
            set { _iceCreamList = value; }
        }

        // Constructors
        public Order() { }

        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
        }

        // Methods
        public void ModifyIceCream(int num)
        {
            bool repeat = true;
            do
            {
                IceCream currentIceCream = IceCreamList[num];
                Console.WriteLine($"\n{currentIceCream.ToString()}");

                //  Prompt the user for modification
                int reply;
                while (true)
                {
                    try
                    {
                        //  Input validation
                        Console.WriteLine("=========================================" +
                                            "\n[1] Option" +
                                            "\n[2] Scoops" +
                                            "\n[3] Ice cream flavour" +
                                            "\n[4] Ice cream toppings" +
                                            "\n[5] Chocolate dipped (if applicable)" +
                                            "\n[6] Waffle flavour (if applicable)" +
                                            "\n[0] Exit" +
                                            "\n=========================================");
                        Console.Write("What would you like to modify: ");
                        reply = int.Parse(Console.ReadLine().Trim());

                        // Check if the reply is between 1 and 6 which is the available options
                        if (reply < 0 || reply > 6)
                        {
                            Console.WriteLine("Please enter a value from 1 to 6.");
                            continue;
                        }

                        //  Check if the option is applicable for the ice cream
                        if (currentIceCream is Cup && (reply == 5 || reply == 6))
                        { Console.WriteLine("This option is not applicable for cup.\n"); }

                        else if (currentIceCream is Cone && reply == 6)
                        { Console.WriteLine("This option is not applicable for cone.\n"); }

                        else if (currentIceCream is Waffle && reply == 5)
                        { Console.WriteLine("This option is not applicable for waffle.\n"); }

                        else { break; }
                    }
                    catch (FormatException)
                    { Console.WriteLine("Please enter a valid integer.\n"); }
                }


                if (reply == 1)    //  Modify Option
                {
                    while (true)
                    {
                        string option = Program.getOption();

                        if (option == currentIceCream.Option)  //  Check if the option is the same as the current option
                        { Console.WriteLine($"Your ice cream option is already a {currentIceCream.Option}."); }

                        else
                        {
                            /*  Remove Ice Cream from the list  */
                            IceCreamList.RemoveAt(num);

                            /*  Add the modified ice cream option back to the list in the same order */
                            IceCreamList.Insert(num, Program.makeIceCream(option, currentIceCream.Scoops, currentIceCream.Flavours, currentIceCream.Toppings));

                            Console.WriteLine("Ice cream option has been modified successfully!\n");
                            break;
                        }
                    }
                }
                else if (reply == 2)    // Modify Scoops
                {
                    while (true)
                    {
                        try
                        {
                            int scoops = Program.getScoops();    //  Get the number of scoops from the user

                            if (scoops == currentIceCream.Scoops)  //  Check if the number of scoops is the same as the current number of scoops
                            { Console.WriteLine($"Your ice cream already has {currentIceCream.Scoops} scoops.\n"); }

                            else
                            {
                                currentIceCream.Scoops = scoops;    //  Modify the scoops in the ice cream
                                List<Flavour> flavoursList = Program.getFlavoursList(currentIceCream.Scoops);    // Get the flavours from the user
                                currentIceCream.Flavours = flavoursList;    //  Modify the flavours in the ice cream
                                Console.WriteLine("\nIce cream scoops has been modified successfully!\n");
                                break;
                            }
                        }
                        catch (FormatException)
                        { Console.WriteLine("Please enter a valid integer.\n"); }
                    }
                }
                else if (reply == 3)    // Modify Flavours
                {
                    List<Flavour> flavourList = Program.getFlavoursList(currentIceCream.Scoops);    // Get flavours from the user

                    currentIceCream.Flavours = flavourList; //  Modify the flavours in the ice cream
                    Console.WriteLine("\nIce cream flavour has been modified successfully!\n");
                    break;
                }
                else if (reply == 4)    // Modify Toppings
                {
                    int numToppings = Program.getNumToppings();    //  Get the number of toppings from the 
                    List<Topping> toppingList = Program.getToppingList(numToppings);   // Get toppings from the user

                    currentIceCream.Toppings = toppingList; //  Modify the toppings in the ice cream
                    Console.WriteLine("\nIce cream toppings has been modified successfully\n");
                }
                else if (reply == 5)    // Modify chocolate dipped
                {
                    bool dipped = Program.getDipped();  //  Get the chocolate dipped from the user

                    Cone cone = (Cone)currentIceCream;    //  Down cast the ice cream to cone
                    cone.Dipped = dipped;    //  Modify the chocolate dipped in the ice cream
                    Console.WriteLine("Ice cream chocolate dipped has been modified successfully.\n");
                }
                else if (reply == 6) // Modify Waffle Flavour
                {
                    
                    string waffleFlavour = Program.getWaffleFlavour();    //  Get the waffle flavour from the user
                    Waffle waffle = (Waffle)currentIceCream;  //  Down cast the ice cream to waffle

                    waffle.WaffleFlavour = waffleFlavour;  //  Modify the waffle flavour in the ice cream
                    Console.WriteLine("Ice cream waffle flavour has been modified successfully.\n");
                }
                else { return; }    //  Exit the method

                //  Prompt the user if they want to modify the ice cream again
                while (true)
                {
                    Console.Write("Do you want to modify the ice cream again? (y/n): ");
                    string reply2 = Console.ReadLine().Trim().ToLower();

                    if (reply2 == "y")
                    { repeat = true; break; }

                    else if (reply2 == "n")
                    { repeat = false; break; }

                    else
                    { Console.WriteLine("Please enter a valid option (y/n).\n"); }
                }
            } while (repeat);

        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int num)
        {
            // Remove the ice cream from the list based on the index
            IceCreamList.RemoveAt(num);
        }

        public double CalculateTotal()
        {
            double totalPrice = 0;

            // Access each ice cream in the list and calculate the price
            foreach (IceCream iceCream in IceCreamList)
            {
                totalPrice += iceCream.CalculatePrice();
            }
            return totalPrice;
        }

        public override string ToString()
        {
            string orderString;
            if (TimeFulfilled == null)
            {
                orderString = $"OrderID: {Id} | Time Received: {TimeReceived.ToString("dd MMM yyyy, HH:mm:ss tt")} | " +
                                $"Time Fulfilled: Not Avaialble\n";
            }
            else
            {
                orderString = $"OrderID: {Id} | Time Received: {TimeReceived.ToString("dd MMM yyyy, HH:mm:ss tt")} | " +
                                $"Time Fulfilled: {((DateTime)TimeFulfilled).ToString("dd MMM yyyy, HH:mm:ss tt")}\n";
            }
            string iceCreamString = "";
            for (int i=0; i<IceCreamList.Count; i++)
            {
                iceCreamString+= $"\n[{i+1}]{IceCreamList[i].ToString()}\n";
            }

            return orderString+iceCreamString;
        }
    }
    
}
