using UI;
using Units;

namespace TownView
{
    public class UnitBuilding : Building, UnitPlayingCard
    {
        private Unit unit;

        public Unit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public UnitBuilding(string name, string description, bool[] requirements, Cost cost, int localID, int LOCAL_SPRITEID_BLUEPRINT)
            : base(name, description, requirements, cost, localID, LOCAL_SPRITEID_BLUEPRINT)
        {
            Unit = unit;
        }


        public int GetImage()
        {
            throw new System.NotImplementedException();
        }

        public int GetAttack()
        {
            return Unit.Unitstats.Attack;
        }

        public int GetDefense()
        {
            return Unit.Unitstats.Defence;
        }

        public int GetMagic()
        {
            return -1;
        }

        public int GetSpeed()
        {
            return Unit.Unitstats.Speed;
        }

        public int GetHealthPoints()
        {
            return Unit.Unitstats.Health;
        }

        public string GetUnitName()
        {
            return Unit.Name;
        }

        public Move[] GetMoves()
        {
            return Unit.Moves;
        }

        public string GetAbility()
        {
            return "Not yet implemented, buddy. :-)";        }
    }
}

