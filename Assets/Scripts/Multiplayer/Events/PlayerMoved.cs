using System;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerMoved : GameEvent
    {
        private const int ID_FROM = 0;
        private const int ID_TO = 0;
        private const String DESCRIPTION = "Player moves from one (x,y) to another (x,y) in the map.";

        public Hero hero;
        public Point to;

        public PlayerMoved(Hero hero, Point to ) : base(ID_FROM, DESCRIPTION)
        {
            this.hero = hero;
            this.to = to;
        }

        /// <summary>
        /// Executes the movement logically.
        /// </summary>
        public override void execute()
        {
            // Preparing movement:
            MovementManager movement = new MovementManager(gm.Reactions, gm.CanWalk, gm.AStar, gm);
            movement.PrepareMovement(to, hero);

            // Actually moving:
            while (movement.HasNextStep())
                movement.NextStep();
        }

        public override void unpackJSON( String JSON )
        {
            PlayerMoved  obj = JsonUtility.FromJson<PlayerMoved>(JSON);
            this.hero = obj.hero;
            this.to = obj.to;
        }

        public override string packJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}