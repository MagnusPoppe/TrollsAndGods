namespace Multiplayer
{
    public abstract class CombatEvent : Event
    {
        private int idFrom, idTo;

        protected CombatEvent(int id, string description, int idFrom, int idTo) : base(id, description)
        {
            this.idFrom = idFrom;
            this.idTo = idTo;
        }
    }
}