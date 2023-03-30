using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int cost;
    [SerializeField] private Text costText;
    [SerializeField] private AudioSource potionSound;
    
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        costText.text = cost.ToString();
    }

    public void BuyHealPotion(int health)
    {
        if (Inventory.Instance.Money >= cost)
        {
            player.RestoreHealth(health);
            potionSound.Play();
        }
    }
    public void BuyMaxHealthPotion(int maxHealth)
    {
        if (Inventory.Instance.Money >= cost)
        {
            player.SetMaxHealth(maxHealth);
            potionSound.Play();
        }
    }
    public void BuySpeedPotion(float speed)
    {
        if (Inventory.Instance.Money >= cost)
        {
            player.SetSpeed(speed);
            potionSound.Play();
        }
    }
    public void BuyDamagePotion(int damage)
    {
        if (Inventory.Instance.Money >= cost)
        {
            player.SetDamage(damage);
            potionSound.Play();
        }
    }
}
