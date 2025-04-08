using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    //[SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button generateCodeButton;
    [SerializeField] private Button hostPlayButton;
    [SerializeField] private Button clientPlayButton;
    [SerializeField] private Button cancelHostPanelButton;
    [SerializeField] private Button cancelJoinPanelButton;
    [SerializeField] private Button playerColorChooseScene;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TextMeshProUGUI codeText;

    private async void Start()
    {
        
         await UnityServices.InitializeAsync();

        // await AuthenticationService.Instance.SignInAnonymouslyAsync();


        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player signed in: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException e)
            {
            Debug.LogError($"Authentication failed: {e.Message}");
            }
        }
        else
        {
            Debug.Log("Player is already signed in. Skipping authentication.");
        }


        hostPanel.SetActive(false);
        joinPanel.SetActive(false);
        SettingsPanel.SetActive(false);

        hostButton.onClick.AddListener(HostPanelActive);
        clientButton.onClick.AddListener(JoinPanelActive);
        exitButton.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(SettingsPanelActive);
    
        generateCodeButton.onClick.AddListener(CreateRelay);
        hostPlayButton.onClick.AddListener(EnterHostPlay);
        clientPlayButton.onClick.AddListener(() => JoinRelay(joinInput.text));
        cancelHostPanelButton.onClick.AddListener(HostPanelInactive);
        cancelJoinPanelButton.onClick.AddListener(JoinPanelInactive);

        playerColorChooseScene.onClick.AddListener(LoadPlayerColorChooseScene);
    }
        

    private async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(8);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        codeText.text = "Code: " + joinCode;

        var relayServerData = new RelayServerData(allocation,"dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        //NetworkManager.Singleton.StartHost();
    }

    private async void JoinRelay(string joinCode)
    {
        if (!AuthenticationService.Instance.IsSignedIn)
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    try
    {
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient(); // Client automatically joins the correct scene

        Debug.Log("Client successfully started!");
    }
    catch (RelayServiceException e)
    {
        Debug.LogError($"Failed to join relay: {e.Message}");
    }
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void EnterHostPlay()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started successfully. Loading GameScene...");
            
            // Step 4: Load the game scene using Netcode
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start host!");
        }

    }

    private void HostPanelInactive()
    {
        hostPanel.SetActive(false);
    }
    private void HostPanelActive()
    {
        hostPanel.SetActive(true);
    }

    private void JoinPanelActive()
    {
        joinPanel.SetActive(true);
    }
    private void JoinPanelInactive()
    {
        joinPanel.SetActive(false);
    }

    private void LoadPlayerColorChooseScene()
    {
        SceneManager.LoadScene("ChoosePlayerColor");
    }

    private void SettingsPanelActive()
    {
        SettingsPanel.SetActive(false);
    }
}
