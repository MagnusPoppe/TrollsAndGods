namespace Units
{
    /// <summary>
    /// Superclass for unit moves
    /// </summary>
    public class Move
    {
        string name;
        string description;
        int minDamage;
        int maxDamage;
        int damageType; // TODO Element

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Name of the move</param>
        /// <param name="description">Description of the move</param>
        /// <param name="minDamage">Minimum damage the move can inflict</param>
        /// <param name="maxDamage">Maximum damage the move can inflict</param>
        /// <param name="damageType">The damage type of the given move</param>
        public Move(string name, string description, int minDamage, int maxDamage, int damageType)
        {
            this.Name = name;
            this.Description = description;
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
            this.damageType = damageType;
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public string Description
        {
            get { return description; }

            set { description = value; }
        }

        public int MinDamage
        {
            get { return minDamage; }

            set { minDamage = value; }
        }

        public int MaxDamage
        {
            get { return maxDamage; }

            set { maxDamage = value; }
        }

        public int DamageType
        {
            get { return damageType; }

            set { damageType = value; }
        }
    }
}