using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;

public class SessionManager : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Check if it's the first time this device/player has signed in
            if (!GameData.HasSignedInBefore)
            {
                SaveSystem.LoadNewPlayer(); // New player setup
            }
            else
            {
                SaveSystem.LoadPlayer(); // Load existing player data
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            SaveSystem.LoadPlayer(); // Fallback
        }
    }
}
