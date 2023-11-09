using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNPC : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject renderer;

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (player.isCatView)
        {
            renderer.gameObject.SetActive(true);
        }
        else
        {
            renderer.gameObject.SetActive(false);
        }
    }
}
