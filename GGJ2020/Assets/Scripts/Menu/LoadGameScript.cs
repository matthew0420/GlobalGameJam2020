using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScript : MonoBehaviour
{
    public SpriteRenderer thisRenderer;
    public AudioClip MenuSelectSound;
    public string sceneToLoad;

    public void Start()
    {
        SpriteRenderer thisRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        // this object was clicked - do something
        AudioSource.PlayClipAtPoint(MenuSelectSound, this.transform.position, 1.0f);
        thisRenderer.color = new Color(101f, 15f, 112f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
