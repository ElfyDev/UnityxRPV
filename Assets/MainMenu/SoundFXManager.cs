using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundfx;

    public float mastervolume = 0.9f;
    public static SoundFXManager instance;
     private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
        public void playSFXClip(AudioClip audioclip, Transform spawntransform)
    {
        // spawn object
        AudioSource audioSource = Instantiate(soundfx, spawntransform.position, Quaternion.identity);
        // assign clip
        audioSource.clip = audioclip;
        // assign volume
        audioSource.volume = mastervolume;
        // play clip
        audioSource.Play();
        
        float cliplength = audioSource.clip.length;

        Destroy(audioSource.gameObject, cliplength);
    }

  

}


