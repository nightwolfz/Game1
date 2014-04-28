using UnityEngine;


public class TextEffects : MonoBehaviour
{
    private GameObject _flash;
    private TextMesh _text;
    //private Color _color = new Color(255, 255, 255, 0.0f);

    public enum Size
    {
        Huge, Big, Medium, Small
    }
    public enum Effect
    {
        Tutorial, Slow, Normal, Fast
    }

    public void Add(string text, 
        Size size = Size.Medium, 
        Effect effect = Effect.Normal, 
        Vector2 position = new Vector2(),
        Color? color = null
        )
    {
        _flash = (GameObject)Instantiate(Resources.Load<GameObject>("UiText"));
        _flash.transform.parent = transform;
        _flash.transform.position = position;

        _text = _flash.GetComponent<TextMesh>();
        _text.text = text;
        _text.color = color ?? new Color(255, 255, 255, 0.0f);

        switch (size)
        {
            case Size.Huge: _text.fontSize = 192; break;
            case Size.Big: _text.fontSize = 128; break;
            case Size.Medium: _text.fontSize = 96; break;
            case Size.Small: _text.fontSize = 64; break;
        }

        switch (effect)
        {
            case Effect.Tutorial:
                _flash.transform.position = new Vector3(0, 42);
                LeanTween.value(_flash, TutorialEffect, 1f, 0f, 5f)
                .setOnComplete(c => Destroy(_flash));
            break;
            case Effect.Slow:
                LeanTween.value(_flash, FadeInOut, 1f, 0f, 1.6f)
                .setOnComplete(c => Destroy(_flash));
            break;
            case Effect.Normal:
                LeanTween.value(_flash, FadeInOut, 1f, 0f, 0.8f).setEase(LeanTweenType.easeInExpo)
                .setOnComplete(c => Destroy(_flash));
            break;
            case Effect.Fast:
                LeanTween.value(_flash, FadeInOut, 1f, 0f, 0.4f)
                .setOnComplete(c => Destroy(_flash));
            break;
        }
    }

    private void FadeInOut(float alphaValue)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alphaValue);
        _flash.transform.position += new Vector3(0, alphaValue / 20);
    }

    private void TutorialEffect(float alphaValue)
    {
        _text.color = new Color(255, 227, 0, alphaValue);
        _flash.transform.position += new Vector3(0, alphaValue / 400);
    }

}


