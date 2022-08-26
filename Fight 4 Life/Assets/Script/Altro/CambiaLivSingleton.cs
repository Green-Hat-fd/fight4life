using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiaLivSingleton : MonoBehaviour
{
    public static CambiaLivSingleton istanza_CambiaLiv { get; private set; }

    private void Awake()
    {
        //Se c'è un clone di questo script (nella scena), distruggimi
        if (istanza_CambiaLiv != null && istanza_CambiaLiv != this)
        {
            Destroy(gameObject);
        }
        else
        {
            istanza_CambiaLiv = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
