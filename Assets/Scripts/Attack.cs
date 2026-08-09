using PriorityPauseSystem.Extension;
using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));

        while (true)
        {
            _animator.SetTrigger("Attack");
            yield return CoroutineExtension.WaitForSeconds(
                3f + Random.Range(0, 2f), 
                pauseLevel: PriorityPauseSystem.PauseLevelConstants.Attack, 
                ignoreTimeScale: false
            );
        }
    }
}
