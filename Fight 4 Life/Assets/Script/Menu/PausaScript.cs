using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PausaScript : MonoBehaviour
{
    bool inPausa;

    [SerializeField]
    PlayableDirector tlEntrata, tlUscita;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inPausa = !inPausa;

            CheckPausa();
        }
    }

    void CheckPausa()
    {
        if (inPausa)
        {
            tlEntrata.Play();
        }
        else
        {
            tlUscita.Play();
        }
    }

    //Variabile Set personalizzate
    public void ScriviPausa(bool p)
    {
        inPausa = p;

        CheckPausa();
    }
}
