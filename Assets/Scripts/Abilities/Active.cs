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

    public Active(string name, string description, int cooldown) : base(name, description)
    {
        Cooldown = cooldown;
    }
}
