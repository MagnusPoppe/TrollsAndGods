using System;

namespace Multiplayer
{
    public abstract class GameEvent : Event
    {
        protected GameEvent(int id, string description) : base(id, description)
        {
        }

        public abstract void unpackJSON( String JSON );
        public abstract String packJSON();
    }
}