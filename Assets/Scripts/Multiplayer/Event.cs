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

        public int Dfrom
        {
            get { return IDfrom; }
            set { IDfrom = value; }
        }

        public int Dto
        {
            get { return IDto; }
            set { IDto = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
