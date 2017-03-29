
namespace Multiplayer
{

    public class BattleBegin : GameEvent
    {
        private int playerID, heroID;

        public BattleBegin(int id, string description) : base(id, description)
        {
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        public override string unpackJSON(string JSON)
        {
            throw new System.NotImplementedException();
        }

        public override string packJSON()
        {
            throw new System.NotImplementedException();
        }
    }

}