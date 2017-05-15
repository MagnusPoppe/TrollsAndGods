using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.Abilities
{
    /// <summary>
    /// Stoneskin ability
    /// </summary>
    class StoneSkin : Passive
    {
        private const string name = "Stone Skin";
        private const string description = "Trolls are known for their thick skin. +10% to defense";

        /// <summary>
        /// Default constructor 
        /// </summary>
        public StoneSkin() : base(name, description)
        {
        }
    }
}