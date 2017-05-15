using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAi
{
    private GraphicalBattlefield graphicalBattlefield;
    GameObject[,] field;
    GameObject[,] unitsOnField;
    private int height, width;
    private int x, y;

    public CombatAi(GraphicalBattlefield graphicalBattlefield, GameObject[,] field, GameObject[,] unitsOnField, int width, int height)
    {
        this.graphicalBattlefield = graphicalBattlefield;
        this.width = width;
        this.height = height;
        this.field = field;
        this.unitsOnField = unitsOnField;
    }

    public void act(UnitGameObject activeUnit)
    {
        x = activeUnit.LogicalPos.x;
        y = activeUnit.LogicalPos.y;
        UnitGameObject[] possibleTargets = new UnitGameObject[UnitTree.TREESIZE+1];
        int next = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (field[i, j].GetComponent<GroundGameObject>().IsOccupied && unitsOnField[i, j] != null &&
                    unitsOnField[i,j].GetComponent<UnitGameObject>().AttackingSide)
                {
                    possibleTargets[next++] = unitsOnField[i, j].GetComponent<UnitGameObject>();
                }
            }
        }
        UnitGameObject target = possibleTargets[0];
        int distance = HandyMethods.DistanceHex(activeUnit.LogicalPos,target.LogicalPos);
        for (int i = 1; i < possibleTargets.Length; i++)
        {
            if (possibleTargets[i] == null)
            {
                break;
            }
            int tmpDistance = HandyMethods.DistanceHex(activeUnit.LogicalPos, possibleTargets[i].LogicalPos);
            if (tmpDistance < distance)
            {
                target = possibleTargets[i];
                distance = tmpDistance;
            }
        }

        checkPos(target.LogicalPos.x,target.LogicalPos.y);
    }

    private void checkPos(int cx, int cy)
    {
        if (unitsOnField[cx, cy].GetComponent<UnitGameObject>().Attackable)
        {
            GroundGameObject[] neighbours = findNeighboursHex(cx, cy);
            Point tmpGoal = neighbours[0].LogicalPos;
            if (!neighbours[0].Reachable)
            {
                for (int i = 1; i < neighbours.Length; i++)
                {
                    if (neighbours[i] == null)
                    {
                        break;
                    }
                    if (neighbours[i].Reachable)
                    {
                        tmpGoal = neighbours[i].LogicalPos;
                        break;
                    }
                }
            }
            graphicalBattlefield.attackUnit(unitsOnField[cx, cy].GetComponent<UnitGameObject>(), tmpGoal);
        }
        else
        {
            while (!field[cx, cy].GetComponent<GroundGameObject>().Reachable)
            {
                if (cx < x)
                {
                    cx++;
                }
                else if (cx > x)
                {
                    cx--;
                }
                if (cy < y)
                {
                    cy++;
                }
                else if (cy > y)
                {
                    cy--;
                }
            }
            graphicalBattlefield.moveUnit(new Point(cx, cy));
        }

    }

    private GroundGameObject[] findNeighboursHex(int posX, int posY)
    {
        GroundGameObject[] neighbours = new GroundGameObject[6];
        int logPos = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                //ignore self
                if (x == 1 && y == 1)
                    continue;
                //ignores two positions based on if y is odd or even
                //this is to simulate the hex grid
                else if (posY % 2 == 0 && x == 2 && (y == 0 || y == 2))
                    continue;
                else if (posY % 2 == 1 && x == 0 && (y == 0 || y == 2))
                    continue;
                //adds neighbour if inside bounds
                if (posX + x - 1 >= 0 && posX + x - 1 < width
                    && posY + y - 1 >= 0 && posY + y - 1 < height)
                {
                    neighbours[logPos] = field[posX + x - 1, posY + y - 1].GetComponent<GroundGameObject>();
                    logPos++;
                }
            }
        }
        return neighbours;
    }
}
