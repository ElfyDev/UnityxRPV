using DigitalRuby.RainMaker;
using UnityEngine;
using UnityEngine.Video;

public class VideoDisappear : MonoBehaviour
{
    public VideoPlayer videoPlayer; // assign your Video Player
    public GameObject videoObject;  // assign the RawImage or Canvas that shows the video
    public AudioSource audiosoource;
    public GameObject buttons;
    private void Awake()
    {
        buttons.SetActive(false);
    }
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        audiosoource.Play();
        videoObject.SetActive(false);
        buttons.SetActive(true);
    }
}
