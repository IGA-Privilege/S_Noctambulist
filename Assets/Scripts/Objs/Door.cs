using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            TeleportPlayerSingleDistance(player);
        }
    }

    private void TeleportPlayerSingleDistance(PlayerController player)
    {
        player.MoveSingleDistance(0.7f);
    }
}
