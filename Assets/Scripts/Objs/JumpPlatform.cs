using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpPlatform : MonoBehaviour
{
    public bool isRightPlaform;
    [SerializeField] private MeshRenderer _meshRenderer1;
    [SerializeField] private MeshRenderer _meshRenderer2;
    [SerializeField] private ParticleSystem _jumpParticle;

    public void OnPlayerJumpOnto()
    {
        if (!isRightPlaform)
        {
            StartCoroutine(DisappearForAWhile(2f));
        }
        else
        {
            StartCoroutine(SpawnParticleEffect());
        }
    }

    private IEnumerator SpawnParticleEffect()
    {
        yield return new WaitForSeconds(0.65f);
        Instantiate(_jumpParticle, transform.position, _jumpParticle.transform.rotation);
    }

    private IEnumerator DisappearForAWhile(float duration)
    {
        yield return new WaitForSeconds(1f);
        _meshRenderer1.enabled = false;
        _meshRenderer2.enabled = false;
        yield return new WaitForSeconds(duration);
        _meshRenderer1.enabled = true;
        _meshRenderer2.enabled = true;
    }


}
