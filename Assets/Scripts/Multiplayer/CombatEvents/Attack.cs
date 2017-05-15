using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class Attack : CombatEvent
    {
        public Point pos;
        public Point goal;

        public Attack(int id, string description, Point pos, Point goal) : base(id, description)
        {
            this.pos = pos;
            this.goal = goal;
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }

        
    } 
}
