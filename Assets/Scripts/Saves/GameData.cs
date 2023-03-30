using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int money;
    public int day;
    public int health;
    public int maxHealth;
    public int damage;
    public float speed;
    public int waveType;


    public List<Buildings> buildings;
    public List<AlliesSolders> solders;

    public GameData()
    {
        waveType = 0;
        money = 350;
        day = 1;
        health = 10;
        maxHealth = health;
        damage = 1;
        speed = 3;

        buildings = new List<Buildings>();
        solders = new List<AlliesSolders>();
    }
}
