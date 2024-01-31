//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System.Collections.Generic;
using System;

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
            IceCream currentIceCream = IceCreamList[num];
            Console.WriteLine(currentIceCream.ToString());

            //  Prompt the user for the new information for the modifications they wish to make to the ice cream selected: option, scoops, flavours, toppings, dipped cone (if applicable), waffle flavour (if applicable) and update the ice cream object’s info accordingly
            int reply;
            while (true)
            {
                try     //  Input validation
                {
                    Console.WriteLine("\n[1] Option" +
                                        "\n[2] Scoops" +
                                        "\n[3] Ice cream flavour" +
                                        "\n[4] Ice cream toppings" +
                                        "\n[5] Chcolate dipped (if applicable)" +
                                        "\n[6] Waffle flavour (if applicable)");
                    Console.Write("Enter your option: ");
                    reply = int.Parse(Console.ReadLine().Trim());

                    // Check if the reply is between 1 and 6 which is the available options
                    if (reply < 1 || reply > 6)
                    {
                        Console.WriteLine("Please enter a value from 1 to 6. Try again.");
                        continue;
                    }

                    //  Check if the option is applicable for the ice cream
                    if (currentIceCream is Cup && (reply == 5 || reply == 6))
                    { Console.WriteLine("This option is not applicable for cup. Try again."); }

                    else if (currentIceCream is Cone && reply == 6)
                    { Console.WriteLine("This option is not applicable for cone. Try again."); }

                    else if (currentIceCream is Waffle && reply == 5)
                    { Console.WriteLine("This option is not applicable for waffle. Try again."); }

                    else { break; }
                }
                catch (FormatException)
                { Console.WriteLine("Please enter a valid integer. try again."); }
            }

            string option;

            if (reply == 1)    //  Modify Option
            {
               
            }
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
                                $"Time Fulfilled: {TimeFulfilled}.ToString(\"dd MMM yyyy, HH:mm:ss tt\")\n";
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
