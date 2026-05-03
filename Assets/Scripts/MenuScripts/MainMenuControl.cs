using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] AudioSource buttonPress;
    [SerializeField] GameObject fadeOut;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        buttonPress.Play();
        fadeOut.SetActive(true);
        SceneFlow.sceneNumber += 1;
        StartCoroutine(PlayTheGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator PlayTheGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneFlow.sceneNumber);
    }
}
