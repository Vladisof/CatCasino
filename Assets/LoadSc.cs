using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSc : MonoBehaviour
{
    void Start()
    {
        Invoke("Zzz", 3);
    }

    public void Zzz()
    {
        SceneManager.LoadScene(1);
    }
}
