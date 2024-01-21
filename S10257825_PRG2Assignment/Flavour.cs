//==========================================================
// Student Number : S10259469
// Student Name : Keshwindren Gandipanh
// Partner Name : Chang Guan Quan
//==========================================================

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        class Flavour
        {
            //fields specific for this class (association)
            private string type;
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            private bool premium;
            public bool Premium
            {
                get { return premium; }
                set { premium = value; }
            }

            private int quantity;
            public int Quantity
            {
                get { return quantity; }
                set { quantity = value; }
            }

            //unparameterised constructor 
            public Flavour()
            {

            }

            //parameterised constructor
            public Flavour (string type, bool premium, int quantity)
            {
                this.type = type;
                this.premium = premium;
                this.quantity = quantity;
            }

            //tostring method
            public override string ToString()
            {
                return $"{Type}";
            }
        }
    }
}
