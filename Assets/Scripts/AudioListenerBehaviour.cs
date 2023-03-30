using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        if (player != null)
            transform.position = new Vector2(player.position.x, player.position.y);
    }
}
