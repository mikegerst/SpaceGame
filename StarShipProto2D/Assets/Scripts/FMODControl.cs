using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Managers;

public class FMODControl : MonoBehaviour
{
    [FMODUnity.EventRef] string music = "event:/AmbientSpace";
    FMOD.Studio.EventInstance musicEvent;

    float speedParam = 0f;
    // Start is called before the first frame update
    void Start()
    {
        var studioSystem = FMODUnity.RuntimeManager.StudioSystem;
        var coreSystem = FMODUnity.RuntimeManager.CoreSystem;
        PlayStateManager.SceneChangingToMusic += KillFmodMusicEvent;
        Starship.traversing += StartMusic;
        Starship.stayingStill += StopMusic;
        Starship.playerDied += StopSpaceMusic;
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(music);
        musicEvent.start();
    }

    private void KillFmodMusicEvent()
    {
        musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }


    private void StartMusic()
    {
        StartCoroutine(FadeInMusic());
    }

    private void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    private void StopSpaceMusic()
    {
        musicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    private IEnumerator FadeInMusic()
    {
        while(speedParam < 20f)
        {
            speedParam += .1f;

            musicEvent.setParameterByName("MoveTime", speedParam);

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator FadeOutMusic()
    {
        while (speedParam > 0)
        {
            speedParam -= 1f;

            musicEvent.setParameterByName("MoveTime", speedParam);

            yield return new WaitForSeconds(.1f);
        }
    }
}
