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

            if (inPausa)
            {
                // Mettere suoni e altra roba

                tlEntrata.Play();
            }
            else
            {
                // Mettere suoni e altra roba

                tlUscita.Play();
            }
        }
    }
}
