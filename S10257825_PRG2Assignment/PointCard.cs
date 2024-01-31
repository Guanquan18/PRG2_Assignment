//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

namespace S10257825_PRG2Assignment
{
    public class PointCard
    {
        // Properties
        private int _points;
        private int _punchCard;
        private string _tier;

        // Attributes
        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }
        public int PunchCard
        {
            get { return _punchCard; }
            set { _punchCard = value; }
        }
        public string Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        // Constructors
        public PointCard() { }

        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }

        // Methods
        public void AddPoints(int points)
        { Points += points; }

        public void RedeemPoints(int points)
        { Points -= points; }

        public void Punch()
        {
            if (PunchCard < 10) // Increment only if less than 10
            { PunchCard ++; }
        }

        public void ResetPunchCard()    // Reset punch card to 0
        { PunchCard = 0; }

        public void UpdateTier()
        {
            if (Points >= 100)
            {
                Tier = "Gold";
            }
            else if (Points >= 50 && Tier != "Gold") // Prevents downgrade from Gold to Silver
            {
                Tier = "Silver";
            }
        }

        public override string ToString()
        {
            return $"===================================" +
                $"\n| {"Memebrship",-10} | {"Points",-6} | {"PunchCard",-9} |" +
                $"\n===================================" +
                $"\n| {Tier,-10} | {Points,-6} | {PunchCard,-9} |" +
                $"\n-----------------------------------\n";
        }

    }
}
