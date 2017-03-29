using System;

namespace Multiplayer
{
    public abstract class Event
    {

        private int id;
        private string description;

        public Event(int id,string description)
        {
            this.id = id;
            this.description = description;
        }

        public abstract void execute();

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
