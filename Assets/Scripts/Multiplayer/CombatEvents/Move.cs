using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class Move : CombatEvent
    {
        public Move(int id, string description) : base(id, description)
        {
        }

        public override void execute()
        {
            throw new System.NotImplementedException();
        }
    } 
}
