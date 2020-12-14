using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {
    public AudioMixer BGM, Action;
    public Slider BGMslider, Actionslider;
    void Start () {
        init ();
    }

    void Update () {
        asyncMixer (BGMslider, BGM);
        asyncMixer (Actionslider, Action);
    }

    void init () {
        initMixer (BGMslider, BGM);
        initMixer (Actionslider, Action);
    }

    void initMixer (Slider slider, AudioMixer audioMixer) {
        float valueDB = 0;
        audioMixer.GetFloat ("master", out valueDB);
        slider.value = Mathf.Pow (10, valueDB / 20f);
    }
    void asyncMixer (Slider slider, AudioMixer audioMixer) {
        audioMixer.SetFloat ("master", Mathf.Log10 (slider.value) * 20f);
    }

}