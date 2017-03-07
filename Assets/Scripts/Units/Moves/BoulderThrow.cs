using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Units
{
    class BoulderThrow : Move
    {
        const string NAME = "Boulder Throw";
        const string DESCRIPTION = "The troll tosses a piece of its rocky body at their enemy.";
        const int MINDAMAGE = 2;
        const int MAXDAMAGE = 4;
        static int DAMAGETYPE = Element.BLUDGEONING;

        public BoulderThrow() 
            :base(NAME, DESCRIPTION, MINDAMAGE, MAXDAMAGE, DAMAGETYPE)
        {

        }
    }
}
