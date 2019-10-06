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
	public GameObject	UiPanelStockP1;
	public GameObject	UiPanelStockP1Stock1;
	public GameObject	UiPanelStockP1Stock2;
	public GameObject	UiPanelStockP1Stock3;
	public GameObject	UiPanelStockP1StockX;
	public RawImage		UiImageStockP1StockX;
	public Text			UiTextStockP1StockX;
	public GameObject	UiPanelStockP2;
	public GameObject	UiPanelStockP2Stock1;
	public GameObject	UiPanelStockP2Stock2;
	public GameObject	UiPanelStockP2Stock3;
	public GameObject	UiPanelStockP2StockX;
	public RawImage		UiImageStockP2StockX;
	public Text			UiTextStockP2StockX;

	//
	// Public attributes
	public EInGameEndRules Rules = EInGameEndRules.time;

	public uint RoundDurationMinute = 3;
	public uint RoundDurationSeconds = 0;

	public uint RoundStocksCount = 3;

	public uint? iRoundStocksCount = 3;

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
		if(EInGameEndRules.stock_and_time == Rules || EInGameEndRules.time == Rules)
		{
			RoundDurationMinute = (uint)(Mathf.Min(99, RoundDurationMinute));
			RoundDurationSeconds = (uint)(Mathf.Min(59, RoundDurationSeconds));
			fTotalTime = RoundDurationMinute * 60.0f + RoundDurationSeconds;
			fRemainingTime = fTotalTime;

			UiTime.gameObject.SetActive(true);
		}

		//
		// Stock related
		if (EInGameEndRules.stock_and_time == Rules || EInGameEndRules.stock_and_time == Rules)
		{
			iRoundStocksCount = RoundStocksCount;

			UiPanelStockP1.SetActive(true);
			UiPanelStockP2.SetActive(true);

			if (null != Player1)
			{
				UiImageStockP1StockX.GetComponent<RawImage>().material = Player1.GetComponent<PlayerGameplay>().Head.material;
				UiImageStockP1StockX.GetComponent<RawImage>().material.shader = Shader.Find("UI/Unlit/Detail");

				RawImage[] images1 = UiPanelStockP1Stock1.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images1)
				{
					image.material = Player1.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}

				RawImage[] images2 = UiPanelStockP1Stock2.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images2)
				{
					image.material = Player1.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}

				RawImage[] images3 = UiPanelStockP1Stock3.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images3)
				{
					image.material = Player1.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}
			}
			if (null != Player2)
			{
				UiImageStockP2StockX.GetComponent<RawImage>().material = Player2.GetComponent<PlayerGameplay>().Head.material;
				UiImageStockP2StockX.GetComponent<RawImage>().material.shader = Shader.Find("UI/Unlit/Detail");

				RawImage[] images1 = UiPanelStockP2Stock1.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images1)
				{
					image.material = Player2.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}

				RawImage[] images2 = UiPanelStockP2Stock2.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images2)
				{
					image.material = Player2.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}

				RawImage[] images3 = UiPanelStockP2Stock3.GetComponentsInChildren<RawImage>();
				foreach (RawImage image in images3)
				{
					image.material = Player2.GetComponent<PlayerGameplay>().Head.material;
					image.material.shader = Shader.Find("UI/Unlit/Detail");
				}
			}

			if		(iRoundStocksCount < 2)	{	UiPanelStockP1Stock1.SetActive(true);	UiPanelStockP2Stock1.SetActive(true);	}
			else if	(iRoundStocksCount < 3)	{	UiPanelStockP1Stock2.SetActive(true);	UiPanelStockP2Stock2.SetActive(true);	}
			else if	(iRoundStocksCount < 4)	{	UiPanelStockP1Stock3.SetActive(true);	UiPanelStockP2Stock3.SetActive(true);	}
			else
			{
				UiPanelStockP1StockX.SetActive(true);
				UiPanelStockP2StockX.SetActive(true);

				UiTextStockP1StockX.text = ((int)iRoundStocksCount).ToString("D2");
				UiTextStockP2StockX.text = ((int)iRoundStocksCount).ToString("D2");
			}
		}
		else
		{
			iRoundStocksCount = null;
		}

		//
		// Player-related
		if(null != Player1)
		{
			Player1.SetUiManager(this);
			Player1.SetStocks(iRoundStocksCount);
		}
		if (null != Player2)
		{
			Player2.SetUiManager(this);
			Player2.SetStocks(iRoundStocksCount);
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
				if(EInGameEndRules.time == Rules|| EInGameEndRules.stock_and_time == Rules)
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
