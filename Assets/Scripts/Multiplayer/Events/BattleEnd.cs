
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for handling a battle ending
    /// </summary>
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

        /// <summary>
        /// Executes the action, Ending the battle.
        /// </summary>
        public override void execute()
        {
            //todo test
            Gm.exitCombat(winner);
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override void unpackJSON(string JSON)
        {
            BattleEnd obj = JsonUtility.FromJson<BattleEnd>(JSON);
            playerID = obj.playerID;
            heroID = obj.heroID;
            pos = obj.pos;
            winner = obj.winner;
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}