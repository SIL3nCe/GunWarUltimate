using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EInGameEndRules
{
	stock,
	time,
	stock_and_time,
}

public enum EInGameState
{
	countdown,
	playing,
	paused,
	finished,
}

public class UIManager : MonoBehaviour
{
	//
	// Public UI
	public Text UiTime;
	public Text UiP1Percentage;
	public Text UiP2Percentage;

	//
	// Public attributes
	public EInGameEndRules eRules = EInGameEndRules.time;

	public uint RoundDurationMinute = 3;
	public uint RoundDurationSeconds = 0;

	public uint? RoundStocksCount = 3;

	//
	// Context-related
	EInGameState eInGameState;

	//
	// Time-related
	private float fTotalTime;
	private float fRemainingTime;

	//
	// Player-related
	public PlayerGameplay	Player1;
	public PlayerGameplay	Player2;
	private bool			bP1Dead;
	private bool			bP2Dead;

	// Start is called before the first frame update
	void Start()
    {
		//
		// Context
		eInGameState = EInGameState.countdown;

		//
		// UI
		UiP1Percentage.text = 0.ToString("D3") + "%";
		UiP2Percentage.text = 0.ToString("D3") + "%";

		//
		// Time-related
		if(EInGameEndRules.stock_and_time == eRules || EInGameEndRules.time == eRules)
		{
			RoundDurationMinute = (uint)(Mathf.Min(99, RoundDurationMinute));
			RoundDurationSeconds = (uint)(Mathf.Min(59, RoundDurationSeconds));
			fTotalTime = RoundDurationMinute * 60.0f + RoundDurationSeconds;
			fRemainingTime = fTotalTime;
		}
		else
		{
			UiTime.enabled = false;
		}

		//
		// Stock related
		bool bUseStocks = EInGameEndRules.stock_and_time == eRules || EInGameEndRules.stock_and_time == eRules;
		if (bUseStocks)
		{

		}
		else
		{
			RoundStocksCount = null;
		}

		//
		// Player-related
		if(null != Player1)
		{
			Player1.SetUiManager(this);
			Player1.SetStocks(RoundStocksCount);
		}
		if (null != Player2)
		{
			Player2.SetUiManager(this);
			Player2.SetStocks(RoundStocksCount);
		}
	}

    // Update is called once per frame
    void Update()
    {
		//
		// Inputs
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(EInGameState.paused == eInGameState)
			{
				eInGameState = EInGameState.playing;
			}
			else
			{
				eInGameState = EInGameState.paused;
			}
		}

		switch(eInGameState)
		{
			case EInGameState.countdown:
			{
				eInGameState = EInGameState.playing;
				break;
			}
			case EInGameState.playing:
			{
				//
				// Check stocks
				bool bFinished = bP1Dead || bP2Dead;

				//
				// Check time
				if(EInGameEndRules.time == eRules|| EInGameEndRules.stock_and_time == eRules)
				{
					fRemainingTime = Mathf.Max(fRemainingTime - Time.deltaTime, 0.0f);

					int iMinutesNb = (int) (fRemainingTime / 60);
					int iSecondsNb = (int)(fRemainingTime - iMinutesNb * 60.0f);
					UiTime.text = iMinutesNb.ToString("D2") + ":" + iSecondsNb.ToString("D2");

					if(fRemainingTime == 0.0f)
					{
						bFinished = true;
					}
				}


				if(bFinished)
				{
					eInGameState = EInGameState.finished;
				}
				break;
			}
			case EInGameState.paused:
			case EInGameState.finished:
			{
				break;
			}
		}
	}

	public void OnPlayerDied(PlayerGameplay player)
	{
		uint? stocks = player.GetRemainingStocks();
		if (null != stocks && 0 == stocks)
		{
			if (PlayerEnum.p1 == player.playerEnum)
			{
				bP1Dead = true;
			}
			else if (PlayerEnum.p2 == player.playerEnum)
			{
				bP2Dead = true;
			}
		}
	}

	public void OnPlayerDamageTaken(PlayerGameplay player)
	{
		if(PlayerEnum.p1 == player.playerEnum)
		{
			UiP1Percentage.text = ((int)(player.GetPercentage())).ToString("D3") + "%";
		}
		else if (PlayerEnum.p2 == player.playerEnum)
		{
			UiP2Percentage.text = ((int)(player.GetPercentage())).ToString("D3") + "%";
		}
	}
}
