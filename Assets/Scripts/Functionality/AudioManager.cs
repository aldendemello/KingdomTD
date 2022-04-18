using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] string _musicParameter = "musicVolume";
    [SerializeField] string _sfxParameter = "sfxVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] float _multiplier = 20f;
    // Start is called before the first frame update
    private void Awake() {
    }

    public void musicVolumeChanged() {
        _mixer.SetFloat(_musicParameter, Mathf.Log10(_musicSlider.value) * 20f);
    }

    public void sfxVolumeChanged() {
        _mixer.SetFloat(_sfxParameter, Mathf.Log10(_sfxSlider.value) * 20f);
    }

    private void OnDisable(){
        PlayerPrefs.SetFloat(_musicParameter, _musicSlider.value);
        PlayerPrefs.SetFloat(_sfxParameter, _sfxSlider.value);
    }

    void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(_musicParameter, _musicSlider.value);
        _sfxSlider.value = PlayerPrefs.GetFloat(_sfxParameter, _sfxSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
