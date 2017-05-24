namespace TownView
{
    /// <summary>
    /// Superclass for Town buildings of the Resource type. Has a variable "Earning" for how many resources it will add to the player who owns this building each week
    /// </summary>
    public class ResourceBuilding : Building
    {
        private Earn earnings;

        public Earn Earnings
        {
            get { return earnings; }
            set { earnings = value;  }
        }

        /// <summary>
        /// Public constructor for resource building
        /// </summary>
        /// <param name="name">Name of the given building</param>
        /// <param name="description">Description of the current building</param>
        /// <param name="requirements">The building(s) that must be built before this one is available</param>
        /// <param name="cost">The Resource cost to make the given building</param>
        /// <param name="localID">The local spriteID of the given building</param>
        /// <param name="LOCAL_SPRITEID_BLUEPRINT">The local spriteID for the blueprint of the building</param>
        public ResourceBuilding( string name, string description, bool[] requirements, Cost cost, int localID, int LOCAL_SPRITEID_BLUEPRINT, Earn earnings)
            : base (name, description, requirements, cost, localID, LOCAL_SPRITEID_BLUEPRINT)
        {
            this.earnings = earnings;
        }
    }
}

