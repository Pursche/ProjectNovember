using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : IOnButtonClick
{
    public string sceneName;
    public LoadSceneMode loadMode = LoadSceneMode.Single;

    public override void OnClick()
    {
        SceneManager.LoadScene(sceneName, loadMode);
    }
}
