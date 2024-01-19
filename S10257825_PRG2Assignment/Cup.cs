//==========================================================
// Student Number : S10259469
// Student Name : Keshwindren Gandipanh
// Partner Name : Chang Guan Quan
//==========================================================

using System.Collections.Generic;

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
         class Cup : IceCream
        {
            //unparameterised constructor
            //make sure to implement base 
            public Cup () : base ()
            {

            }
            
            //parameterised constructor 
            //retrive the base values from the superclass
            public Cup (string option, int  scoops, List<Flavour> Flavours, List<Topping> Toppings) : base (option, scoops, Flavours, Toppings)
            {
                this.Option = option;
                this.Scoops = scoops;
                this.Flavours = Flavours;
                this.Toppings = Toppings;
            }

            //method for Cup class
            public override double CalculatePrice()
            {
                //logic for the calculate price method of cup 
                double totalPrice = 0; //initialise the beginning price which is zero

                if (Scoops == 1)
                {
                    totalPrice = +4;  //one scoop of regular flavour
                }
                else if (Scoops == 2)
                {
                    totalPrice += 5.5;  //two scoops of regular flavour
                }
                else if (Scoops == 3)
                {
                    totalPrice += 6.50;  //three scoops of regular flavour
                }

                foreach (Flavour f in Flavours) //loop through the list containing flavours
                {
                    if (f.Premium)
                    {
                        totalPrice += 2 * f.Quantity;  //applies additional charges of 2 dollars for premium flavours (durian,ube,sea salt)
                    }
                }
                totalPrice += 1 * Toppings.Count;  //applies charge of 1 dollar for every topping on top of scoops checks toppings from the toppings list

                return totalPrice;
            }

            //tostring method 
            public override string ToString()
            {
                //logical output for the tostring method
                return $"Cup : {Scoops} Scoops | Flavour : {string.Join(", ", Flavours)} | Price : ${CalculatePrice()})";
            }
        }
    }
}
