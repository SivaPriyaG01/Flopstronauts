// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Newtonsoft.Json;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Leaderboards;
// using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards;
using Unity.Services.Core;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;


public class LeaderboardScript : MonoBehaviour
{
    const string LeaderboardId = "FlopstronautsLB";

    private Dictionary<string, (string username, int score)> leaderboardData = new();

    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(LoginSignUpScript.PlayerSession.Username, LoginSignUpScript.PlayerSession.Password);
                Debug.Log("Signed in successfully with username/password.");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError($"Authentication failed: {ex.Message}");
                return;
            }
        }

        await FetchLeaderboardData();
    }

    private async Task FetchLeaderboardData()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("your_leaderboard_id");

        List<Task> fetchTasks = new();

        foreach (var entry in scoresResponse.Results)
        {
            //fetchTasks.Add(FetchPlayerData(entry));
        }

        await Task.WhenAll(fetchTasks);
        DisplayLeaderboard();
    }

    // private async Task FetchPlayerData(LeaderboardEntry entry)
    // {
    //     string playerId = entry.PlayerId;
    //     int score = (int)entry.Score;

    //     try
    //     {
    //         var keys = new HashSet<string> { "username" };
    //         var loadOptions = new LoadOptions { PlayerId = playerId };
    //         var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys, loadOptions);

    //         string username = cloudData.ContainsKey("username") ? cloudData["username"] : "Unknown";
    //         leaderboardData[playerId] = (username, score);
    //     }
    //     catch (System.Exception ex)
    //     {
    //         Debug.LogWarning($"Failed to fetch username for {playerId}: {ex.Message}");
    //         leaderboardData[playerId] = ("Unknown", score);
    //     }
    // }

    private void DisplayLeaderboard()
    {
        foreach (var data in leaderboardData)
        {
            Debug.Log($"Username: {data.Value.username} | Score: {data.Value.score}");
        }
    }

    // string VersionId { get; set; }
    // int Offset { get; set; }
    // int Limit { get; set; }
    // int RangeLimit { get; set; }
    // List<string> FriendIds { get; set; }

    // IAuthenticationService authService;

    
    // async void Awake()
    // {
    //     await UnityServices.InitializeAsync();
    //     authService = AuthenticationService.Instance;

    //     // await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(LoginSignUpScript.PlayerSession.Username,LoginSignUpScript.PlayerSession.Password);
    // }

    // public async void AddScore(int score,string leaderboardId=LeaderboardId)
    // {
    //     if(authService.IsSignedIn)
    //     {
    //         var playerEntry = await LeaderboardsService.Instance
    //         .AddPlayerScoreAsync(leaderboardId, score);
    //         Debug.Log(JsonConvert.SerializeObject(playerEntry));
    // }
    //     }
        

    // public async void GetPlayerScore(string leaderboardId=LeaderboardId)
    // {
    //     if(authService.IsSignedIn)
    //     {
    //         var scoreResponse = await LeaderboardsService.Instance
    //         .GetPlayerScoreAsync(leaderboardId);
    //         Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    //     }
        
    // }

    // public async void GetPlayerRange(string leaderboardId=LeaderboardId)
    // {
    //     // Returns a total of 11 entries (the given player plus 5 on either side)
    //     var rangeLimit = 5;
    //     var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
    //         leaderboardId,
    //         new GetPlayerRangeOptions{ RangeLimit = rangeLimit }
    //     );
    //     Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    // }

    void FuntionForLeaderoard()
    {
        // Fectch from Firebase
        //fetch from leaderboard
        //compare playerid
        //display
    }
}
