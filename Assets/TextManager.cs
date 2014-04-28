using UnityEngine;

namespace Assets
{
    static public class TextManager
    {
        //static TextManager()
        //{
        //    _textEffects = GameObject.Find("Level").GetComponent<TextEffects>();
        //}

        static public void Show(
            string text, 
            TextEffects.Size size = TextEffects.Size.Medium, 
            TextEffects.Effect effect = TextEffects.Effect.Normal,
            Vector2 position = new Vector2())
        {
            var o = new GameObject("Flashing_Text");
            o.transform.position = position;
            o.AddComponent<TextEffects>().Add(text, size, effect);
            Object.Destroy(o, 5);
        }

        static public void Show(string text, Vector2 position = new Vector2(), Color? color = null)
        {
            var o = new GameObject("Flashing_Text");
            o.transform.position = position;
            o.AddComponent<TextEffects>().Add(text, TextEffects.Size.Medium, TextEffects.Effect.Fast, position, color);
            Object.Destroy(o, 3);
        }

    }
}
