//==========================================================
// Student Number : S10257825
// Student Name : Chang Guan Quan
// Partner Name : Keshwindren Gandipanh
//==========================================================

namespace S10257825_PRG2Assignment
{
    internal partial class Program
    {
        class PointCard
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
            {
                Points += points;
            }

            public void RedeemPoints(int points)
            {
                Points -= points;
            }

            public void Punch()
            {
                PunchCard -= 1;
            }

            public override string ToString()
            {
                return base.ToString() + $"Points: {Points}\nPunch Card: {PunchCard}\nTier: {Tier}";
            }

        }
    }
}
