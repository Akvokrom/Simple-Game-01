using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimondBonusText : MonoBehaviour
{
    void Start()
    {
       Invoke("destroy", 1f);
    }

    void Update()
    {

    }

    void destroy()
    {
        Destroy(gameObject);
    }
 }
