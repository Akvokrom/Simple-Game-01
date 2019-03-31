using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDimonds : MonoBehaviour
{
    void Start()
    {
        Invoke("nocollisions", 0.1f);
    }

    void Update()
    {
        
    }
    void nocollisions()
    {
        int miniDimonds = gameObject.transform.childCount;
        for (int i = 0; i < miniDimonds; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
        Invoke("destroy", 1f);
    }
    void destroy()
    {
        Destroy(gameObject);
    }
}
