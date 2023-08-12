using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StorageDoor : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Collider doorCollider;
    private readonly float doorOpenRotationY = -90f;
    private float eulerY = 0f;
    private Quaternion originalRotation;
    private bool _hasOpened = false;


    private void Update()
    {
        if (!_hasOpened)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 2f)
            {
                StartCoroutine(OpenDoor());
                _hasOpened = true;
            }
        }

    }

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    private IEnumerator OpenDoor()
    {
        doorCollider.enabled = false;
        while (true)
        {
            float openSpeed = 1f;
            eulerY = Mathf.Lerp(eulerY, doorOpenRotationY, openSpeed * Time.deltaTime);
            transform.rotation = originalRotation * Quaternion.Euler(0f, eulerY, 0f);
            yield return new WaitForSeconds(0);
        }
    }
}
