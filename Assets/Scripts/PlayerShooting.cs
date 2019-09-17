using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("按下s键 播放射击");
            AkSoundEngine.PostEvent("PlayerShoot", GameObject.Find("WwiseGlobal"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("按下H键 播放受伤");
            AkSoundEngine.PostEvent("Hurt", this.gameObject);
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(280, 250, 200, 50), "射击"))
        {
            AkSoundEngine.PostEvent("PlayerShoot", GameObject.Find("WwiseGlobal"));
        }
        if (GUI.Button(new Rect(280, 350, 200, 50), "受伤"))
        {
            AkSoundEngine.PostEvent("Hurt", this.gameObject);
            AkSoundEngine.PostEvent("PlayerShoot", GameObject.Find("WwiseGlobal"));
        }
    }
}
