using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logical side of the battlefield
/// </summary>
public class BattleField {

    int[,] canWalk;
    int width, height;
    AStarAlgo aStar;
    Hero attacker, defender;
    UnitTree attackingUnits, defendingUnits;
    UnitAndAmount[,] unitsPos;

    /// <summary>
    /// Constructor for Battlefield for battle between heroes
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">The attacking hero</param>
    /// <param name="defender">The defending hero</param>
    /// <param name="canWalk">Canwalk</param>
    public BattleField(int width, int height, Hero attacker, Hero defender, int[,] canWalk)
    {
        Width = width;
        Height = height;
        Attacker = attacker;
        Defender = defender;
        CanWalk = canWalk;
        aStar = new AStarAlgo(canWalk, width, height, true);

        attackingUnits = attacker.Units;
        defendingUnits = defender.Units;

        Unit[] attackingUnitsTab = attackingUnits.GetUnits();
        Unit[] defendingUnitsTab = defendingUnits.GetUnits();

        int increment = height / UnitTree.TREESIZE;
        int place = 0;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (attackingUnitsTab[i] != null)
            {
                UnitAndAmount atroop = new UnitAndAmount(attacker.Units, i);
                unitsPos[0, place] = atroop;
            }
            if (defendingUnitsTab[i] != null)
            {
                UnitAndAmount dtroop = new UnitAndAmount(defender.Units, i);
                unitsPos[width-1, place] = dtroop;
            }
            
            place += increment;
        }

    }

    /// <summary>
    /// Constructor for Battlefield for battle with heroless units
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">The attacking hero</param>
    /// <param name="defender">The defending units</param>
    /// <param name="canWalk">Canwalk</param>
    public BattleField(int width, int height, Hero attacker, UnitTree defender, int[,] canWalk)
    {
        Width = width;
        Height = height;
        Attacker = attacker;
        CanWalk = canWalk;
        aStar = new AStarAlgo(canWalk, width, height, true);

        Unit[] attackingUnits = attacker.Units.GetUnits();
        Unit[] defendingUnits = defender.GetUnits();

        int increment = height / UnitTree.TREESIZE;
        int place = 0;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (attackingUnits[i] != null)
            {
                UnitAndAmount atroop = new UnitAndAmount(attacker.Units, i);
                unitsPos[0, place] = atroop;
            }
            if (defendingUnits[i] != null)
            {
                UnitAndAmount dtroop = new UnitAndAmount(defender, i);
                unitsPos[width - 1, place] = dtroop;
            }
            place += increment;
        }
    }

    public List<Vector2> unitMove(Point start, Point goal)
    {
        List<Vector2> path = AStar.calculate(start, goal);
        if (path.Count != 0)
        {
            unitsPos[goal.x, goal.y] = unitsPos[start.x, start.y];
            unitsPos[start.x, start.y] = null;
        }
        return path;
    }
    /// <summary>
    /// Method for moving unit and attacking target
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="goal">Goal position</param>
    /// <param name="attackedUnitPos">Position of unit to be attacked</param>
    /// <returns>List with movement path, null if no path</returns>
    public List<Vector2> UnitMoveAndAttack(Point start, Point goal, Point attackedUnitPos)
    {
        List<Vector2> path = aStar.calculate(start, goal);
        if (path.Count != 0)
        {
            unitsPos[goal.x, goal.y] = unitsPos[start.x, start.y];
            unitsPos[start.x, start.y] = null;
            UnitAndAmount attackingUnits = unitsPos[goal.x, goal.y];
            UnitAndAmount defendingUnits = unitsPos[attackedUnitPos.x, attackedUnitPos.y];
            attackingUnits.dealDamage(defendingUnits,false,0);
            if (defendingUnits.Unit.HaveNotRetaliated && defendingUnits.Amount > 0)
            {
                defendingUnits.dealDamage(attackingUnits,false,0);
                defendingUnits.Unit.HaveNotRetaliated = false;
            }
            if (attackingUnits.Amount < 0) attackingUnits.Amount = 0;
            if (defendingUnits.Amount < 0) defendingUnits.Amount = 0;
            
        }
        return path;
    }

    /// <summary>
    /// Method for attacking without moving
    /// </summary>
    /// <param name="attackingUnitPos">Position of attacking unit</param>
    /// <param name="defendingUnitPos">Position of defending unit</param>
    public void attackWithoutMoving (Point attackingUnitPos, Point defendingUnitPos, bool ranged)
    {
        int distance = HandyMethods.DistanceHex(attackingUnitPos, defendingUnitPos);
        UnitAndAmount attackingUnits = unitsPos[attackingUnitPos.x, attackingUnitPos.y];
        UnitAndAmount defendingUnits = unitsPos[defendingUnitPos.x, defendingUnitPos.y];
        attackingUnits.dealDamage(defendingUnits, ranged, distance);
        if (defendingUnits.Unit.HaveNotRetaliated && defendingUnits.Amount > 0 && !ranged)
        {
            defendingUnits.dealDamage(attackingUnits, ranged, distance);
            defendingUnits.Unit.HaveNotRetaliated = false;
        }
        if (attackingUnits.Amount < 0) attackingUnits.Amount = 0;
        if (defendingUnits.Amount < 0) defendingUnits.Amount = 0;
    }

    public void endCombat()
    {
        for (int i = 0; i<UnitTree.TREESIZE; i++)
        {
            if (attackingUnits.GetUnits()[i] != null && attackingUnits.getUnitAmount(i) == 0)
            {
                attackingUnits.removeUnit(i);
            }
            if (defendingUnits.GetUnits()[i] != null && defendingUnits.getUnitAmount(i) == 0)
            {
                defendingUnits.removeUnit(i);
            }
        }
    }

    public int[,] CanWalk
    {
        get
        {
            return canWalk;
        }

        set
        {
            canWalk = value;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public AStarAlgo AStar
    {
        get
        {
            return aStar;
        }

        set
        {
            aStar = value;
        }
    }

    public Hero Attacker
    {
        get
        {
            return attacker;
        }

        set
        {
            attacker = value;
        }
    }

    public Hero Defender
    {
        get
        {
            return defender;
        }

        set
        {
            defender = value;
        }
    }

    /// <summary>
    /// Inner class that combines unit and amount into one class.
    /// </summary>
    private class UnitAndAmount
    {
        UnitTree unitTree;
        int posInUnitTree;
        Unit unit;
        int amount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ut">UnitTree</param>
        /// <param name="i">Index of unit in UnitTree</param>
        public UnitAndAmount(UnitTree ut, int i)
        {
            UnitTree = ut;
            posInUnitTree = i;
            Unit = ut.GetUnits()[i];
            Amount = ut.getUnitAmount(i);
        }

        /// <summary>
        /// Method for dealing damage
        /// </summary>
        /// <param name="defendingUnits">Defending side</param>
        public void dealDamage(UnitAndAmount defendingUnits, bool ranged, int distance)
        {
            //Calculates min and max damage
            int minDamage = Unit.Unitstats.MinDamage * Amount;
            int maxDamage = Unit.Unitstats.MaxDamage * Amount;
            //Gets elementalAdvantage
            int elementAdvantage = defendingUnits.unit.Element.getElementResistance(unit.Element.getElement());
            //Determines total damage based on elementalAdvantage
            int totalDamage;
            if (elementAdvantage == Element.MIN)
            {
                totalDamage = maxDamage;
            }
            else if (elementAdvantage == Element.MAX)
            {
                totalDamage = minDamage;
            }
            else
            {
                totalDamage = Random.Range(minDamage, maxDamage+1);
            }
            //Checks for meele penalty or outside effective range
            if (unit.GetType().Equals(typeof(Ranged)))
            {
                Ranged r = (Ranged)unit;
                if (!ranged && r.MeleePenalty)
                {
                    totalDamage /= 2;
                }
                else if (ranged && r.Unitstats.EffectiveRange < distance)
                {
                    totalDamage /= 2;
                }
            }
            //Checks if crit
            if (unit.Unitstats.Luck / 10.0f >= Random.value) totalDamage *= 2;
            //Adjust totaldamage based on attack vs defense
            totalDamage = (int)(totalDamage*((float)unit.Unitstats.Attack / defendingUnits.Unit.Unitstats.Defence));
            //Applies damage and kills units.
            int killedUnits = totalDamage / defendingUnits.Unit.Unitstats.Health;
            defendingUnits.Amount -= killedUnits;
            defendingUnits.UnitTree.changeAmount(-killedUnits, PosInUnitTree);
            defendingUnits.unit.CurrentHealth -= totalDamage % defendingUnits.Unit.Unitstats.Health;
            if (defendingUnits.unit.CurrentHealth <= 0)
            {
                defendingUnits.Amount--;
                defendingUnits.Unit.CurrentHealth = defendingUnits.Unit.Unitstats.Health + defendingUnits.Unit.CurrentHealth;
            }
            
        }

        public Unit Unit
        {
            get
            {
                return unit;
            }

            set
            {
                unit = value;
            }
        }

        public int Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public UnitTree UnitTree
        {
            get
            {
                return unitTree;
            }

            set
            {
                unitTree = value;
            }
        }

        public int PosInUnitTree
        {
            get
            {
                return posInUnitTree;
            }

            set
            {
                posInUnitTree = value;
            }
        }
    }
}
