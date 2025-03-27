using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip inGameMusic, outGameMusic, buttonClickSound; 
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

        AssignButtonClickSounds();
    }

    void AssignButtonClickSounds()
    {
        Button[] buttons = FindObjectsOfType<Button>(); // Find all buttons in the scene

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(PlayButtonClickSound);
        }
    }

    void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
