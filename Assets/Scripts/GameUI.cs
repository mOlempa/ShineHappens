using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    GameObject timingBar;

    private void Start()
    {
        timingBar.SetActive(false);
    }

    public void ShowTimingBar()
    {
        timingBar.SetActive(true);
    }
    public void HideTimingBar()
    {
        timingBar.SetActive(false);
    }

}
