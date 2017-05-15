using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Abilities
{
    class StoneSkin : Passive
    {
        private const string name = "Stone Skin";
        private const string description = "Trolls are known for their thick skin. +10%%%% to defense";

        public StoneSkin() : base(name, description)
        {
        }
    }
}
