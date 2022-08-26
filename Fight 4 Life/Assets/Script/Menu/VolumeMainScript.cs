using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMainScript : MonoBehaviour
{
    public enum TVolume
    {
        [InspectorName("-----")] ScegliereIlTipo,
        Musica,
        SoundEffect
    };

    [SerializeField]
    TVolume tipoDiVolume;

    float volIniz;


    private void Awake()
    {
        //Prende il volume messo nell'editor
        volIniz = GetComponent<AudioSource>().volume;
    }

    private void Update()
    {
        //Aggiusta il volume in base al volume scelto nelle opzioni
        switch (tipoDiVolume)
        {
            case TVolume.Musica:
                GetComponent<AudioSource>().volume = volIniz * OpzioniMainScript.volumeMusica;
                break;

            case TVolume.SoundEffect:
                GetComponent<AudioSource>().volume = volIniz * OpzioniMainScript.volumeSuoni;
                break;
        }
    }
}
