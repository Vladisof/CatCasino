using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loaad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("zzz", 3);
    }

    public void zzz()
    {
        SceneManager.LoadScene(1);
    }
}
