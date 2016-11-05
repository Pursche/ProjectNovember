using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actors
{

}

public class Android : Actors
{
    public string androidID;
    public string androidName;
    public List<Part> androidParts = new List<Part>();

    public void EquipItem(string partName)
    {
        androidParts.Add(EntryPoint.parts.Find(x => x.partName == partName));
    }
}

public class Player : Android
{
    public string playerName;
}

public class Part : Actors
{
    public enum enumPartSlot { Weapon, Structural }

    public string partName;
    public string partID;
    public enumPartSlot partSlot;

    public float partMass;
    public float partRating;
    public float partHitPoints;
    public float partCoverage;


}

public class Power : Part
{ 
    public enum partType { }

    public float powerSupply;
    public float powerStorage;
    public float genHeat;

}

public class Propulsion : Part
{
    public enum partType { Treads, Legs, Wheels, Hover_Unit, Fighter_Unit }

    public partType propulsionType;
    public float moveModifier;
    public float powerUsed;
    public float genHeat;
}

public class Utility : Part
{
    public enum partType { }
}

public class Weapon : Part
{
    public enum partType {
        Special_Weapon,
        Melee_Weapon,
        Ballistic_Guns,
        Ballistic_Cannon,
        Energy_Cannons,
        Energy_Gun,
        Launcher
    }
    public enum attackType { Bashing, Piercing, Slashing }
    public enum defenceType { Emp, Ballistic, Energy, Thermal }

    // Overview
    public partType weaponType;
    public float genHeat;

    // Attack
    public bool weaponRanged;
    public float weaponCost; // Redo this with Energy / Matter system?
    public attackType weaponAttackType;

    // if Ranged
    public float weaponRecoil;
    public float weaponTargeting;
    public float weaponReloadTime; // Delay

    // onHit
    public float weaponMinAttack;
    public float weaponMaxAttack;
    public float weaponCritical;


}

public class Manipulation : Part
{
    public enum partType { }
    public float genHeat;
}
