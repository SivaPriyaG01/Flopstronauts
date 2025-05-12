// // using System.Collections.Generic;
// // using System.Threading.Tasks;
// // using Newtonsoft.Json;
// // using Unity.Services.Authentication;
// // using Unity.Services.Core;
// // using Unity.Services.Leaderboards;
// // using UnityEngine;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using Unity.Services.Authentication;
// using Unity.Services.CloudSave;
// using Unity.Services.Leaderboards;
// using Unity.Services.Core;
// using Newtonsoft.Json;
// using Unity.Services.Leaderboards.Models;
// using TMPro;


// public class LeaderboardScript : MonoBehaviour
// {
//     const string LeaderboardId = "FlopstronautsLB";

//     private Dictionary<string, (string username, int score)> leaderboardData = new();
//     [SerializeField] TMP_Text usernameDisplay;
//     [SerializeField] TMP_Text scoreDisplay;
//     [SerializeField] TMP_Text rankDisplay;
//     [SerializeField] GameObject scoreEntryContainer;
//     [SerializeField] GameObject LeaderboardPanel;

//     async void Start()
//     {
//         await UnityServices.InitializeAsync();

//         if (!AuthenticationService.Instance.IsSignedIn)
//         {
//             try
//             {
//                 await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(LoginSignUpScript.PlayerSession.Username, LoginSignUpScript.PlayerSession.Password);
//                 Debug.Log("Signed in successfully with username/password.");
//             }
//             catch (AuthenticationException ex)
//             {
//                 Debug.LogError($"Authentication failed: {ex.Message}");
//                 return;
//             }
//         }

//         await FetchLeaderboardData();
//     }

//     private async Task FetchLeaderboardData()
//     {
//         var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("your_leaderboard_id");

//         List<Task> fetchTasks = new();

//         foreach (var entry in scoresResponse.Results)
//         {
//             //fetchTasks.Add(FetchPlayerData(entry));
//         }

//         await Task.WhenAll(fetchTasks);
//         DisplayLeaderboard();
//     }

//     // private async Task FetchPlayerData(LeaderboardEntry entry)
//     // {
//     //     string playerId = entry.PlayerId;
//     //     int score = (int)entry.Score;

//     //     try
//     //     {
//     //         var keys = new HashSet<string> { "username" };
//     //         var loadOptions = new LoadOptions { PlayerId = playerId };
//     //         var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys, loadOptions);

//     //         string username = cloudData.ContainsKey("username") ? cloudData["username"] : "Unknown";
//     //         leaderboardData[playerId] = (username, score);
//     //     }
//     //     catch (System.Exception ex)
//     //     {
//     //         Debug.LogWarning($"Failed to fetch username for {playerId}: {ex.Message}");
//     //         leaderboardData[playerId] = ("Unknown", score);
//     //     }
//     // }

//     private void DisplayLeaderboard()
//     {
//         foreach (var data in leaderboardData)
//         {
//             Debug.Log($"Username: {data.Value.username} | Score: {data.Value.score}");

//             AssignValues("1",data.Value.username,data.Value.score.ToString());
//         }


//     }

//     private void AssignValues(string rank,string username, string score)
//     {
//         var tempRank=scoreEntryContainer.transform.Find("Rank").gameObject.GetComponent<TextMeshProUGUI>();
//         var tempUsername=scoreEntryContainer.transform.Find("Username").gameObject.GetComponent<TextMeshProUGUI>();
//         var tempScore = scoreEntryContainer.transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
//         tempRank.text=rank;
//         tempUsername.text=username;
//         tempScore.text=score;

//         Instantiate(scoreEntryContainer);
//     }

//     // string VersionId { get; set; }
//     // int Offset { get; set; }
//     // int Limit { get; set; }
//     // int RangeLimit { get; set; }
//     // List<string> FriendIds { get; set; }

//     // IAuthenticationService authService;

    
//     // async void Awake()
//     // {
//     //     await UnityServices.InitializeAsync();
//     //     authService = AuthenticationService.Instance;

//     //     // await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(LoginSignUpScript.PlayerSession.Username,LoginSignUpScript.PlayerSession.Password);
//     // }

//     // public async void AddScore(int score,string leaderboardId=LeaderboardId)
//     // {
//     //     if(authService.IsSignedIn)
//     //     {
//     //         var playerEntry = await LeaderboardsService.Instance
//     //         .AddPlayerScoreAsync(leaderboardId, score);
//     //         Debug.Log(JsonConvert.SerializeObject(playerEntry));
//     // }
//     //     }
        

//     // public async void GetPlayerScore(string leaderboardId=LeaderboardId)
//     // {
//     //     if(authService.IsSignedIn)
//     //     {
//     //         var scoreResponse = await LeaderboardsService.Instance
//     //         .GetPlayerScoreAsync(leaderboardId);
//     //         Debug.Log(JsonConvert.SerializeObject(scoreResponse));
//     //     }
        
//     // }

//     // public async void GetPlayerRange(string leaderboardId=LeaderboardId)
//     // {
//     //     // Returns a total of 11 entries (the given player plus 5 on either side)
//     //     var rangeLimit = 5;
//     //     var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
//     //         leaderboardId,
//     //         new GetPlayerRangeOptions{ RangeLimit = rangeLimit }
//     //     );
//     //     Debug.Log(JsonConvert.SerializeObject(scoresResponse));
//     // }

//     void FuntionForLeaderoard()
//     {
//         // Fectch from Firebase
//         //fetch from leaderboard
//         //compare playerid
//         //display
//     }
// }


using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using TMPro;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    const string LeaderboardId = "FlopstronautsLB";

    [Header("UI References")]
    [SerializeField] private GameObject scoreEntryPrefab;
    [SerializeField] private Transform leaderboardPanel;

    private FirebaseRealtimeDB firebaseDB;

    async void Start()
    {
        firebaseDB = FindObjectOfType<FirebaseRealtimeDB>();

        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(
                    LoginSignUpScript.PlayerSession.Username,
                    LoginSignUpScript.PlayerSession.Password
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Auth failed: " + ex.Message);
                return;
            }
        }

        await ShowLeaderboard();
    }

    private async Task ShowLeaderboard()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);

        leaderboardPanel.DetachChildren(); // Clear existing

        int rank = 1;
        foreach (var entry in scoresResponse.Results)
        {
            string playerID = entry.PlayerId;
            int score = (int)entry.Score;
            string username = await firebaseDB.GetUsername(playerID);

            AddEntryToUI(rank.ToString(), username, score.ToString());
            rank++;
        }
    }

    private void AddEntryToUI(string rank, string username, string score)
    {
        GameObject entryObj = Instantiate(scoreEntryPrefab, leaderboardPanel);
        entryObj.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rank;
        entryObj.transform.Find("Username").GetComponent<TextMeshProUGUI>().text = username;
        entryObj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = score;
    }
}
