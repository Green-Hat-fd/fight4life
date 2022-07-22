using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton istanza { get; private set; }

    private void Awake()
    {
        //Se c'� un clone di questo script (nella scena), distruggimi
        if (istanza != null && istanza == this)
        {
            Destroy(this);
        }
        else
        {
            istanza = this;
            DontDestroyOnLoad(this);
        }
    }
}