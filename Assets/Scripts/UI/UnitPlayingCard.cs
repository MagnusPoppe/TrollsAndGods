using Units;

namespace UI
{
    /// <summary>
    /// Interface for player cards for units.
    /// </summary>
    interface UnitPlayingCard : Window
    {
        int GetImage();
        int GetAttack();
        int GetDefense();
        int GetMagic();
        int GetSpeed();
        int GetHealthPoints();
        string GetUnitName(); // TODO: GetUnit();
        Move[] GetMoves(); 
        string GetAbility(); // TODO: Ability
    }
}
