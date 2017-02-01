using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {

    string name;
    string description;

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

    public Ability(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
