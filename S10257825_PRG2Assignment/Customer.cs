//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

using System.Collections.Generic;
using System;

namespace S10257825_PRG2Assignment
{
    public class Customer
    {
        // Properties
        private string _name;
        private int _memberId;
        private DateTime _dob;
        private Order _currentOrder;
        private List<Order> _orderHistory = new List<Order>();
        private PointCard _rewards;

        // Attributes
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int MemberId
        {
            get { return _memberId; }
            set { _memberId = value; }
        }

        public DateTime Dob
        {
            get { return _dob; }
            set { _dob = value; }
        }

        public Order CurrentOrder
        {
            get { return _currentOrder; }
            set { _currentOrder = value; }
        }
        public List<Order> OrderHistory
        {
            get { return _orderHistory; }
            set { _orderHistory = value; }
        }

        public PointCard Rewards
        {
            get { return _rewards; }
            set { _rewards = value; }
        }

        // Constructors
        public Customer() { }

        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
        }

        // Methods
        public Order MakeOrder()
        {
            Order newOrder = new Order(10,DateTime.Now);
            return newOrder;
        }

        public bool isBirthday()
        {
            DateTime today = DateTime.Now;  // Get today's date
            if (today == Dob)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"| {MemberId,-10} | {Name,-15} | {Dob.ToString("dd/MM/yyyy"),-15} | {Rewards.Tier,-10} | {Rewards.Points,-6} | {Rewards.PunchCard,-10} |";
        }
    }
}
