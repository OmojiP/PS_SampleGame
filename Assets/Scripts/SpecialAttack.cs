using PriorityPauseSystem;
using PriorityPauseSystem.Extension;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] Animator _uiAnimator;
    [SerializeField] Animator _modelAnimator;
    [SerializeField] Button _button;

    public event System.Action OnSpecialAttackStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(() => StartCoroutine(OnButtonClick()));

        // キャラクターのアニメーションはスペシャル技までで使用するので、それ以上は停止するように登録しておく
        _modelAnimator.RegisterToPauseSystem(PriorityPauseSystem.PauseLevelConstants.SpecialAttack);
        // UIのアニメーションはスペシャル技までで使用するので、それ以上は停止するように登録しておく
        _uiAnimator.RegisterToPauseSystem(PriorityPauseSystem.PauseLevelConstants.SpecialAttack);
    }

    IEnumerator OnButtonClick()
    {
        // AttackまでのPauseLevelで一時停止するようにする
        using (PauseSystem.PushPause(PriorityPauseSystem.PauseLevelConstants.Attack))
        {
            OnSpecialAttackStart?.Invoke();

            _uiAnimator.SetTrigger("SpecialAttack");

            yield return CoroutineExtension.WaitForSeconds(
                2f, 
                pauseLevel: PriorityPauseSystem.PauseLevelConstants.SpecialAttack, 
                ignoreTimeScale: false
            );

            _modelAnimator.SetTrigger("SpecialAttack");
  
            yield return CoroutineExtension.WaitForSeconds(
                3f,
                pauseLevel: PriorityPauseSystem.PauseLevelConstants.SpecialAttack,
                ignoreTimeScale: false
            );
        }
    }
}
