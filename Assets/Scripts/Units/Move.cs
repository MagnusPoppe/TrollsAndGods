using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Units
{
    public class Move
    {
        string name;
        string description;
        int minDamage;
        int maxDamage;
        int damageType;

        public Move(string name, string description, int minDamage, int maxDamage, int damageType)
        {
            this.Name = name;
            this.Description = description;
            this.MinDamage = minDamage;
            this.MaxDamage = maxDamage;
            this.damageType = damageType;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public int MinDamage
        {
            get
            {
                return minDamage;
            }

            set
            {
                minDamage = value;
            }
        }

        public int MaxDamage
        {
            get
            {
                return maxDamage;
            }

            set
            {
                maxDamage = value;
            }
        }

        public int DamageType
        {
            get
            {
                return damageType;
            }

            set
            {
                damageType = value;
            }
        }


    }
}
