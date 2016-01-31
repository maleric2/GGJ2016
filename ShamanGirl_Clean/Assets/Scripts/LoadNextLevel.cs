using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadNextLevel : MonoBehaviour {

    private Collider triggerCollider;

	// Use this for initialization
	void Start () {
        triggerCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.maxLevelScore == GameManager.instance.score)
            triggerCollider.enabled = true;
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("New level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
