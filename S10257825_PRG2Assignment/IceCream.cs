//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System;
using System.Collections.Generic;

namespace S10257825_PRG2Assignment
{
    public abstract class IceCream : IEquatable<List<Flavour>>, IEquatable<List<Topping>>
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
            
            public override string ToString()
            {
                string toppings = string.Join(", ", Toppings) ;
                if (Toppings.Count == 0)
                {
                    toppings = "No Toppings";
                }
                return $"\n\tFLavours : [{string.Join(", ",Flavours)}]\n\tToppings: [{toppings}]";
            }

            public bool Equals(List<Flavour> f)
            {
                return Flavours == f;
            }
            
            public bool Equals(List<Topping> t)
            {
                return Toppings == t;
            }
        }
    
}
