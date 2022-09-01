using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Managers;

public class FMODControl : MonoBehaviour
{
    [FMODUnity.EventRef] string music = "event:/Music";
    FMOD.Studio.EventInstance musicEvent;

    int numberOfEnemiesOnScreen = 0;
    float speedParam = 0f;
    // Start is called before the first frame update
    void Start()
    {
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
            speedParam += 1f;

            musicEvent.setParameterByName("MoveTime", speedParam);

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator FadeOutMusic()
    {
        while (speedParam > 0)
        {
            speedParam -= 5f;

            musicEvent.setParameterByName("MoveTime", speedParam);

            yield return new WaitForSeconds(.1f);
        }
    }

    private void InitiateFightMusic()
    {
        numberOfEnemiesOnScreen++;
        float f;
        musicEvent.getParameterByName("Fight", out f);
        if (f < .1f)
            musicEvent.setParameterByName("Fight", .1f);
    }

    private void CheckIfEnemiesGone()
    {
        numberOfEnemiesOnScreen--;
        print($"Enemies on Screen: {numberOfEnemiesOnScreen}");
        if (numberOfEnemiesOnScreen == 0)
            musicEvent.setParameterByName("Fight", 0f);
    }

    private void OnEnable()
    {
        Enemy.enemyInView += InitiateFightMusic;
        Enemy.enemyDied += CheckIfEnemiesGone;
        PlayStateManager.SceneChangingToMusic += KillFmodMusicEvent;
        Starship.traversing += StartMusic;
        Starship.stayingStill += StopMusic;
        Starship.playerDied += StopSpaceMusic;
    }

    private void OnDisable()
    {
        Enemy.enemyInView -= InitiateFightMusic;
        Enemy.enemyDied -= CheckIfEnemiesGone;
        PlayStateManager.SceneChangingToMusic -= KillFmodMusicEvent;
        Starship.traversing -= StartMusic;
        Starship.stayingStill -= StopMusic;
        Starship.playerDied -= StopSpaceMusic;
    }
}
