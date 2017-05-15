
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for handling a battle beginning
    /// </summary>
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

        /// <summary>
        /// Executing the action, beginning the battle, only showing the battle if the player is a part of the battle.
        /// </summary>
        public override void execute()
        {
            //todo not show battle if not involved
            Reaction reaction = Gm.Reactions[pos.x, pos.y];
            if (reaction.HasPreReact())
            {
                reaction = reaction.PreReaction;
            }
            if (reaction.GetType() == typeof(HeroMeetReact))
            {
                HeroMeetReact hmr = (HeroMeetReact) reaction;
                Gm.enterCombat(15, 11, Gm.getPlayer(playerID).Heroes[heroID], hmr.Hero);
            }
            else
            {
                UnitReaction ur = (UnitReaction) reaction;
                Gm.enterCombat(15, 11, Gm.getPlayer(playerID).Heroes[heroID], ur.Units, true);
            }
        }

        /// <summary>
        /// Packs this into JSON
        /// </summary>
        /// <returns>JSON of this object</returns>
        public override void unpackJSON(string JSON)
        {
            BattleBegin obj = JsonUtility.FromJson<BattleBegin>(JSON);
            playerID = obj.playerID;
            heroID = obj.heroID;
            pos = obj.pos;
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