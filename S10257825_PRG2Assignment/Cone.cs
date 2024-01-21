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
        class Cone : IceCream
        {
            //fields specific to this class
            private bool dipped;
            public bool Dipped
            {
                get { return dipped; }
                set { dipped = value; }
            }

            //unparameterised constructor
            //make sure to implement base
            public Cone () : base () { }

            //parameterised constructor 
            //retrive the base values from the superclass
            public Cone(string option, int scoops, List<Flavour> Flavours, List<Topping> Toppings, bool dipped) : base(option, scoops, Flavours, Toppings)
            {
                this.Dipped = dipped;
            }

            //method for cone class
            public override double CalculatePrice()
            {
                //logic for cone calculate price method
                double totalPrice = 0; //initialise beginning cost for the cones

                if (Scoops == 1)
                {
                    totalPrice += 4; //price for 1 scoop of regular flavour 
                }
                else if (Scoops == 2)
                {
                    totalPrice += 5.5;  //total for 2 scoops of regular flavour
                }
                else if (Scoops == 3)
                {
                    totalPrice += 6.5;  //total for 3 scoops of regular flavour
                }

                if (Dipped)
                {
                    totalPrice += 2;  //additional charge of 2 dollars if the cone is dipped in chocolate
                }

                foreach (Flavour f in Flavours)
                {
                    if (f.Premium)
                    {
                        totalPrice += 2 * f.Quantity;  //additional charge of 2 dollars if the flavour is a premium flavour durian,ube,sea salt
                    }
                }

                totalPrice += 1 * Toppings.Count;  //additional charge of 1 dollar for every topping

                return totalPrice;

            }

            //tostring method for cones class
            public override string ToString()
            {
                return $"\nOption: {Option} | {Scoops} Scoops | " + base.ToString() + $" | Price : ${CalculatePrice():c2}";
                
            }
        }
    }
}
