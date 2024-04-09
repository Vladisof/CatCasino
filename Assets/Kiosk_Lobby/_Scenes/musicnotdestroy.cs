using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicnotdestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setmusic();
    }

    void setmusic()
    {
        if(FindObjectsOfType(GetType()).Length>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
