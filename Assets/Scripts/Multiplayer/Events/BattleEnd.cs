
using UnityEngine;

namespace Multiplayer
{

    public class BattleEnd : GameEvent
    {

        public int playerID, heroID;
        public Point pos;
        public bool winner;

        public BattleEnd(int id, string description, int playerId, int heroId, Point pos, bool winner) : base(id, description)
        {
            playerID = playerId;
            heroID = heroId;
            this.pos = pos;
            this.winner = winner;
        }

        public override void execute()
        {
            //todo test
            Gm.exitCombat(winner);
        }

        public override void unpackJSON(string JSON)
        {
            BattleEnd obj = JsonUtility.FromJson<BattleEnd>(JSON);
            playerID = obj.playerID;
            heroID = obj.heroID;
            pos = obj.pos;
            winner = obj.winner;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}