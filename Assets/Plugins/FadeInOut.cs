using UnityEngine;
using System.Collections;

public class FadeInOutText : MonoBehaviour {

    private GameObject _flashText;
    private Color _guiColor = new Color(1, 1, 1, 0.0f);

	void Start () {
        _flashText = (GameObject)Instantiate(Resources.Load<GameObject>("UiText"));
	}

    IEnumerator Fade(float start, float end, float length)
    {
        /*for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * (2 / length))
        {
            if (_flashText != null) _flashText.guiText.color = new Color(_guiColor.r, _guiColor.g, _guiColor.b, Mathf.Lerp(start, end, i));
            //yield return new WaitForEndOfFrame();
        }*/
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * (1 / length))
        {
            _flashText.guiText.color = new Color(_guiColor.r, _guiColor.g, _guiColor.b, Mathf.Lerp(end, start, i));
            yield return new WaitForEndOfFrame();
        }

        print("destroy");
        if (_flashText != null) Destroy(_flashText);
    }

    // ------------- other fade functions -----------------
    float alpha = 0;
    float duration = 1;

    IEnumerator FadeIn()
    {
        while (alpha < 1)
        {
            alpha += Time.deltaTime * (1 / duration);
            _flashText.guiText.color = new Color(_guiColor.r, _guiColor.g, _guiColor.b, alpha);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * (1 / duration);
            _flashText.guiText.color = new Color(_guiColor.r, _guiColor.g, _guiColor.b, alpha);
            yield return new WaitForEndOfFrame();
        }
    }
}
