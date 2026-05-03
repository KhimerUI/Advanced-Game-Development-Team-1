using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditFlow : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    public int secondsWaiting;

    void Start()
    {
        StartCoroutine(BeginFade());
        SceneFlow.sceneNumber += 1;
        StartCoroutine(ContinueFlow());
    }

    IEnumerator BeginFade()
    {
        yield return new WaitForSeconds(secondsWaiting);
        fadeOut.SetActive(true);
    }

    IEnumerator ContinueFlow()
    {
        yield return new WaitForSeconds(secondsWaiting + 2);
        SceneManager.LoadScene(SceneFlow.sceneNumber);
    }
}
