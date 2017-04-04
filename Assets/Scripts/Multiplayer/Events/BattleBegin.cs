
using UnityEngine;

namespace Multiplayer
{

    public class BattleBegin : GameEvent
    {
        public int playerID, heroID;
        public Point pos;

        public BattleBegin(int id, string description, int playerId, int heroId, Point pos) : base(id, description)
        {
            playerID = playerId;
            heroID = heroId;
            this.pos = pos;
        }

        public override void execute()
        {
            //todo engage in combat(not show unless player is active in battle)
            throw new System.NotImplementedException();
        }

        public override void unpackJSON(string JSON)
        {
            BattleBegin obj = JsonUtility.FromJson<BattleBegin>(JSON);
            playerID = obj.playerID;
            heroID = obj.heroID;
            pos = obj.pos;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }

}