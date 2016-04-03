using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Encoder;

[System.Serializable]
public class HeroDetails
{
    public string name;
	public Texture2D portrait;
    [HideInInspector]
    public string[] splitValues;
    public float winRate;
    public string role;
    public string subRole;
    public bool picked = false;
	public List<OnMap> onMap = new List<OnMap>();
    public List<HeroSynergyCounterDetails> heroSynergy;
    public List<HeroSynergyCounterDetails> heroCounter;
}

[System.Serializable]
public class PlayerHeroDetails
{
    public string name;
	public string subRoleName;
    [HideInInspector]
    public string[] splitValues;
    public float winRate;
    public int level;
    public int gamesPlayed;
}

[System.Serializable]
public class RoleCompDetails
{
	public string name;
	public float winRate;
	[HideInInspector]
	public string[] splitValues;
	public List<string> role;
	public List<int> numOfEachRole = new List<int>();
}

[System.Serializable]
public class SubRoleCompDetails
{
	public string name;
    public float winRate;
	[HideInInspector]
	public string[] splitValues;
	public List<string> subRole;
	public List<int> numOfEachSubRole = new List<int>();
	public int rankTeam1 = 0;
	public int rankTeam2 = 0;
}

[System.Serializable]
public class RoleCompsOnMapDetails
{
	public string mapName;
	[HideInInspector]
	public List<string> splitValues;
	public List<RoleCompDetails> comps;
	public float lowestWinRate = 1.0f;
}

[System.Serializable]
public class SubRoleCompsOnMapDetails
{
	public string mapName;
    [HideInInspector]
    public List<string> splitValues;
    public List<SubRoleCompDetails> comps;
	public List<SubRoleValue> subRoleValues = new List<SubRoleValue>();
	public float lowestWinRate = 1.0f;
}

[System.Serializable]
public class SubRoleValue
{
	public string subRoleName;
	public int totalHeroes;
	public float occurrencesWinPercent;
	public int occurrences; //number of times this sub role appears in the top 100 sub role comps with weight for top comps
	public int occurrenceRank;
	public float occurrences2WinPercent;
	public int occurrences2;
	public int occurrenceRank2;
	public int scarcity; //number of heroes in the game with this sub role
	public int scarcityRank;
	public int aboveAverageScarcity; //number of heroes in the game with this sub role with an above average win rate for the sub role
	public float aboveAverageScarcityPercent;
	public int aboveAverageScarcityRank;
	public int aboveAverageScarcityTeam1; //number of heroes in the game with this sub role with an above average win rate for the sub role
	public float aboveAverageScarcityPercentTeam1;
	public float aboveAverageScarcityRankTeam1;
	public int aboveAverageScarcityTeam2; //number of heroes in the game with this sub role with an above average win rate for the sub role
	public float aboveAverageScarcityPercentTeam2;
	public float aboveAverageScarcityRankTeam2;
	public string topHeroWinRateName; //hero with this sub role that has the highest overall win rate
	public float topHeroWinRate = 0.0f;  
	public float topHeroWinRateRank;
	public float staticAverageHeroWinRate;
	public float averageHeroWinRate; //average win rate of all heroes with this sub role
	public int averageHeroWinRateRank;
	public string topHeroSynergyName; //hero with this sub role that has the highest synergy score with the heroes already chosen for the friendly team
	public float topHeroSynergy;
	public int topHeroSynergyRank;
	public string topHeroCounterName; //hero with this sub role that has the highest counter score against the heroes already chosen for the enemy team
	public float topHeroCounter;
	public int topHeroCounterRank;
	public string topHeroOnMapName; //hero with this sub role that has the highest on-map bonus score (win rate higher than their overall win rate)
	public float topHeroOnMap;
	public int topHeroOnMapRank;
	public float overallRank;
	public float overallRank2;
	public float team1Rank;
	public float team2Rank;
}

[System.Serializable]
public class HeroSynergyCounterDetails
{
    public string name;
    [HideInInspector]
    public string[] splitValues;
    public float winRateWithAgainst;
    public float estAdvantage;
}

[System.Serializable]
public class SubRoleDetails
{
	public string name;
	public int subRoleCount;
	public string playerTopHeroScoreName; //hero with this sub role that has the highest score for the player
	public int playerTopHeroScore;
	public int playerTopHeroScoreRank;
	public float playerWinRate; //Player's combined average win rate for all heroes with this sub role
	public float playerWinRateRank;
	public string playerTopHeroWinRateName; //hero with this sub role that has the highest win rate for the player
	public float playerTopHeroWinRate = 0.0f;
	public int playerTopHeroWinRateRank;
	public string playerAverageHeroWinRateName; //average win rate for the player for heroes with this sub role
	public float playerAverageHeroWinRate = 0.0f;
	public int playerAverageHeroWinRateRank;
	public string playerMostPlayedHeroGamesPlayedName; //hero with this sub role that has the most games played for the player
	public int playerMostPlayedHeroGamesPlayed;
	public int playerMostPlayedHeroGamesPlayedRank;
	public float playerOverallRank;
	public float combinedRank;
}

[System.Serializable]
public class Match
{
    public string chosenMapName;
    public Team friendlyTeam;
    public Team enemyTeam;
}

[System.Serializable]
public class Team
{
    public Player[] player;
    public WithHeroes withHeroes;
    public OnMap onMap;
	public RoleCompOnMap roleCompOnMap;
	public SubRoleCompOnMap subRoleCompOnMap;
	public List<int> numOfEachSubRole = new List<int>();
	public List<int> numOfEachRole = new List<int>();
	public HeroSynergy heroSynergies;
    public HeroCounters heroCounters;
    public int score;
}

[System.Serializable]
public class WithHeroes
{
    public float winRate = 0.5f; //Average win rate of players with heroes chosen.
    public int score;
}

[System.Serializable]
public class OnMap
{
	public string name;
    public float winRate = 0.5f; //Average win rate of players on map chosen.
	public int score;
}

[System.Serializable]
public class RoleCompOnMap
{
	public List<string> fullTeamComp = new List<string>();
	public float winRate = 0.5f; //Average win rate of role composition on map chosen.
	public int score;
}

[System.Serializable]
public class SubRoleCompOnMap
{
	public List<string> fullTeamComp = new List<string>();
    public float winRate = 0.5f; //Average win rate of sub-role composition on map chosen.
	public int score;
}

[System.Serializable]
public class HeroSynergy
{
    public float winRate = 0.5f; //Average win rate of heroes with each combination of allied heroes.
	public float advRate = 0.5f;
	public float finalCombination = 0.5f;
	public int score;
}

[System.Serializable]
public class HeroCounters
{
    public float winRate = 0.5f; //Average win rate of heroes against each combination of enemy heroes.
	public float advRate = 0.5f;
	public float finalCombination = 0.5f;
	public int score;
}

[System.Serializable]
public class Player
{
    public string name;
    public string customName;
	public int idNumber;
	public bool foundOnHotslogs = false;
    public List<PlayerHeroDetails> heroDetails = new List<PlayerHeroDetails>();
	public List<CurrentHeroScores> currentHeroScores = new List<CurrentHeroScores>();
	public int qmMmr = -5000;
	public int hlMmr = -5000;
	public int tlMmr = -5000;
	public int gamesPlayedWithChosenHero = 0;
	public string chosenHeroName;
	public string chosenHeroRole;
	public string chosenHeroSubRole;
    public List<MapDetails> mapDetails = new List<MapDetails>();
	public List<SubRoleDetails> subRoleDetails = new List<SubRoleDetails>();
    public float chosenHeroWinRate = 0.5f; //Win rate with hero chosen.
	public int chosenHeroScore = 0;
    public float chosenMapWinRate = 0.5f; //Win rate on map chosen.
	public int chosenMapScore = 0;
    public float chosenHeroOnMapWinRate = 0.0f; //Win rate with sub-role chosen.
	public int chosenHeroOnMapScore = 0;
    public int score;
	public Sprite hotsLogsBanner;
}

[System.Serializable]
public class CurrentHeroScores
{
	public string name = "";
	public int score;
}

[System.Serializable]
public class MapDetails
{
    public string name;
	public int gamesPlayed;
	public float winRate;
}

[System.Serializable]
public class SubRoleGroups
{
    public string[] splits;
}

public class PullDataTest : MonoBehaviour
{
	public string versionNum = "v1.00";
	public List<string> allMaps = new List<string>();

	[HideInInspector]
	public string[] mapPageLines;
	[HideInInspector]
	public List<string> mapPageLinesFiltered = new List<string>();

	public List<string> allHeroNames = new List<string>();
	public List<HeroDetails> allHeroDetails = new List<HeroDetails>();

	private string heroPage;
	[HideInInspector]
	public string[] heroPageLines;
	[HideInInspector]
	public List<string> heroPageLinesFiltered = new List<string>();

	private string subRolePage;
	[HideInInspector]
	public string[] subRolePageLines;
	[HideInInspector]
	public List<string> subRolePageLinesFiltered = new List<string>();
	[HideInInspector]
	public string[] subRolePageLinesFilteredSplits;
	[HideInInspector]
	public List<SubRoleGroups> subRoleGroups;

	public List<string> allSubRoleNames = new List<string>();
	public List<string> allRoleNames = new List<string>();
	public List<SubRoleCompsOnMapDetails> allSubRolesOnMapDetails = new List<SubRoleCompsOnMapDetails>();
	public List<RoleCompsOnMapDetails> allRolesOnMapDetails = new List<RoleCompsOnMapDetails>();

	public Match match;

	public int gamesPlayedThreshold = 0;
	public float maxExperiencePenalty = 0.025f;

	[Range(0.0f, 1.0f)]
	public float playerHeroWeight = 1.0f;
	public void SetPlayerHeroWeight(float val)
	{
		playerHeroWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float playerMapWeight = 1.0f;
	public void SetPlayerMapWeight(float val)
	{
		playerMapWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float heroMapWeight = 1.0f;
	public void SetHeroMapWeight(float val)
	{
		heroMapWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float subRoleCompMapWeight = 1.0f;
	public void SetSubRoleCompMapWeight(float val)
	{
		subRoleCompMapWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float synergyWeight = 1.0f;
	public void SetSynergyWeight(float val)
	{
		synergyWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float counterWeight = 1.0f;
	public void SetCounterWeight(float val)
	{
		counterWeight = val;
	}

	[Range(0.0f, 1.0f)]
	public float estAdvantageOrWinRate = 0.0f;

	public Dropdown mapNameDropdown;
	public InputField[] playerNameInput;
	public GameObject[] playerCustomNameOverlay;
	public Dropdown[] playerHeroDropdown;
	public Image[] playerHeroPortrait;
	public Image[] playerHeroBorder;
	public Text[] playerMmrText;
	public Text[] playerScoreText;
	public Image[] playerSubRoleImage;
	public Image[] playerFoundIndicator;
	public Image[] playerBackgroundImage;
	public Text[] friendNameText;
	public Dropdown[] friendListDropdown;
	public InputField[] friendNameInputField;

	public Toggle alwaysOnTopToggle;

	public Sprite tankSubRoleImage;
	public Color tankColor;
	public Sprite bruiserSubRoleImage;
	public Color bruiserColor;
	public Sprite healerSubRoleImage;
	public Color healerColor;
	public Sprite supportSubRoleImage;
	public Color supportColor;
	public Sprite ambusherSubRoleImage;
	public Color ambusherColor;
	public Sprite burstDamageSubRoleImage;
	public Color burstDamageColor;
	public Sprite sustainedDamageSubRoleImage;
	public Color sustainedDamageColor;
	public Sprite siegeSubRoleImage;
	public Color siegeColor;
	public Sprite utilitySubRoleImage;
	public Color utilityColor;
	public Sprite genericSubRoleImage;
	public Sprite genericHeroImage;

	public Image[] subCompRecommendation1;
	public Image[] subCompRecommendation2;
	public Image[] subCompRecommendation3;
	public Image[] subCompRecommendation4;
	public Image[] subCompRecommendation5;
	public Image[] subCompRecommendation6;

	public GameObject[] playerHeroRecommendations;

	public Text[] recommendedCompWinRateText;

	public bool downloadFinished = false;

	public GameObject hoverPanel;

	public Image hoverBanner;

	public GameObject sliderOptions;

	public GameObject heroPickPanel;
	public GameObject heroPickPortraitPrefab;
	public int currentlyPickingPlayer;

	private int loadingInitialCounter = 0;
	public GameObject loadingPanel;
	public Text loadingTextMain;
	public Text loadingText2;
	public float loadingPercent1 = 0.0f;

	public GameObject optionsPanel;

	public GameObject updateButton;

	public InputField gamesPlayedInput;
	public InputField maxPenaltyInput;
	public Dropdown mmrTypeDropdown;
	public int mmrType = 1;

	public List<string> friendsList = new List<string>();
	public List<string> friendsListFiltered = new List<string>();

	public Camera photoCamera;
	public GameObject uploadingPanel;
	public GameObject uploadingConfirmationBox;

	public Image[] suggestedSubRoles;
	public GameObject[] suggestedHeroOrder;

	public Text playerChoosingText;

	public bool alwaysOnTop = false;

	public bool foundBaseComp1 = false;
	public bool foundSubRoleComp1 = false;

	public bool foundBaseComp2 = false;
	public bool foundSubRoleComp2 = false;

	// Static singleton property
	public static PullDataTest Instance
	{
		get; private set;
	}

	void Awake()
	{
		// First we check if there are any other instances conflicting
		if (Instance != null && Instance != this)
		{
			// If that is the case, we destroy other instances
			Destroy(gameObject);
		}

		// Here we save our singleton instance
		Instance = this;

		loadingPanel.SetActive(true);
		Screen.SetResolution(1248, 702, false);
		LoadEverything();
	}

	void Start()
	{
		StartCoroutine(CheckConnectionToMasterServer());
	}

	private IEnumerator CheckConnectionToMasterServer()
	{
		Ping pingMasterServer = new Ping("8.8.8.8");

		float startTime = Time.time;
		while (!pingMasterServer.isDone && Time.time < startTime + 5.0f)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (pingMasterServer.isDone)
		{
			StartCoroutine(GetVersionNumber());
			SetHeroRecommendationPlayers();
		}
		else
		{
			//No Internet
		}
	}

	/// <summary>
	/// Triggers RunPlayerHeroData() for each player name/id array for friendly and enemy team.
	/// </summary>
	public void GetPlayerHeroData()
	{
		foreach (Player player in match.friendlyTeam.player)
		{
			if (player.name != null && player.name != "")
			{
				Debug.Log("Running Player Data for Friendly Team " + player.name);
			}
			else
			{
				Debug.Log("Running Player Data for Friendly Team Anonymous Player");
				player.customName = "";
				player.idNumber = new int();
			}
			//StartCoroutine(RunPlayerHeroData(player));
		}

		foreach (Player player in match.enemyTeam.player)
		{
			if (player.name != null && player.name != "")
			{
				Debug.Log("Running Player Data for Enemy Team " + player.name);
			}
			else
			{
				Debug.Log("Running Player Data for Enemy Team Anonymous Player");
				player.customName = "";
				player.idNumber = new int();
			}
			//StartCoroutine(RunPlayerHeroData(player));
		}

		//System.IO.File.WriteAllText(Application.dataPath + "/" + player.name + "_playerData.txt", playerPage);
	}

	IEnumerator GetVersionNumber()
	{
		string url = "http://randomseedgames.com/hots-matchup";

		WWW www = new WWW(url);
		yield return www;
		string versionPage = www.text;

		string[] versionPageLines = versionPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		string versionPageLineFiltered = "";

		foreach (string line in versionPageLines)
		{
			if (line.Contains("Current Version: "))
			{
				versionPageLineFiltered = line;

				int positionInLine = line.IndexOf(':');

				Debug.Log(line.Substring(positionInLine + 3, 4));

				if (line.Substring(positionInLine + 3, 4) == versionNum)
				{
					StartCoroutine(GetHeroAndMapData());
				}
				else
				{
					AppUpdateAvailable();
				}
			}
			else
			{
				//Internet connection not available
			}
		}
	}

	public void AppUpdateAvailable()
	{
		updateButton.SetActive(true);
	}

	/// <summary>
	/// Uses the player name or id to search hotslogs.com and retreive their data.
	/// </summary>
	IEnumerator RunPlayerHeroData(Player player, int playerNum)
	{
		if (player.name == null || player.name == "")
		{
			player.customName = "";
			player.idNumber = new int();
		}

		player.chosenHeroWinRate = 0.5f;
		player.chosenHeroOnMapWinRate = 0.0f;
		player.chosenMapWinRate = 0.5f;
		playerMmrText[playerNum].text = "";
		player.subRoleDetails = new List<SubRoleDetails>();

		string url = "";

		if (!IsDigitsOnly(player.name))
		{
			url = "http://www.hotslogs.com/PlayerSearch?Name=";
		}
		else
		{
			url = "http://www.hotslogs.com/Player/Profile?PlayerID=";
		}
		WWW www = new WWW(url + player.name);
		yield return www;
		string playerPage = www.text;

		string[] playerPageLines = playerPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		List<string> playerPageLinesFiltered = new List<string>();
		List<string> playerMapPageLinesFiltered = new List<string>();
		List<string> playerSubRolePageLinesFiltered = new List<string>();
		string playerMmrPageLineFiltered = "";
		string playerCustomNamePageLineFiltered = "";
		string playerIdNumberPageLineFiltered = "";

		foreach (string line in playerPageLines)
		{
			if (line.Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Heroes/Portraits/") && !line.Contains("$create") && !line.Contains("display:none"))
			{
				playerPageLinesFiltered.Add(line);
			}
			else if (line.Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Maps/") && line.Contains("title"))
			{
				playerMapPageLinesFiltered.Add(line);
			}
			else if (line.Contains("td bgcolor="))
			{
				playerSubRolePageLinesFiltered.Add(line);
			}
			else if (line.Contains("(Current MMR: "))
			{
				playerMmrPageLineFiltered = line;
			}
			else if (line.Contains("Profile: ") && line.Contains("Heroes of the Storm"))
			{
				playerCustomNamePageLineFiltered = line;
			}
			else if (line.Contains("./Profile?PlayerID="))
			{
				playerIdNumberPageLineFiltered = line;
			}
		}

		player.heroDetails = new List<PlayerHeroDetails>();
		if (playerPageLinesFiltered.Count <= 0 || playerPageLinesFiltered == null)
		{
			playerFoundIndicator[playerNum].color = Color.white;
			playerCustomNameOverlay[playerNum].SetActive(false);
			player.foundOnHotslogs = false;
			player.hotsLogsBanner = null;
			if (!IsDigitsOnly(player.name) && player.name != null && player.name != "")
			{
				playerFoundIndicator[playerNum].color = Color.red;
				player.mapDetails = new List<MapDetails>();
				player.chosenMapWinRate = 0.5f;
				Debug.Log("Could not identify a single player by the name " + player.name + ". Please input ID# from hotslogs.com profile URL instead of name.");

				playerCustomNameOverlay[playerNum].SetActive(true);
				playerCustomNameOverlay[playerNum].transform.Find("Text").GetComponent<Text>().text = "Player Name Not Found";
			}
			else if (IsDigitsOnly(player.name) && player.name != null && player.name != "")
			{
				playerFoundIndicator[playerNum].color = Color.red;
				player.mapDetails = new List<MapDetails>();
				player.chosenMapWinRate = 0.5f;
				Debug.Log("No player with the ID# " + player.name + " was found. Please input a valid player ID#.");

				playerCustomNameOverlay[playerNum].SetActive(true);
				playerCustomNameOverlay[playerNum].transform.Find("Text").GetComponent<Text>().text = "Player ID# Not Found";
			}
		}
		else
		{
			playerFoundIndicator[playerNum].color = Color.green;
			player.foundOnHotslogs = true;

			for (int i = 0; i < playerPageLinesFiltered.Count; i++)
			{
				player.heroDetails.Add(new PlayerHeroDetails());
			}

			for (int i = 0; i < playerPageLinesFiltered.Count; i++)
			{
				//playerPageLinesFiltered[i] = playerPageLinesFiltered[i].Replace("</span>", "");

				if (playerPageLinesFiltered[i].Contains("GoldStar"))
				{
					playerPageLinesFiltered[i] = playerPageLinesFiltered[i].Replace("<img style=\"width: 15px; background-color: initial;\" src=\"//d1i1jxrdh2kvwy.cloudfront.net/Images/GoldStar.png\"><span style=\"vertical-align: middle; font-weight: bold;\"", "").Replace("</span>", "").Replace("&nbsp;", "");
					player.heroDetails[i].splitValues = playerPageLinesFiltered[i].Split('>');
					player.heroDetails[i].name = player.heroDetails[i].splitValues[10].Replace("</a", "");
				}
				else
				{
					player.heroDetails[i].splitValues = playerPageLinesFiltered[i].Split('>');
					player.heroDetails[i].name = player.heroDetails[i].splitValues[11].Replace("</a", "");
				}

				for (int m = 0; m < allHeroDetails.Count; m++)
				{
					if (allHeroDetails[m].name == player.heroDetails[i].name)
					{
						player.heroDetails[i].subRoleName = allHeroDetails[m].subRole;
					}
				}

				float tempFloat = 0.0f;
				if (float.TryParse(player.heroDetails[i].splitValues[20].Replace(" %</td", ""), out tempFloat))
				{
					player.heroDetails[i].winRate = tempFloat / 100.0f;
				}
				else
				{
					player.heroDetails[i].winRate = 0.0f;
				}

                if (player.heroDetails[i].splitValues[14].Replace("</td", "") != "")
				{
					player.heroDetails[i].level = int.Parse(player.heroDetails[i].splitValues[14].Replace("</td", ""));
				}
				else
				{
					player.heroDetails[i].level = 0;
                }

				player.heroDetails[i].gamesPlayed = int.Parse(player.heroDetails[i].splitValues[16].Replace("</td", ""));
			}

			for (int i = 0; i < playerMapPageLinesFiltered.Count; i++)
			{
				player.mapDetails.Add(new MapDetails());
			}

			for (int i = 0; i < playerMapPageLinesFiltered.Count; i++)
			{
				string[] tempMapString = playerMapPageLinesFiltered[i].Split('>');

				player.mapDetails[i].name = tempMapString[10].Replace("</td", "");
				player.mapDetails[i].gamesPlayed = int.Parse(tempMapString[14].Replace("</td", ""));
				player.mapDetails[i].winRate = float.Parse(tempMapString[18].Replace(" %</td", "")) / 100.0f;
			}

			player.subRoleDetails = new List<SubRoleDetails>();
			for (int i = 0; i < playerSubRolePageLinesFiltered.Count; i++)
			{
				player.subRoleDetails.Add(new SubRoleDetails());
			}

			for (int i = 0; i < playerSubRolePageLinesFiltered.Count; i++)
			{
				string[] tempSubRoleString = playerSubRolePageLinesFiltered[i].Split('>');

				player.subRoleDetails[i].name = tempSubRoleString[2].Replace("</span", "");
				if (!tempSubRoleString[7].Contains("&nbsp;"))
				{
					player.subRoleDetails[i].playerWinRate = float.Parse(tempSubRoleString[7].Replace(" %</td", "")) / 100.0f;
				}
				else
				{
					player.subRoleDetails[i].playerWinRate = 0.0f;
				}
			}

			string[] tempMmrString = playerMmrPageLineFiltered.Split('(');
			if (tempMmrString.Length >= 2 && tempMmrString.Length < 3)
			{
				player.qmMmr = int.Parse(tempMmrString[1].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
			}
			else if (tempMmrString.Length >= 3 && tempMmrString.Length < 4)
			{
				player.hlMmr = int.Parse(tempMmrString[1].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
				player.qmMmr = int.Parse(tempMmrString[2].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
			}
			else if (tempMmrString.Length >= 4)
			{
				player.tlMmr = int.Parse(tempMmrString[1].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
				player.hlMmr = int.Parse(tempMmrString[2].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
				player.qmMmr = int.Parse(tempMmrString[3].Replace(" ", "").Split(')')[0].Replace("CurrentMMR:", ""));
			}
			UpdatePlayerMmr();

			string[] profileString = playerCustomNamePageLineFiltered.Split(':');
			player.customName = profileString[profileString.Length - 1].Replace(" ", "");
			playerCustomNameOverlay[playerNum].transform.Find("Text").GetComponent<Text>().text = player.customName;

			string[] idSplit = playerIdNumberPageLineFiltered.Split('=');
			player.idNumber = int.Parse(idSplit[3].Replace("\" id", ""));

			WWW www2 = new WWW("https://www.hotslogs.com/Images/PlayerProfileImage/" + player.idNumber + ".jpeg");
			yield return www2;
			player.hotsLogsBanner = Sprite.Create(www2.texture, new Rect(0, 0, www2.texture.width, www2.texture.height), new Vector2(0.5f, 0.5f));
		}

		SubRoleValue();
		SetPlayerHeroScores(playerNum);
		if (playerNum <= 4)
		{
			PlayerSubRoleValue(match.friendlyTeam, playerNum);
		}
		else
		{
			PlayerSubRoleValue(match.enemyTeam, playerNum - 5);
		}
		GetChosenHeroStats(player);
		GetChosenMapStats(player);
	}

	public void RunFriendName0(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 0));
	}
	public void RunFriendName1(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 1));
	}
	public void RunFriendName2(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 2));
	}
	public void RunFriendName3(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 3));
	}
	public void RunFriendName4(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 4));
	}
	public void RunFriendName5(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 5));
	}
	public void RunFriendName6(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 6));
	}
	public void RunFriendName7(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 7));
	}
	public void RunFriendName8(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 8));
	}
	public void RunFriendName9(string playerName)
	{
		StartCoroutine(RunFriendName(playerName, 9));
	}

	IEnumerator RunFriendName(string playerName, int friendNum)
	{
		if (playerName != "")
		{
			string url = "";

			if (!IsDigitsOnly(playerName))
			{
				url = "http://www.hotslogs.com/PlayerSearch?Name=";
			}
			else
			{
				url = "http://www.hotslogs.com/Player/Profile?PlayerID=";
			}
			WWW www = new WWW(url + playerName);
			yield return www;
			string playerPage = www.text;

			string[] playerPageLines = playerPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> playerPageLinesFiltered = new List<string>();
			string playerCustomNamePageLineFiltered = "";
			string playerCustomName = "";

			foreach (string line in playerPageLines)
			{
				if (line.Contains("Profile: ") && line.Contains("Heroes of the Storm"))
				{
					playerCustomNamePageLineFiltered = line;
				}
			}

			if (playerCustomNamePageLineFiltered.Length <= 0 || playerCustomNamePageLineFiltered == null)
			{
				if (!IsDigitsOnly(playerName) && playerName != null && playerName != "")
				{
					Debug.Log("Could not identify a single player by the name " + playerName + ". Please input ID# from hotslogs.com profile URL instead of name.");
					friendNameText[friendNum].text = "Player Not Found";
				}
				else if (IsDigitsOnly(playerName) && playerName != null && playerName != "")
				{
					Debug.Log("No player with the ID# " + playerName + " was found. Please input a valid player ID#.");
					friendNameText[friendNum].text = "ID# Not Found";
				}
			}
			else
			{
				string[] profileString = playerCustomNamePageLineFiltered.Split(':');
				playerCustomName = profileString[profileString.Length - 1].Replace(" ", "");
				friendNameText[friendNum].text = playerCustomName;
				friendsList[friendNum] = playerName;
				FilterFriendsList();
			}
		}
		else
		{
			friendNameText[friendNum].text = "";
			friendsList[friendNum] = playerName;
			FilterFriendsList();
		}

		SaveEverything();
	}

	public void FilterFriendsList()
	{
		friendsListFiltered = new List<string>();
		friendsListFiltered.Add("");
		for (int i = 0; i < friendsList.Count; i++)
		{
			if (friendsList[i] != "ID# Not Found" && friendsList[i] != "Player Not Found" && friendsList[i] != "")
			{
				friendsListFiltered.Add(friendNameText[i].text);
			}
		}
		UpdateFriendsListDropdowns();
	}

	public void UpdateFriendsListDropdowns()
	{
		for (int i = 0; i < friendListDropdown.Length; i++)
		{
			friendListDropdown[i].ClearOptions();
			friendListDropdown[i].AddOptions(friendsListFiltered);
		}
	}

	public void SelectFriendFromList(int playerNum)
	{
		for (int i = 0; i < friendNameText.Length; i++)
		{
			if (friendNameText[i].text == friendListDropdown[playerNum].options[friendListDropdown[playerNum].value].text)
			{
				playerNameInput[playerNum].text = friendsList[i];
			}
		}

		if (playerNum <= 4)
		{
			UpdateFriendlyPlayerName(playerNum);
		}
		else if (playerNum > 4)
		{
			UpdateEnemyPlayerName(playerNum);
		}
	}

	public void UpdateMmrType()
	{
		mmrType = mmrTypeDropdown.value;
		UpdatePlayerMmr();
		SaveEverything();
	}

	public void UpdatePlayerMmr()
	{
		for (int i = 0; i < playerMmrText.Length; i++)
		{
			if (mmrType == 0)
			{
				if (i <= 4)
				{
					if (match.friendlyTeam.player[i].qmMmr > -5000 && match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.friendlyTeam.player[i].qmMmr;
					}
					else if (match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
				else if (i > 4)
				{
					if (match.enemyTeam.player[i - 5].qmMmr > -5000 && match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.enemyTeam.player[i - 5].qmMmr;
					}
					else if (match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
			}
			else if (mmrType == 1)
			{
				if (i <= 4)
				{
					if (match.friendlyTeam.player[i].hlMmr > -5000 && match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.friendlyTeam.player[i].hlMmr;
					}
					else if (match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
				else if (i > 4)
				{
					if (match.enemyTeam.player[i - 5].hlMmr > -5000 && match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.enemyTeam.player[i - 5].hlMmr;
					}
					else if (match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
			}
			else if (mmrType == 2)
			{
				if (i <= 4)
				{
					if (match.friendlyTeam.player[i].tlMmr > -5000 && match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.friendlyTeam.player[i].tlMmr;
					}
					else if (match.friendlyTeam.player[i].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
				else if (i > 4)
				{
					if (match.enemyTeam.player[i - 5].tlMmr > -5000 && match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR: " + match.enemyTeam.player[i - 5].tlMmr;
					}
					else if (match.enemyTeam.player[i - 5].foundOnHotslogs)
					{
						playerMmrText[i].text = "MMR NOT FOUND";
					}
					else
					{
						playerMmrText[i].text = "";
					}
				}
			}
		}
	}

	public void OpenOptionsScreen()
	{
		optionsPanel.SetActive(true);
	}

	public void CloseOptionsScreen()
	{
		optionsPanel.SetActive(false);
	}

	/// <summary>
	/// Gets all hero data and map names from hotslogs.com.
	/// </summary>
	IEnumerator GetHeroAndMapData()
	{
		WWW www = new WWW("http://www.hotslogs.com/Default");
		yield return www;
		heroPage = www.text;

		heroPageLines = heroPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		heroPageLinesFiltered = new List<string>();
		List<string> mapPageLinesFiltered = new List<string>();

		foreach (string line in heroPageLines)
		{
			if (line.Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Heroes/Portraits/") && line.Contains("title"))
			{
				heroPageLinesFiltered.Add(line);
			}
			else if (line.Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Maps/") && line.Contains("title"))
			{
				mapPageLinesFiltered.Add(line);
			}
		}

		for (int i = 0; i < heroPageLinesFiltered.Count; i++)
		{
			allHeroDetails.Add(new HeroDetails());
		}

		allHeroNames = new List<string>();
		allHeroNames.Add("");
		for (int i = 0; i < heroPageLinesFiltered.Count; i++)
		{
			allHeroDetails[i].splitValues = heroPageLinesFiltered[i].Split('>');

			allHeroDetails[i].name = allHeroDetails[i].splitValues[5].Replace("</a", "");
			allHeroNames.Add(allHeroDetails[i].name);

			loadingTextMain.text = "Loading\n" + Mathf.Max(0, ((((float)i / allHeroDetails.Count) * 100.0f) - 1)).ToString("F0") + "%\n" + allHeroDetails[i].name;

			WWW www2 = new WWW("http://www.heroesfire.com/images/wikibase/icon/heroes/" + allHeroDetails[i].name.ToLower().Replace(" ", "-").Replace("'", "").Replace(".", "") + ".png");
			yield return www2;
			allHeroDetails[i].portrait = www2.texture;

			allHeroDetails[i].winRate = float.Parse(allHeroDetails[i].splitValues[12].Replace(" %</td", "")) / 100.0f;
			allHeroDetails[i].role = allHeroDetails[i].splitValues[18].Replace("</td", "");

			GameObject heroPickPortrait = (GameObject)Instantiate(heroPickPortraitPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			heroPickPortrait.name = allHeroDetails[i].name;
			heroPickPortrait.transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<Image>().sprite = Sprite.Create(allHeroDetails[i].portrait, new Rect(0, 0, allHeroDetails[i].portrait.width, allHeroDetails[i].portrait.height), new Vector2(0.5f, 0.5f));
			heroPickPortrait.transform.SetParent(heroPickPanel.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("AllHeroGrid").gameObject.transform);
			heroPickPortrait.transform.localScale = new Vector3(1, 1, 1);
			heroPickPortrait.transform.localPosition = new Vector3(0, 0, 0);
		}
		SortChildrenByName(heroPickPanel.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("AllHeroGrid").gameObject);
		heroPickPanel.SetActive(true);
		yield return new WaitUntil(() => heroPickPanel.activeInHierarchy);
		heroPickPanel.transform.Find("Scroll View").transform.Find("ScrollbarVerticalHeroes").GetComponent<Scrollbar>().value = 1;
		heroPickPanel.SetActive(false);
		allHeroNames.Sort();
		foreach (Dropdown dropdown in playerHeroDropdown)
		{
			dropdown.AddOptions(allHeroNames);
		}

		for (int i = 0; i < mapPageLinesFiltered.Count; i++)
		{
			allMaps.Add("");
		}

		for (int i = 0; i < mapPageLinesFiltered.Count; i++)
		{
			string[] tempMapString = mapPageLinesFiltered[i].Split('"');

			allMaps[i] = tempMapString[31].Replace("&#39;", "'");
		}

		mapNameDropdown.AddOptions(new List<string> { "All Maps" });
		mapNameDropdown.AddOptions(allMaps);

		StartCoroutine(GetSubRoleData());
	}

	public void SortChildrenByName(GameObject toSortChildren)
	{
		List<Transform> children = new List<Transform>();
		for (int i = toSortChildren.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = toSortChildren.transform.GetChild(i);
			children.Add(child);
			child.SetParent(null);
		}
		children.Sort((Transform t1, Transform t2) => {
			return t1.name.CompareTo(t2.name);
		});
		foreach (Transform child in children)
		{
			child.SetParent(toSortChildren.transform);
		}
	}

	/// <summary>
	/// Gets all hero sub roles from hotslogs.com.
	/// </summary>
	IEnumerator GetSubRoleData()
	{
		loadingTextMain.text = "Loading\n99%\n" + "All Map Data";

		WWW www = new WWW("http://www.hotslogs.com/Info/HeroSubRole");
		yield return www;
		subRolePage = www.text;

		subRolePageLines = subRolePage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		subRolePageLinesFiltered = new List<string>();

		foreach (string line in subRolePageLines)
		{
			if (line.Contains("<table><tr><th style="))
			{
				subRolePageLinesFiltered.Add(line);
			}
		}

		subRolePageLinesFilteredSplits = subRolePageLinesFiltered[0].Split('=');

		for (int i = 0; i < subRolePageLinesFilteredSplits.Length; i++)
		{
			subRoleGroups.Add(new SubRoleGroups());
			subRoleGroups[i].splits = subRolePageLinesFilteredSplits[i].Split('>');
		}

		for (int f = 0; f < subRoleGroups.Count; f++)
		{
			for (int h = 0; h < subRoleGroups[f].splits.Length; h++)
			{
				subRoleGroups[f].splits[h] = subRoleGroups[f].splits[h].Replace("</th", "").Replace("</td", "");

				for (int g = 0; g < allHeroDetails.Count; g++)
				{
					if (allHeroDetails[g].name == subRoleGroups[f].splits[h])
					{
						allHeroDetails[g].subRole = subRoleGroups[f].splits[1];
					}
				}
			}
		}

		StartCoroutine(GetSubRoleCompData(""));
		foreach (string mapName in allMaps)
		{
			StartCoroutine(GetSubRoleCompData(mapName));
		}

		StartCoroutine(GetHeroSynergyCounters());

		for (int i = 0; i < 10; i++)
		{
			SetPlayerHeroScores(i);
		}
	}

	/// <summary>
	/// Gets all hero sub role comp win rates from hotslogs.com.
	/// </summary>
	IEnumerator GetSubRoleCompData(string mapName)
	{
		WWW www = new WWW("http://www.hotslogs.com/Sitewide/TeamCompositions?Grouping=3&Map=" + mapName.Replace(" ", "%20").Replace("'", "%27"));
		yield return www;
		string subRoleCompPage = www.text;

		string[] subRoleCompPageLines = subRoleCompPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		List<string> subRoleCompPageLinesFiltered = new List<string>();

		foreach (string line in subRoleCompPageLines)
		{
			if (line.Contains("rgSorted") && !line.Contains("onclick="))
			{
				subRoleCompPageLinesFiltered.Add(line);
			}
		}

		WWW www2 = new WWW("http://www.hotslogs.com/Sitewide/TeamCompositions?Grouping=1&Map=" + mapName.Replace(" ", "%20").Replace("'", "%27"));
		yield return www2;
		string roleCompPage = www2.text;

		string[] roleCompPageLines = roleCompPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		List<string> roleCompPageLinesFiltered = new List<string>();

		foreach (string line in roleCompPageLines)
		{
			if (line.Contains(" alt=") && !line.Contains("Default"))
			{
				roleCompPageLinesFiltered.Add(line);
			}
		}

		//All Sub Roles On Map, add for each map
		SubRoleCompsOnMapDetails srcomdTemp = new SubRoleCompsOnMapDetails();
		allSubRolesOnMapDetails.Add(srcomdTemp);

		//All Roles On Map, add for each map
		RoleCompsOnMapDetails rcomdTemp = new RoleCompsOnMapDetails();
		allRolesOnMapDetails.Add(rcomdTemp);

		if (mapName == "")
		{
			srcomdTemp.mapName = "All Maps";
			rcomdTemp.mapName = "All Maps";
        }
		else
		{
			srcomdTemp.mapName = mapName;
			rcomdTemp.mapName = mapName;
		}

		srcomdTemp.splitValues = new List<string>();
		rcomdTemp.splitValues = new List<string>();

		srcomdTemp.comps = new List<SubRoleCompDetails>();
		rcomdTemp.comps = new List<RoleCompDetails>();

		for (int m = 0; m < allSubRoleNames.Count; m++)
		{
			SubRoleValue srvTemp = new SubRoleValue();
			srcomdTemp.subRoleValues.Add(srvTemp);
			srvTemp.subRoleName = allSubRoleNames[m];
		}

		for (int f = 0; f < subRoleCompPageLinesFiltered.Count; f++)
		{
			srcomdTemp.comps.Add(new SubRoleCompDetails());

			srcomdTemp.comps[f].splitValues = subRoleCompPageLinesFiltered[f].Split(new string[] { ">" }, StringSplitOptions.RemoveEmptyEntries);

			for (int g = 0; g < srcomdTemp.comps[f].splitValues.Length; g++)
			{
				if (srcomdTemp.comps[f].splitValues[g].Contains(" %</td"))
				{
					srcomdTemp.comps[f].winRate = float.Parse(srcomdTemp.comps[f].splitValues[g].Replace(" %</td", "")) / 100.0f;
					if (srcomdTemp.comps[f].winRate < srcomdTemp.lowestWinRate)
					{
						srcomdTemp.lowestWinRate = srcomdTemp.comps[f].winRate;
                    }
				}
			}

			srcomdTemp.comps[f].subRole = new List<string>();
			srcomdTemp.comps[f].subRole.Add(srcomdTemp.comps[f].splitValues[6].Replace("</span", ""));
			srcomdTemp.comps[f].subRole.Add(srcomdTemp.comps[f].splitValues[10].Replace("</span", ""));
			srcomdTemp.comps[f].subRole.Add(srcomdTemp.comps[f].splitValues[14].Replace("</span", ""));
			srcomdTemp.comps[f].subRole.Add(srcomdTemp.comps[f].splitValues[18].Replace("</span", ""));
			srcomdTemp.comps[f].subRole.Add(srcomdTemp.comps[f].splitValues[22].Replace("</span", ""));
			srcomdTemp.comps[f].subRole.Sort();
			srcomdTemp.comps[f].name = srcomdTemp.comps[f].subRole[0] + "-" + srcomdTemp.comps[f].subRole[1] + "-" + srcomdTemp.comps[f].subRole[2] + "-" + srcomdTemp.comps[f].subRole[3] + "-" + srcomdTemp.comps[f].subRole[4];

			for (int m = 0; m < allSubRoleNames.Count; m++)
			{
				srcomdTemp.comps[f].numOfEachSubRole.Add(0);

				for (int h = 0; h < srcomdTemp.comps[f].subRole.Count; h++)
				{
					if (srcomdTemp.comps[f].subRole[h] == allSubRoleNames[m])
					{
						srcomdTemp.comps[f].numOfEachSubRole[m]++;
					}
				}
			}
		}

		for (int f = 0; f < roleCompPageLinesFiltered.Count; f++)
		{
			rcomdTemp.comps.Add(new RoleCompDetails());

			rcomdTemp.comps[f].splitValues = roleCompPageLinesFiltered[f].Split(new string[] { ">", "title=\"" }, StringSplitOptions.RemoveEmptyEntries);

			for (int g = 0; g < rcomdTemp.comps[f].splitValues.Length; g++)
			{
				if (rcomdTemp.comps[f].splitValues[g].Contains(" %</td"))
				{
					rcomdTemp.comps[f].winRate = float.Parse(rcomdTemp.comps[f].splitValues[g].Replace(" %</td", "")) / 100.0f;

					if (rcomdTemp.comps[f].winRate > srcomdTemp.lowestWinRate)
					{
						rcomdTemp.comps[f].winRate = srcomdTemp.lowestWinRate;
                    }

					if (rcomdTemp.comps[f].winRate < rcomdTemp.lowestWinRate)
					{
						rcomdTemp.lowestWinRate = rcomdTemp.comps[f].winRate;
					}
				}
			}

			rcomdTemp.comps[f].role = new List<string>();
			rcomdTemp.comps[f].role.Add(rcomdTemp.comps[f].splitValues[6].Substring(0, rcomdTemp.comps[f].splitValues[6].IndexOf('\"')));
			rcomdTemp.comps[f].role.Add(rcomdTemp.comps[f].splitValues[10].Substring(0, rcomdTemp.comps[f].splitValues[10].IndexOf('\"')));
			rcomdTemp.comps[f].role.Add(rcomdTemp.comps[f].splitValues[14].Substring(0, rcomdTemp.comps[f].splitValues[14].IndexOf('\"')));
			rcomdTemp.comps[f].role.Add(rcomdTemp.comps[f].splitValues[18].Substring(0, rcomdTemp.comps[f].splitValues[18].IndexOf('\"')));
			rcomdTemp.comps[f].role.Add(rcomdTemp.comps[f].splitValues[22].Substring(0, rcomdTemp.comps[f].splitValues[22].IndexOf('\"')));
			rcomdTemp.comps[f].role.Sort();
			rcomdTemp.comps[f].name = rcomdTemp.comps[f].role[0] + "-" + rcomdTemp.comps[f].role[1] + "-" + rcomdTemp.comps[f].role[2] + "-" + rcomdTemp.comps[f].role[3] + "-" + rcomdTemp.comps[f].role[4];

			for (int m = 0; m < allRoleNames.Count; m++)
			{
				rcomdTemp.comps[f].numOfEachRole.Add(0);

				for (int h = 0; h < rcomdTemp.comps[f].role.Count; h++)
				{
					if (rcomdTemp.comps[f].role[h] == allRoleNames[m])
					{
						rcomdTemp.comps[f].numOfEachRole[m]++;
					}
				}
			}
		}

		RecommendedComp(match.friendlyTeam, 1);
		RecommendedComp(match.enemyTeam, 2);
		loadingInitialCounter++;

		if (loadingInitialCounter > allMaps.Count)
		{
			SubRoleValue();
			loadingPanel.SetActive(false);
		}
	}

	public void SubRoleValue()
	{
		for (int n = 0; n < allSubRolesOnMapDetails.Count; n++)
		{
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].scarcity = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcity = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1 = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2 = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRate = 0.0f;
				allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate = 0.0f;
				allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate = 0.0f;
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrencesWinPercent = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2WinPercent = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrences = 0;
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2 = 0;
			}
		}

		//occurrences of each sub role
		for (int n = 0; n < allSubRolesOnMapDetails.Count; n++)
		{
			for (int f = 0; f < allSubRolesOnMapDetails[n].comps.Count; f++)
			{
				for (int m = 0; m < allSubRoleNames.Count; m++)
				{
					int foundSubRole = 0;
					for (int h = 0; h < allSubRolesOnMapDetails[n].comps[f].subRole.Count; h++)
					{
						if (allSubRolesOnMapDetails[n].comps[f].subRole[h] == allSubRoleNames[m])
						{
							foundSubRole++;
						}
					}

					if (allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[0] >= match.friendlyTeam.numOfEachSubRole[0]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[1] >= match.friendlyTeam.numOfEachSubRole[1]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[2] >= match.friendlyTeam.numOfEachSubRole[2]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[3] >= match.friendlyTeam.numOfEachSubRole[3]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[4] >= match.friendlyTeam.numOfEachSubRole[4]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[5] >= match.friendlyTeam.numOfEachSubRole[5]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[6] >= match.friendlyTeam.numOfEachSubRole[6]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[7] >= match.friendlyTeam.numOfEachSubRole[7]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[8] >= match.friendlyTeam.numOfEachSubRole[8])
					{
						if (foundSubRole >= match.friendlyTeam.numOfEachSubRole[m] + 1)
						{
							for (int p = 0; p < allSubRolesOnMapDetails[n].subRoleValues.Count; p++)
							{
								if (allSubRolesOnMapDetails[n].subRoleValues[p].subRoleName == allSubRoleNames[m])
								{
									allSubRolesOnMapDetails[n].subRoleValues[p].occurrences++;
									allSubRolesOnMapDetails[n].subRoleValues[p].occurrencesWinPercent += allSubRolesOnMapDetails[n].comps[f].winRate * ((100.0f - (float)(f+1))/100.0f);
								}
							}
						}
					}

					if (allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[0] >= match.enemyTeam.numOfEachSubRole[0]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[1] >= match.enemyTeam.numOfEachSubRole[1]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[2] >= match.enemyTeam.numOfEachSubRole[2]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[3] >= match.enemyTeam.numOfEachSubRole[3]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[4] >= match.enemyTeam.numOfEachSubRole[4]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[5] >= match.enemyTeam.numOfEachSubRole[5]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[6] >= match.enemyTeam.numOfEachSubRole[6]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[7] >= match.enemyTeam.numOfEachSubRole[7]
					&& allSubRolesOnMapDetails[n].comps[f].numOfEachSubRole[8] >= match.enemyTeam.numOfEachSubRole[8])
					{
						if (foundSubRole >= match.enemyTeam.numOfEachSubRole[m] + 1)
						{
							for (int p = 0; p < allSubRolesOnMapDetails[n].subRoleValues.Count; p++)
							{
								if (allSubRolesOnMapDetails[n].subRoleValues[p].subRoleName == allSubRoleNames[m])
								{
									allSubRolesOnMapDetails[n].subRoleValues[p].occurrences2++;
									allSubRolesOnMapDetails[n].subRoleValues[p].occurrences2WinPercent += allSubRolesOnMapDetails[n].comps[f].winRate * ((100.0f - (float)(f + 1)) / 100.0f);
								}
							}
						}
					}
				}
			}
		}

		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			for (int j = 0; j < allSubRoleNames.Count; j++)
			{
				if (allHeroDetails[i].subRole == allSubRoleNames[j])
				{
					for (int n = 0; n < allSubRolesOnMapDetails.Count; n++)
					{
						for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
						{
							if (allSubRolesOnMapDetails[n].subRoleValues[m].subRoleName == allHeroDetails[i].subRole)
							{
								allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes++;

								if (!allHeroDetails[i].picked)
								{
									allSubRolesOnMapDetails[n].subRoleValues[m].scarcity++;

									if (allHeroDetails[i].onMap.Count > 0 && allSubRolesOnMapDetails[n].mapName != "All Maps")
									{
										for (int z = 0; z < allHeroDetails[i].onMap.Count; z++)
										{
											if (allHeroDetails[i].onMap[z].name == allSubRolesOnMapDetails[n].mapName)
											{
												if (allHeroDetails[i].onMap[z].winRate > allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRate) //Top hero win rate in sub role on map
												{
													allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateName = allHeroDetails[i].name;
													allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRate = allHeroDetails[i].onMap[z].winRate;
												}
											}
										}
									}
									else if (allHeroDetails[i].winRate > allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRate) //Top hero win rate in sub role
									{
										allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateName = allHeroDetails[i].name;
										allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRate = allHeroDetails[i].winRate;
									}

									allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate += allHeroDetails[i].winRate; //Average hero win rate in sub role
								}

								allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate += allHeroDetails[i].winRate;
							}
						}
					}
				}
			}
		}

		for (int n = 0; n < allSubRolesOnMapDetails.Count; n++)
		{
			//occurrence Rank
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrencesWinPercent = allSubRolesOnMapDetails[n].subRoleValues[m].occurrencesWinPercent / allSubRolesOnMapDetails[n].subRoleValues[m].occurrences;
            }
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.occurrencesWinPercent.CompareTo(s2.occurrencesWinPercent));
			allSubRolesOnMapDetails[n].subRoleValues.Reverse();
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].occurrencesWinPercent > 0.000001f)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank = m;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank = 9999;
				}
			}

			//occurrence Rank 2
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2WinPercent = allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2WinPercent / allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2;
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.occurrences2WinPercent.CompareTo(s2.occurrences2WinPercent));
			allSubRolesOnMapDetails[n].subRoleValues.Reverse();
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].occurrences2WinPercent > 0.000001f)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank2 = m;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank2 = 9999;
				}
			}

			//Scarcity Rank
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.scarcity.CompareTo(s2.scarcity));
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].scarcity > 0)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].scarcityRank = m;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].scarcityRank = 9999;
				}
			}

			//Top Hero Win Rate Rank
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.topHeroWinRate.CompareTo(s2.topHeroWinRate));
			allSubRolesOnMapDetails[n].subRoleValues.Reverse();
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateRank = m;
			}

			//Average Hero Win Rate Rank
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate = allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate / allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes;
				allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate = allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate / allSubRolesOnMapDetails[n].subRoleValues[m].scarcity;
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.averageHeroWinRate.CompareTo(s2.averageHeroWinRate));
			allSubRolesOnMapDetails[n].subRoleValues.Reverse();
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRateRank = m;
			}

			//Above average Scarcity
			for (int i = 0; i < allHeroDetails.Count; i++)
			{
				for (int j = 0; j < allSubRoleNames.Count; j++)
				{
					if (allHeroDetails[i].subRole == allSubRoleNames[j])
					{
						for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
						{
							if (allSubRolesOnMapDetails[n].subRoleValues[m].subRoleName == allHeroDetails[i].subRole)
							{
								if (!allHeroDetails[i].picked)
								{
									if (allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate < allHeroDetails[i].winRate)
									{
										allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcity++;
									}
								}

								if (allSubRolesOnMapDetails[n].subRoleValues[m].staticAverageHeroWinRate < allHeroDetails[i].winRate)
								{
									bool foundOnTeam1 = false;
									for (int p = 0; p < match.friendlyTeam.player.Length; p++)
									{
										if (allHeroDetails[i].name == match.friendlyTeam.player[p].chosenHeroName)
										{
											foundOnTeam1 = true;
										}
									}
									if (!foundOnTeam1)
									{
										allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2++;
									}

									bool foundOnTeam2 = false;
									for (int p = 0; p < match.enemyTeam.player.Length; p++)
									{
										if (allHeroDetails[i].name == match.enemyTeam.player[p].chosenHeroName)
										{
											foundOnTeam2 = true;
										}
									}
									if (!foundOnTeam2)
									{
										allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1++;
									}
								}
							}
						}
					}
				}
			}
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityPercent = ((float)allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcity / (float)allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes);
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityPercentTeam1 = ((float)allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1 / (float)allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes);
				allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityPercentTeam2 = ((float)allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2 / (float)allSubRolesOnMapDetails[n].subRoleValues[m].totalHeroes);
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.aboveAverageScarcityPercent.CompareTo(s2.aboveAverageScarcityPercent));
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcity > 0)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRank = m;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRank = allSubRoleNames.Count - 1;
				}
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.aboveAverageScarcityPercentTeam1.CompareTo(s2.aboveAverageScarcityPercentTeam1));
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1 == allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam1 = 0;
				}
				else if (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1 > 0)
				{
					int playersPickedOnTeam = 0;
					for (int k = 0; k < match.friendlyTeam.player.Length; k++)
					{
						if (match.friendlyTeam.player[k].chosenHeroName != "" && match.friendlyTeam.player[k].chosenHeroName != null)
						{
							playersPickedOnTeam++;
						}
					}

					if (playersPickedOnTeam > 0)
					{
						allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam1 = m * (1 - ((float)playersPickedOnTeam / 5.0f));
					}
					else
					{
						allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam1 = m;
                    }
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam1 = allSubRoleNames.Count - 1;
				}
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.aboveAverageScarcityPercentTeam2.CompareTo(s2.aboveAverageScarcityPercentTeam2));
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				if (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam1 == allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam2 = 0;
				}
				else if (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityTeam2 > 0)
				{
					int playersPickedOnTeam = 0;
					for (int k = 0; k < match.enemyTeam.player.Length; k++)
					{
						if (match.enemyTeam.player[k].chosenHeroName != "" && match.enemyTeam.player[k].chosenHeroName != null)
						{
							playersPickedOnTeam++;
						}
					}

					if (playersPickedOnTeam > 0)
					{
						allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam2 = m * (1 - ((float)playersPickedOnTeam / 5.0f));
					}
					else
					{
						allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam2 = m;
					}
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam2 = allSubRoleNames.Count - 1;
				}
			}

			//Overall Rank 1
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].overallRank =
					((allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateRank * (1.0f - allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate)) +
					//(allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateRank +
					//allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRank +
					//allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRateRank +
					(allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank * 1.65f)
					//allSubRolesOnMapDetails[n].subRoleValues[m].scarcityRank
					);
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.overallRank.CompareTo(s2.overallRank));

			//Overall Rank 2
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				allSubRolesOnMapDetails[n].subRoleValues[m].overallRank2 =
					((allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateRank * (1.0f - allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRate)) +
					//(allSubRolesOnMapDetails[n].subRoleValues[m].topHeroWinRateRank +
					//allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRank +
					//allSubRolesOnMapDetails[n].subRoleValues[m].averageHeroWinRateRank +
					(allSubRolesOnMapDetails[n].subRoleValues[m].occurrenceRank2 * 1.65f)
					//allSubRolesOnMapDetails[n].subRoleValues[m].scarcityRank
					);
			}
			allSubRolesOnMapDetails[n].subRoleValues.Sort((s1, s2) => s1.overallRank2.CompareTo(s2.overallRank2));

			//Team 1 Rank
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				int totalFound = 0;
				for (int t = 0; t < allSubRoleNames.Count; t++)
				{
					if (allSubRolesOnMapDetails[n].subRoleValues[m].subRoleName == allSubRoleNames[t])
					{
						for (int c = 0; c < allSubRolesOnMapDetails[n].comps.Count; c++)
						{
							for (int s = 0; s < allSubRolesOnMapDetails[n].comps[c].subRole.Count; s++)
							{
								if (allSubRolesOnMapDetails[n].comps[c].subRole[s] == allSubRoleNames[t])
								{
									if (allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[t] > match.friendlyTeam.numOfEachSubRole[t]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[0] >= match.friendlyTeam.numOfEachSubRole[0]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[1] >= match.friendlyTeam.numOfEachSubRole[1]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[2] >= match.friendlyTeam.numOfEachSubRole[2]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[3] >= match.friendlyTeam.numOfEachSubRole[3]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[4] >= match.friendlyTeam.numOfEachSubRole[4]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[5] >= match.friendlyTeam.numOfEachSubRole[5]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[6] >= match.friendlyTeam.numOfEachSubRole[6]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[7] >= match.friendlyTeam.numOfEachSubRole[7]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[8] >= match.friendlyTeam.numOfEachSubRole[8])
									{
										totalFound++;
									}
								}
							}
						}
					}
				}
				if (totalFound <= 0)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].team1Rank = allSubRolesOnMapDetails[n].subRoleValues[m].overallRank + 9999;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].team1Rank = allSubRolesOnMapDetails[n].subRoleValues[m].overallRank + (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam1 / 1.25f);
				}
			}

			//Team 2 Rank
			for (int m = 0; m < allSubRolesOnMapDetails[n].subRoleValues.Count; m++)
			{
				int totalFound = 0;
				for (int t = 0; t < allSubRoleNames.Count; t++)
				{
					if (allSubRolesOnMapDetails[n].subRoleValues[m].subRoleName == allSubRoleNames[t])
					{
						for (int c = 0; c < allSubRolesOnMapDetails[n].comps.Count; c++)
						{
							for (int s = 0; s < allSubRolesOnMapDetails[n].comps[c].subRole.Count; s++)
							{
								if (allSubRolesOnMapDetails[n].comps[c].subRole[s] == allSubRoleNames[t])
								{
									if (allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[t] > match.enemyTeam.numOfEachSubRole[t]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[0] >= match.enemyTeam.numOfEachSubRole[0]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[1] >= match.enemyTeam.numOfEachSubRole[1]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[2] >= match.enemyTeam.numOfEachSubRole[2]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[3] >= match.enemyTeam.numOfEachSubRole[3]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[4] >= match.enemyTeam.numOfEachSubRole[4]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[5] >= match.enemyTeam.numOfEachSubRole[5]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[6] >= match.enemyTeam.numOfEachSubRole[6]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[7] >= match.enemyTeam.numOfEachSubRole[7]
										&& allSubRolesOnMapDetails[n].comps[c].numOfEachSubRole[8] >= match.enemyTeam.numOfEachSubRole[8])
									{
										totalFound++;
									}
								}
							}
						}
					}
				}
				if (totalFound <= 0)
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].team2Rank = allSubRolesOnMapDetails[n].subRoleValues[m].overallRank2 + 9999;
				}
				else
				{
					allSubRolesOnMapDetails[n].subRoleValues[m].team2Rank = allSubRolesOnMapDetails[n].subRoleValues[m].overallRank2 + (allSubRolesOnMapDetails[n].subRoleValues[m].aboveAverageScarcityRankTeam2 / 1.25f);
				}
			}
		}

		GetGeneralHeroRecommendations();
	}

	public void GetGeneralHeroRecommendations()
	{
		int incrementPlayer = 0;
		int incrementSubRole = 0;
		int incrementPlayer2 = 0;
		int incrementSubRole2 = 0;
		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			for (int m = 0; m < allSubRolesOnMapDetails.Count; m++)
			{
				if (allSubRolesOnMapDetails[m].mapName == match.chosenMapName || (allSubRolesOnMapDetails[m].mapName == "All Maps" && match.chosenMapName == ""))
				{
					allSubRolesOnMapDetails[m].subRoleValues.Sort((s1, s2) => s1.team1Rank.CompareTo(s2.team1Rank));
					incrementPlayer = 0;
					incrementSubRole = 0;
					for (int n = 0; n < match.friendlyTeam.player.Length; n++)
					{
						if (match.friendlyTeam.player[n].chosenHeroName != "" && match.friendlyTeam.player[n].chosenHeroName != null)
						{
							incrementPlayer++;
						}
						if (allSubRolesOnMapDetails[m].subRoleValues[incrementSubRole - Mathf.Min(incrementSubRole, incrementPlayer)].topHeroWinRateName == allHeroDetails[i].name)
						{
							playerHeroRecommendations[(incrementSubRole * 2)].name = allHeroDetails[i].name;
							playerHeroRecommendations[(incrementSubRole * 2)].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<Image>().sprite = Sprite.Create(allHeroDetails[i].portrait, new Rect(0, 0, allHeroDetails[i].portrait.width, allHeroDetails[i].portrait.height), new Vector2(0.5f, 0.5f));
						}
						incrementSubRole++;
					}

					for (int y = 0; y < allSubRolesOnMapDetails[m].subRoleValues.Count; y++)
					{
						if (allHeroDetails[i].name == allSubRolesOnMapDetails[m].subRoleValues[y].topHeroWinRateName && currentlyPickingPlayer <= 4)
						{
							suggestedHeroOrder[y].SetActive(true);
							suggestedSubRoles[y].gameObject.SetActive(true);

							suggestedHeroOrder[y].name = allHeroDetails[i].name;
							suggestedHeroOrder[y].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<Image>().sprite = Sprite.Create(allHeroDetails[i].portrait, new Rect(0, 0, allHeroDetails[i].portrait.width, allHeroDetails[i].portrait.height), new Vector2(0.5f, 0.5f));

							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Tank")
							{
								suggestedSubRoles[y].sprite = tankSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Bruiser")
							{
								suggestedSubRoles[y].sprite = bruiserSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Healer")
							{
								suggestedSubRoles[y].sprite = healerSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Support")
							{
								suggestedSubRoles[y].sprite = supportSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Ambusher")
							{
								suggestedSubRoles[y].sprite = ambusherSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Burst Damage")
							{
								suggestedSubRoles[y].sprite = burstDamageSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Sustained Damage")
							{
								suggestedSubRoles[y].sprite = sustainedDamageSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Siege")
							{
								suggestedSubRoles[y].sprite = siegeSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Utility")
							{
								suggestedSubRoles[y].sprite = utilitySubRoleImage;
							}
						}
					}

					allSubRolesOnMapDetails[m].subRoleValues.Sort((s1, s2) => s1.team2Rank.CompareTo(s2.team2Rank));
					incrementPlayer2 = 0;
					incrementSubRole2 = 0;
					for (int n = 0; n < match.enemyTeam.player.Length; n++)
					{
						if (match.enemyTeam.player[n].chosenHeroName != "" && match.enemyTeam.player[n].chosenHeroName != null)
						{
							incrementPlayer2++;
						}
						if (allSubRolesOnMapDetails[m].subRoleValues[incrementSubRole2 - Mathf.Min(incrementSubRole2, incrementPlayer2)].topHeroWinRateName == allHeroDetails[i].name)
						{
							playerHeroRecommendations[(incrementSubRole2 * 2) + 10].name = allHeroDetails[i].name;
							playerHeroRecommendations[(incrementSubRole2 * 2) + 10].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<Image>().sprite = Sprite.Create(allHeroDetails[i].portrait, new Rect(0, 0, allHeroDetails[i].portrait.width, allHeroDetails[i].portrait.height), new Vector2(0.5f, 0.5f));
						}
						incrementSubRole2++;
					}

					for (int y = 0; y < allSubRolesOnMapDetails[m].subRoleValues.Count; y++)
					{
						if (allHeroDetails[i].name == allSubRolesOnMapDetails[m].subRoleValues[y].topHeroWinRateName && currentlyPickingPlayer > 4)
						{
							suggestedHeroOrder[y].SetActive(true);
							suggestedSubRoles[y].gameObject.SetActive(true);

							suggestedHeroOrder[y].name = allHeroDetails[i].name;
							suggestedHeroOrder[y].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<Image>().sprite = Sprite.Create(allHeroDetails[i].portrait, new Rect(0, 0, allHeroDetails[i].portrait.width, allHeroDetails[i].portrait.height), new Vector2(0.5f, 0.5f));

							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Tank")
							{
								suggestedSubRoles[y].sprite = tankSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Bruiser")
							{
								suggestedSubRoles[y].sprite = bruiserSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Healer")
							{
								suggestedSubRoles[y].sprite = healerSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Support")
							{
								suggestedSubRoles[y].sprite = supportSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Ambusher")
							{
								suggestedSubRoles[y].sprite = ambusherSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Burst Damage")
							{
								suggestedSubRoles[y].sprite = burstDamageSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Sustained Damage")
							{
								suggestedSubRoles[y].sprite = sustainedDamageSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Siege")
							{
								suggestedSubRoles[y].sprite = siegeSubRoleImage;
							}
							if (allSubRolesOnMapDetails[m].subRoleValues[y].subRoleName == "Utility")
							{
								suggestedSubRoles[y].sprite = utilitySubRoleImage;
							}
						}
					}
				}
			}
		}
	}

	public void PlayerSubRoleValue(Team team, int playerNum)
	{
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRate = 0.0f;
			team.player[playerNum].subRoleDetails[m].playerTopHeroScore = -5000;
			team.player[playerNum].subRoleDetails[m].playerTopHeroWinRate = 0.0f;
			team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayed = 0;
		}

		for (int i = 0; i < team.player[playerNum].heroDetails.Count; i++)
		{
			for (int j = 0; j < allSubRoleNames.Count; j++)
			{
				if (team.player[playerNum].heroDetails[i].subRoleName == allSubRoleNames[j])
				{
					for (int n = 0; n < team.player[playerNum].subRoleDetails.Count; n++)
					{
						if (team.player[playerNum].subRoleDetails[n].name == team.player[playerNum].heroDetails[i].subRoleName)
						{
							for (int p = 0; p < allHeroDetails.Count; p++)
							{
								if (allHeroDetails[p].name == team.player[playerNum].heroDetails[i].name && !allHeroDetails[p].picked)
								{
									for (int t = 0; t < team.player[playerNum].currentHeroScores.Count; t++) //Top hero score in sub role
									{
										if (team.player[playerNum].currentHeroScores[t].name == team.player[playerNum].heroDetails[i].name)
										{
											if (team.player[playerNum].currentHeroScores[t].score > team.player[playerNum].subRoleDetails[n].playerTopHeroScore)
											{
												team.player[playerNum].subRoleDetails[n].playerTopHeroScoreName = team.player[playerNum].heroDetails[i].name;
												team.player[playerNum].subRoleDetails[n].playerTopHeroScore = team.player[playerNum].currentHeroScores[t].score;
											}
										}
									}

									if (team.player[playerNum].heroDetails[i].winRate > allSubRolesOnMapDetails[n].subRoleValues[n].topHeroWinRate) //Top hero win rate in sub role
									{
										team.player[playerNum].subRoleDetails[n].playerTopHeroWinRateName = team.player[playerNum].heroDetails[i].name;
										team.player[playerNum].subRoleDetails[n].playerTopHeroWinRate = team.player[playerNum].heroDetails[i].winRate;
									}

									if (team.player[playerNum].heroDetails[i].gamesPlayed > team.player[playerNum].subRoleDetails[n].playerMostPlayedHeroGamesPlayed) //Top hero games played
									{
										team.player[playerNum].subRoleDetails[n].playerMostPlayedHeroGamesPlayedName = team.player[playerNum].heroDetails[i].name;
										team.player[playerNum].subRoleDetails[n].playerMostPlayedHeroGamesPlayed = team.player[playerNum].heroDetails[i].gamesPlayed;
									}

									team.player[playerNum].subRoleDetails[n].playerAverageHeroWinRate += team.player[playerNum].heroDetails[i].winRate; //Average hero win rate in sub role
								}
							}
						}
					}
				}
			}
		}

		//Player Top Hero Score Rank
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerTopHeroScore.CompareTo(s2.playerTopHeroScore));
		team.player[playerNum].subRoleDetails.Reverse();
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].playerTopHeroScoreName != "" && team.player[playerNum].subRoleDetails[m].playerTopHeroScoreName != null)
			{
				team.player[playerNum].subRoleDetails[m].playerTopHeroScoreRank = m;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerTopHeroScoreRank = allSubRoleNames.Count - 1;
			}
		}

		//Player Top Hero Win Rate Rank
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerTopHeroWinRate.CompareTo(s2.playerTopHeroWinRate));
		team.player[playerNum].subRoleDetails.Reverse();
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].playerTopHeroWinRateName != "" && team.player[playerNum].subRoleDetails[m].playerTopHeroWinRateName != null)
			{
				team.player[playerNum].subRoleDetails[m].playerTopHeroWinRateRank = m;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerTopHeroWinRateRank = allSubRoleNames.Count - 1;
			}
		}

		//Player Average Hero Win Rate Rank
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			team.player[playerNum].subRoleDetails[m].subRoleCount = 0;
			for (int i = 0; i < team.player[playerNum].heroDetails.Count; i++)
			{
				for (int p = 0; p < allHeroDetails.Count; p++)
				{
					if (allHeroDetails[p].name == team.player[playerNum].heroDetails[i].name && !allHeroDetails[p].picked)
					{
						if (team.player[playerNum].heroDetails[i].subRoleName == team.player[playerNum].subRoleDetails[m].name)
						{
							team.player[playerNum].subRoleDetails[m].subRoleCount++;
						}
					}
				}
			}
		}
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].subRoleCount > 0)
			{
				team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRate = team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRate / team.player[playerNum].subRoleDetails[m].subRoleCount;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRate = 0;
			}
		}
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerAverageHeroWinRate.CompareTo(s2.playerAverageHeroWinRate));
		team.player[playerNum].subRoleDetails.Reverse();
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRate >= 0.0001f)
			{
				team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRateRank = m;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRateRank = allSubRoleNames.Count - 1;
			}
		}

		//Player Sub Role Win Rate Rank
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerWinRate.CompareTo(s2.playerWinRate));
		team.player[playerNum].subRoleDetails.Reverse();
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].playerWinRate >= 0.0001f)
			{
				team.player[playerNum].subRoleDetails[m].playerWinRateRank = m;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerWinRateRank = allSubRoleNames.Count - 1;
			}
		}

		//Player Sub Role Top Games Played
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerMostPlayedHeroGamesPlayed.CompareTo(s2.playerMostPlayedHeroGamesPlayed));
		team.player[playerNum].subRoleDetails.Reverse();
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			if (team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayedName != "" && team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayedName != null)
			{
				team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayedRank = m;
			}
			else
			{
				team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayedRank = allSubRoleNames.Count - 1;
			}
		}

		//Player Average Sub Role Rank
		for (int m = 0; m < team.player[playerNum].subRoleDetails.Count; m++)
		{
			team.player[playerNum].subRoleDetails[m].playerOverallRank =
				(team.player[playerNum].subRoleDetails[m].playerTopHeroScoreRank +
				team.player[playerNum].subRoleDetails[m].playerTopHeroWinRateRank +
				team.player[playerNum].subRoleDetails[m].playerAverageHeroWinRateRank +
				team.player[playerNum].subRoleDetails[m].playerWinRateRank +
				team.player[playerNum].subRoleDetails[m].playerMostPlayedHeroGamesPlayedRank) / 5.0f;
		}
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.playerOverallRank.CompareTo(s2.playerOverallRank));

		//Combined Rank
		for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
		{
			if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName || (allSubRolesOnMapDetails[i].mapName == "All Maps" && match.chosenMapName == ""))
			{
				for (int m = 0; m < allSubRolesOnMapDetails[i].subRoleValues.Count; m++)
				{
					for (int n = 0; n < team.player[playerNum].subRoleDetails.Count; n++)
					{
						if (allSubRolesOnMapDetails[i].subRoleValues[m].subRoleName == team.player[playerNum].subRoleDetails[n].name)
						{
							team.player[playerNum].subRoleDetails[n].combinedRank =
								(allSubRolesOnMapDetails[i].subRoleValues[m].overallRank +
								team.player[playerNum].subRoleDetails[n].playerOverallRank) / 2.0f;
						}
					}
				}
			}
		}
		team.player[playerNum].subRoleDetails.Sort((s1, s2) => s1.combinedRank.CompareTo(s2.combinedRank));
	}

	/// <summary>
	/// Gets all hero synergies and counters from each individual hero page on hotslogs.com.
	/// </summary>
	IEnumerator GetHeroSynergyCounters()
	{
		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			string tempHeroName = allHeroDetails[i].name.Replace("'", "%27").Replace(" ", "%20");
			WWW www = new WWW("http://www.hotslogs.com/Sitewide/HeroDetails?Hero=" + tempHeroName);
			yield return www;
			string singleHeroPage = www.text;

			string[] singleHeroPageLines = singleHeroPage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> singleHeroPageSynergyLinesFiltered = new List<string>();
			List<string> singleHeroPageCounterLinesFiltered = new List<string>();
			List<string> singleHeroPageMapLinesFiltered = new List<string>();

			int startLine1 = 0;
			int endLine1 = 0;
			int startLine2 = 0;
			int endLine2 = 0;

			for (int j = 0; j < singleHeroPageLines.Length; j++)
			{
				if (singleHeroPageLines[j].Contains("Win Percent Against"))
				{
					startLine1 = j;
				}
				if (singleHeroPageLines[j].Contains("Win Percent With"))
				{
					endLine1 = j - 1;
					startLine2 = j;
				}
				if (singleHeroPageLines[j].Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Maps/") && !singleHeroPageLines[j].Contains("{0}.png"))
				{
					singleHeroPageMapLinesFiltered.Add(singleHeroPageLines[j]);
				}
			}
			endLine2 = singleHeroPageLines.Length - 1;

			allHeroDetails[i].onMap = new List<OnMap>();
			for (int j = 0; j < singleHeroPageMapLinesFiltered.Count; j++)
			{
				string[] tempMapString = singleHeroPageMapLinesFiltered[j].Split('>');

				allHeroDetails[i].onMap.Add(new OnMap());
				allHeroDetails[i].onMap[j].name = tempMapString[5].Replace("</a", "");
				allHeroDetails[i].onMap[j].winRate = float.Parse(tempMapString[10].Replace(" %</td", "")) / 100.0f;
			}

			for (int h = startLine1; h < endLine1; h++)
			{
				if (singleHeroPageLines[h].Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Heroes/Portraits/") && !singleHeroPageLines[h].Contains("$create"))
				{
					singleHeroPageCounterLinesFiltered.Add(singleHeroPageLines[h]);
				}
			}

			for (int g = startLine2; g < endLine2; g++)
			{
				if (singleHeroPageLines[g].Contains("//d1i1jxrdh2kvwy.cloudfront.net/Images/Heroes/Portraits/") && !singleHeroPageLines[g].Contains("$create"))
				{
					singleHeroPageSynergyLinesFiltered.Add(singleHeroPageLines[g]);
				}
			}

			//Debug.Log(allHeroDetails[i].name + " Synergy & Counters Finished Downloading");

			allHeroDetails[i].heroSynergy = new List<HeroSynergyCounterDetails>();
			for (int f = 0; f < singleHeroPageSynergyLinesFiltered.Count; f++)
			{
				allHeroDetails[i].heroSynergy.Add(new HeroSynergyCounterDetails());
				allHeroDetails[i].heroSynergy[f].splitValues = singleHeroPageSynergyLinesFiltered[f].Split('>');
				allHeroDetails[i].heroSynergy[f].name = allHeroDetails[i].heroSynergy[f].splitValues[5].Replace("</a", "");
				allHeroDetails[i].heroSynergy[f].winRateWithAgainst = float.Parse(allHeroDetails[i].heroSynergy[f].splitValues[10].Replace(" %</td", "")) / 100.0f;
				allHeroDetails[i].heroSynergy[f].estAdvantage = float.Parse(allHeroDetails[i].heroSynergy[f].splitValues[12].Replace(" %</td", "")) / 100.0f;
			}

			allHeroDetails[i].heroCounter = new List<HeroSynergyCounterDetails>();
			for (int f = 0; f < singleHeroPageCounterLinesFiltered.Count; f++)
			{
				allHeroDetails[i].heroCounter.Add(new HeroSynergyCounterDetails());
				allHeroDetails[i].heroCounter[f].splitValues = singleHeroPageCounterLinesFiltered[f].Split('>');
				allHeroDetails[i].heroCounter[f].name = allHeroDetails[i].heroCounter[f].splitValues[5].Replace("</a", "");
				allHeroDetails[i].heroCounter[f].winRateWithAgainst = float.Parse(allHeroDetails[i].heroCounter[f].splitValues[10].Replace(" %</td", "")) / 100.0f;
				allHeroDetails[i].heroCounter[f].estAdvantage = float.Parse(allHeroDetails[i].heroCounter[f].splitValues[12].Replace(" %</td", "")) / 100.0f;
			}

			SubRoleValue();
		}

		downloadFinished = true;
		loadingText2.text = "";

		CompForTeamNum(1);
		CompForTeamNum(2);

		if (downloadFinished)
		{
			SynergiesForTeamNum(1);
			SynergiesForTeamNum(2);
			CountersForTeamNum(1);
			CountersForTeamNum(2);
		}
	}

	/// <summary>
	/// Retrieves the stats for the chosen hero and the sub role for a specific player.
	/// </summary>
	public void GetChosenHeroStats(Player player)
	{
		bool foundPlayerHeroDetails = false;
		int totalHeroes = 0;
		float allHeroesWinRate = 0.0f;
		if (player.name != null && player.name != "" && player.heroDetails.Count > 0)
		{
			Debug.Log("Running Chosen Hero and Sub Role Stats for " + player.name);
			for (int i = 0; i < player.heroDetails.Count; i++)
			{
				if (player.heroDetails[i].level >= 5)
				{
					totalHeroes++;
					allHeroesWinRate += player.heroDetails[i].winRate;
				}
				else
				{
					for (int k = 0; k < allHeroDetails.Count; k++)
					{
						if (player.chosenHeroName == allHeroDetails[k].name)
						{
							totalHeroes++;
							allHeroesWinRate += allHeroDetails[k].winRate;
						}
					}
				}
				if (player.heroDetails[i].name == player.chosenHeroName)
				{
					for (int j = 0; j < player.subRoleDetails.Count; j++)
					{
						for (int k = 0; k < allHeroDetails.Count; k++)
						{
							if (player.subRoleDetails[j].name == allHeroDetails[k].subRole && player.chosenHeroName == allHeroDetails[k].name)
							{
								foundPlayerHeroDetails = true;
								player.chosenHeroRole = allHeroDetails[k].role;
								player.chosenHeroSubRole = allHeroDetails[k].subRole;
								player.gamesPlayedWithChosenHero = player.heroDetails[i].gamesPlayed;
								if (player.heroDetails[i].gamesPlayed > gamesPlayedThreshold) //only subtract or add experience bonus from their own win rate
								{
									player.chosenHeroWinRate = player.heroDetails[i].winRate - (Mathf.Lerp(0, maxExperiencePenalty, (Mathf.Max((gamesPlayedThreshold - player.heroDetails[i].gamesPlayed), 0) / (gamesPlayedThreshold * 1.0f))));
								}
								else //Use more of the player's own win rate the closer they are to the gamesPlayedThreshold but still subtract for low experience
								{
									float tempPlayerWinRate = (player.heroDetails[i].winRate) * player.heroDetails[i].gamesPlayed;
									float tempGlobalWinRate = allHeroDetails[k].winRate * (gamesPlayedThreshold - player.heroDetails[i].gamesPlayed);
									player.chosenHeroWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - (Mathf.Lerp(0, maxExperiencePenalty, (Mathf.Max((gamesPlayedThreshold - player.heroDetails[i].gamesPlayed), 0) / (gamesPlayedThreshold * 1.0f))));
								}
							}
						}
					}
				}
			}
		}

		if (!foundPlayerHeroDetails)
		{
			bool foundPlayerInAllHeroes = false;

			if (player.name != null && player.name != "")
			{
				Debug.Log("Chosen Hero Stats Not Found for " + player.name + ". Using public average for Chosen Hero.");
			}
			else
			{
				Debug.Log("Chosen Hero Stats Not Found for Anonymous Player. Using public average for Chosen Hero.");
			}

			for (int i = 0; i < allHeroDetails.Count; i++)
			{
				if (allHeroDetails[i].name == player.chosenHeroName)
				{
					foundPlayerInAllHeroes = true;
					player.chosenHeroRole = allHeroDetails[i].role;
					player.chosenHeroSubRole = allHeroDetails[i].subRole;
					player.gamesPlayedWithChosenHero = 0;
					if (player.foundOnHotslogs)
					{
						player.chosenHeroWinRate = allHeroDetails[i].winRate - maxExperiencePenalty;
					}
					else
					{
						player.chosenHeroWinRate = allHeroDetails[i].winRate;
					}
				}
			}

			if (!foundPlayerInAllHeroes)
			{
				if (totalHeroes > 0 && (player.chosenHeroName == "" || player.chosenHeroName == null))
				{
					player.chosenHeroWinRate = allHeroesWinRate / totalHeroes;
				}
				else
				{
					player.chosenHeroWinRate = 0.5f;
				}
				player.chosenHeroOnMapWinRate = 0.0f;
				player.chosenHeroRole = "";
				player.chosenHeroSubRole = "";
				player.gamesPlayedWithChosenHero = 0;
			}
		}

		GetChosenHeroOnMap(player);
		CompForTeamNum(1);
		CompForTeamNum(2);

		if (downloadFinished)
		{
			SynergiesForTeamNum(1);
			SynergiesForTeamNum(2);
			CountersForTeamNum(1);
			CountersForTeamNum(2);
		}
	}

	/// <summary>
	/// Retrieves the score for sting heroName for a specific player.
	/// </summary>
	public int GetDropdownHeroScore(Player player, string heroName)
	{
		float thisHeroWinRate = 0.0f;
		float thisHeroOnMapWinRate = 0.0f;
		bool foundPlayerHeroDetails = false;
		int totalHeroes = 0;
		float allHeroesWinRate = 0.0f;
		if (player.name != null && player.name != "" && player.heroDetails.Count > 0)
		{
			for (int i = 0; i < player.heroDetails.Count; i++)
			{
				if (player.heroDetails[i].level >= 5)
				{
					totalHeroes++;
					allHeroesWinRate += player.heroDetails[i].winRate;
				}
				else
				{
					for (int k = 0; k < allHeroDetails.Count; k++)
					{
						if (heroName == allHeroDetails[k].name)
						{
							totalHeroes++;
							allHeroesWinRate += allHeroDetails[k].winRate;
						}
					}
				}
				if (player.heroDetails[i].name == heroName)
				{
					for (int j = 0; j < player.subRoleDetails.Count; j++)
					{
						for (int k = 0; k < allHeroDetails.Count; k++)
						{
							if (heroName == allHeroDetails[k].name)
							{
								foundPlayerHeroDetails = true;
								if (player.heroDetails[i].gamesPlayed > gamesPlayedThreshold) //only subtract or add experience bonus from their own win rate
								{
									thisHeroWinRate = player.heroDetails[i].winRate;
								}
								else //Use more of the player's own win rate the closer they are to the gamesPlayedThreshold but still subtract for low experience
								{
									float tempPlayerWinRate = (player.heroDetails[i].winRate) * player.heroDetails[i].gamesPlayed;
									float tempGlobalWinRate = allHeroDetails[k].winRate * (gamesPlayedThreshold - player.heroDetails[i].gamesPlayed);
									thisHeroWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - (Mathf.Lerp(0, maxExperiencePenalty, (Mathf.Max((gamesPlayedThreshold - player.heroDetails[i].gamesPlayed), 0) / (gamesPlayedThreshold * 1.0f))));
								}
							}
						}
					}
				}
			}
		}

		if (!foundPlayerHeroDetails)
		{
			bool foundPlayerInAllHeroes = false;

			for (int i = 0; i < allHeroDetails.Count; i++)
			{
				if (allHeroDetails[i].name == heroName)
				{
					foundPlayerInAllHeroes = true;
					if (player.foundOnHotslogs)
					{
						thisHeroWinRate = allHeroDetails[i].winRate - maxExperiencePenalty;
					}
					else
					{
						thisHeroWinRate = allHeroDetails[i].winRate;
					}
				}
			}

			if (!foundPlayerInAllHeroes)
			{
				if (totalHeroes > 0 && (heroName == "" || heroName == null))
				{
					thisHeroWinRate = allHeroesWinRate / totalHeroes;
				}
				else
				{
					thisHeroWinRate = 0.0f;
				}
				player.chosenHeroOnMapWinRate = 0.0f;
			}
		}

		//Hero On Map
		bool foundMapName = false;
		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			if (allHeroDetails[i].name == heroName)
			{
				if (allHeroDetails[i].onMap.Count > 0)
				{
					for (int l = 0; l < allHeroDetails[i].onMap.Count; l++)
					{
						if (allHeroDetails[i].onMap[l].name == match.chosenMapName)
						{
							foundMapName = true;
							thisHeroOnMapWinRate = allHeroDetails[i].onMap[l].winRate - allHeroDetails[i].winRate;
						}
					}
				}
				if (!foundMapName)
				{
					thisHeroOnMapWinRate = 0.0f;
				}
			}
		}

		int thisHeroScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, thisHeroWinRate, playerHeroWeight) * 1000.0f - 500.0f));
		int thisMapScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, player.chosenMapWinRate, playerMapWeight) * 1000.0f - 500.0f));
		int thisHeroOnMapScore = (int)(Mathf.Round(Mathf.Lerp(0.0f, thisHeroOnMapWinRate, heroMapWeight) * 1000.0f));
		return thisHeroScore + thisMapScore + thisHeroOnMapScore;
	}

	/// <summary>
	/// Retrieves the general hero stats for the chosen map for a specific player.
	/// </summary>
	public void GetChosenHeroOnMap(Player player)
	{
		bool foundMapName = false;
		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			if (allHeroDetails[i].name == player.chosenHeroName)
			{
				if (allHeroDetails[i].onMap.Count > 0)
				{
					for (int l = 0; l < allHeroDetails[i].onMap.Count; l++)
					{
						if (allHeroDetails[i].onMap[l].name == match.chosenMapName)
						{
							foundMapName = true;
							player.chosenHeroOnMapWinRate = allHeroDetails[i].onMap[l].winRate - allHeroDetails[i].winRate;
						}
					}
				}
				if (!foundMapName)
				{
					player.chosenHeroOnMapWinRate = 0.0f;
				}
			}
		}
	}

	/// <summary>
	/// Retrieves the stats for the chosen map for a specific player.
	/// </summary>
	public void GetChosenMapStats(Player player)
	{
		bool foundMapName = false;
		float allMapsWinRate = 0.0f;
		int totalMaps = 0;
		if (player.name != null && player.name != "")
		{
			for (int i = 0; i < player.mapDetails.Count; i++)
			{
				if (player.mapDetails[i].gamesPlayed >= gamesPlayedThreshold)
				{
					allMapsWinRate += player.mapDetails[i].winRate;
					totalMaps++;
				}
				if (player.mapDetails[i].name == match.chosenMapName)
				{
					if (player.mapDetails[i].gamesPlayed > gamesPlayedThreshold) //only subtract or add experience bonus from their own win rate
					{
						player.chosenMapWinRate = player.mapDetails[i].winRate;
						//player.chosenMapWinRate = player.mapDetails[i].winRate - (Mathf.Max((gamesPlayedThreshold - player.mapDetails[i].gamesPlayed), 0) / (gamesPlayedThreshold * 20));
					}
					else //Use more of the player's own win rate the closer they are to the gamesPlayedThreshold but still subtract for low experience
					{
						float tempPlayerWinRate = (player.mapDetails[i].winRate) * player.mapDetails[i].gamesPlayed;
						float tempGlobalWinRate = 0.5f * (gamesPlayedThreshold - player.mapDetails[i].gamesPlayed);
						player.chosenMapWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - ((((float)gamesPlayedThreshold - (float)player.mapDetails[i].gamesPlayed) / (float)gamesPlayedThreshold) * maxExperiencePenalty);
						//player.chosenMapWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - (Mathf.Max((gamesPlayedThreshold - player.mapDetails[i].gamesPlayed)) / (gamesPlayedThreshold * 20));
					}
					foundMapName = true;
				}
			}
		}
		else
		{
			player.chosenMapWinRate = 0.5f;
		}

		if (!foundMapName && totalMaps != 0)
		{
			player.chosenMapWinRate = allMapsWinRate / totalMaps;
		}
		else if (!foundMapName)
		{
			if (player.foundOnHotslogs)
			{
				player.chosenMapWinRate = 0.5f - maxExperiencePenalty;
			}
			else
			{
				player.chosenMapWinRate = 0.5f;
			}
		}
	}

	public void AverageHeroesForTeamNum(int num)
	{
		if (num == 1)
		{
			AverageHeroesForTeam(match.friendlyTeam);
		}
		else if (num == 2)
		{
			AverageHeroesForTeam(match.enemyTeam);
		}
	}

	public void AverageHeroesForTeam(Team team)
	{
		float tempAdd = 0.0f;
		foreach (Player player in team.player)
		{
			tempAdd += player.chosenHeroWinRate;
		}

		team.withHeroes.winRate = tempAdd / team.player.Length;
	}

	public void AverageOnMapForTeamNum(int num)
	{
		if (num == 1)
		{
			AverageOnMapForTeam(match.friendlyTeam);
		}
		else if (num == 2)
		{
			AverageOnMapForTeam(match.enemyTeam);
		}
	}

	public void AverageOnMapForTeam(Team team)
	{
		float tempAdd = 0.0f;
		foreach (Player player in team.player)
		{
			tempAdd += player.chosenMapWinRate;
		}

		team.onMap.winRate = tempAdd / team.player.Length;
	}

	public void CompForTeamNum(int num)
	{
		if (num == 1)
		{
			CompForTeam(match.friendlyTeam, 1);
		}
		else if (num == 2)
		{
			CompForTeam(match.enemyTeam, 2);
		}
	}

	public void CompForTeam(Team team, int teamNum)
	{
		team.subRoleCompOnMap.fullTeamComp = new List<string>();
		team.roleCompOnMap.fullTeamComp = new List<string>();
		for (int i = 0; i < team.player.Length; i++)
		{
			team.subRoleCompOnMap.fullTeamComp.Add(team.player[i].chosenHeroSubRole);
			team.subRoleCompOnMap.fullTeamComp.Sort();

			team.roleCompOnMap.fullTeamComp.Add(team.player[i].chosenHeroRole);
			team.roleCompOnMap.fullTeamComp.Sort();
		}



		//Regular role comp
		bool foundMatchingBase = false;
		for (int i = 0; i < allRolesOnMapDetails.Count; i++)
		{
			if (match.chosenMapName != "")
			{
				if (allRolesOnMapDetails[i].mapName == match.chosenMapName)
				{
					for (int j = 0; j < allRolesOnMapDetails[i].comps.Count; j++)
					{
						if (team.roleCompOnMap.fullTeamComp.SequenceEqual(allRolesOnMapDetails[i].comps[j].role))
						{
							team.roleCompOnMap.winRate = allRolesOnMapDetails[i].comps[j].winRate;
							foundMatchingBase = true;
							Debug.Log("Found Matching Base For Chosen Map");
						}
					}
				}
			}
			else
			{
				if (allRolesOnMapDetails[i].mapName == "All Maps")
				{
					for (int j = 0; j < allRolesOnMapDetails[i].comps.Count; j++)
					{
						if (team.roleCompOnMap.fullTeamComp.SequenceEqual(allRolesOnMapDetails[i].comps[j].role))
						{
							team.roleCompOnMap.winRate = allRolesOnMapDetails[i].comps[j].winRate;
							foundMatchingBase = true;
							Debug.Log("Found Matching Base For All Maps");
						}
					}
				}
			}
		}
		if (!foundMatchingBase && teamNum == 1)
		{
			foundBaseComp1 = false;
		}
		else if(foundMatchingBase && teamNum == 1)
		{
			foundBaseComp1 = true;
		}
		if (!foundMatchingBase && teamNum == 2)
		{
			foundBaseComp2 = false;
		}
		else if (foundMatchingBase && teamNum == 2)
		{
			foundBaseComp2 = true;
		}

		//Sub role comp
		bool foundMatchingComp = false;
		for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
		{
			if (match.chosenMapName != "")
			{
				if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName)
				{
					for (int j = 0; j < allSubRolesOnMapDetails[i].comps.Count; j++)
					{
						if (team.subRoleCompOnMap.fullTeamComp.SequenceEqual(allSubRolesOnMapDetails[i].comps[j].subRole))
						{
							team.subRoleCompOnMap.winRate = allSubRolesOnMapDetails[i].comps[j].winRate;
							foundMatchingComp = true;
							Debug.Log("Found Matching Comp For Chosen Map");
						}
					}
				}
			}
			else
			{
				if (allSubRolesOnMapDetails[i].mapName == "All Maps")
				{
					for (int j = 0; j < allSubRolesOnMapDetails[i].comps.Count; j++)
					{
						if (team.subRoleCompOnMap.fullTeamComp.SequenceEqual(allSubRolesOnMapDetails[i].comps[j].subRole))
						{
							team.subRoleCompOnMap.winRate = allSubRolesOnMapDetails[i].comps[j].winRate;
							foundMatchingComp = true;
							Debug.Log("Found Matching Comp For All Maps");
						}
					}
				}
			}
		}
		if (!foundMatchingComp && teamNum == 1)
		{
			foundSubRoleComp1 = false;
		}
		else if(foundMatchingComp && teamNum == 1)
		{
			foundSubRoleComp1 = true;
		}
		if (!foundMatchingComp && teamNum == 2)
		{
			foundSubRoleComp2 = false;
		}
		else if (foundMatchingComp && teamNum == 2)
		{
			foundSubRoleComp2 = true;
		}

		if (teamNum == 1)
		{
			RecommendedComp(team, 1);
		}
		if (teamNum == 2)
		{
			RecommendedComp(team, 2);
		}
	}

	public void RecommendedComp(Team team, int teamNum)
	{
		team.numOfEachSubRole.Clear();
		for (int m = 0; m < allSubRoleNames.Count; m++)
		{
			team.numOfEachSubRole.Add(0);

			for (int h = 0; h < team.subRoleCompOnMap.fullTeamComp.Count; h++)
			{
				if (team.subRoleCompOnMap.fullTeamComp[h] == allSubRoleNames[m])
				{
					team.numOfEachSubRole[m]++;
				}
			}
		}

		team.numOfEachRole.Clear();
		for (int m = 0; m < allRoleNames.Count; m++)
		{
			team.numOfEachRole.Add(0);

			for (int h = 0; h < team.roleCompOnMap.fullTeamComp.Count; h++)
			{
				if (team.roleCompOnMap.fullTeamComp[h] == allRoleNames[m])
				{
					team.numOfEachRole[m]++;
				}
			}
		}

		int teamCompRank = 0;
		team.subRoleCompOnMap.winRate = 0.0f;
		for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
		{
			if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName || (allSubRolesOnMapDetails[i].mapName == "All Maps" && match.chosenMapName == ""))
			{
				int findThree = 0;
				for (int j = 0; j < allSubRolesOnMapDetails[i].comps.Count; j++)
				{
					if (allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[0] >= team.numOfEachSubRole[0]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[1] >= team.numOfEachSubRole[1]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[2] >= team.numOfEachSubRole[2]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[3] >= team.numOfEachSubRole[3]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[4] >= team.numOfEachSubRole[4]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[5] >= team.numOfEachSubRole[5]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[6] >= team.numOfEachSubRole[6]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[7] >= team.numOfEachSubRole[7]
						&& allSubRolesOnMapDetails[i].comps[j].numOfEachSubRole[8] >= team.numOfEachSubRole[8])
					{
						teamCompRank++;
						team.subRoleCompOnMap.winRate += allSubRolesOnMapDetails[i].comps[j].winRate;

						if (teamNum == 1)
						{
							allSubRolesOnMapDetails[i].comps[j].rankTeam1 = teamCompRank;

							if (teamCompRank == 1)
							{
								findThree++;
								recommendedCompWinRateText[0].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation1, true);
							}
							if (teamCompRank == 2)
							{
								findThree++;
								recommendedCompWinRateText[1].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation2, true);
							}
							if (teamCompRank == 3)
							{
								findThree++;
								recommendedCompWinRateText[2].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation3, true);
							}
						}
						else if (teamNum == 2)
						{
							allSubRolesOnMapDetails[i].comps[j].rankTeam2 = teamCompRank;

							if (teamCompRank == 1)
							{
								findThree++;
								recommendedCompWinRateText[3].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation4, true);
							}
							if (teamCompRank == 2)
							{
								findThree++;
								recommendedCompWinRateText[4].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation5, true);
							}
							if (teamCompRank == 3)
							{
								findThree++;
								recommendedCompWinRateText[5].text = (allSubRolesOnMapDetails[i].comps[j].winRate * 100).ToString("F1") + "%";
								MatchSubCompImage(allSubRolesOnMapDetails[i].comps[j].subRole, subCompRecommendation6, true);
							}
						}
					}
					else
					{
						if (teamNum == 1)
						{
							allSubRolesOnMapDetails[i].comps[j].rankTeam1 = 0;
						}
						if (teamNum == 2)
						{
							allSubRolesOnMapDetails[i].comps[j].rankTeam2 = 0;
						}
					}
				}
				if (teamNum == 1)
				{
					if (findThree <= 0)
					{
						recommendedCompWinRateText[0].text = "NA";
						MatchSubCompImage(null, subCompRecommendation1, false);
						recommendedCompWinRateText[1].text = "NA";
						MatchSubCompImage(null, subCompRecommendation2, false);
						recommendedCompWinRateText[2].text = "NA";
						MatchSubCompImage(null, subCompRecommendation3, false);
					}
					if (findThree == 1)
					{
						recommendedCompWinRateText[1].text = "NA";
						MatchSubCompImage(null, subCompRecommendation2, false);
						recommendedCompWinRateText[2].text = "NA";
						MatchSubCompImage(null, subCompRecommendation3, false);
					}
					if (findThree == 2)
					{
						recommendedCompWinRateText[2].text = "NA";
						MatchSubCompImage(null, subCompRecommendation3, false);
					}
				}
				else if (teamNum == 2)
				{
					if (findThree <= 0)
					{
						recommendedCompWinRateText[3].text = "NA";
						MatchSubCompImage(null, subCompRecommendation4, false);
						recommendedCompWinRateText[4].text = "NA";
						MatchSubCompImage(null, subCompRecommendation5, false);
						recommendedCompWinRateText[5].text = "NA";
						MatchSubCompImage(null, subCompRecommendation6, false);
					}
					if (findThree == 1)
					{
						recommendedCompWinRateText[4].text = "NA";
						MatchSubCompImage(null, subCompRecommendation5, false);
						recommendedCompWinRateText[5].text = "NA";
						MatchSubCompImage(null, subCompRecommendation6, false);
					}
					if (findThree == 2)
					{
						recommendedCompWinRateText[5].text = "NA";
						MatchSubCompImage(null, subCompRecommendation6, false);
					}
				}
			}
		}

		if (teamCompRank > 0)
		{
			int tempNumPlayers = 0;
			for (int j = 0; j < team.player.Length; j++)
			{
				if (team.player[j].chosenHeroName == "" || team.player[j].chosenHeroName == null)
				{
					tempNumPlayers++;
				}
			}
			if (tempNumPlayers >= 5)
			{
				team.subRoleCompOnMap.winRate = 0.5f;
			}
			else
			{
				team.subRoleCompOnMap.winRate = (team.subRoleCompOnMap.winRate + (0.5f * (teamCompRank / (5.0f - tempNumPlayers)))) / (teamCompRank + (teamCompRank / (5.0f - tempNumPlayers)));
			}
		}
		else
		{
			int tempNumPlayers = 0;
			for (int j = 0; j < team.player.Length; j++)
			{
				if (team.player[j].chosenHeroName == "" || team.player[j].chosenHeroName == null)
				{
					tempNumPlayers++;
				}
			}
			if (tempNumPlayers > 0)
			{
				for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
				{
					if (match.chosenMapName != "")
					{
						if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName)
						{
							team.subRoleCompOnMap.winRate = allSubRolesOnMapDetails[i].lowestWinRate;
						}
					}
					else
					{
						if (allSubRolesOnMapDetails[i].mapName == "All Maps")
						{
							team.subRoleCompOnMap.winRate = allSubRolesOnMapDetails[i].lowestWinRate;
						}
					}
				}
			}
			else
			{
				if (foundBaseComp1 && teamNum == 1)
				{
					team.subRoleCompOnMap.winRate = team.roleCompOnMap.winRate;
				}
				else if (!foundBaseComp1 && teamNum == 1)
				{
					for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
					{
						if (match.chosenMapName != "")
						{
							if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName)
							{
								team.subRoleCompOnMap.winRate = allRolesOnMapDetails[i].lowestWinRate;
							}
						}
						else
						{
							if (allSubRolesOnMapDetails[i].mapName == "All Maps")
							{
								team.subRoleCompOnMap.winRate = allRolesOnMapDetails[i].lowestWinRate;
							}
						}
					}
				}

				if (foundBaseComp2 && teamNum == 2)
				{
					team.subRoleCompOnMap.winRate = team.roleCompOnMap.winRate;
				}
				else if (!foundBaseComp2 && teamNum == 2)
				{
					for (int i = 0; i < allSubRolesOnMapDetails.Count; i++)
					{
						if (match.chosenMapName != "")
						{
							if (allSubRolesOnMapDetails[i].mapName == match.chosenMapName)
							{
								team.subRoleCompOnMap.winRate = allRolesOnMapDetails[i].lowestWinRate;
							}
						}
						else
						{
							if (allSubRolesOnMapDetails[i].mapName == "All Maps")
							{
								team.subRoleCompOnMap.winRate = allRolesOnMapDetails[i].lowestWinRate;
							}
						}
					}
				}
			}
		}
	}

	public void MatchSubCompImage(List<string> subComp, Image[] image, bool match)
	{
		if (match)
		{
			for (int l = 0; l < subComp.Count; l++)
			{
				if (subComp[l] == "Tank")
				{
					image[l].sprite = tankSubRoleImage;
				}
				if (subComp[l] == "Bruiser")
				{
					image[l].sprite = bruiserSubRoleImage;
				}
				if (subComp[l] == "Healer")
				{
					image[l].sprite = healerSubRoleImage;
				}
				if (subComp[l] == "Support")
				{
					image[l].sprite = supportSubRoleImage;
				}
				if (subComp[l] == "Ambusher")
				{
					image[l].sprite = ambusherSubRoleImage;
				}
				if (subComp[l] == "Burst Damage")
				{
					image[l].sprite = burstDamageSubRoleImage;
				}
				if (subComp[l] == "Sustained Damage")
				{
					image[l].sprite = sustainedDamageSubRoleImage;
				}
				if (subComp[l] == "Siege")
				{
					image[l].sprite = siegeSubRoleImage;
				}
				if (subComp[l] == "Utility")
				{
					image[l].sprite = utilitySubRoleImage;
				}
			}
		}
		else
		{
			for (int l = 0; l < image.Length; l++)
			{
				image[l].sprite = genericSubRoleImage;
			}
		}
	}

	public void SynergiesForTeamNum(int num)
	{
		if (num == 1)
		{
			SynergiesForTeam(match.friendlyTeam);
		}
		else if (num == 2)
		{
			SynergiesForTeam(match.enemyTeam);
		}
	}

	public void SynergiesForTeam(Team team)
	{
		float tempSynergy = 0.0f;
		float tempAdv = 0.0f;
		int tempTotal = 0;
		bool foundSynergy = false;
		for (int i = 0; i < team.player.Length; i++)
		{
			for (int j = 0; j < team.player.Length; j++)
			{
				if (team.player[i].chosenHeroName != team.player[j].chosenHeroName)
				{
					if (team.player[i].chosenHeroName != null && team.player[i].chosenHeroName != "")
					{
						for (int k = 0; k < allHeroDetails.Count; k++)
						{
							if (team.player[i].chosenHeroName == allHeroDetails[k].name)
							{
								for (int l = 0; l < allHeroDetails[k].heroSynergy.Count; l++)
								{
									if (team.player[j].chosenHeroName == allHeroDetails[k].heroSynergy[l].name)
									{
										foundSynergy = true;
										tempSynergy += allHeroDetails[k].heroSynergy[l].winRateWithAgainst;
										tempAdv += allHeroDetails[k].heroSynergy[l].estAdvantage;
										tempTotal++;
									}
								}
							}
						}
					}
					else
					{
						tempSynergy += 0.5f;
						tempTotal++;
					}
				}
			}
		}

		if (foundSynergy && tempTotal != 0.0f)
		{
			team.heroSynergies.winRate = tempSynergy / tempTotal;
			team.heroSynergies.advRate = (tempAdv / tempTotal) + 0.5f;
		}
		else
		{
			team.heroSynergies.winRate = 0.5f;
			team.heroSynergies.advRate = 0.5f;
		}
	}

	public void CountersForTeamNum(int num)
	{
		if (num == 1)
		{
			CountersForTeam(match.friendlyTeam, match.enemyTeam);
		}
		else if (num == 2)
		{
			CountersForTeam(match.enemyTeam, match.friendlyTeam);
		}
	}

	public void CountersForTeam(Team team, Team team2)
	{
		float tempCounter = 0.0f;
		float tempAdv = 0.0f;
		bool foundCounters = false;
		int tempTotal = 0;
		for (int i = 0; i < team.player.Length; i++)
		{
			for (int j = 0; j < team2.player.Length; j++)
			{
				if (team.player[i].chosenHeroName != team2.player[j].chosenHeroName)
				{
					if (team.player[i].chosenHeroName != null && team.player[i].chosenHeroName != "" && team2.player[j].chosenHeroName != null && team2.player[j].chosenHeroName != "")
					{
						for (int k = 0; k < allHeroDetails.Count; k++)
						{
							if (team.player[i].chosenHeroName == allHeroDetails[k].name)
							{
								for (int l = 0; l < allHeroDetails[k].heroCounter.Count; l++)
								{
									if (team2.player[j].chosenHeroName == allHeroDetails[k].heroCounter[l].name)
									{
										foundCounters = true;
										tempCounter += allHeroDetails[k].heroCounter[l].winRateWithAgainst;
										tempAdv += allHeroDetails[k].heroSynergy[l].estAdvantage;
										tempTotal++;
									}
								}
							}
						}
					}
					else
					{
						tempCounter += 0.5f;
						tempTotal++;
					}
				}
			}
		}

		if (foundCounters && tempTotal != 0.0f)
		{
			team.heroCounters.winRate = tempCounter / tempTotal;
			team.heroCounters.advRate = (tempAdv / tempTotal) + 0.5f;
		}
		else
		{
			team.heroCounters.winRate = 0.5f;
			team.heroCounters.advRate = 0.5f;
		}
	}

	bool IsDigitsOnly(string str)
	{
		foreach (char c in str)
		{
			if (c < '0' || c > '9')
				return false;
		}

		return true;
	}

	public void UpdateMapName()
	{
		if (mapNameDropdown.options[mapNameDropdown.value].text == "All Maps")
		{
			match.chosenMapName = "";
		}
		else
		{
			match.chosenMapName = mapNameDropdown.options[mapNameDropdown.value].text;
		}

		foreach (Player player in match.friendlyTeam.player)
		{
			GetChosenMapStats(player);
			GetChosenHeroOnMap(player);
		}
		foreach (Player player in match.enemyTeam.player)
		{
			GetChosenMapStats(player);
			GetChosenHeroOnMap(player);
		}

		SubRoleValue();
		for (int i = 0; i < 10; i++)
		{
			SetPlayerHeroScores(i);
			if (i <= 4)
			{
				PlayerSubRoleValue(match.friendlyTeam, i);
			}
			else
			{
				PlayerSubRoleValue(match.enemyTeam, i - 5);
			}
		}

		CompForTeamNum(1);
		CompForTeamNum(2);

		if (downloadFinished)
		{
			SynergiesForTeamNum(1);
			SynergiesForTeamNum(2);
			CountersForTeamNum(1);
			CountersForTeamNum(2);
		}
	}

	public void SetHeroRecommendationPlayers()
	{
		for (int k = 0; k < playerHeroRecommendations.Length; k++)
		{
			playerHeroRecommendations[k].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<ClickHeroSelect>().playerNum = (k / 2);
			playerHeroRecommendations[k].transform.Find("PortraitMask").transform.Find("Portrait").gameObject.GetComponent<ClickHeroSelect>().recommendation = true;
		}
	}

	public void UpdateFriendlyPlayerHeroName(int playerNum)
	{
		playerHeroBorder[playerNum].color = Color.white;

		match.friendlyTeam.player[playerNum].chosenHeroName = playerHeroDropdown[playerNum].options[playerHeroDropdown[playerNum].value].text;
		GetChosenHeroStats(match.friendlyTeam.player[playerNum]);
		CheckPickedHeroes();

		bool foundHero = false;
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			if (match.friendlyTeam.player[playerNum].chosenHeroName == allHeroDetails[k].name)
			{
				foundHero = true;
				playerHeroPortrait[playerNum].sprite = Sprite.Create(allHeroDetails[k].portrait, new Rect(0, 0, allHeroDetails[k].portrait.width, allHeroDetails[k].portrait.height), new Vector2(0.5f, 0.5f));
				UpdateSubRoleImage(match.friendlyTeam.player[playerNum], playerNum);
				CheckPickedHeroes();
			}
		}

		if (!foundHero)
		{
			playerHeroPortrait[playerNum].sprite = genericHeroImage;
			UpdateSubRoleImage(match.friendlyTeam.player[playerNum], playerNum);
		}

		SubRoleValue();
		PlayerSubRoleValue(match.friendlyTeam, playerNum);
	}

	public void UpdateFriendlyPlayerHeroNameFromPortrait(int playerNum, string heroName)
	{
		playerHeroBorder[playerNum].color = Color.white;

		if (heroName != "01_Random")
		{
			match.friendlyTeam.player[playerNum].chosenHeroName = heroName;
			GetChosenHeroStats(match.friendlyTeam.player[playerNum]);
		}

		for (int i = 0; i < playerBackgroundImage.Length; i++)
		{
			playerBackgroundImage[i].enabled = true;
			playerNameInput[i].gameObject.SetActive(true);
			playerHeroDropdown[i].gameObject.SetActive(true);
			if (playerCustomNameOverlay[i].activeSelf)
			{
				playerCustomNameOverlay[i].GetComponent<Image>().enabled = true;
				playerCustomNameOverlay[i].transform.Find("Text").GetComponent<Text>().enabled = true;
			}
		}

		if (heroName != "01_Random")
		{
			for (int m = 0; m < playerHeroDropdown[playerNum].options.Count; m++)
			{
				if (playerHeroDropdown[playerNum].options[m].text == heroName)
				{
					playerHeroDropdown[playerNum].value = m;
				}
			}
		}
		else
		{
			playerHeroDropdown[playerNum].value = 0;
			CheckPickedHeroes();
		}

		bool foundHero = false;
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			if (match.friendlyTeam.player[playerNum].chosenHeroName == allHeroDetails[k].name)
			{
				foundHero = true;
				playerHeroPortrait[playerNum].sprite = Sprite.Create(allHeroDetails[k].portrait, new Rect(0, 0, allHeroDetails[k].portrait.width, allHeroDetails[k].portrait.height), new Vector2(0.5f, 0.5f));
				UpdateSubRoleImage(match.friendlyTeam.player[playerNum], playerNum);
				CheckPickedHeroes();
			}
		}

		if (!foundHero)
		{
			playerHeroPortrait[playerNum].sprite = genericHeroImage;
			UpdateSubRoleImage(match.friendlyTeam.player[playerNum], playerNum);
		}

		SubRoleValue();
		PlayerSubRoleValue(match.friendlyTeam, playerNum);
	}

	public void UpdateEnemyPlayerHeroName(int playerNum)
	{
		playerHeroBorder[playerNum].color = Color.white;

		match.enemyTeam.player[playerNum - 5].chosenHeroName = playerHeroDropdown[playerNum].options[playerHeroDropdown[playerNum].value].text;
		GetChosenHeroStats(match.enemyTeam.player[playerNum - 5]);
		CheckPickedHeroes();

		bool foundHero = false;
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			if (match.enemyTeam.player[playerNum - 5].chosenHeroName == allHeroDetails[k].name)
			{
				foundHero = true;
				playerHeroPortrait[playerNum].sprite = Sprite.Create(allHeroDetails[k].portrait, new Rect(0, 0, allHeroDetails[k].portrait.width, allHeroDetails[k].portrait.height), new Vector2(0.5f, 0.5f));
				UpdateSubRoleImage(match.enemyTeam.player[playerNum - 5], playerNum);
				CheckPickedHeroes();
			}
		}

		if (!foundHero)
		{
			playerHeroPortrait[playerNum].sprite = genericHeroImage;
			UpdateSubRoleImage(match.enemyTeam.player[playerNum - 5], playerNum);
		}

		SubRoleValue();
		PlayerSubRoleValue(match.enemyTeam, playerNum - 5);
	}

	public void UpdateEnemyPlayerHeroNameFromPortrait(int playerNum, string heroName)
	{
		playerHeroBorder[playerNum].color = Color.white;

		if (heroName != "01_Random")
		{
			match.enemyTeam.player[playerNum - 5].chosenHeroName = heroName;
			GetChosenHeroStats(match.enemyTeam.player[playerNum - 5]);
		}

		for (int i = 0; i < playerBackgroundImage.Length; i++)
		{
			playerBackgroundImage[i].enabled = true;
			playerNameInput[i].gameObject.SetActive(true);
			playerHeroDropdown[i].gameObject.SetActive(true);
			if (playerCustomNameOverlay[i].activeSelf)
			{
				playerCustomNameOverlay[i].GetComponent<Image>().enabled = true;
				playerCustomNameOverlay[i].transform.Find("Text").GetComponent<Text>().enabled = true;
			}
		}

		if (heroName != "01_Random")
		{
			for (int m = 0; m < playerHeroDropdown[playerNum].options.Count; m++)
			{
				if (playerHeroDropdown[playerNum].options[m].text == heroName)
				{
					playerHeroDropdown[playerNum].value = m;
				}
			}
		}
		else
		{
			playerHeroDropdown[playerNum].value = 0;
			CheckPickedHeroes();
		}

		bool foundHero = false;
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			if (match.enemyTeam.player[playerNum - 5].chosenHeroName == allHeroDetails[k].name)
			{
				foundHero = true;
				playerHeroPortrait[playerNum].sprite = Sprite.Create(allHeroDetails[k].portrait, new Rect(0, 0, allHeroDetails[k].portrait.width, allHeroDetails[k].portrait.height), new Vector2(0.5f, 0.5f));
				UpdateSubRoleImage(match.enemyTeam.player[playerNum - 5], playerNum);
				CheckPickedHeroes();
			}
		}

		if (!foundHero)
		{
			playerHeroPortrait[playerNum].sprite = genericHeroImage;
			UpdateSubRoleImage(match.enemyTeam.player[playerNum - 5], playerNum);
		}

		SubRoleValue();
		PlayerSubRoleValue(match.enemyTeam, playerNum - 5);

	}

	public void CheckPickedHeroes()
	{
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			allHeroDetails[k].picked = false;
		}
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			for (int m = 0; m < playerHeroDropdown.Length; m++)
			{
				if (allHeroDetails[k].name == playerHeroDropdown[m].options[playerHeroDropdown[m].value].text)
				{
					allHeroDetails[k].picked = true;
				}
			}
		}
	}

	void UpdateSubRoleImage(Player player, int playerNum)
	{
		if (player.chosenHeroSubRole == "")
		{
			foreach (Transform child in playerSubRoleImage[playerNum].gameObject.transform)
			{
				child.gameObject.SetActive(true);
			}
			playerSubRoleImage[playerNum].enabled = false;
			playerSubRoleImage[playerNum].sprite = genericSubRoleImage;
		}
		else
		{
			foreach (Transform child in playerSubRoleImage[playerNum].gameObject.transform)
			{
				child.gameObject.SetActive(false);
			}
			playerSubRoleImage[playerNum].enabled = true;
		}
		if (player.chosenHeroSubRole == "Tank")
		{
			playerSubRoleImage[playerNum].sprite = tankSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Bruiser")
		{
			playerSubRoleImage[playerNum].sprite = bruiserSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Healer")
		{
			playerSubRoleImage[playerNum].sprite = healerSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Support")
		{
			playerSubRoleImage[playerNum].sprite = supportSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Ambusher")
		{
			playerSubRoleImage[playerNum].sprite = ambusherSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Burst Damage")
		{
			playerSubRoleImage[playerNum].sprite = burstDamageSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Sustained Damage")
		{
			playerSubRoleImage[playerNum].sprite = sustainedDamageSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Siege")
		{
			playerSubRoleImage[playerNum].sprite = siegeSubRoleImage;
		}
		if (player.chosenHeroSubRole == "Utility")
		{
			playerSubRoleImage[playerNum].sprite = utilitySubRoleImage;
		}
	}

	public void UpdateFriendlyPlayerName(int playerNum)
	{
		match.friendlyTeam.player[playerNum].name = playerNameInput[playerNum].text;
		StartCoroutine(RunPlayerHeroData(match.friendlyTeam.player[playerNum], playerNum));
		playerCustomNameOverlay[playerNum].transform.Find("Text").GetComponent<Text>().text = "Gathering Player Data";
		playerCustomNameOverlay[playerNum].SetActive(true);
	}
	public void UpdateEnemyPlayerName(int playerNum)
	{
		match.enemyTeam.player[playerNum - 5].name = playerNameInput[playerNum].text;
		StartCoroutine(RunPlayerHeroData(match.enemyTeam.player[playerNum - 5], playerNum));
		playerCustomNameOverlay[playerNum].transform.Find("Text").GetComponent<Text>().text = "Gathering Player Data";
		playerCustomNameOverlay[playerNum].SetActive(true);
	}

	public void HidePlayerCustomName(int playerNum)
	{
		playerCustomNameOverlay[playerNum].SetActive(false);
	}

	public void AlwaysOnTop(bool aot) {

		alwaysOnTop = aot;
		SaveEverything();
	}

	public void SaveEverything()
	{
		Debug.Log("Saved");
		string[] friendsListArray = new string[friendsList.Count];
		for (int k = 0; k < friendsList.Count; k++)
		{
			friendsListArray[k] = friendsList[k];
		}
		PlayerPrefsX.SetStringArray("FriendsList", friendsListArray);
		PlayerPrefs.SetInt("GamesPlayedThreshold", gamesPlayedThreshold);
		PlayerPrefs.SetFloat("MaxExperiencePenalty", maxExperiencePenalty);
		PlayerPrefs.SetInt("MmrType", mmrType);
		if (alwaysOnTop)
		{
			PlayerPrefs.SetInt("AlwaysOnTop", 1);
		}
		else
		{
			PlayerPrefs.SetInt("AlwaysOnTop", 0);
		}
	}

	public void LoadEverything()
	{
		if (PlayerPrefs.HasKey("GamesPlayedThreshold"))
		{
			gamesPlayedThreshold = PlayerPrefs.GetInt("GamesPlayedThreshold");
		}
		else
		{
			gamesPlayedThreshold = 25;
		}
		if (PlayerPrefs.HasKey("MaxExperiencePenalty"))
		{
			maxExperiencePenalty = PlayerPrefs.GetFloat("MaxExperiencePenalty");
		}
		else
		{
			maxExperiencePenalty = 0.025f;
		}
		if (PlayerPrefs.HasKey("MmrType"))
		{
			mmrType = PlayerPrefs.GetInt("MmrType");
		}
		else
		{
			mmrType = 1;
		}
		if (PlayerPrefs.HasKey("AlwaysOnTop"))
		{
			int tempInt = PlayerPrefs.GetInt("AlwaysOnTop");
			if (tempInt == 0)
			{
				alwaysOnTop = false;
				alwaysOnTopToggle.isOn = false;
            }
			else
			{
				alwaysOnTop = true;
				alwaysOnTopToggle.isOn = true;
			}
		}
		else
		{
			alwaysOnTop = false;
			alwaysOnTopToggle.isOn = false;
		}

		gamesPlayedInput.text = gamesPlayedThreshold.ToString();
		maxPenaltyInput.text = (maxExperiencePenalty * 100).ToString();
		mmrTypeDropdown.value = mmrType;

		//Load Friend's List
		if (PlayerPrefs.HasKey("FriendsList"))
		{
			string[] friendsListArray = PlayerPrefsX.GetStringArray("FriendsList");
			for (int k = 0; k < friendsListArray.Length; k++)
			{
				friendsList[k] = friendsListArray[k];
				friendNameInputField[k].text = friendsList[k];
				StartCoroutine(RunFriendName(friendsList[k], k));
			}
			FilterFriendsList();
		}
	}

	public void OpenSliderPanels()
	{
		sliderOptions.SetActive(true);
	}

	public void CloseSliderPanels()
	{
		sliderOptions.SetActive(false);
	}

	public void OpenPlayerHotsPage(int playerNum)
	{
		if (playerNum <= 4)
		{
			if (match.friendlyTeam.player[playerNum].foundOnHotslogs)
			{
				Application.OpenURL("http://www.hotslogs.com/Player/Profile?PlayerID=" + match.friendlyTeam.player[playerNum].idNumber);
			}
		}
		else if (playerNum > 4)
		{
			if (match.enemyTeam.player[playerNum - 5].foundOnHotslogs)
			{
				Application.OpenURL("http://www.hotslogs.com/Player/Profile?PlayerID=" + match.enemyTeam.player[playerNum - 5].idNumber);
			}
		}
	}

	public void AppScreenshotButton(){

		uploadingConfirmationBox.SetActive(true);
    }

	public void ConfirmAppScreenshot(bool upload) {

		uploadingConfirmationBox.SetActive(false);
		if (upload)
		{
			StartCoroutine(AppScreenshotUpload());
		}
	}

	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Keypad0))
		//{
		//	PlayerPrefs.DeleteAll();
		//}

		if (Input.GetAxis("Mouse ScrollWheel") > 0.0001f || Input.GetAxis("Mouse ScrollWheel") < -0.0001f)
		{
			heroPickPanel.transform.Find("Scroll View").transform.Find("ScrollbarVerticalHeroes").GetComponent<Scrollbar>().value += Input.GetAxis("Mouse ScrollWheel");
		}

		int friendlyTotalScore = 0;
		for (int k = 0; k < match.friendlyTeam.player.Length; k++)
		{
			match.friendlyTeam.player[k].chosenHeroScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.player[k].chosenHeroWinRate, playerHeroWeight) * 1000.0f - 500.0f));
			//match.friendlyTeam.player[k].chosenSubRoleScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.player[k].chosenSubRoleWinRate, match.friendlyTeam.subRoleCompOnMap.weight) * 1000.0f - 500.0f));
			match.friendlyTeam.player[k].chosenMapScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.player[k].chosenMapWinRate, playerMapWeight) * 1000.0f - 500.0f));
			match.friendlyTeam.player[k].chosenHeroOnMapScore = (int)(Mathf.Round(Mathf.Lerp(0.0f, match.friendlyTeam.player[k].chosenHeroOnMapWinRate, heroMapWeight) * 1000.0f));
			match.friendlyTeam.player[k].score = match.friendlyTeam.player[k].chosenHeroScore + match.friendlyTeam.player[k].chosenMapScore + match.friendlyTeam.player[k].chosenHeroOnMapScore;
			playerScoreText[k].text = match.friendlyTeam.player[k].score.ToString();
			friendlyTotalScore += match.friendlyTeam.player[k].score;
		}

		int enemyTotalScore = 0;
		for (int k = 0; k < match.enemyTeam.player.Length; k++)
		{
			match.enemyTeam.player[k].chosenHeroScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.player[k].chosenHeroWinRate, playerHeroWeight) * 1000.0f - 500.0f));
			//match.enemyTeam.player[k].chosenSubRoleScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.player[k].chosenSubRoleWinRate, match.enemyTeam.subRoleCompOnMap.weight) * 1000.0f - 500.0f));
			match.enemyTeam.player[k].chosenMapScore = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.player[k].chosenMapWinRate, playerMapWeight) * 1000.0f - 500.0f));
			match.enemyTeam.player[k].chosenHeroOnMapScore = (int)(Mathf.Round(Mathf.Lerp(0.0f, match.enemyTeam.player[k].chosenHeroOnMapWinRate, heroMapWeight) * 1000.0f));
			match.enemyTeam.player[k].score = match.enemyTeam.player[k].chosenHeroScore + match.enemyTeam.player[k].chosenMapScore + match.enemyTeam.player[k].chosenHeroOnMapScore;
			playerScoreText[k + 5].text = match.enemyTeam.player[k].score.ToString();
			enemyTotalScore += match.enemyTeam.player[k].score;
		}

		match.friendlyTeam.subRoleCompOnMap.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.subRoleCompOnMap.winRate, subRoleCompMapWeight) * 1000.0f - 500.0f));
		match.friendlyTeam.heroSynergies.finalCombination = Mathf.Lerp(match.friendlyTeam.heroSynergies.advRate, match.friendlyTeam.heroSynergies.winRate, estAdvantageOrWinRate);
		match.friendlyTeam.heroSynergies.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.heroSynergies.finalCombination, synergyWeight) * 1000.0f - 500.0f));
		match.friendlyTeam.heroCounters.finalCombination = Mathf.Lerp(match.friendlyTeam.heroCounters.advRate, match.friendlyTeam.heroCounters.winRate, estAdvantageOrWinRate);
		match.friendlyTeam.heroCounters.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.friendlyTeam.heroCounters.finalCombination, counterWeight) * 1000.0f - 500.0f));
		match.friendlyTeam.score = match.friendlyTeam.subRoleCompOnMap.score + match.friendlyTeam.heroSynergies.score + match.friendlyTeam.heroCounters.score;
		playerScoreText[10].text = match.friendlyTeam.score.ToString();
		friendlyTotalScore = (friendlyTotalScore / 5);
		playerScoreText[14].text = friendlyTotalScore.ToString();
		playerScoreText[12].text = ((friendlyTotalScore + match.friendlyTeam.score) / 2).ToString();

		match.enemyTeam.subRoleCompOnMap.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.subRoleCompOnMap.winRate, subRoleCompMapWeight) * 1000.0f - 500.0f));
		match.enemyTeam.heroSynergies.finalCombination = Mathf.Lerp(match.enemyTeam.heroSynergies.advRate, match.enemyTeam.heroSynergies.winRate, estAdvantageOrWinRate);
		match.enemyTeam.heroSynergies.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.heroSynergies.finalCombination, synergyWeight) * 1000.0f - 500.0f));
		match.enemyTeam.heroCounters.finalCombination = Mathf.Lerp(match.enemyTeam.heroCounters.advRate, match.enemyTeam.heroCounters.winRate, estAdvantageOrWinRate);
		match.enemyTeam.heroCounters.score = (int)(Mathf.Round(Mathf.Lerp(0.5f, match.enemyTeam.heroCounters.finalCombination, counterWeight) * 1000.0f - 500.0f));
		match.enemyTeam.score = match.enemyTeam.subRoleCompOnMap.score + match.enemyTeam.heroSynergies.score + match.enemyTeam.heroCounters.score;
		playerScoreText[11].text = match.enemyTeam.score.ToString();
		enemyTotalScore = (enemyTotalScore / 5);
		playerScoreText[15].text = enemyTotalScore.ToString();
		playerScoreText[13].text = ((enemyTotalScore + match.enemyTeam.score) / 2).ToString();

		if (((float)Screen.width / (float)Screen.height) > ((float)16 / (float)9))
		{
			Screen.SetResolution((int)(((float)16 / (float)9) * (float)Screen.height), Screen.height, false);
		}
		if (((float)Screen.width / (float)Screen.height) < ((float)4 / (float)3))
		{
			Screen.SetResolution((int)(((float)4 / (float)3) * (float)Screen.height), Screen.height, false);
		}

		if (Screen.height < 351)
		{
			Screen.SetResolution(624, 351, false);
		}
		if (Screen.width < 624)
		{
			Screen.SetResolution(624, 351, false);
		}
	}

	public void HoverPlayerIndicator(int playerNum)
	{
		if (playerNum <= 4)
		{
			if (match.friendlyTeam.player[playerNum].hotsLogsBanner != null)
			{
				hoverBanner.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (Screen.height / 8), 0);
				hoverBanner.gameObject.SetActive(true);
				hoverBanner.sprite = match.friendlyTeam.player[playerNum].hotsLogsBanner;
			}
		}
		else if (playerNum > 4)
		{
			if (match.enemyTeam.player[playerNum - 5].hotsLogsBanner != null)
			{
				hoverBanner.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (Screen.height / 8), 0);
				hoverBanner.gameObject.SetActive(true);
				hoverBanner.sprite = match.enemyTeam.player[playerNum - 5].hotsLogsBanner;
			}
		}
	}

	public void HoverHeroPortrait(int playerNum, string heroName)
	{
		hoverPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (Screen.height / 8), 0);

		string playerGamesPlayedText = "";
		string playerWinRateText = "";
		string playerWinRateOnMap = "";
		string winRateOnMap = "";

		if (playerFoundIndicator[playerNum].color == Color.green)
		{
			bool foundMatch = false;
			bool foundOnce = false;
			for (int k = 0; k < allHeroDetails.Count; k++)
			{
				if (playerNum <= 4)
				{
					for (int l = 0; l < match.friendlyTeam.player[playerNum].heroDetails.Count; l++)
					{
						if (allHeroDetails[k].name == heroName && match.friendlyTeam.player[playerNum].heroDetails[l].name == heroName)
						{
							foundMatch = true;
						}
						if (foundMatch && !foundOnce)
						{
							playerGamesPlayedText = "<color=#9C175F>Games Played with Hero:</color>\n" + match.friendlyTeam.player[playerNum].heroDetails[l].gamesPlayed + " Games\n";
							if (match.friendlyTeam.player[playerNum].heroDetails[l].gamesPlayed >= gamesPlayedThreshold)
							{
								playerWinRateText = "<color=#9C175F>Player's Win Rate With Hero:</color>\n"
									+ (match.friendlyTeam.player[playerNum].heroDetails[l].winRate * 100.0f).ToString("F1") + "% = " + ((match.friendlyTeam.player[playerNum].heroDetails[l].winRate * 1000) - 500).ToString("F0") + " Points";
							}
							else
							{
								float tempPlayerWinRate = (match.friendlyTeam.player[playerNum].heroDetails[l].winRate) * match.friendlyTeam.player[playerNum].heroDetails[l].gamesPlayed;
								float tempGlobalWinRate = allHeroDetails[k].winRate * (gamesPlayedThreshold - match.friendlyTeam.player[playerNum].heroDetails[l].gamesPlayed);
								float estimatedWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - (Mathf.Lerp(0, maxExperiencePenalty, (Mathf.Max((gamesPlayedThreshold - match.friendlyTeam.player[playerNum].heroDetails[l].gamesPlayed), 0) / (gamesPlayedThreshold * 1.0f))));
								playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>\n"
									+ (estimatedWinRate * 100.0f).ToString("F1") + "% = " + ((estimatedWinRate * 1000) - 500).ToString("F0") + " Points";
							}
							foundOnce = true;
						}
						else if (!foundMatch && !foundOnce && allHeroDetails[k].name == heroName)
						{
							playerGamesPlayedText = "<color=#9C175F>No Games Played with Hero</color>\n\n";
							float estimatedWinRate = allHeroDetails[k].winRate - maxExperiencePenalty;
							playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>\n"
								+ (estimatedWinRate * 100.0f).ToString("F1") + "% = " + ((estimatedWinRate * 1000) - 500).ToString("F0") + " Points";
						}

						if (match.chosenMapName != "" && match.chosenMapName != null)
						{
							playerWinRateOnMap = "\n<color=#9C175F>Player WR On Map Bonus:</color>\n"
							+ ((match.friendlyTeam.player[playerNum].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenMapScore + " Points";
						}
						else
						{
							playerWinRateOnMap = "\n<color=#9C175F>Player Avrg Map WR Bonus:</color>\n"
							+ ((match.friendlyTeam.player[playerNum].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenMapScore + " Points";
						}
					}
				}
				else if (playerNum > 4)
				{
					for (int l = 0; l < match.enemyTeam.player[playerNum - 5].heroDetails.Count; l++)
					{
						if (allHeroDetails[k].name == heroName && match.enemyTeam.player[playerNum - 5].heroDetails[l].name == heroName)
						{
							foundMatch = true;
						}
						if (foundMatch && !foundOnce)
						{
							playerGamesPlayedText = "<color=#9C175F>Games Played with Hero:</color>\n" + match.enemyTeam.player[playerNum - 5].heroDetails[l].gamesPlayed + " Games\n";
							if (match.enemyTeam.player[playerNum - 5].heroDetails[l].gamesPlayed >= gamesPlayedThreshold)
							{
								playerWinRateText = "<color=#9C175F>Player's Win Rate With Hero:</color>\n"
									+ (match.enemyTeam.player[playerNum - 5].heroDetails[l].winRate * 100.0f).ToString("F1") + "% = " + ((match.enemyTeam.player[playerNum - 5].heroDetails[l].winRate * 1000) - 500).ToString("F0") + " Points";
							}
							else
							{
								float tempPlayerWinRate = (match.enemyTeam.player[playerNum - 5].heroDetails[l].winRate) * match.enemyTeam.player[playerNum - 5].heroDetails[l].gamesPlayed;
								float tempGlobalWinRate = allHeroDetails[k].winRate * (gamesPlayedThreshold - match.enemyTeam.player[playerNum - 5].heroDetails[l].gamesPlayed);
								float estimatedWinRate = ((tempPlayerWinRate + tempGlobalWinRate) / gamesPlayedThreshold) - (Mathf.Lerp(0, maxExperiencePenalty, (Mathf.Max((gamesPlayedThreshold - match.enemyTeam.player[playerNum - 5].heroDetails[l].gamesPlayed), 0) / (gamesPlayedThreshold * 1.0f))));
								playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>\n"
									+ (estimatedWinRate * 100.0f).ToString("F1") + "% = " + ((estimatedWinRate * 1000) - 500).ToString("F0") + " Points";
							}
							foundOnce = true;
						}
						else if (!foundMatch && !foundOnce && allHeroDetails[k].name == heroName)
						{
							playerGamesPlayedText = "<color=#9C175F>No Games Played with Hero</color>\n\n";
							float estimatedWinRate = allHeroDetails[k].winRate - maxExperiencePenalty;
							playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>\n"
								+ (estimatedWinRate * 100.0f).ToString("F1") + "% = " + ((estimatedWinRate * 1000) - 500).ToString("F0") + " Points";
						}

						if (match.chosenMapName != "" && match.chosenMapName != null)
						{
							playerWinRateOnMap = "\n<color=#9C175F>Player WR On Map Bonus:</color>\n"
							+ ((match.enemyTeam.player[playerNum - 5].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenMapScore + " Points";
						}
						else
						{
							playerWinRateOnMap = "\n<color=#9C175F>Player Avrg Map WR Bonus:</color>\n"
							+ ((match.enemyTeam.player[playerNum - 5].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenMapScore + " Points";
						}
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < allHeroDetails.Count; k++)
			{
				if (allHeroDetails[k].name == heroName)
				{
					playerWinRateText = "<color=#36249C>Global Hero Win Rate:</color>"
					+ "\n" + (allHeroDetails[k].winRate * 100.0f).ToString("F1") + "% = " + ((allHeroDetails[k].winRate * 1000) - 500).ToString("F0") + " Points";
				}
			}
			playerWinRateOnMap = "";
		}

		bool foundHero = false;
		for (int k = 0; k < allHeroDetails.Count; k++)
		{
			if (allHeroDetails[k].name == heroName)
			{
				foundHero = true;
				if (match.chosenMapName != "" && match.chosenMapName != null)
				{
					for (int l = 0; l < allHeroDetails[k].onMap.Count; l++)
					{
						if (allHeroDetails[k].onMap[l].name == match.chosenMapName)
						{
							winRateOnMap = "\n<color=#36249C>Global Hero WR On Map Bonus:</color>\n"
							+ ((allHeroDetails[k].onMap[l].winRate * 100.0f) - (allHeroDetails[k].winRate * 100.0f)).ToString("F1") + "% = " + ((allHeroDetails[k].onMap[l].winRate * 1000) - (allHeroDetails[k].winRate * 1000)).ToString("F0") + " Points";
						}
					}
				}
				else
				{
					winRateOnMap = "";
				}

				if (playerNum <= 4)
				{
					hoverPanel.transform.Find("Text").GetComponent<Text>().text =
						"<size=23><color=#007C9C><i>" + heroName.ToUpper() + "</i></color></size>\n"
						+ playerGamesPlayedText
						+ playerWinRateText
						+ playerWinRateOnMap
						+ winRateOnMap;
				}
				else
				{
					hoverPanel.transform.Find("Text").GetComponent<Text>().text =
						"<size=23><color=#E81512><i>" + heroName.ToUpper() + "</i></color></size>\n"
						+ playerGamesPlayedText
						+ playerWinRateText
						+ playerWinRateOnMap
						+ winRateOnMap;
				}
			}
		}
		if (!foundHero)
		{
			hoverPanel.transform.Find("Text").GetComponent<Text>().text = "<size=23><i>Random Hero</i></size>";
		}

		hoverPanel.SetActive(true);
	}

	public void HidePlayerBanner()
	{
		hoverBanner.gameObject.SetActive(false);
	}

	public void HoverPlayerScore(int playerNum)
	{
		hoverPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (Screen.height / 8), 0);

		string playerGamesPlayedText = "";
		string playerWinRateText = "";
		string playerWinRateOnMap = "";
		string winRateOnMap = "";

		if (playerFoundIndicator[playerNum].color == Color.green)
		{
			if (playerNum <= 4)
			{
				if (match.friendlyTeam.player[playerNum].chosenHeroName != "" && match.friendlyTeam.player[playerNum].chosenHeroName != null)
				{
					playerGamesPlayedText = "<color=#9C175F>Games Played with Hero:</color>\n" + match.friendlyTeam.player[playerNum].gamesPlayedWithChosenHero + " Games\n";
					if (match.friendlyTeam.player[playerNum].gamesPlayedWithChosenHero >= gamesPlayedThreshold)
					{
						playerWinRateText = "<color=#9C175F>Player's Win Rate With Hero:</color>";
					}
					else
					{
						playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>";
					}
				}
				else
				{
					playerWinRateText = "<color=#9C175F>Player's Avrg Hero Win Rate:</color>";
				}

				if (match.chosenMapName != "" && match.chosenMapName != null)
				{
					playerWinRateOnMap = "\n<color=#9C175F>Player WR On Map Bonus:</color>\n"
					+ ((match.friendlyTeam.player[playerNum].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenMapScore + " Points";
				}
				else
				{
					playerWinRateOnMap = "\n<color=#9C175F>Player Avrg Map WR Bonus:</color>\n"
					+ ((match.friendlyTeam.player[playerNum].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenMapScore + " Points";
				}
			}
			else if (playerNum > 4)
			{
				if (match.enemyTeam.player[playerNum - 5].chosenHeroName != "" && match.friendlyTeam.player[playerNum - 5].chosenHeroName != null)
				{
					playerGamesPlayedText = "<color=#9C175F>Games Played with Hero:</color>\n" + match.enemyTeam.player[playerNum - 5].gamesPlayedWithChosenHero + " Games\n";
					if (match.enemyTeam.player[playerNum - 5].gamesPlayedWithChosenHero >= gamesPlayedThreshold)
					{
						playerWinRateText = "<color=#9C175F>Player's Win Rate With Hero:</color>";
					}
					else
					{
						playerWinRateText = "<color=#9C175F>Player's Est WR With Hero:</color>";
					}
				}
				else
				{
					playerWinRateText = "<color=#9C175F>Player's Avrg Hero Win Rate:</color>";
				}

				if (match.chosenMapName != "" && match.chosenMapName != null)
				{
					playerWinRateOnMap = "\n<color=#9C175F>Player WR On Map Bonus:</color>\n"
					+ ((match.enemyTeam.player[playerNum - 5].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenMapScore + " Points";
				}
				else
				{
					playerWinRateOnMap = "\n<color=#9C175F>Player Avrg Map WR Bonus:</color>\n"
					+ ((match.enemyTeam.player[playerNum - 5].chosenMapWinRate - 0.5f) * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenMapScore + " Points";
				}
			}
		}
		else
		{
			playerWinRateText = "<color=#36249C>Global Hero Win Rate:</color>";
			playerWinRateOnMap = "";
		}

		if (playerNum <= 4)
		{
			if (match.friendlyTeam.player[playerNum].chosenHeroName != "" && match.friendlyTeam.player[playerNum].chosenHeroName != null)
			{
				if (match.chosenMapName != "" && match.chosenMapName != null)
				{
					winRateOnMap = "\n<color=#36249C>Global Hero WR On Map Bonus:</color>\n"
					+ (match.friendlyTeam.player[playerNum].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenHeroOnMapScore + " Points";
				}
				else
				{
					winRateOnMap = "";
					//if (playerFoundIndicator[playerNum].color == Color.green)
					//{
					//	winRateOnMap = "\nGlobal Hero Bonus:\n"
					//	+ (match.friendlyTeam.player[playerNum].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenHeroOnMapScore + " Points";
					//}
					//else
					//{
					//	winRateOnMap = "\nGlobal Hero Bonus All Maps:\n"
					//	+ (match.friendlyTeam.player[playerNum].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenHeroOnMapScore + " Points";
					//}
				}
			}

			hoverPanel.transform.Find("Text").GetComponent<Text>().text =
				playerGamesPlayedText
				+ playerWinRateText
				+ "\n" + (match.friendlyTeam.player[playerNum].chosenHeroWinRate * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.player[playerNum].chosenHeroScore + " Points"
				+ playerWinRateOnMap
				+ winRateOnMap;
		}
		else if (playerNum > 4)
		{
			if (match.enemyTeam.player[playerNum - 5].chosenHeroName != "" && match.friendlyTeam.player[playerNum - 5].chosenHeroName != null)
			{
				if (match.chosenMapName != "" && match.chosenMapName != null)
				{
					winRateOnMap = "\n<color=#36249C>Global Hero WR On Map Bonus:</color>\n"
					+ (match.enemyTeam.player[playerNum - 5].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenHeroOnMapScore + " Points";
				}
				else
				{
					winRateOnMap = "";
					//if (playerFoundIndicator[playerNum].color == Color.green)
					//{
					//	winRateOnMap = "\nGlobal Hero Win Rate:\n"
					//	+ (match.enemyTeam.player[playerNum - 5].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenHeroOnMapScore + " Points";
					//}
					//else
					//{
					//	winRateOnMap = "\nGlobal Hero Win Rate All Maps:\n"
					//	+ (match.enemyTeam.player[playerNum - 5].chosenHeroOnMapWinRate * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenHeroOnMapScore + " Points";
					//}
				}
			}

			hoverPanel.transform.Find("Text").GetComponent<Text>().text =
				playerGamesPlayedText
				+ playerWinRateText
				+ "\n" + (match.enemyTeam.player[playerNum - 5].chosenHeroWinRate * 100.0f).ToString("F1") + "% = " + match.enemyTeam.player[playerNum - 5].chosenHeroScore + " Points"
				+ playerWinRateOnMap
				+ winRateOnMap;
		}

		hoverPanel.SetActive(true);
	}

	public void HoverTeamScore(int teamNum)
	{
		hoverPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (Screen.height / 8), 0);

		if (teamNum == 1)
		{
			string tempCompString = "";
			int tempCount = 0;
			for (int k = 0; k < match.friendlyTeam.player.Length; k++)
			{
				if (match.friendlyTeam.player[k].chosenHeroName == "" || match.friendlyTeam.player[k].chosenHeroName == null)
				{
					tempCount++;
				}
			}
			if (tempCount == 0)
			{
				tempCompString = "<color=#36249C>Comp on Map Win Rate:</color>\n";
			}
			else
			{
				tempCompString = "<color=#36249C>Estimated Comp Win Rate:</color>\n";
			}
			hoverPanel.transform.Find("Text").GetComponent<Text>().text =
				tempCompString
				+ (match.friendlyTeam.subRoleCompOnMap.winRate * 100.0f).ToString("F1") + "% = " + match.friendlyTeam.subRoleCompOnMap.score + " Points\n"
				+ "<color=#36249C>Hero Average Synergy:</color>\n"
				+ ((match.friendlyTeam.heroSynergies.finalCombination * 100.0f) - 50).ToString("F1") + "% = " + match.friendlyTeam.heroSynergies.score + " Points\n"
				+ "<color=#36249C>Hero Average Counters:</color>\n"
				+ ((match.friendlyTeam.heroCounters.finalCombination * 100.0f) - 50).ToString("F1") + "% = " + match.friendlyTeam.heroCounters.score + " Points";
		}

		if (teamNum == 2)
		{
			string tempCompString2 = "";
			int tempCount = 0;
			for (int k = 0; k < match.enemyTeam.player.Length; k++)
			{
				if (match.enemyTeam.player[k].chosenHeroName == "" || match.enemyTeam.player[k].chosenHeroName == null)
				{
					tempCount++;
				}
			}
			if (tempCount == 0)
			{
				tempCompString2 = "<color=#36249C>Comp on Map Win Rate:</color>\n";
			}
			else
			{
				tempCompString2 = "<color=#36249C>Estimated Comp Win Rate:</color>\n";
			}
			hoverPanel.transform.Find("Text").GetComponent<Text>().text =
				tempCompString2
				+ (match.enemyTeam.subRoleCompOnMap.winRate * 100.0f).ToString("F1") + "% = " + match.enemyTeam.subRoleCompOnMap.score + " Points\n"
				+ "<color=#36249C>Hero Average Synergy:</color>\n"
				+ ((match.enemyTeam.heroSynergies.finalCombination * 100.0f) - 50).ToString("F1") + "% = " + match.enemyTeam.heroSynergies.score + " Points\n"
				+ "<color=#36249C>Hero Average Counters:</color>\n"
				+ ((match.enemyTeam.heroCounters.finalCombination * 100.0f) - 50).ToString("F1") + "% = " + match.enemyTeam.heroCounters.score + " Points";
		}

		hoverPanel.SetActive(true);
	}

	public void HideHoverPanel()
	{
		hoverPanel.SetActive(false);
	}

	public void UpdateGamesPlayedThreshold()
	{
		if (int.Parse(gamesPlayedInput.text) < 1)
		{
			gamesPlayedInput.text = "1";
		}
		gamesPlayedThreshold = int.Parse(gamesPlayedInput.text);
		SaveEverything();

		for (int i = 0; i < match.friendlyTeam.player.Length; i++)
		{
			UpdateFriendlyPlayerHeroName(i);
			//GetChosenHeroOnMap(match.friendlyTeam.player[i]);
			//SetPlayerHeroScores(i);
		}
		for (int i = 0; i < match.enemyTeam.player.Length; i++)
		{
			UpdateEnemyPlayerHeroName(i + 5);
			//GetChosenHeroOnMap(match.enemyTeam.player[i]);
			//SetPlayerHeroScores(i + 5);
		}
	}

	public void UpdateMaxPenalty()
	{
		if (float.Parse(maxPenaltyInput.text) < 0.1f)
		{
			maxPenaltyInput.text = "0.1";
		}
		maxExperiencePenalty = float.Parse(maxPenaltyInput.text) / 100.0f;
		SaveEverything();

		for (int i = 0; i < match.friendlyTeam.player.Length; i++)
		{
			UpdateFriendlyPlayerHeroName(i);
			//GetChosenHeroOnMap(match.friendlyTeam.player[i]);
			//SetPlayerHeroScores(i);
		}
		for (int i = 0; i < match.enemyTeam.player.Length; i++)
		{
			UpdateEnemyPlayerHeroName(i + 5);
			//GetChosenHeroOnMap(match.enemyTeam.player[i]);
			//SetPlayerHeroScores(i + 5);
		}
	}

	public void OnClickHeroPortrait(int playerNum)
	{
		for (int y = 0; y < suggestedHeroOrder.Length; y++)
		{
			suggestedHeroOrder[y].SetActive(false);
			suggestedSubRoles[y].gameObject.SetActive(false);
		}

		for (int i = 0; i < playerHeroBorder.Length; i++)
		{
			playerHeroBorder[i].color = Color.white;
		}

		for (int i = 0; i < playerBackgroundImage.Length; i++)
		{
			playerBackgroundImage[i].enabled = false;
			playerNameInput[i].gameObject.SetActive(false);
			playerHeroDropdown[i].gameObject.SetActive(false);
			if (playerCustomNameOverlay[i].activeSelf)
			{
				playerCustomNameOverlay[i].GetComponent<Image>().enabled = false;
				playerCustomNameOverlay[i].transform.Find("Text").GetComponent<Text>().enabled = false;
			}
		}

		if (playerNum <= 4)
		{
			if (match.friendlyTeam.player[playerNum].customName != "")
			{
				playerChoosingText.text = "<color=#007C9C>" + match.friendlyTeam.player[playerNum].customName + " Hero Select</color>";
			}
			else
			{
				playerChoosingText.text = "<color=#007C9C>Friendly Player " + (playerNum + 1).ToString() + " Hero Select</color>";
			}
		}
		else
		{
			if (match.enemyTeam.player[playerNum - 5].customName != "")
			{
				playerChoosingText.text = "<color=#E81512>" + match.enemyTeam.player[playerNum - 5].customName + " Hero Select</color>";
			}
			else
			{
				playerChoosingText.text = "<color=#E81512>Enemy Player " + (playerNum - 5 + 1).ToString() + " Hero Select</color>";
			}
		}
		playerHeroBorder[playerNum].color = new Color(255.0f / 255.0f, 196.0f / 255.0f, 69.0f / 255.0f);

		heroPickPanel.SetActive(true);
		heroPickPanel.transform.Find("Scroll View").transform.Find("ScrollbarVerticalHeroes").GetComponent<Scrollbar>().value = 1;
		currentlyPickingPlayer = playerNum;

		GetGeneralHeroRecommendations();
    }

	public void ResetAllHeroSelections()
	{
		if (!heroPickPanel.activeSelf)
		{
			for (int i = 0; i < match.friendlyTeam.player.Length; i++)
			{
				UpdateFriendlyPlayerHeroNameFromPortrait(i, "01_Random");
			}
			for (int i = 0; i < match.enemyTeam.player.Length; i++)
			{
				UpdateEnemyPlayerHeroNameFromPortrait(i + 5, "01_Random");
			}
		}
	}

	public void BackOutOfHeroSelect()
	{
		playerHeroBorder[currentlyPickingPlayer].color = Color.white;

		if (currentlyPickingPlayer <= 4)
		{
			UpdateFriendlyPlayerHeroNameFromPortrait(currentlyPickingPlayer, match.friendlyTeam.player[currentlyPickingPlayer].chosenHeroName);
		}
		else if (currentlyPickingPlayer > 4)
		{
			UpdateEnemyPlayerHeroNameFromPortrait(currentlyPickingPlayer, match.enemyTeam.player[currentlyPickingPlayer - 5].chosenHeroName);
		}

		heroPickPanel.SetActive(false);
		HideHoverPanel();
	}

	public void OnSelectHero(string heroName)
	{
		if (currentlyPickingPlayer <= 4)
		{
			UpdateFriendlyPlayerHeroNameFromPortrait(currentlyPickingPlayer, heroName);
		}
		else if (currentlyPickingPlayer > 4)
		{
			UpdateEnemyPlayerHeroNameFromPortrait(currentlyPickingPlayer, heroName);
		}

		heroPickPanel.SetActive(false);
		HideHoverPanel();
	}

	public void SetPlayerHeroScores(int playerNum)
	{
		if (playerNum <= 4)
		{
			match.friendlyTeam.player[playerNum].currentHeroScores = new List<CurrentHeroScores>();
			for (int i = 0; i < allHeroDetails.Count; i++)
			{
				int tempScoreForPlayer = GetDropdownHeroScore(match.friendlyTeam.player[playerNum], allHeroDetails[i].name);
				match.friendlyTeam.player[playerNum].currentHeroScores.Add(new CurrentHeroScores());
				match.friendlyTeam.player[playerNum].currentHeroScores[i].name = allHeroDetails[i].name;
				match.friendlyTeam.player[playerNum].currentHeroScores[i].score = tempScoreForPlayer;
			}
		}

		if (playerNum > 4)
		{
			match.enemyTeam.player[playerNum - 5].currentHeroScores = new List<CurrentHeroScores>();
			for (int i = 0; i < allHeroDetails.Count; i++)
			{
				int tempScoreForPlayer = GetDropdownHeroScore(match.enemyTeam.player[playerNum - 5], allHeroDetails[i].name);
				match.enemyTeam.player[playerNum - 5].currentHeroScores.Add(new CurrentHeroScores());
				match.enemyTeam.player[playerNum - 5].currentHeroScores[i].name = allHeroDetails[i].name;
				match.enemyTeam.player[playerNum - 5].currentHeroScores[i].score = tempScoreForPlayer;
			}
		}
	}

	IEnumerator AppScreenshotUpload()
	{
		uploadingPanel.SetActive(true);
		string filename = "/screenshot.png";
		yield return new WaitForEndOfFrame();
		PngEncoder.CaptureToPng(Screen.width, Screen.height, photoCamera, Application.persistentDataPath + filename);
		//Application.CaptureScreenshot(Application.persistentDataPath + filename);
		yield return new WaitForEndOfFrame();

		//Make sure that the file save properly
		float startTime = Time.time;
		while (false == File.Exists(Application.persistentDataPath + filename))
		{
			if (Time.time - startTime > 5.0f)
			{
				yield break;
			}
			yield return null;
		}

		//Read the saved file back into bytes
		byte[] rawImage = File.ReadAllBytes(Application.persistentDataPath + filename);

		//Before we try uploading it to Imgur we need to Server Certificate Validation Callback
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		//Attempt to upload the image
		using (var w = new WebClient())
		{
			string clientID = "cf84403b302c7de";
			w.Headers.Add("Authorization", "Client-ID " + clientID);
			var values = new NameValueCollection
			{
				{ "image", Convert.ToBase64String(rawImage) },
				{ "type", "base64" },
				{ "description", "Taken from HotS Matchup, the Heroes of the Storm team builder and match analysis tool. http://randomseedgames.com/hots-matchup" }
			};

			byte[] response = w.UploadValues("https://api.imgur.com/3/image.xml", values);

			XDocument xml = XDocument.Load(new MemoryStream(response));
			string xmlString = xml.ToString();

			string[] xmlPageLines = xmlString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			Application.OpenURL(xmlPageLines[22].Replace("<link>", "").Replace(".png</link>", ""));
		}

		File.Delete(Application.persistentDataPath + filename);

		uploadingPanel.SetActive(false);
	}

	public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None)
		{
			for (int i = 0; i < chain.ChainStatus.Length; i++)
			{
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
				{
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build((X509Certificate2)certificate);
					if (!chainIsValid)
					{
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}

	public void OnClickHeroDropdown(int playerNum)
	{
		if (playerNum <= 4)
		{
			match.friendlyTeam.player[playerNum].currentHeroScores.Clear();
		}
		if (playerNum > 4)
		{
			match.enemyTeam.player[playerNum - 5].currentHeroScores.Clear();
		}

		for (int i = 0; i < allHeroDetails.Count; i++)
		{
			for (int j = 0; j < playerHeroDropdown[playerNum].options.Count; j++)
			{
				if (playerHeroDropdown[playerNum].options[j].text == allHeroDetails[i].name)
				{
					int tempScoreForPlayer = 0;
					if (playerNum <= 4)
					{
						tempScoreForPlayer = GetDropdownHeroScore(match.friendlyTeam.player[playerNum], playerHeroDropdown[playerNum].options[j].text);
						match.friendlyTeam.player[playerNum].currentHeroScores.Add(new CurrentHeroScores());
						match.friendlyTeam.player[playerNum].currentHeroScores[i].name = allHeroDetails[i].name;
						match.friendlyTeam.player[playerNum].currentHeroScores[i].score = tempScoreForPlayer;
                    }
					if (playerNum > 4)
					{
						tempScoreForPlayer = GetDropdownHeroScore(match.enemyTeam.player[playerNum - 5], playerHeroDropdown[playerNum].options[j].text);
						match.enemyTeam.player[playerNum - 5].currentHeroScores.Add(new CurrentHeroScores());
						match.enemyTeam.player[playerNum - 5].currentHeroScores[i].name = allHeroDetails[i].name;
						match.enemyTeam.player[playerNum - 5].currentHeroScores[i].score = tempScoreForPlayer;
					}

					playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Label").GetComponent<Text>().text = "<size=21>" + playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Label").GetComponent<Text>().text + " </size><i><size=18><color=grey>" + tempScoreForPlayer.ToString("F0") + "</color></size></i>";
                    if (allHeroDetails[i].subRole == "")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = Color.white;
					}
					else if (allHeroDetails[i].subRole == "Tank")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = tankColor;
					}
					else if (allHeroDetails[i].subRole == "Bruiser")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = bruiserColor;
					}
					else if (allHeroDetails[i].subRole == "Healer")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = healerColor;
					}
					else if (allHeroDetails[i].subRole == "Support")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = supportColor;
					}
					else if (allHeroDetails[i].subRole == "Ambusher")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = ambusherColor;
					}
					else if (allHeroDetails[i].subRole == "Burst Damage")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = burstDamageColor;
					}
					else if (allHeroDetails[i].subRole == "Sustained Damage")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = sustainedDamageColor;
					}
					else if (allHeroDetails[i].subRole == "Siege")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = siegeColor;
					}
					else if (allHeroDetails[i].subRole == "Utility")
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = utilityColor;
					}
					else
					{
						playerHeroDropdown[playerNum].gameObject.transform.Find("Dropdown List").Find("Viewport").Find("Content").Find("Item " + (j) + ": " + allHeroDetails[i].name).Find("Item Background").GetComponent<Image>().color = Color.white;
					}
				}
			}
        }
	}
}