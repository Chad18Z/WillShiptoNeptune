using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region Fields

    const int maxEnergy = 100;                  // the max energy the player has
    const int gruntSpacePirateDamage = 20;      // the amount of damage a grunt does to the player
    const int berserkerSpacePirateDamage = 40;  // the amount of damage the berserker does to the player
    const int playerMaxHealth = 100;            // the max health that the player has
    const float tileSize = 1.28f;               // the size of the tiles used for the environment
    public enum Tools { TripWire, StasisGrenade, PingDevice};      // handles control for player devices, need to add ping device when it is implemented

    #endregion

    #region Properties

    /// <summary>
    /// Returns the max energy for the players shooting ability
    /// </summary>
    public static int MaxEnergy
    {
        get { return maxEnergy; }
    }

    /// <summary>
    /// Returns the amount of damage the grunt space pirate does
    /// </summary>
    public static int GruntSpacePirateDamage
    {
        get { return gruntSpacePirateDamage; }
    }

    /// <summary>
    /// Returns the amount of damage the berserker space pirate does
    /// </summary>
    public static int BerserkerSpacePirateDamage
    {
        get { return berserkerSpacePirateDamage; }
    }

    /// <summary>
    /// Returns the max health of the player
    /// </summary>
    public static int PlayerMaxHealth
    {
        get { return playerMaxHealth; }
    }

    /// <summary>
    /// Returns the size of the tiles used for the environment
    /// </summary>
    public static float TileSize
    {
        get { return tileSize; }
    }

    #endregion
}
