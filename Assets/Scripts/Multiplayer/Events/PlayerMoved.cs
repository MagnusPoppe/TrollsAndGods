namespace Multiplayer
{
    public class PlayerMoved : GameEvent
    {
        private const int ID_FROM = 0;
        private const int ID_TO = 0;

        public PlayerMoved(int IDFrom, int IDTo, string description) : base(IDFrom, IDTo, description)
        {
            
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        public override string unpackJSON()
        {
            throw new System.NotImplementedException();
        }

        public override string packJSON()
        {
            throw new System.NotImplementedException();
        }
    }
}