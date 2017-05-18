using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    /// <summary>
    /// Event for handling a unit moving in battle
    /// </summary>
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
