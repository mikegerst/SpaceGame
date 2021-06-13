using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Runtime.InteropServices;
using Managers;

public class FModManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    FMOD.Studio.EventInstance MusicLight;
    FMOD.Studio.EVENT_CALLBACK beatCallBack;

    GCHandle timelineHandle;

    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBar = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    TimelineInfo timelineInfo;

    [FMODUnity.EventRef] string musicLights = "event:/MusicLights";

    public FMOD.Studio.System system;

    public static int beat;
    int lastBeat = 0;

    public delegate void BeatChanged();
    public static event BeatChanged BeatChange;

    FMODUnity.StudioListener listener;
    void Start()
    {

        GameManager.OnGameStart += StartMusic;
        PlayStateManager.SceneChangingToTraverse += KillMusicEvent;
        timelineInfo = new TimelineInfo();
        beatCallBack = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        FMOD.Studio.System.create(out system);
        MusicLight = FMODUnity.RuntimeManager.CreateInstance(musicLights);

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        MusicLight.setUserData(GCHandle.ToIntPtr(timelineHandle));

        MusicLight.setCallback(beatCallBack, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER | FMOD.Studio.EVENT_CALLBACK_TYPE.SOUND_PLAYED);
        
        MusicLight.start();


    }

    // Update is called once per frame
    void Update()
    {
        /*if (beat == 1)
        {
            
            if(BeatChange!=null)
                BeatChange();

            lastBeat = beat;
        }*/

    }


    public void KillMusicEvent()
    {
        MusicLight.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartMusic()
    {
        MusicLight.setParameterByName("Start", 1f);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    {
        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        
                        timelineInfo.currentMusicBar = parameter.bar;
                        beat = parameter.beat;

                        Debug.Log($"Position: {parameter.position} Bar: {parameter.bar} Beat: {beat}");
                        if (beat == 1)
                        {
                            BeatChange();
                        }
                        
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                        Debug.Log($"Marker: {parameter.name}");
                        
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.SOUND_PLAYED:
                    {
                        var parameter = (FMOD.Studio.SOUND_INFO)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.SOUND_INFO));
                      
                        Debug.Log($"Sound Name: {parameter.name} {parameter.exinfo} {parameter.mode} {parameter.name_or_data} {parameter.subsoundindex}");
                    };
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
}
