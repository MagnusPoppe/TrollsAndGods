
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