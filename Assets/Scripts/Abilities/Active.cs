/// <summary>
/// Superclass for active ablities
/// </summary>
public class Active : Ability
{
    private int cooldown;

    public int Cooldown
    {
        get
        {
            return cooldown;
        }

        set
        {
            cooldown = value;
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Name of ability</param>
    /// <param name="description">Description of ability</param>
    /// <param name="cooldown">Cooldown timer for the ability</param>
    public Active(string name, string description, int cooldown) : base(name, description)
    {
        Cooldown = cooldown;
    }
}
