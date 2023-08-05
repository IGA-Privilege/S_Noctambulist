using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MeshCollider bedCollider;

    private void Update()
    {
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (player.isCatView)
        {
            bedCollider.enabled = false;
        }
        else
        {
            bedCollider.enabled = true;
        }
    }
}
