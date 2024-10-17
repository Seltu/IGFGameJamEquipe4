using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks; // Array to hold different music tracks
    
    private int currentTrackIndex = -1; // Index to keep track of the current song
    private bool isMusicChanging = false; // Flag to track if music is changing

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Call this function to change music based on the game situation
    public void PlayMusic(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicTracks.Length && trackIndex != currentTrackIndex && !isMusicChanging)
        {
            StartCoroutine(ChangeMusicTrack(trackIndex));
        }
    }

    // Coroutine to change the music smoothly
    private IEnumerator ChangeMusicTrack(int trackIndex)
    {
        isMusicChanging = true;

        // If music is playing, fade it out
        if (audioSource.isPlaying)
        {
            for (float vol = audioSource.volume; vol > 0; vol -= Time.deltaTime)
            {
                audioSource.volume = vol;
                yield return null;
            }
            audioSource.Stop();
        }

        // Assign the new track and fade it in
        currentTrackIndex = trackIndex;
        audioSource.clip = musicTracks[trackIndex];
        audioSource.Play();

        for (float vol = 0; vol < 0.5; vol += Time.deltaTime)
        {
            audioSource.volume = vol;
            yield return null;
        }

        isMusicChanging = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Situation 1 (1 on alphabetical keyboard)
        {
            PlayMusic(0); // Plays the first track
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Situation 2 (2 on alphabetical keyboard)
        {
            PlayMusic(1); // Plays the second track
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Situation 3 (3 on alphabetical keyboard)
        {
            PlayMusic(2); // Plays the third track
        }
    }
}
