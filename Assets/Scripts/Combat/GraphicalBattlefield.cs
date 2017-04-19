using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using MapGenerator;

/// <summary>
/// Class handles the graphical side of combat
/// </summary>
public class GraphicalBattlefield : MonoBehaviour
{
    private GameManager gm;
    BattleField battleField;
    int[,] canwalk;
    int width, height;
    bool inCombat;
    GameObject[,] field;
    GameObject[,] unitsOnField;
    GameObject parent;
    bool isWalking, finishedWalking, attacking;
    UnitGameObject[] initative;
    int whoseTurn;
    int livingAttackers, livingDefenders;
    private PossibleMovement possibleMovement;
    private GameObject hexagon, unit;
    private const int OFFSETX = -4, OFFSETY = -3;
    private const float ODDOFFSETX = 0.25f, ODDOFFSETY = -0.0f;
    private List<Vector2> path;
    private int step, frame;
    public float animationSpeed = 0.1f;
    private float towardNextStep;
    private Sprite attackSheet, moveSheet, troll;
    private RuntimeAnimatorController walkingAnimator, attackingAnimator;

    // Use this for initialization
    void Start () {
        InCombat = false;
        IsWalking = finishedWalking = false;
        hexagon = UnityEngine.Resources.Load<GameObject>("Sprites/Combat/HexagonPrefab");
        parent = GameObject.Find("Combat");
        unit = UnityEngine.Resources.Load<GameObject>("Sprites/Combat/Unit");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //todo implement loading of animations dynamically based on unit
        attackSheet = UnityEngine.Resources.Load<Sprite>("Sprites/Combat/Attack");
        moveSheet = UnityEngine.Resources.Load<Sprite>("Sprites/Combat/TrollWalk");
        troll = UnityEngine.Resources.Load<Sprite>("Sprites/Units/Troll/Frame 1");
        walkingAnimator = UnityEngine.Resources.Load<RuntimeAnimatorController>("Sprites/Combat/TrollWalk_0");
        attackingAnimator = UnityEngine.Resources.Load<RuntimeAnimatorController>("Sprites/Combat/Attack_0");
    }

	// Update is called once per frame
	void Update () {
		if (InCombat)
        {
            if (IsWalking)
            {
                if (towardNextStep >= 1)
                {
                    towardNextStep = 0;
                    step++;
                    if (step == path.Count)
                    {
                        finishedWalking = true;
                        isWalking = false;
                    }
                    else if ((int)path[step-1].y % 2 == 0)
                    {
                        if ((int)path[step].x < (int)path[step - 1].x)
                        {
                            getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                        {
                            getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
                        }
                    }
                    else
                    {
                        if ((int)path[step].x <= (int)path[step - 1].x)
                        {
                            getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                        {
                            getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
                        }
                    }
                }
                else
                {
                    Vector3 destination = field[(int)path[step].x, (int)path[step].y].transform.localPosition;
                    getUnitWhoseTurnItIs().transform.localPosition = Vector2.MoveTowards(getUnitWhoseTurnItIs().transform.localPosition, destination, animationSpeed);
                    towardNextStep += animationSpeed;
                }
            }
            else if (finishedWalking)
            {
                unitsOnField[(int)path[step - 1].x, (int)path[step - 1].y] = unitsOnField[getUnitWhoseTurnItIs().LogicalPos.x, getUnitWhoseTurnItIs().LogicalPos.y];
                unitsOnField[getUnitWhoseTurnItIs().LogicalPos.x, getUnitWhoseTurnItIs().LogicalPos.y] = null;
                getUnitWhoseTurnItIs().LogicalPos = new Point(path[step-1]);
                field[getUnitWhoseTurnItIs().LogicalPos.x, getUnitWhoseTurnItIs().LogicalPos.y]
                    .GetComponent<GroundGameObject>().IsOccupied = true;
                canwalk[getUnitWhoseTurnItIs().LogicalPos.x, getUnitWhoseTurnItIs().LogicalPos.y] = MapMaker.CANNOTWALK;

                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = !getUnitWhoseTurnItIs().AttackingSide;

                if (attacking)
                {
                    getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().sprite = attackSheet;
                    getUnitWhoseTurnItIs().GetComponent<Animator>().runtimeAnimatorController = attackingAnimator;
                    frame = 0;
                }
                else
                {
                    getUnitWhoseTurnItIs().GetComponent<Animator>().runtimeAnimatorController = null;
                    getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().sprite = troll;
                    nextTurn();
                }
                finishedWalking = false;
            }
            else if(attacking)
            {
                frame++;
                if (frame == 50)
                {
                    attacking = false;
                    getUnitWhoseTurnItIs().GetComponent<Animator>().runtimeAnimatorController = null;
                    getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().sprite = troll;
                    nextTurn();
                }
            }
            else if (livingAttackers == 0 || livingDefenders == 0)
            {
                endCombat();
            }
        }
	}

    /// <summary>
    /// Readies battlefield between two heroes
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending hero</param>
    public void beginCombat(int width, int height, Hero attacker, Hero defender)
    {
        Width = width;
        Height = height;
        InCombat = true;
        Canwalk = new int[width, height];
        //todo add obstacles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Canwalk[x, y] = MapGenerator.MapMaker.CANWALK;
            }
        }
        BattleField = new BattleField(width, height, attacker, defender, Canwalk);

        populateField();
        populateInitative(attacker, defender.Units);
        possibleMovement = new PossibleMovement(field,unitsOnField,canwalk,width,height);
        flipReachableAndAttackable();
    }

    /// <summary>
    /// Readies battlefield between hero and neutral units
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending units</param>
    public void beginCombat(int width, int height, Hero attacker, UnitTree defender)
    {
        Width = width;
        Height = height;
        InCombat = true;
        Canwalk = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Canwalk[x, y] = MapGenerator.MapMaker.CANWALK;
            }
        }
        BattleField = new BattleField(width, height, attacker, defender, Canwalk);

        populateField();
        populateInitative(attacker, defender);
        possibleMovement = new PossibleMovement(field, unitsOnField, canwalk, width, height);
        flipReachableAndAttackable();
    }

    /// <summary>
    /// fills 2d array for groundtiles
    /// </summary>
    public void populateField()
    {
        field = new GameObject[width,height];
        float gx = 0, gy = 0;
        float sx = 1.0f, sy = 1.0f;
        for (int x = 0; x < width; x++)
        {
            float scaleFactorY = 0.1f;
            float scaleFactorX = 0.1f;
            float totalRemovedX = 0;
            for (int y = 0; y < height; y++)
            {
                GameObject go = Instantiate(hexagon);
                go.name = "Ground x=" + x + " y=" + y;
                GroundGameObject ggo = go.GetComponent<GroundGameObject>();
                ggo.GraphicalBattlefield = this;
                ggo.LogicalPos = new Point(x, y);
                field[x, y] = go;
                go.transform.SetParent(parent.transform);

                float fromCenter = ((width / 2) - x ) / (width + 0.0f);
                totalRemovedX += (0.5f *  fromCenter * ((1 - sx) ) );
                gx += (0.5f *  fromCenter * ((1 - sx)));
                if (y % 2 == 0)
                {
                    go.transform.localPosition = new Vector2(gx+OFFSETX, gy+OFFSETY);
                }
                else
                {
                    go.transform.localPosition = new Vector2(gx + OFFSETX + ODDOFFSETX, gy + OFFSETY + ODDOFFSETY);
                }
                sy -= scaleFactorY;
                scaleFactorY *= .6f;

                sx -= scaleFactorX;
                scaleFactorX *= .3f;

                go.transform.localScale = new Vector3(sx, sy, 1);
                gy += 0.42f - ( 0.43f * (1-sy)); // Scaling y axis with the percent scaled down.
            }
            gx -= totalRemovedX;
            gx += 0.43f ;
            sy = sx = 1f;
            gy = 0;
        }
    }

    /// <summary>
    /// Readies initative and populates 2d array for units
    /// </summary>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending units</param>
    public void populateInitative(Hero attacker, UnitTree defender)
    {
        unitsOnField = new GameObject[width,height];
        Initative = new UnitGameObject[UnitTree.TREESIZE * 2];
        int logPos = 0;
        int increment = Height / UnitTree.TREESIZE;
        int place = 0;
        livingAttackers = livingDefenders = 0;
        // Adds attacking units
        UnitTree units = attacker.Units;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = Instantiate(unit, parent.transform);
                go.name = "a" + i;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = troll;
                sr.sortingLayerName = "CombatUnits";
                TextMesh tm = go.GetComponentInChildren<TextMesh>();
                tm.text = ""+units.getUnitAmount(i);
                tm.GetComponent<Renderer>().sortingLayerName = "CombatUnits";
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = true;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                ugo.LogicalPos = new Point(0, place);
                Initative[logPos++] = ugo;
                //set correct graphical pos
                go.transform.localPosition = field[0, place].transform.localPosition;
                UnitsOnField[0, place] = go;
                field[0, place].GetComponent<GroundGameObject>().IsOccupied = true;
                livingAttackers++;
            }
            place += increment;
        }

        //Adds defending units
        units = defender;
        place = 0;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = Instantiate(unit, parent.transform);
                go.name = "d" + i;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = troll;
                sr.sortingLayerName = "CombatUnits";
                TextMesh tm = go.GetComponentInChildren<TextMesh>();
                tm.text = "" + units.getUnitAmount(i);
                tm.GetComponent<Renderer>().sortingLayerName = "CombatUnits";
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = false;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                ugo.LogicalPos = new Point(width-1, place);
                Initative[logPos++] = ugo;
                //set correct graphical pos
                go.transform.localPosition = field[width - 1, place].transform.localPosition;
                UnitsOnField[Width-1, place] = go;
                field[width-1, place].GetComponent<GroundGameObject>().IsOccupied = true;
                livingDefenders++;
            }
            place += increment;
        }
        Array.Sort(initative);
        Array.Reverse(initative);
        WhoseTurn = 0;
        initative[whoseTurn].ItsTurn = true;
    }

    /// <summary>
    /// Ends combat
    /// </summary>
    public void endCombat()
    {
        battleField.endCombat();
        InCombat = false;
        gm.exitCombat(livingAttackers != 0);
    }

    /// <summary>
    /// Initiates attack on unit
    /// </summary>
    /// <param name="defender">Unit thats being attacked</param>
    /// <param name="goal">Place on field attacking unit is going to</param>
    public void attackUnit(UnitGameObject defender, Point goal)
    {
        if (!isWalking && !finishedWalking && !attacking)
        {
            UnitGameObject activeUnit = initative[whoseTurn];
            Unit attackingUnit = activeUnit.UnitTree.GetUnits()[activeUnit.PosInUnitTree];
            //If unit does not need to move, call on method for attacking without moving
            if (activeUnit.LogicalPos.Equals(goal))
            {
                battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, false);
                beginAttacking(defender.LogicalPos.x);
            }
            else
            {
                //checks if unit is ranged, and if it has ammo. if not move and attack
                if (attackingUnit.IsRanged)
                {
                    Ranged r = (Ranged) attackingUnit;
                    if (r.Ammo > 0 && !r.Threatened)
                    {
                        battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, true);
                        beginAttacking(defender.LogicalPos.x);
                    }
                    else
                    {
                        path = battleField.UnitMoveAndAttack(activeUnit.LogicalPos, goal, defender.LogicalPos);
                        BeginWalking();
                        attacking = true;
                    }
                }
                else
                {
                    path = battleField.UnitMoveAndAttack(activeUnit.LogicalPos, goal, defender.LogicalPos);
                    BeginWalking();
                    attacking = true;
                }
            }
            //Updates living units counts
            if (defender.UnitTree.getUnitAmount(defender.PosInUnitTree) == 0)
            {
                if (defender.AttackingSide) livingAttackers--;
                else livingDefenders--;
            }
            if (activeUnit.UnitTree.getUnitAmount(activeUnit.PosInUnitTree) == 0)
            {
                if (activeUnit.AttackingSide) livingAttackers--;
                else livingDefenders--;
            }
            Debug.Log(livingAttackers + " " + livingDefenders);
            updateAmount(defender);
            updateAmount(getUnitWhoseTurnItIs());
        }
    }

    /// <summary>
    /// Moves unit logically by calling on unitmove and graphically by calling on beginwalking
    /// </summary>
    /// <param name="goal">Destination</param>
    public void moveUnit(Point goal)
    {
        if (!finishedWalking && !isWalking && !attacking)
        {
            UnitGameObject activeUnit = initative[whoseTurn];
            path = battleField.unitMove(activeUnit.LogicalPos, goal);
            BeginWalking();
        }
    }

    /// <summary>
    /// Begins walking animation
    /// </summary>
    public void BeginWalking()
    {
        field[initative[whoseTurn].LogicalPos.x, initative[whoseTurn].LogicalPos.y].GetComponent<GroundGameObject>()
            .IsOccupied = false;
        canwalk[initative[whoseTurn].LogicalPos.x, initative[whoseTurn].LogicalPos.y] = MapMaker.CANWALK;
        isWalking = true;
        step = 0;
        getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().sprite = moveSheet;
        getUnitWhoseTurnItIs().GetComponent<Animator>().runtimeAnimatorController = walkingAnimator;

        if (getUnitWhoseTurnItIs().LogicalPos.y % 2 == 0)
        {
            if ((int)path[step].x < getUnitWhoseTurnItIs().LogicalPos.x)
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            if ((int)path[step].x <= getUnitWhoseTurnItIs().LogicalPos.x)
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    public void beginAttacking(int x)
    {
        attacking = true;
        frame = 0;
        getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().sprite = attackSheet;
        getUnitWhoseTurnItIs().GetComponent<Animator>().runtimeAnimatorController = attackingAnimator;

        if (getUnitWhoseTurnItIs().LogicalPos.y % 2 == 0)
        {
            if (x < getUnitWhoseTurnItIs().LogicalPos.x)
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            if (x <= getUnitWhoseTurnItIs().LogicalPos.x)
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                getUnitWhoseTurnItIs().GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    /// <summary>
    /// Moves to next in initative
    /// </summary>
    public void nextTurn()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                field[x, y].GetComponent<GroundGameObject>().Reachable = false;
                field[x, y].GetComponent<GroundGameObject>().MarkReachable(false);
                if (unitsOnField[x, y] != null) unitsOnField[x, y].GetComponent<UnitGameObject>().Attackable = false;
            }
        }
        initative[whoseTurn].ItsTurn = false;
        whoseTurn++;
        if (whoseTurn == initative.Length || initative[whoseTurn] == null) whoseTurn = 0;
        initative[whoseTurn].ItsTurn = true;
        flipReachableAndAttackable();
        getUnitWhoseTurnItIs().UnitTree.GetUnits()[getUnitWhoseTurnItIs().PosInUnitTree].HaveNotRetaliated = true;
    }

    /// <summary>
    /// method flips reachable and attackable bools in UnitGameObjects based on possibleMovement and ranged
    /// </summary>
    private void flipReachableAndAttackable()
    {
        Unit unitWhoseTurnItIs = Initative[whoseTurn].UnitTree.GetUnits()[Initative[whoseTurn].PosInUnitTree];
        possibleMovement.flipReachable(Initative[whoseTurn].LogicalPos, unitWhoseTurnItIs.Unitstats.Speed);
        if (unitWhoseTurnItIs.IsRanged)
        {
            Ranged ranged = (Ranged)unitWhoseTurnItIs;
            if (ranged.Ammo > 0 && !ranged.Threatened)
            {
                flipAttackable();
            }
        }
    }

    /// <summary>
    /// Flips all enemy units to attackable
    /// </summary>
    private void flipAttackable()
    {
        for (int i = 0; i < initative.Length; i++)
        {
            if (initative[i] != null && initative[i].AttackingSide != getUnitWhoseTurnItIs().AttackingSide)
            {
                initative[i].Attackable = true;
            }
        }
    }

    /// <summary>
    /// Updates unit amount text representation
    /// </summary>
    /// <param name="ugo">Unit to be updated</param>
    public void updateAmount(UnitGameObject ugo)
    {
        ugo.GetComponentInChildren<TextMesh>().text = ""+ugo.UnitTree.getUnitAmount(ugo.PosInUnitTree);
    }

    /// <summary>
    /// Gets the unit whose turn it is
    /// </summary>
    /// <returns>UnitGameObject whose turn it is</returns>
    public UnitGameObject getUnitWhoseTurnItIs()
    {
        return Initative[WhoseTurn];
    }

    public UnitGameObject[] Initative
    {
        get
        {
            return initative;
        }

        set
        {
            initative = value;
        }
    }

    public int WhoseTurn
    {
        get
        {
            return whoseTurn;
        }

        set
        {
            whoseTurn = value;
        }
    }

    public BattleField BattleField
    {
        get
        {
            return battleField;
        }

        set
        {
            battleField = value;
        }
    }

    public int[,] Canwalk
    {
        get
        {
            return canwalk;
        }

        set
        {
            canwalk = value;
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

    public bool InCombat
    {
        get
        {
            return inCombat;
        }

        set
        {
            inCombat = value;
        }
    }

    public GameObject[,] Field
    {
        get
        {
            return field;
        }

        set
        {
            field = value;
        }
    }

    public GameObject[,] UnitsOnField
    {
        get
        {
            return unitsOnField;
        }

        set
        {
            unitsOnField = value;
        }
    }

    public bool IsWalking
    {
        get
        {
            return isWalking;
        }

        set
        {
            isWalking = value;
        }
    }
}
