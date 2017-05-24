namespace Abilities
{
    /// <summary>
    /// Super class for hero and unit abilities
    /// </summary>
    public class Ability
    {
        string name;
        string description;

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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Ability(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}