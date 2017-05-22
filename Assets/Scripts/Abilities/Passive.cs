
namespace Abilities
{
    /// <summary>
    /// Superclass for passive abilities
    /// </summary>
    public class Passive : Ability
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Name of ability</param>
        /// <param name="description">Description of ability</param>
        public Passive(string name, string description) : base(name, description)
        {
        }
    }
}
