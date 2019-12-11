using System.Collections;
using System.Collections.Generic;
using Script.Global;
using UnityEngine;

public class UiSystem : MonoBehaviour
{
    public void StartWave()
    {
        Spawner.Instance.Spawn("spider", 10, 2f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
