using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomDoor : MonoBehaviour
{
    [SerializeField] ClozeTextUI cloze;
    [SerializeField] Collider doorCollider;
    private readonly float doorOpenRotationY = -90f;
    private Quaternion originalRotation;
    private float eulerY = 0f;
    private bool _hasOpened = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(OpenDoor());
        }

        if (!_hasOpened)
        {
            if (cloze.HasFinished)
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
