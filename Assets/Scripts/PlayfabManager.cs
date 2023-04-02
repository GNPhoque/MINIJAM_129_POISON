using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
	[SerializeField] GameObject namePanel;
	[SerializeField] TMPro.TMP_InputField nameField;
	[SerializeField] TMPro.TextMeshProUGUI resultTime;

	[SerializeField] GameObject debug;
	[SerializeField] TMPro.TextMeshProUGUI debugText;

	[SerializeField] GameObject leaderboardRowPrefab;
	[SerializeField] Transform leaderboardRowParent;

	[SerializeField] GameObject bufferring;

	int playerTime;
	string displayName = "";

	public static string DeviceUniqueIdentifier
	{
		get
		{
			var deviceId = "";


#if UNITY_EDITOR
			deviceId = SystemInfo.deviceUniqueIdentifier + "-editor";
#elif UNITY_ANDROID
                AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
                AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");
                AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
                deviceId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
#elif UNITY_WEBGL
                if (!PlayerPrefs.HasKey("UniqueIdentifier"))
                    PlayerPrefs.SetString("UniqueIdentifier", Guid.NewGuid().ToString());
                deviceId = PlayerPrefs.GetString("UniqueIdentifier");
#else
                deviceId = SystemInfo.deviceUniqueIdentifier;
#endif
			return deviceId;
		}
	}

	private void Start()
	{
		playerTime = TimerSystem.GetTimeScore();
		TimerSystem.ResetTimer();

		Login();
	}

	void Login()
	{
		var request = new LoginWithCustomIDRequest
		{
			CustomId = DeviceUniqueIdentifier,
			CreateAccount = true,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
			{
				GetPlayerProfile = true
			}
		};
		PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
	}

	public void SendLeaderboard()
	{
		var request = new UpdatePlayerStatisticsRequest
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName="GameScore",
					Value = -playerTime
				}
			}
		};
		Debug.Log($"Sending {displayName} : {request.Statistics[0].Value}");
		PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
	}

	public void GetLeaderboard()
	{
		bufferring.SetActive(true);
		var request = new GetLeaderboardRequest
		{
			StatisticName = "GameScore",
			StartPosition = 0,
			MaxResultsCount = 10,
		};
		StartCoroutine(WaitBeforePull(request, OnLeaderboardGet, OnError));
	}

	private static IEnumerator WaitBeforePull(GetLeaderboardRequest request, Action<GetLeaderboardResult> OnLeaderboardGet, Action<PlayFabError> OnError)
	{
		yield return new WaitForSeconds(1f);
		PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
	}

	public void SubmitNameButton()
	{
		var request = new UpdateUserTitleDisplayNameRequest
		{
			DisplayName = nameField.text
		};
		PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
	}



	private void OnLoginSuccess(LoginResult obj)
	{
		Debug.Log($"Successful {(obj.NewlyCreated ? "acount creation!" : "login")}");
		
		//From main menu button
		if (playerTime == 0)
		{
			GetLeaderboard();
			return;
		}

		displayName = "";
		//Get player name
		if (obj.InfoResultPayload.PlayerProfile != null)
			displayName = obj.InfoResultPayload.PlayerProfile.DisplayName;

		if (string.IsNullOrEmpty(displayName))
		{
			namePanel.SetActive(true);
		}
		else
		{
			SendLeaderboard();
		}
	}

	private void OnError(PlayFabError error)
	{
		Debug.LogError("Database connection error");
		Debug.LogError(error.GenerateErrorReport());
		debug.SetActive(true);
		debugText.text = error.GenerateErrorReport();
	}

	private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj)
	{
		Debug.Log("Succesfull leaderboard update");
		GetLeaderboard();
	}

	private void OnLeaderboardGet(GetLeaderboardResult obj)
	{
		Debug.Log($"Leaderboard retrieved with {obj.Leaderboard.Count} rows");
		foreach (Transform item in leaderboardRowParent)
		{
			Destroy(item.gameObject);
		}
		foreach (var item in obj.Leaderboard)
		{
			Debug.Log($"{++item.Position} : {item.DisplayName} {item.StatValue}");
			GameObject newRow = Instantiate(leaderboardRowPrefab, leaderboardRowParent);
			TMPro.TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
			texts[0].text = item.Position.ToString();
			texts[1].text = item.DisplayName;
			texts[2].text = (-item.StatValue).ToString("00:00:00");
		}
		bufferring.SetActive(false);
	}

	private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
	{
		displayName = obj.DisplayName;
		Debug.Log("Updated user display name to "+ displayName);
		namePanel.SetActive(false);
		SendLeaderboard();
	}
}
