using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNPC : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MeshRenderer renderer;

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (player.isCatView)
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }
}
