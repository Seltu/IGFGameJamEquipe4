using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks; // Array to hold different music tracks

    private int currentTrackIndex = -1; // Index to keep track of the current song
    private bool isMusicChanging = false; // Flag to track if music is changing

    private GameObject gameOverPanel; // Reference to GameOverPanel_Prefab
    private bool isGameOverPanelActive = false;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        PlayMusic(0);

        // Delay the search for GameOverPanel_Prefab
        StartCoroutine(CheckForGameOverPanel());
    }

    private System.Collections.IEnumerator CheckForGameOverPanel()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before checking (to reduce startup lag)

        // Find the "GameOverPanel_Prefab" under the "Canvas"
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            gameOverPanel = canvas.transform.Find("GameOverPanel_Prefab")?.gameObject;
            if (gameOverPanel == null)
            {
                Debug.LogWarning("GameOverPanel_Prefab not found under Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene.");
        }
    }

    private void Update()
    {
        // Check if the gameOverPanel is active (only if the reference is available)
        if (gameOverPanel != null && gameOverPanel.activeSelf && !isGameOverPanelActive)
        {
            isGameOverPanelActive = true;
            PlayMusic(2);
        }
        else if (!gameOverPanel?.activeSelf == true && isGameOverPanelActive)
        {
            isGameOverPanelActive = false;
        }

        // Example input controls for testing
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // Situation 1 (1 on alphabetical keyboard)
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
