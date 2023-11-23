using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.isCatView)
            {
                player.TeleportPlayerTo(destination.position);
                Debug.Log("1111");
            }
        }
    }
}
