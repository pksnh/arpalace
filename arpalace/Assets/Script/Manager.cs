using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
