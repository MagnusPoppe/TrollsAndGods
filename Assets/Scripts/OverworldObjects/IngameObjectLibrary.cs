using System;
using UnityEngine;

public class IngameObjectLibrary
{
    // CONSTANTS:
    const int ENVIROMENT_TYPES = 8;
    const int OBJECT_TYPES     = 50;

    // TILETYPES (ARRAY 1-DIMENTION):
    const int ENVIROMENT_NEUTRAL        = 0;
    const int ENVIROMENT_DIRT           = 1;
    const int ENVIROMENT_GRASS          = 2;
    const int ENVIROMENT_LAVA           = 3;
    const int ENVIROMENT_WATER          = 4;
    const int ENVIROMENT_DARKNESS       = 5;

    // OBJECT TYPES (ARRAY 2-DIMENTION):
    const int TILE_TYPE                 = 0;
    const int BUILDING_TYPE_RESOURCE    = 1;
    const int BUILDING_TYPE_DWELLING    = 2;
    const int BUILDING_TYPE_TOWN        = 3;
    const int BUILDING_TYPE_MISC        = 4;
    const int PICKUP_TYPE_RESOURCE      = 5;
    const int PICKUP_TYPE_ARTIFACT      = 6;

    // TILE TYPES (ARRAY 3-DIMENTION):
    const int NORTH_EDGE     = 3;
    const int NORTHEAST_EDGE = 4;
    const int EAST_EDGE      = 5;
    const int SOUTHEAST_EDGE = 6;
    const int SOUTH_EDGE     = 7;
    const int SOUTHWEST_EDGE = 8;
    const int WEST_EDGE      = 9;
    const int NORTHWEST_EDGE = 10;

    GameObject[,,] library;


    public IngameObjectLibrary()
    {
    }

}
 
