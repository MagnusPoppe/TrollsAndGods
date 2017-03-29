using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
    private int IDfrom, IDto;
    private string description;

    public abstract void execute();
}
