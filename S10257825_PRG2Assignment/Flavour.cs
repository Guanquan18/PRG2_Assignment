//==========================================================
// Student Number : S10259469
// Student Name : Keshwindren Gandipanh
// Partner Name : Chang Guan Quan
//==========================================================

namespace S10257825_PRG2Assignment
{
    public class Flavour
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

        //unparameterised constructor 
        public Flavour() { }

        //parameterised constructor
        public Flavour (string type, bool premium)
        {
            this.type = type;
            this.premium = premium;
        }

        //tostring method
        public override string ToString()
        {
            return $"{Type}";
        }
    }
}
