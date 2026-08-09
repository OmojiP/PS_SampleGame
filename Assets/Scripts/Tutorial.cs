using LitMotion;
using LitMotion.Extensions;
using PriorityPauseSystem;
using PriorityPauseSystem.Extension;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Text _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpecialAttack[] specialAttacks = FindObjectsByType<SpecialAttack>(sortMode: FindObjectsSortMode.None);
        foreach (var specialAttack in specialAttacks)
        {
            specialAttack.OnSpecialAttackStart += () =>
            {
                StartCoroutine(PlayTutorial("チュートリアル: これがスペシャル技です。超強力な一撃が出ます", 0.05f, PriorityPauseSystem.PauseLevelConstants.Tutorial));
            };
        }

        _canvasGroup.gameObject.SetActive(false);
    }


    IEnumerator PlayTutorial(string text, float characterSpan, int pauseLevel)
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _text.text = string.Empty;

        yield return CoroutineExtension.WaitForSeconds(
            0.5f,
            pauseLevel: pauseLevel,
            ignoreTimeScale: false
        );

        // TutorialのPauseLevelよりも1つ低いPauseLevelで一時停止するようにする
        using (PauseSystem.PushPause(PriorityPauseSystem.PauseLevelConstants.Tutorial - 1))
        {
            yield return LMotion.Create(0f, 1f, 0.5f)
                .BindToAlpha(_canvasGroup)
                .RegisterToPauseSystem(pauseLevel, out _)
                .ToYieldInstruction();

            foreach (var character in text.ToCharArray())
            {
                _text.text += character;
               
                yield return CoroutineExtension.WaitForSeconds(
                    characterSpan,
                    pauseLevel: pauseLevel,
                    ignoreTimeScale: false
                );
            }

            yield return CoroutineExtension.WaitForSeconds(
                1f,
                pauseLevel: pauseLevel,
                ignoreTimeScale: false
            );

            yield return LMotion.Create(1f, 0f, 0.5f)
                .BindToAlpha(_canvasGroup)
                .RegisterToPauseSystem(pauseLevel, out _)
                .ToYieldInstruction();
        }

        yield return CoroutineExtension.WaitForSeconds(
            0.5f,
            pauseLevel: pauseLevel,
            ignoreTimeScale: false
        );

        _canvasGroup.gameObject.SetActive(false);
    }
}
