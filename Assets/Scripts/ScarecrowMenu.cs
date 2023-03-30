using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScarecrowMenu : MenuUI
{
    [SerializeField] private int _health;
    [SerializeField] private int _lvl;
    [SerializeField] private int _repairCost;
    [SerializeField] private int _moneyEarned;
    [SerializeField] private int _moneyPerSecond;

    private float _addMoneyTime;

    private void Start()
    {
        _addMoneyTime = Time.deltaTime + 1;

    }

    private void FixedUpdate()
    {
        if (Time.deltaTime <= _addMoneyTime)
        {
            _moneyEarned += _moneyPerSecond;
            _addMoneyTime = Time.deltaTime + 1;
        }
    }

    public void CollectMoneyButton()
    {
        Inventory.Instance.Money += _moneyEarned;
        _moneyEarned = 0;
    }

}
