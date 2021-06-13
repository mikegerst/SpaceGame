using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodTestForInstrument : MonoBehaviour
{
    [FMODUnity.EventRef] private static FMOD.Studio.EventInstance fmodEvent;
    [FMODUnity.EventRef] private static FMOD.Studio.EventInstance melodyEvent;


    // Start is called before the first frame update
    void Start()
    {
        fmodEvent = FMODUnity.RuntimeManager.CreateInstance("event:/PlayingAlongChords");

        melodyEvent = FMODUnity.RuntimeManager.CreateInstance("event:/PlayingAlongMelody");

        melodyEvent.start();

        fmodEvent.start();
    }

    public static void ChangeChord(float parameter)
    {
        fmodEvent.setParameterByName("Chord", parameter);
    }


    public static void ChangeNote(float note)
    {
        melodyEvent.setParameterByName("Note", note);
    }

    
}
