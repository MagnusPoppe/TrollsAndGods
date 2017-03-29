using System;

namespace Multiplayer
{
    public abstract class GameEvent : Event
    {
        protected GameEvent(int IDFrom, int IDTo, string description) : base(IDFrom, IDTo, description)
        {

        }

        public abstract String unpackJSON();
        public abstract String packJSON();
    }
}