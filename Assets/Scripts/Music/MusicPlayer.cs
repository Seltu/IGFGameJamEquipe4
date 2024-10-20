using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public AudioClip[] musicTracks; // Array to hold different music tracks

    private int currentTrackIndex = -1; // Index to keep track of the current song
    private bool isMusicChanging = false; // Flag to track if music is changing

    private GameObject gameOverPanel; // Reference to GameOverPanel_Prefab
    private bool isGameOverPanelActive = false;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        foreach (AudioClip clip in musicTracks)
        {
            clip.LoadAudioData();
        }

        PlayMusic(0);
    }

    private void Update()
    {
        // Example input controls for testing
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Situation 1 (1 on alphabetical keyboard)
        {
            PlayMusic(1); // Plays the first track
        }
    }

    // Call this function to change music based on the game situation
    public void PlayMusic(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicTracks.Length && trackIndex != currentTrackIndex && !isMusicChanging)
        {
            ChangeMusicTrack(trackIndex);
        }
    }

    // Simplified track change without coroutine to reduce memory footprint
    private void ChangeMusicTrack(int trackIndex)
    {
        isMusicChanging = true;

        // Stop the current track if playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Change to the new track and start playing
        currentTrackIndex = trackIndex;
        audioSource.clip = musicTracks[trackIndex];
        audioSource.Play();

        isMusicChanging = false;
    }
}
