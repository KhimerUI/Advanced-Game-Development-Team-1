using UnityEngine;

public class SceneFlow : MonoBehaviour
{
    public static int sceneNumber;
    [SerializeField] int internalNumber;

    void Start()
    {
        internalNumber = sceneNumber;
    }

}
