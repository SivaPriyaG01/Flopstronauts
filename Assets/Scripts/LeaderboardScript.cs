using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "FlopstronautsLB";

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    IAuthenticationService authService;

    async void Awake()
    {
        await UnityServices.InitializeAsync();

        // await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(LoginSignUpScript.PlayerSession.Username,LoginSignUpScript.PlayerSession.Password);
    }

    public async void AddScore(int score,string leaderboardId=LeaderboardId)
    {
        if(authService.IsSignedIn)
        {
            var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(leaderboardId, score);
            Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }
        }
        

    public async void GetPlayerScore(string leaderboardId=LeaderboardId)
    {
        if(authService.IsSignedIn)
        {
            var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
        
    }

    public async void GetPlayerRange(string leaderboardId=LeaderboardId)
    {
        // Returns a total of 11 entries (the given player plus 5 on either side)
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
            leaderboardId,
            new GetPlayerRangeOptions{ RangeLimit = rangeLimit }
        );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}
