using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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

    [Header("Victory")]
	public GameObject	UiPanelVictory;
	public Text			UiTextWinner;
    public UiChooseCharacter ChampSelectionObject;

	//
	// Terrain-related
    [Header("Terrain")]
	public SpawnerManager spawnerManager;

	//
	// Public attributes
    [Header("Rules")]
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
	[Header("Players")]
	public PlayerGameplay	Player1;
	public PlayerGameplay	Player2;

	struct PlayerUI
	{
		public PlayerGameplay	Player;

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
	void OnEnable()
    {
		//
		// Context
		spawnerManager.Initialize();
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
				PlayerArray[(int)(EPlayerEnum.p2)].UiPercentage		= UiP2Percentage;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock		= UiPanelStockP2;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock1	= UiPanelStockP2Stock1;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock2	= UiPanelStockP2Stock2;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStock3	= UiPanelStockP2Stock3;
				PlayerArray[(int)(EPlayerEnum.p2)].UiPanelStockX	= UiPanelStockP2StockX;
				PlayerArray[(int)(EPlayerEnum.p2)].UiImageStockX	= UiImageStockP2StockX;
				PlayerArray[(int)(EPlayerEnum.p2)].UiTextStockX		= UiTextStockP2StockX;
			}

			ArrayList aSpawnLocations = new ArrayList();
			foreach (PlayerUI playerUI in PlayerArray)
			{
				if (null != playerUI.Player)
				{
					//
					// Update properties
					playerUI.Player.SetUiManager(this);
					playerUI.Player.SetStocks(iRoundStocksCount);

					//
					// Update % text
					playerUI.UiPercentage.text = 0.ToString("D3") + "%";

					//
					// Set CountdownSpawn
					Transform newPose = spawnerManager.GetSpawnLocation(SpawnerManager.ESpawner.player);
					int iMaxLoopCount = 5;
					int iLoopIndex = 0;
					while (aSpawnLocations.Contains(newPose))
					{
						if(iLoopIndex > iMaxLoopCount)
						{
							// Must at least have PlayerCount spawner locations
							Assert.IsFalse(true); 
						}
						++iLoopIndex;
						newPose = spawnerManager.GetSpawnLocation(SpawnerManager.ESpawner.player);
					}
					playerUI.Player.SetNextSpawnLocation(newPose);

					//
					// Hide character
					playerUI.Player.HideMeshes();
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
                        image.enabled = true;
                        image.material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
						image.material.shader = Shader.Find("UI/Unlit/Detail");
					}

					RawImage[] images2 = playerUI.UiPanelStock2.GetComponentsInChildren<RawImage>();
					foreach (RawImage image in images2)
                    {
                        image.enabled = true;
                        image.material = playerUI.Player.GetComponent<PlayerGameplay>().Head.material;
						image.material.shader = Shader.Find("UI/Unlit/Detail");
					}

					RawImage[] images3 = playerUI.UiPanelStock3.GetComponentsInChildren<RawImage>();
					foreach (RawImage image in images3)
                    {
                        image.enabled = true;
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

				//
				// Disable spawning
				spawnerManager.DisableSpawning();
			}
		}

        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.R))
            { // Go on victory screen
                eInGameState = EInGameState.finished;
                fRemainingTime = 0.0f;
            }
        }

        switch (eInGameState)
		{
			case EInGameState.countdown:
			{
				eInGameState = EInGameState.playing;

				//
				// Enable spawning
				spawnerManager.EnableSpawning();

				break;
			}
			case EInGameState.playing:
			{
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
                    fRemainingTime = 0.0f;

					//
					// Disable spawning
					spawnerManager.DisableSpawning();
				}
				break;
			}
			case EInGameState.paused:
			{
				break;
			}
			case EInGameState.finished:
			{
                if (fRemainingTime == 0.0f)
                {
                    UiPanelVictory.SetActive(true);
                    if (0 == Player1.GetRemainingStocks())
                    {
                        UiTextWinner.text = "P2";
                    }
                    else if (0 == Player2.GetRemainingStocks())
                    {
                        UiTextWinner.text = "P1";
                    }
                    else
                    {
                        if (Player1.GetRemainingStocks() == Player2.GetRemainingStocks())
                        {
                            if (Player1.GetPercentage() > Player2.GetPercentage())
                            {
                                UiTextWinner.text = "P2";
                            }
                            else
                            {
                                UiTextWinner.text = "P1";
                            }
                        }
                        else
                        {
                            if (Player1.GetRemainingStocks() > Player2.GetRemainingStocks())
                            {
                                UiTextWinner.text = "P1";
                            }
                            else
                            {
                                UiTextWinner.text = "P2";
                            }
                        }
                    }
                }

                fRemainingTime += Time.deltaTime;

                if (fRemainingTime >= 2.0f && (Input.GetKeyUp(KeyCode.Keypad0) || Input.GetKeyUp(KeyCode.Space)))
                { // Return to champ select
                    SceneManager.LoadScene("ChampSelect", LoadSceneMode.Single);
                }
                break;
			}
		}
	}

	public void OnPlayerDied(PlayerGameplay player)
	{
		uint? stocks = player.GetRemainingStocks();
        int arrayId = (int)(player.playerEnum);
        if (null != stocks && null != PlayerArray[arrayId].Player)
		{
			PlayerArray[arrayId].UiPercentage.text =  0.ToString("D3") + "%";
			if (3 < stocks)
			{
				PlayerArray[arrayId].UiTextStockX.text = ((int)(stocks)).ToString("D2");
			}
			else if (2 < stocks) // 3+ -> 3 = only panel 3 images
			{
				PlayerArray[arrayId].UiPanelStockX.SetActive(false);
				PlayerArray[arrayId].UiPanelStock3.SetActive(true);
			}
			else if (1 < stocks) // 3 -> 2 = only panel 3 images
			{
				PlayerArray[arrayId].UiPanelStock3.GetComponent<StockDisplay>().Image3.enabled = false;
			}
			else if (0 < stocks) // 2 -> 1, can be 3/2
			{
				if (PlayerArray[arrayId].UiPanelStock3.activeInHierarchy)
				{
					PlayerArray[arrayId].UiPanelStock3.GetComponent<StockDisplay>().Image2.enabled = false;
				}
				else 
				{
					PlayerArray[arrayId].UiPanelStock2.GetComponent<StockDisplay>().Image2.enabled = false;
				}
			}
			else
			{
				bFinished = true;
				foreach(PlayerUI playerUI in PlayerArray)
				{
					if(null != playerUI.Player)
					{
						playerUI.UiPanelStock3.SetActive(false);
						playerUI.UiPanelStock2.SetActive(false);
						playerUI.UiPanelStock1.SetActive(false);
					}
				}
			}

            if (!bFinished)
            {
                Transform newPose = spawnerManager.GetSpawnLocation(SpawnerManager.ESpawner.player);
                PlayerArray[arrayId].Player.SetNextSpawnLocation(newPose);
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
