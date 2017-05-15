namespace Units
{

    /// <summary>
    /// Tree swing move
    /// </summary>
    class TreeSwing : Move
    {
        const string NAME = "Tree Swing";
        const string DESCRIPTION = "The giant swings their tree wildly at the enemy.";
        const int MINDAMAGE = 3;
        const int MAXDAMAGE = 8;
        static int DAMAGETYPE = Element.BLUDGEONING;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TreeSwing()
            : base(NAME, DESCRIPTION, MINDAMAGE, MAXDAMAGE, DAMAGETYPE)
        {

        }
    }
}
