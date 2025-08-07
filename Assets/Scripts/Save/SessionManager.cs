using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;

[UnityEngine.Scripting.Preserve]
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
        }
        catch (Exception e)
        {
            Debug.Log(e);
            // SaveSystem.LoadPlayer(); // Fallback
        }

        SaveSystem.LoadPlayer(); // Load existing player data

    }
}
