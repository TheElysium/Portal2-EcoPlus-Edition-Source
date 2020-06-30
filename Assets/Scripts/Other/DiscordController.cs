using Discord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
	public static DiscordController instance;

    public Discord.Discord discord;

	private Discord.Activity activity;
	private Discord.ActivityManager activityManager;

	private void Awake()
	{
		{
			if (instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}
	}
	void Start()
	{

		Debug.Log("start discord");
		DontDestroyOnLoad(this);
		discord = new Discord.Discord(723655425374945461, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
		activityManager = discord.GetActivityManager();
		activity = new Discord.Activity
		{
			Assets =
			{
				LargeImage = "p2_icon", // Larger Image Asset Key
				LargeText = "Le jeu incredidble" // Large Image Tooltip
			},
			State = "v0.4 Epico Update",
			Details = "Ranch Games"
		};
		UpdateActivity(activity);
	}

	void Update()
    {
		discord.RunCallbacks();

	}

	public void UpdateLevel()
	{
		activity = new Discord.Activity
		{
			Assets =
			{
				LargeImage = "p2_icon", // Larger Image Asset Key
				LargeText = "v0.4 Epico Update" // Large Image Tooltip
			},
			State = SceneManager.GetActiveScene().name,
			Details = "Ranch Games"
		};
		UpdateActivity(activity);
	}

	private void UpdateActivity(Activity activity)
	{
		activityManager.UpdateActivity(activity, (res) =>
		{
			if (res == Discord.Result.Ok)
			{
				Debug.LogError(activity.Name + " : appliquée");
			}
		});
	}
	
}
