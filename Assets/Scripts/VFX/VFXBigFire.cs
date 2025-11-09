using System.Collections;
using UnityEngine;

public class VFXBigFire : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }


    public void StartFire(float duration)
    {
        StartCoroutine(FireCo(5));
    }



    private IEnumerator FireCo(float duration)
    {
        _animator.SetBool("Fire", true);
        yield return new WaitForSeconds(duration);
        StopFire();
    }


    public void StopFire()
    {
        _animator.SetBool("Fire", false);
    }
}
