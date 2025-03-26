using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip inGameMusic, outGameMusic; 
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene change event
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip newClip = scene.name == "GameScene" ? inGameMusic : outGameMusic;

        if (audioSource.clip != newClip) // Only change if it's different
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
