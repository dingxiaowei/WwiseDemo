using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    GameObject mGlobalObject;
    AkAudioListener mListener;
    List<string> mLoadBankList = new List<string>();
    Dictionary<string, string> mBankInfoDict = new Dictionary<string, string>();

    GameObject mMusicEmitter;
    GameObject mVoiceEmitter;
    GameObject mSoundEmitter;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        mGlobalObject = GameObject.Find("WwiseGlobal");
        if (mGlobalObject == null)
        {
            var global = new GameObject("WwiseGlobal");
            GameObject.DontDestroyOnLoad(global);
            AkInitializer akInitializer = global.AddComponent<AkInitializer>();
            object settingObj = Resources.Load("AkWwiseInitializationSettings");
            if (settingObj != null)
            {
                AkWwiseInitializationSettings settings = settingObj as AkWwiseInitializationSettings;
                akInitializer.InitializationSettings = settings;
                global.SetActive(true);
            }

            mMusicEmitter = CreateEmitter("MusicEmitter");
            mSoundEmitter = CreateEmitter("SoundEmitter");
            mVoiceEmitter = CreateEmitter("VoiceEmitter");
        }
        else
        {
            mMusicEmitter = GameObject.Find("MusicEmitter");
            mSoundEmitter = GameObject.Find("SoundEmitter");
            mVoiceEmitter = GameObject.Find("VoiceEmitter");
            if (mMusicEmitter == null)
            {
                mMusicEmitter = CreateEmitter("MusicEmitter");
            }
            if (mSoundEmitter == null)
            {
                mSoundEmitter = CreateEmitter("SoundEmitter");
            }
            if (mVoiceEmitter == null)
            {
                mVoiceEmitter = CreateEmitter("VoiceEmitter");
            }
        }

        mListener = Camera.main.gameObject.GetComponent<AkAudioListener>();
        //var audios = Game.Data.sound_raw.GetDataList();
        //foreach (var audio in audios)
        //{
        //    mBankInfoDict.Add(audio.event_name, audio.bank_name);
        //}

        mBankInfoDict.Add("SFX_BG", "MySoundBank");
        mBankInfoDict.Add("SFX_StopBG", "MySoundBank");
        mBankInfoDict.Add("SFX_Shoot", "MySoundBank");
        mBankInfoDict.Add("VO_OYe", "MyVoiceBank");
        mBankInfoDict.Add("Player_FootSteps", "CharacterBank");
        mBankInfoDict.Add("Player_HeartBeat", "CharacterBank");

        AddBasePath();

        //SetGameObjectOutputBusVolume(mMusicEmitter, mListener, 0);//关闭背景音乐音量
        //SetGameObjectOutputBusVolume(mSoundEmitter, mListener, 0);//关闭音效音量
        //SetGameObjectOutputBusVolume(mVoiceEmitter, mListener, 0);//关闭语音音量
    }



    public void Start()
    {
        Application.lowMemory += OnLowMemory;
    }

    private GameObject CreateEmitter(string name)
    {
        if (mGlobalObject == null || string.IsNullOrEmpty(name))
            return null;
        var obj = new GameObject(name);
        obj.transform.parent = mGlobalObject.transform;
        return obj;
    }

    private void OnLowMemory()
    {

    }

    private void AddBasePath()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        string fileNameBase = Application.persistentDataPath + "/" + "Audio/GeneratedSoundBanks" + "/";
        //#if UNITY_IPHONE
        //        fileNameBase += "iOS";
        //#elif UNITY_ANDROID
        //        fileNameBase += "Android";
        //#endif
        var result = AkSoundEngine.AddBasePath(fileNameBase);
        BDebug.Log($"添加WwiseBasePath:{fileNameBase}，添加结果:{result}");
#endif
    }

    void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_EndOfEvent:
                if (in_cookie != null)
                {
                    AudioCtrl.EventCallback cb = (AudioCtrl.EventCallback)in_cookie;
                    cb();
                }
                break;
            case AkCallbackType.AK_Marker:
                AkMarkerCallbackInfo info = in_info as AkMarkerCallbackInfo;
                Debug.Log(info.strLabel);
                break;
            case AkCallbackType.AK_MusicSyncBeat: //互动音乐节拍点事件

                break;
            default:
                //AkSoundEngine.LogError("Callback Type not march.");
                break;
        }
    }

    bool CheckAndLoadBank(string eventName)
    {
        string bankName;
        //m_BankInfoDict是Event和SoundBank的映射关系字典 
        if (!mBankInfoDict.TryGetValue(eventName, out bankName))
        {
            Debug.LogError(string.Format("加载event（{0}）失败,没有找到所属的SoundBank", eventName));
            return false;
        }
        if (!mLoadBankList.Contains(bankName))
        {
            Debug.Log("加载音效包:" + bankName);
            LoadBank(bankName);
            mLoadBankList.Add(bankName);
        }
        return true;
    }

    public GameObject GetEmitterGameObject(EAudioType type)
    {
        if (type == EAudioType.Music)
            return mMusicEmitter;
        else if (type == EAudioType.Sound)
            return mSoundEmitter;
        else if (type == EAudioType.Voice)
            return mVoiceEmitter;
        else
            return null;
    }

    public void SetEmitterVolume(EAudioType type, float volume)
    {
        SetGameObjectOutputBusVolume(GetEmitterGameObject(type), mListener, volume);
    }

    public void SetGlobalObject(GameObject gameObj)
    {
        mGlobalObject = gameObj;
    }

    public void SetGameObjectOutputBusVolume(GameObject gameObj, AkAudioListener listener, float volume)
    {
        AkSoundEngine.SetGameObjectOutputBusVolume(gameObj, mListener.gameObject, Mathf.Clamp(volume, minVolume, maxVolume));
    }

    public AKRESULT SetGameObjectPosition(GameObject gameObj, Transform transform)
    {
        return AkSoundEngine.SetObjectPosition(gameObj, transform);
    }

    /// <summary>
    /// 设置Position并且要设置方向
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <param name="forward"></param>
    /// <param name="up"></param>
    /// <returns></returns>
    public AKRESULT SetGameObjectPosition(GameObject gameObject, Vector3 position, Vector3 forward, Vector3 up)
    {
        return AkSoundEngine.SetObjectPosition(gameObject, position, forward, up);
    }

    public AKRESULT SetGameObjectPosition(GameObject gmaeObject, float posX, float posY, float posZ, float frontX, float frontY, float frontZ, float topX, float topY, float topZ)
    {
        return AkSoundEngine.SetObjectPosition(gmaeObject, posX, posY, posZ, frontX, frontY, frontZ, topX, topY, topZ);
    }

    public AKRESULT SetCurrentLanguage(string language)
    {
        if (!string.IsNullOrEmpty(language))
        {
            AKRESULT result = AkSoundEngine.SetCurrentLanguage(language);
            return result;
        }
        return AKRESULT.AK_Fail;
    }

    public uint PlaySound(string eventName, GameObject gameObj = null)
    {
        if (!CheckAndLoadBank(eventName))
            return 0;
        if (gameObj == null)
            gameObj = mGlobalObject;
        uint playingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        if (!string.IsNullOrEmpty(eventName))
        {
            playingID = AkSoundEngine.PostEvent(eventName, gameObj);
        }
        return playingID;
    }

    public uint PlaySound(uint eventID, GameObject gameObj = null)
    {
        //if (!CheckAndLoadBank(eventName)) //TODO:可以考虑通过GetIDFromString接口来获取id，但Bank对应的就要设置为id
        //    return 0;
        if (gameObj == null)
            gameObj = mGlobalObject;
        uint playingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        if (eventID != 0)
        {
            playingID = AkSoundEngine.PostEvent(eventID, gameObj);
        }
        return playingID;
    }

    public uint PlaySound(string eventName, AudioCtrl.EventCallback cb, GameObject gameObj = null)
    {
        if (!CheckAndLoadBank(eventName))
            return 0;
        uint playingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        if (gameObj == null)
            gameObj = mGlobalObject;
        if (!string.IsNullOrEmpty(eventName))
            playingID = AkSoundEngine.PostEvent(eventName, gameObj, (uint)AkCallbackType.AK_EndOfEvent, Callback, cb);
        return playingID;
    }

    public AKRESULT StopSound(string eventName, GameObject gameObj = null, int transitionDuration = 300, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
    {
        if (!CheckAndLoadBank(eventName))
            return AKRESULT.AK_Fail;
        if (!string.IsNullOrEmpty(eventName))
        {
            transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
            if (gameObj == null)
                gameObj = mGlobalObject;
            if (gameObj != null)
            {
                AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, gameObj, transitionDuration, curveInterpolation);
                return result;
            }
        }
        return AKRESULT.AK_Fail;
    }

    public AKRESULT StopSound(uint eventID, GameObject gameObj = null, int transitionDuration = 300,
         AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
    {
        transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
        if (gameObj == null)
            gameObj = mGlobalObject;
        AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObj, transitionDuration, curveInterpolation);
        return result;
    }

    public uint StopSound()
    {
        uint playingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        playingID = AkSoundEngine.PostEvent("Stop_All_Sound", mGlobalObject);
        return playingID;
    }

    public uint PlayVoice(string eventName, GameObject gameObj = null)
    {
        if (!CheckAndLoadBank(eventName))
            return 0;
        if (gameObj == null)
            gameObj = mGlobalObject;
        uint playingID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        if (!string.IsNullOrEmpty(eventName))
        {
            playingID = AkSoundEngine.PostEvent(eventName, gameObj);
        }
        return playingID;
    }

    public AKRESULT StopVoice(string eventName, GameObject gameObj = null, int transitionDuration = 200)
    {
        if (!string.IsNullOrEmpty(eventName))
        {
            transitionDuration = Mathf.Clamp(transitionDuration, 0, 10000);
            if (gameObj == null)
                gameObj = mGlobalObject;
            AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, gameObj, transitionDuration, AkCurveInterpolation.AkCurveInterpolation_Linear);
            return result;
        }
        return AKRESULT.AK_Fail;
    }

    public uint StopVoice(GameObject gameObj = null)
    {
        if (gameObj = null)
            gameObj = mGlobalObject;
        uint playintID = AkSoundEngine.AK_INVALID_PLAYING_ID;
        playintID = AkSoundEngine.PostEvent("Stop_Voice_Dialogue", gameObj);
        return playintID;
    }

    public AKRESULT SetRTPC(string name, float value)
    {
        AKRESULT result = AkSoundEngine.SetRTPCValue(name, value);
        return result;
    }

    public AKRESULT SetSwitch(string switchGroup, string switchState, GameObject gameObj = null)
    {
        if (string.IsNullOrEmpty(switchGroup) || string.IsNullOrEmpty(switchState))
        {
            Debug.LogError("没有传Switch参数");
            return AKRESULT.AK_Fail;
        }
        if (gameObj == null)
            gameObj = mGlobalObject;
        AKRESULT result = AkSoundEngine.SetSwitch(switchGroup, switchState, gameObj);
        return result;
    }

    public AKRESULT SetSwitch(uint switchGroup, uint switchState, GameObject gameObj = null)
    {
        if (gameObj == null)
            gameObj = mGlobalObject;
        AKRESULT result = AkSoundEngine.SetSwitch(switchGroup, switchState, gameObj);
        return result;
    }

    public AKRESULT SetState(string stateGroup, string state)
    {
        AKRESULT result = AkSoundEngine.SetState(stateGroup, state);
        return result;
    }

    public AKRESULT SetState(int stateGroupID, int stateValueID)
    {
        AKRESULT result = AkSoundEngine.SetState((uint)stateGroupID, (uint)stateValueID);
        return result;
    }

    public void LoadBank(string bankName)
    {
        if (!string.IsNullOrEmpty(bankName))
        {
            AkBankManager.LoadBank(bankName, false, false);
        }
    }

    public void UnLoadBank(string bankName)
    {
        if (!string.IsNullOrEmpty(bankName))
        {
            AkBankManager.UnloadBank(bankName);
        }
    }

    public void AddDefaultAudioListener(GameObject gameObj)
    {
        if (gameObj != null)
        {
            var listener = gameObj.AddComponent<AkAudioListener>();
            listener.isDefaultListener = true;
        }
    }

    #region VolumeControl
    private const float minVolume = 0.0f;
    private const float maxVolume = 100.0f;
    public static string SoundOn = "Sound_On";
    public static string SoundOff = "Sound_Off";
    public static string VoiceOn = "Voice_On";
    public static string VoiceOff = "Voice_Off";
    public static string MusicOn = "Music_On";
    public static string MusicOff = "Music_Off";

    public struct VolumeParam
    {
        public static string SoundVolume = "Sound_Volume";
        public static string VoiceVolume = "Voice_Volume";
        public static string MusicVolume = "Music_Volume";
    }

    public void SetSoundVolume(float volume)
    {
        volume = Mathf.Clamp(volume, minVolume, maxVolume);
        AkSoundEngine.SetRTPCValue(VolumeParam.SoundVolume, volume);
    }
    public void SetVoiceVolume(float volume)
    {
        volume = Mathf.Clamp(volume, minVolume, maxVolume);
        AkSoundEngine.SetRTPCValue(VolumeParam.VoiceVolume, volume);
    }
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, minVolume, maxVolume);
        AkSoundEngine.SetRTPCValue(VolumeParam.MusicVolume, volume);
    }
    #endregion

    #region AudioContorl
    private static string VoicePlayStart = "Voice_Play_Start";
    private static string VoicePlayEnd = "Voice_Play_End";
    private static string VoiceRecordStart = "Voice_Record_Start";
    private static string VoiceRecordEnd = "Voice_Record_End";

    public void StartVoicePlay()
    {
        AkSoundEngine.PostEvent(VoicePlayStart, mGlobalObject);
    }

    public void EndVoicePlayEnd()
    {
        AkSoundEngine.PostEvent(VoicePlayEnd, mGlobalObject);
    }
    public void StartVoiceRecord()
    {
        AkSoundEngine.PostEvent(VoiceRecordStart, mGlobalObject);
    }
    public void EndVoiceRecord()
    {
        AkSoundEngine.PostEvent(VoiceRecordEnd, mGlobalObject);
    }
    public void CustomMusicStart()
    {
        AkSoundEngine.MuteBackgroundMusic(true); //背景音乐总线的静音和/取消
    }
    public void CustomMusicEnd()
    {
        AkSoundEngine.MuteBackgroundMusic(false);
    }
    #endregion

    #region Pause&Resume
    public void AudioPause()
    {
        AkSoundEngine.PostEvent("Stop_Sound", mGlobalObject);
        AkSoundEngine.Suspend();
    }

    public void AudioResume()
    {
        AkSoundEngine.WakeupFromSuspend();
    }
    #endregion

    public void PostEvents(GameObject obj, AK.Wwise.Event[] events)
    {
        if (AkSoundEngine.IsInitialized() && events != null)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].Post(obj);
            }
        }
    }


    public void CloseEngine()
    {
        AkSoundEngine.Term();
    }
}
