//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System.Collections.Generic;
using System;

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        class Order
        {
            // Properties
            private int _id;
            private DateTime _timeReceived;
            private DateTime? _timeFulfilled;
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
            public DateTime TimeFulfilled
            {
                get { return (DateTime)_timeFulfilled; }
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
                // Access the ice cream from the list based on the index
                IceCream iceCream = IceCreamList[num];
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
                return $"";
            }
        }
    }
}
