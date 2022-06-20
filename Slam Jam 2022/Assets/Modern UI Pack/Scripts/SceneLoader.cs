//Joel Lynch
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Scene Manager that Controls
/// Scene Loading, Unloading
/// and Transitions
/// </summary>

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Scene Transition Time")]
    public float transitionTime = 1;
    void Update()
    {
       
    }

    public void LoadLevel(string sceneName, Animator transition) //Load Level Function Parsing sceneName and Transition of Choice
    {
        StartCoroutine(LevelLoader(sceneName, transition));
    }

    IEnumerator LevelLoader(string sceneName, Animator transition)
    {
        transition.SetTrigger("Start"); //Starts Animation

        yield return new WaitForSeconds(transitionTime); //Waits Before Continuing for the Transition Time

        SceneManager.LoadSceneAsync(sceneName); //Loads the Scene

    }
}

