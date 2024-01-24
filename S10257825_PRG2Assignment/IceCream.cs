//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System.Collections.Generic;

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        abstract class IceCream
        {
            // Properties
            private string _option;
            private int _scoops;
            private List<Flavour> _flavours;
            private List<Topping> _toppings;

            // Attributes
            public string Option
            {
                get { return _option; }
                set { _option = value; }
            }
            public int Scoops
            {
                get { return _scoops; }
                set { _scoops = value; }
            }
            public List<Flavour> Flavours
            {
                get { return _flavours; }
                set { _flavours = value; }
            }
            public List<Topping> Toppings
            {
                get { return _toppings; }
                set { _toppings = value; }
            }

            // Constructors
            public IceCream() { }

            public IceCream(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList)
            {
                Option = option;
                Scoops = scoops;
                Flavours = flavourList;
                Toppings = toppingList;
            }

            public abstract double CalculatePrice();
            /*double totalPrice = 0;
            if (Option == "Cup" || Option == "Cone")
            {
                // Calculate base price for numebr of scoops for cups and Cones
                if (Scoops == 1)
                {   totalPrice+= 4;   }

                else if (Scoops == 2)
                { totalPrice += 5.5; }

                else if (Scoops == 3)
                { totalPrice += 6.5; }
            }
            else
            {
                // Calculate base price for numebr of scoops for Waffles
                if (Scoops == 1)
                { totalPrice += 7; }

                else if (Scoops == 2)
                { totalPrice += 8.5; }

                else if (Scoops == 3)
                { totalPrice += 9.5; }
            }

            // Calculate additonal price for premium flavours
            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    totalPrice += 2*flavour.Quantity;
                }
            }

            // Calculate additonal price for toppings
            totalPrice += 1*Toppings.Count();*/

            public override string ToString()
            {
                string toppings = string.Join(", ", Toppings) ;
                if (Toppings.Count == 0)
                {
                    toppings = "No Toppings";
                }
                else { toppings = $"[{toppings}]"; }
                return $"FLavours : [{string.Join(", ",Flavours)}] | Toppings: {toppings}";
            }

        }
    }
}
