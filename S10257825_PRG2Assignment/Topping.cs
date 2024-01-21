//==========================================================
// Student Number : S10259469
// Student Name : Keshwindren Gandipanh
// Partner Name : Chang Guan Quan
//==========================================================

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        class Topping
        {
            //regular fields 
            private string type;
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            //unparameterised constructor
            public Topping ()
            {

            }

            //parameterised constructor
            public Topping (string type)
            {
                this.Type = type;
            }

            //tostring method 
            public override string ToString()
            {
                return $"{Type}";
            }
        }
    }
}
