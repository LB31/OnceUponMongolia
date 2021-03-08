using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static BackgroundMusic;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudiosSourceBackground;
    public List<BackgroundMusic> AllBackgroundClips;


    public async void AddAndPlaySound(GameObject source, AudioClip clip, bool spatial, float volume, 
        Vector2 minMaxDistance, int timeBetweenReplay, int timeBeforeStart = 0)
    {
        await Task.Delay(timeBeforeStart * 1000);

        AudioSource audioSource = source.AddComponent<AudioSource>();

        var controller = source.AddComponent<AudioController>();
        controller.AudioSource = audioSource;
        controller.WaitTillPlay = timeBetweenReplay;

        audioSource.clip = clip;
        audioSource.spatialBlend = spatial ? 1 : 0;
        audioSource.volume = volume >= 1 ? 1 : volume <= 0 ? 0 : volume;
        //audioSource.loop = true;
        audioSource.minDistance = minMaxDistance.x;
        audioSource.maxDistance = minMaxDistance.y;

        audioSource.Play();
    }

    public void ChangeMusic(Music musicType)
    {
        AudiosSourceBackground.Stop();
        AudiosSourceBackground.clip = AllBackgroundClips.First(music => music.MusicType == musicType).MusicClip;
        AudiosSourceBackground.Play();
    }


}

[Serializable]
public class BackgroundMusic
{
    public Music MusicType;
    public AudioClip MusicClip;

    public enum Music
    {
        Village,
        Ruins,
        Mountain,
        Party
    }

}

public class AudioController : MonoBehaviour
{
    public AudioSource AudioSource;
    public int WaitTillPlay;
    private float t;

    private void Update()
    {
        if (!AudioSource.isPlaying)
        {
            t += Time.deltaTime;
            if(t >= WaitTillPlay)
            {
                t = 0;
                AudioSource.Play();
            }
        }
    }
}
