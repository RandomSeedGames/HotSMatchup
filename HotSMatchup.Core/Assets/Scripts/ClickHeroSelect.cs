using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickHeroSelect : MonoBehaviour
{

	public bool recommendation = false;
	public int playerNum = -1;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!recommendation && playerNum != PullDataTest.Instance.currentlyPickingPlayer)
		{
			playerNum = PullDataTest.Instance.currentlyPickingPlayer;
        }
		//gameObject.transform.parent.transform.parent.gameObject.name;
	}

	public void OnClickSelect()
	{
		for (int i = 0; i < PullDataTest.Instance.playerHeroBorder.Length; i++)
		{
			PullDataTest.Instance.playerHeroBorder[i].color = Color.white;
		}

		if (!recommendation)
		{
			PullDataTest.Instance.OnSelectHero(gameObject.transform.parent.transform.parent.gameObject.name);
		}
		else
		{
			if (playerNum <= 4)
			{
				PullDataTest.Instance.UpdateFriendlyPlayerHeroNameFromPortrait(playerNum, gameObject.transform.parent.transform.parent.gameObject.name);
			}
			else
			{
				PullDataTest.Instance.UpdateEnemyPlayerHeroNameFromPortrait(playerNum, gameObject.transform.parent.transform.parent.gameObject.name);
			}
			PullDataTest.Instance.heroPickPanel.SetActive(false);
			PullDataTest.Instance.HideHoverPanel();
		}
	}

	public void OnHoverPortraitSelf()
	{
		PullDataTest.Instance.HoverHeroPortrait(playerNum, gameObject.transform.parent.transform.parent.gameObject.name);
	}

	public void HidePortraitSelf()
	{
		PullDataTest.Instance.HideHoverPanel();
    }
}

