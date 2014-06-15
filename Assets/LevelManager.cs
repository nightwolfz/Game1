using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AudioClip soundRageFx;

    void Start()
    {
        Load("GameScene");
    }

    public void Load(string name)
    {
        LoadingLogic(name);
    }

    public void LoadingLogic(string name)
    {
        // load transition scene
        //DontDestroyOnLoad(gameObject);
        //Application.LoadLevel("LoadingScene");

        LoadingAnimation();

        // load the target scene
        //Application.LoadLevel(name);
        //Destroy(gameObject);
    }


    private GameObject _ragefx;
    private GameObject _presents;

    public void LoadingAnimation()
    {

        _ragefx = GameObject.Find("_ragefx");
        _presents = GameObject.Find("_presents");
        _presents.GetComponent<TextMesh>().color = new Color(255, 255, 255, 0);

        AudioSource.PlayClipAtPoint(soundRageFx, Vector2.zero);

        /*LeanTween.value(_ragefx, RageFxEffect, 0f, 1f, 1f).setEase(LeanTweenType.easeInSine)
            .setOnComplete(c => LeanTween.value(_presents, PresentsEffect, 0f, 1f, 1f).setEase(LeanTweenType.easeInSine));*/

        Go.to(_ragefx.transform, 2f, new GoTweenConfig().shake(new Vector2(50f, 10f)))
            .setOnCompleteHandler(c => LeanTween.value(_presents, PresentsEffect, 0f, 1f, 1f).setEase(LeanTweenType.easeInSine)); ;


    }

    private void RageFxEffect(float alphaValue)
    {
        _ragefx.GetComponent<TextMesh>().color = new Color(255, 255, 255, alphaValue);
        //_ragefx.transform.position += new Vector3(0, alphaValue / 20);
    }
    private void PresentsEffect(float alphaValue)
    {
        _presents.GetComponent<TextMesh>().color = new Color(255, 255, 255, alphaValue);
        //_presents.transform.position += new Vector3(0, alphaValue / 20);
    }

}
