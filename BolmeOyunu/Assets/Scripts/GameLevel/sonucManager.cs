using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sonucManager : MonoBehaviour
{
    public void OyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel");

    }
    public void AnaMenuDon()
    {
        SceneManager.LoadScene("menuLevel");
    }
}
