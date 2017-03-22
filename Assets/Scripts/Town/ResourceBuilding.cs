namespace TownView
{
    public class ResourceBuilding : Building
    {
        private Earn earnings;

        public Earn Earnings
        {
            get { return earnings; }
            set { earnings = value;  }
        }

        public ResourceBuilding( string name, string description, bool[] requirements, Cost cost, int localID, int LOCAL_SPRITEID_BLUEPRINT, Earn earnings)
            : base (name, description, requirements, cost, localID, LOCAL_SPRITEID_BLUEPRINT)
        {
            this.earnings = earnings;
        }
    }
}

