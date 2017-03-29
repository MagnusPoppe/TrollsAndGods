using System;

namespace Multiplayer
{
    public abstract class Event
    {

        private int IDfrom, IDto;
        private string description;

        public Event(int IDFrom, int IDTo, string description)
        {
            this.IDfrom = IDFrom;
            this.IDto = IDTo;
            this.description = description;
        }

        public abstract void execute();
    }
}
