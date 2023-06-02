using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private InteractionType onInteract;
    [SerializeField] private Sprite relatedSprite;
    [SerializeField] private bool needPrerequisites;
    [SerializeField] private List<Interactable> prerequisiteInvestigations = new List<Interactable>();

    [HideInInspector]
    public bool isInvestigated = false;
    private bool canInteract = true;

    private void Awake()
    {
        if (needPrerequisites)
        {
            canInteract = false;
        }
    }

    private void FixedUpdate()
    {
        if (needPrerequisites)
        {
            bool shouldUnlock = true;
            foreach (var interactable in prerequisiteInvestigations)
            {
                if (!interactable.isInvestigated)
                {
                    shouldUnlock = false;
                }
            }
            if (shouldUnlock)
            {
                canInteract = true;
            }
        }

        // 视觉化方法，如果不再需要用到则直接删除，同时移除URP-HighFidelity-Renderer.asset下的Screen Space Outline组件
        HandleOutlines();

    }

    private void HandleOutlines()
    {
        if ((!isInvestigated) && canInteract)
        {
            gameObject.layer = LayerMask.NameToLayer("Outlined");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void OnPlayerInteract()
    {
        if (canInteract)
        {
            isInvestigated = true;
            canInteract = false;

            switch (onInteract)
            {
                case InteractionType.ShowImage:
                    {
                        GameManager.Instance.OpenItemInfoUI(relatedSprite);
                        break;
                    }
                case InteractionType.OpenDiary:
                    {
                        GameManager.Instance.OpenDiaryUI();
                        break;
                    }
            }
        }
    }


}

enum InteractionType
{
    ShowImage, OpenDiary
}
