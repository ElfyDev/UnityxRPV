using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTransition : MonoBehaviour
{

    [Header("Assign in Inspector")]
    public Button startButton;          // The UI button to start the transition
    public VideoPlayer videoPlayer;     // The Video Player object
    public string nextSceneName;        // The name of the next scene to load
    public RawImage rawImage;
    private void Awake()
    {
        rawImage.enabled = false;
    }
    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(PlayTransitionVideo);
            
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.gameObject.SetActive(false); // Hide it at start
        }
    }

    private void PlayTransitionVideo()
    {
        if (videoPlayer == null)
        {
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            return;
        }

        rawImage.enabled = true;
        // Show and play the video
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        startButton.interactable = false;

        // Debug info to make sure it’s really playing
        InvokeRepeating(nameof(DebugVideoTime), 1f, 1f);
    }
    private void DebugVideoTime()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
