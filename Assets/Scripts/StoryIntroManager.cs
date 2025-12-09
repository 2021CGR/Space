using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryIntroManager : MonoBehaviour
{
    public CanvasGroup panel;
    public Image image;
    public Sprite[] frames;
    public float fadeIn = 0.5f;
    public float fadeOut = 0.4f;
    public float hold = 1.5f;
    public bool freezeGameplay = true;
    public bool clickToAdvance = true;
    public KeyCode advanceKey = KeyCode.Space;
    public KeyCode skipKey = KeyCode.Escape;

    void Start()
    {
        if (panel == null || image == null) return;
        if (frames == null || frames.Length == 0) return;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        if (freezeGameplay) Time.timeScale = 0f;
        if (CursorManager.Instance != null) CursorManager.Instance.SetCursorVisible(true);
        panel.gameObject.SetActive(true);
        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            panel.alpha = 0f;
            float t = 0f;
            while (t < fadeIn)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(0f, 1f, t / fadeIn);
                yield return null;
            }
            float elapsed = 0f;
            while (elapsed < hold)
            {
                if (clickToAdvance && (Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0))) break;
                if (Input.GetKeyDown(skipKey)) { i = frames.Length - 1; break; }
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            t = 0f;
            while (t < fadeOut)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(1f, 0f, t / fadeOut);
                yield return null;
            }
        }
        panel.gameObject.SetActive(false);
        if (freezeGameplay) Time.timeScale = 1f;
        if (CursorManager.Instance != null) CursorManager.Instance.SetCursorVisible(false);
    }
}
