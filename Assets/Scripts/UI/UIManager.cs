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
	// Terrain-related
	public PlayerSpawner Spawner;

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

	struct PlayerUI
	{
		public PlayerGameplay	Player;
		public bool				bDead;

		public Text				UiPercentage;

		public GameObject		UiPanelStock;
		public GameObject		UiPanelStock1;
		public GameObject		UiPanelStock2;
		public GameObject		UiPanelStock3;
		public GameObject		UiPanelStockX;
		public RawImage			UiImageStockX;
		public Text				UiTextStockX;
	}

	private PlayerUI[]	PlayerArray;

	private bool bFinished = false;

	// Start is called before the first frame update
	void Start()
    {
		//
		// Context
		eInGameState = EInGameState.countdown;

		//
		// Time-related
		if (EInGameEndRules.stock_and_time == Rules || EInGameEndRules.time == Rules)
		{
			RoundDurationMinute = (uint)(Mathf.Min(99, RoundDurationMinute));
			RoundDurationSeconds = (uint)(Mathf.Min(59, RoundDurationSeconds));
			fTotalTime = RoundDurationMinute * 60.0f + RoundDurationSeconds;
			fRemainingTime = fTotalTime;

			UiTime.gameObject.SetActive(true);
		}

		//
		// Player-related
		{
			int iPlayerCount = System.Enum.GetValues(typeof(EPlayerEnum)).Length;
			PlayerArray = new PlayerUI[iPlayerCount];
			for (int iPlayerUIIndex = 0; iPlayerUIIndex < iPlayerCount; ++iPlayerUIIndex)
			{
				PlayerArray[iPlayerUIIndex].Player = null;
			}

			if (null != Player1)
			{
				PlayerArray[(int)(EPlayerEnum.p1)].Player = Player1;
				PlayerArray[(int)(EPlayerEnum.p1)].bDead = false;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPercentage		= UiP1Percentage;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPanelStock		= UiPanelStockP1;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPanelStock1	= UiPanelStockP1Stock1;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPanelStock2	= UiPanelStockP1Stock2;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPanelStock3	= UiPanelStockP1Stock3;
				PlayerArray[(int)(EPlayerEnum.p1)].UiPanelStockX	= UiPanelStockP1StockX;
				PlayerArray[(int)(EPlayerEnum.p1)].UiImageStockX	= UiImageStockP1StockX;
				PlayerArray[(int)(EPlayerEnum.p1)].UiTextStockX	= UiTextStockP1StockX;
			}
			if (null != Player2)
			{
				PlayerArray[(int)(EPlayerEnum.p2)].Player = Player2;
				PlayerArray[(int)(EPlayerEnum.p2)].bDead = false;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPercentage		= UiP2Percentage;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock		= UiPanelStockP2;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock1	= UiPanelStockP2Stock1;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock2	= UiPanelStockP2Stock2;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock3	= UiPanelStockP2Stock3;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStockX	= UiPanelStockP2StockX;
				PlayerArray[(int)(EPlayerEnum.p2)].UiImageStockX	= UiImageStockP2StockX;
				PlayerArray[(int)(EPlayerEnum.p2)].UiTextStockX		= UiTextStockP2StockX;
			}
			foreach (PlayerUI playerUI in PlayerArray)
			{
				if (null != playerUI.Player)
				{
					playerUI.Player.SetUiManager(this);
					playerUI.Player.SetStocks(iRoundStocksCount);

					playerUI.UiPercentage.text = 0.ToString("D3") + "%";
				}
			}
		}

		//
		// Stock related
		if (EInGameEndRules.stock_and_time == Rules || EInGameEndRules.stock_and_time == Rules)
		{
			iRoundStocksCount = RoundStocksCount;

			foreach (PlayerUI playerUI in PlayerArray)
			{
				if (null != playerUI.Player)
				{
					playerUI.UiPanelStock.SetActive(true);

					playerUI.UiImageStockX.GetComponent<RawImage>().material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
					playerUI.UiImageStockX.GetComponent<RawImage>().material.shader = Shader.Find("UI/Unlit/Detail");

					RawImage[] images1 = playerUI.UiPanelStock1.GetComponentsInChildren<RawImage>();
					foreach (RawImage image in images1)
					{
						image.material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
						image.material.shader = Shader.Find("UI/Unlit/Detail");
					}

					RawImage[] images2 = playerUI.UiPanelStock2.GetComponentsInChildren<RawImage>();
					foreach (RawImage image in images2)
					{
						image.material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
						image.material.shader = Shader.Find("UI/Unlit/Detail");
					}

					RawImage[] images3 = playerUI.UiPanelStock3.GetComponentsInChildren<RawImage>();
					foreach (RawImage image in images3)
					{
						image.material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
						image.material.shader = Shader.Find("UI/Unlit/Detail");
					}
					
					if		(iRoundStocksCount < 2)	{	playerUI.UiPanelStock1.SetActive(true);	}
					else if	(iRoundStocksCount < 3)	{	playerUI.UiPanelStock2.SetActive(true);	}
					else if	(iRoundStocksCount < 4)	{	playerUI.UiPanelStock3.SetActive(true);	}
					else
					{
						playerUI.UiPanelStockX.SetActive(true);

						playerUI.UiTextStockX.text = ((int)iRoundStocksCount).ToString("D2");
					}
				}
			}

		}
		else
		{
			iRoundStocksCount = null;
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
				for(int iPlayerUIIndex = 0; iPlayerUIIndex < PlayerArray.Length; ++iPlayerUIIndex)
				{
					if(PlayerArray[iPlayerUIIndex].bDead)
					{
						Transform newPose = Spawner.GetSpawnLocation();
						PlayerArray[iPlayerUIIndex].Player.SetNextSpawnLocation(newPose);

						PlayerArray[iPlayerUIIndex].bDead = false;
					}
					bFinished |= PlayerArray[iPlayerUIIndex].bDead;
				}

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
						bFinished |= true;
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
		if (null != stocks && null != PlayerArray[(int)(player.playerEnum)].Player)
		{
			PlayerArray[(int)(player.playerEnum)].bDead = true;
			PlayerArray[(int)(player.playerEnum)].UiPercentage.text =  0.ToString("D3") + "%";
			if (3 < stocks)
			{
				PlayerArray[(int)(player.playerEnum)].UiTextStockX.text = ((int)(stocks)).ToString("D2");
			}
			else if (2 < stocks) // 3+ -> 3 = only panel 3 images
			{
				PlayerArray[(int)(player.playerEnum)].UiPanelStockX.SetActive(false);
				PlayerArray[(int)(player.playerEnum)].UiPanelStock3.SetActive(true);
			}
			else if (1 < stocks) // 3 -> 2 = only panel 3 images
			{
				PlayerArray[(int)(player.playerEnum)].UiPanelStock3.GetComponent<StockDisplay>().Image3.enabled = false;
			}
			else if (0 < stocks) // 2 -> 1, can be 3/2
			{
				if (PlayerArray[(int)player.playerEnum].UiPanelStock3.activeInHierarchy)
				{
					PlayerArray[(int)(player.playerEnum)].UiPanelStock3.GetComponent<StockDisplay>().Image2.enabled = false;
				}
				else 
				{
					PlayerArray[(int)(player.playerEnum)].UiPanelStock2.GetComponent<StockDisplay>().Image2.enabled = false;
				}
			}
			else
			{
				foreach(PlayerUI playerUI in PlayerArray)
				{
					bFinished = true;
					if(null != playerUI.Player)
					{
						playerUI.UiPanelStock3.SetActive(false);
						playerUI.UiPanelStock2.SetActive(false);
						playerUI.UiPanelStock1.SetActive(false);
					}
				}
			}
		}
	}

	public void OnPlayerDamageTaken(PlayerGameplay player)
	{
		if (null != PlayerArray[(int)(player.playerEnum)].Player)
		{
			PlayerArray[(int)(player.playerEnum)].UiPercentage.text = ((int)player.GetPercentage()).ToString("D3") + "%";
		}
	}
}
