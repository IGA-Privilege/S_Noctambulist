using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite humanViewSprite;
    [SerializeField] private Sprite catViewSprite;

    private void Update()
    {
        if (PlayerController.Instance.isCatView)
        {
            spriteRenderer.sprite = catViewSprite;
        }
        else
        {
            spriteRenderer.sprite = humanViewSprite;
        }
    }
}
