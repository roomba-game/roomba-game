using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    static public SFX S;
    private AudioSource source;
    private AudioSource bgm;
    public GameObject mainCamera;
    public AudioClip[] sounds;
    public AudioClip[] music;
    

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        source = this.GetComponent<AudioSource>();
        bgm = mainCamera.GetComponent<AudioSource>();

        setVolume();
    }

    // Plays sound effects
    public void Play(int idx) {
        if (sounds == null || sounds.Length <= idx) return;
        source.clip = sounds[idx];
        source.Play();
    }

    public void setVolume()
    {
        //Set volume for the sfx and the background music
        source.volume = GameData.GD.getVolume("SFX");
        bgm.volume = GameData.GD.getVolume("MUSIC");
    }
}
