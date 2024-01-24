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
        class Waffle : IceCream
        {
            //fields specific to this class
            private string waffleFlavour;
            public string WaffleFlavour
            {
                get { return waffleFlavour; }
                set { waffleFlavour = value;}
            }

            //unparameterised constructor
            //make sure to implement base 
            public Waffle () : base () { }

            //parameterised constructor 
            //retrive the base values from the superclass
            public Waffle(string option, int scoops, List<Flavour> Flavours, List<Topping> Toppings, string waffleFlavour) : base(option, scoops, Flavours, Toppings)
            {
                this.waffleFlavour = waffleFlavour;
            }

            //method for waffle class
            public override double CalculatePrice()
            {
                //logic for the method in waffle class
                double wafflePrice = 0;  //initialise initial cost which is 0 as no purchases yet

                if (Scoops == 1)
                {
                    wafflePrice += 7;  //addition of 3 dollars in charge as it is not cup or cone for 1 scoop
                }
                else if (Scoops == 2)
                {
                    wafflePrice += 8.5;  //addition of 3 dollars in charge as it is not cup or cone for 2 scoops
                }
                else if (Scoops == 3)
                {
                    wafflePrice += 9.5;  //addition of 3 dollars in charge as it is not cup or cone for 3 scoops
                }

                foreach (Flavour w in Flavours )  //loop through the flavours list to find if it is a premium flavour for waffle
                {
                    if (w.Premium)
                    {
                        wafflePrice += 2 * w.Quantity;  //addition of 2 dollars for premium flavours of waffle
                    }
                }

                if (waffleFlavour == "Red velvet" || waffleFlavour == "charcoal" || waffleFlavour == "pandan waffle" )  //the || is the logical OR operator to check if it's between these 3 flavours 
                {
                    wafflePrice += 3;  //special charge of 3 dollars for the 3 flavours specified on the table for waffles 
                }

                wafflePrice += 1 * Toppings.Count;  //1 dollar charge for every topping added on 

                return wafflePrice;
            }

            //tostring method for waffle class
            public override string ToString()
            {
                return $"{Option} | {Scoops} Scoops | " + base.ToString() + $" | Price : {CalculatePrice():c2}";
            }
        }
    }
}
