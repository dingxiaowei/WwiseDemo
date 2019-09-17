using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl
{
    public delegate void EventCallback();

    public static void SoundCtrl(bool bSwitch)
    {
        AudioManager.Instance.PlaySound(bSwitch ? AudioManager.SoundOn : AudioManager.SoundOff);
    }
    public static void VoiceCtrl(bool bSwitch)
    {
        AudioManager.Instance.PlaySound(bSwitch ? AudioManager.VoiceOn : AudioManager.VoiceOff);
    }
    public static void MusicCtrl(bool bSwitch)
    {
        AudioManager.Instance.PlaySound(bSwitch ? AudioManager.MusicOn : AudioManager.MusicOff);
    }

    public static void SetSoundVolume(float volume)
    {
        AudioManager.Instance.SetSoundVolume(volume);
    }

    public static void SetVoiceVolume(float volume)
    {
        AudioManager.Instance.SetVoiceVolume(volume);
    }

    public static void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public static uint PlaySound(string eventName, GameObject gameObj)
    {
        return AudioManager.Instance.PlaySound(eventName, gameObj);
    }

    public static uint PlaySound(string eventName)
    {
        return AudioManager.Instance.PlaySound(eventName);
    }

    public static uint PlaySound(string eventName, EventCallback cb)
    {
        return AudioManager.Instance.PlaySound(eventName, cb);
    }

    public static AKRESULT StopSound(string eventName, GameObject gameObj)
    {
        return AudioManager.Instance.StopSound(eventName, gameObj);
    }

    public static AKRESULT StopSound(string eventName)
    {
        return AudioManager.Instance.StopSound(eventName);
    }

    public static void SetState(string stateGroup, string state)
    {
        AkSoundEngine.SetState(stateGroup, state);
    }

    public static void SetSwitch(string switchGroup, string switchValue, GameObject gameObj)
    {
        AkSoundEngine.SetSwitch(switchGroup, switchValue, gameObj);
    }

    public static void SetRTPCValue(string RTPC, float parameter, GameObject gameObj)
    {
        AkSoundEngine.SetRTPCValue(RTPC, parameter, gameObj);
    }

    public static void SetRTPCValue(string RTPC, float parameter)
    {
        AkSoundEngine.SetRTPCValue(RTPC, parameter);
    }

    public static void SetDefaultAudioListener(GameObject gameObj)
    {
        AudioManager.Instance.AddDefaultAudioListener(gameObj);
    }
}
