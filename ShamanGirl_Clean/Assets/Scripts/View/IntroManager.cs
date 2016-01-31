using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.View;

public class IntroManager : MonoBehaviour {

    public GameObject animatedIntro;
	// Use this for initialization
	void Start () {
        if (animatedIntro.GetComponent<Animator>() != null)
        {
            Invoke("LoadScene", animatedIntro.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
    public void LoadScene()
    {
        SceneManager.LoadScene(MenuManager.SCENE_PLAY);
    }
}
