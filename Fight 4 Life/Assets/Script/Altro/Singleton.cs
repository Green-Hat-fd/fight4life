using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton istanza { get; private set; }

    private void Awake()
    {
        //Se c'è un clone di questo script (nella scena), distruggimi
        if (istanza != null && istanza != this)
        {
            Destroy(gameObject);
        }
        else
        {
            istanza = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
