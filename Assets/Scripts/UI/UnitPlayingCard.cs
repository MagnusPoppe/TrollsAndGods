﻿using Units;
using Abilities;
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
        Ability GetAbility();
    }
}
