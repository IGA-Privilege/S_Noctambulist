using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomDoor : MonoBehaviour
{
    [SerializeField] ClozeTextUI cloze;
    private readonly float doorOpenRotationY = 240f;
    private bool _hasOpened = false;

    private void Update()
    {
        if (!_hasOpened)
        {
            if (cloze.HasFinished)
            {
                StartCoroutine(OpenDoor());
                _hasOpened = true;
            }
        }

    }

    private IEnumerator OpenDoor()
    {
        while (transform.rotation.eulerAngles.y > doorOpenRotationY)
        {
            float openSpeed = 1f;
            float rotationY = Mathf.Lerp(transform.rotation.eulerAngles.y, doorOpenRotationY, openSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
            yield return new WaitForSeconds(0);
        }
    }
}
