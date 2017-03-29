namespace Multiplayer
{
    public abstract class Event
    {
        private int IDfrom, IDto;
        private string description;

        public abstract void execute();
    }
}
