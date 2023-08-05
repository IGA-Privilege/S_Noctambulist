using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StorageDoor : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private readonly float doorOpenRotationY = 120f;
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
