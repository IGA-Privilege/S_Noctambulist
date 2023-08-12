using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Interactable
{
    [SerializeField] private Material catViewMaterial;
    [SerializeField] private Material humanViewMaterial;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        UpdateMaterialState();
    }

    private void UpdateMaterialState()
    {
        if (PlayerController.Instance.isCatView)
        {
            _meshRenderer.material = catViewMaterial;
        }
        else
        {
            _meshRenderer.material = humanViewMaterial;
        }
    }
}
