using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MainMenuButtonScript : NetworkBehaviour
{
    public void ReturnToMainMenu()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            // If the host leaves, shut down the server so all clients disconnect
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenu");
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            // If a client leaves, just disconnect them
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // If offline, just load the main menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
