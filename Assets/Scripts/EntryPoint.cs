using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntryPoint : MonoBehaviour {

    #region Public
    public static List<Part> parts = new List<Part>();
    #endregion

    #region Private

    private List<Android> androids = new List<Android>();
    private Player player;

    #endregion

    // Use this for initialization
    void Start () {
        Debug.Log("Start!");
        GenerateObjects();
        Debug.Log("Amount of parts in list: " + parts.Count);
        GeneratePlayer();
        Debug.Log("Player made");
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    // Make all of the parts, FFS put this in a ScriptableObject dude!! 
    void GenerateObjects()
    {
        // --------------------------------------------------------
        // Initial parts structure totaly 100% riped and taken from
        // http://gridsagegames.com/wiki/Item
        // --------------------------------------------------------

        // Starter Deffinitions
        Power newPower;
        Propulsion newPropulsion;
        Utility newUtility;
        Weapon newWeapon;
        Manipulation newManipulation;

        #region Power_Systems
        // ------------ Power System
        // --- Engines
        #region New_Item
        newPower = new Power();
            newPower.partName = "Standard Engine";
            newPower.powerStorage = 10;
            newPower.powerSupply = 2;
            newPower.genHeat = 1;
            parts.Add(newPower);
        #endregion

        // --- Power_cores
        #region New_Item
            newPower = new Power();
            newPower.partName = "Standard Power Core";
            newPower.powerStorage = 50;
            newPower.powerSupply = 1;
            newPower.genHeat = 3;
            parts.Add(newPower);
        #endregion

        // --- Reactors 
        #region New_Item
            newPower = new Power();
            newPower.partName = "Standard Reactor";
            newPower.powerStorage = 25;
            newPower.powerSupply = 5;
            newPower.genHeat = 10;
            parts.Add(newPower);
        #endregion

        #endregion

        #region Propulsion_Parts
        // ------------ Propulsion Parts
        // --- Legs
        #region New_Item
            newPropulsion = new Propulsion();
            newPropulsion.propulsionType = Propulsion.partType.Legs;
            newPropulsion.partName = "Standard Legs";
            parts.Add(newPropulsion);
        #endregion

        // --- Treads
        #region New_Item
            newPropulsion = new Propulsion();
            newPropulsion.propulsionType = Propulsion.partType.Treads;
            newPropulsion.partName = "Standard Tread";
            parts.Add(newPropulsion);
        #endregion

        // --- Wheels 
        #region New_Item
            newPropulsion = new Propulsion();
            newPropulsion.propulsionType = Propulsion.partType.Wheels;
            newPropulsion.partName = "Standard Wheel";
            parts.Add(newPropulsion);
        #endregion

        // --- Fighter Unit (Thrusters)
        #region New_Item
            newPropulsion = new Propulsion();
            newPropulsion.propulsionType = Propulsion.partType.Fighter_Unit;
            newPropulsion.partName = "Standard Fighter Unit";
            parts.Add(newPropulsion);
        #endregion

        // --- Hover Unit (Slower, Kinda Ion Engine types... )
        #region New_Item
            newPropulsion = new Propulsion();
            newPropulsion.propulsionType = Propulsion.partType.Hover_Unit;
            newPropulsion.partName = "Standard Hover Unit";
            parts.Add(newPropulsion);
        #endregion

        #endregion

        // ------------ Utility Parts
        newUtility = new Utility();

        parts.Add(newUtility);

        #region Weapon_Parts
        // ------------ Weapon Parts
        // --- Legs
        #region New_Item
        newWeapon = new Weapon();
        newWeapon.weaponType = Weapon.partType.Ballistic_Cannon;

        parts.Add(newWeapon);
        #endregion

        #endregion

        // ------------ Manipulation Parts
        newManipulation = new Manipulation();

        parts.Add(newManipulation);
    }

    // Generate player
    void GeneratePlayer()
    {
        player = new Player();
        player.EquipItem("Standard Engine");
        player.EquipItem("Standard Legs");
    }

}
