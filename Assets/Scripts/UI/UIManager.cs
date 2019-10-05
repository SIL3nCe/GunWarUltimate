using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public bool RoundDurationEnabled = true;
	public uint RoundDurationMinute = 3;
	public uint RoundDurationSeconds = 0;

	public bool RoundStocksEnabled = false;
	public uint RoundStocksCount = 3;

	//
	// Context-related
	EInGameState eInGameState;

	//
	// Time-related
	private float fTotalTime;
	private float fRemainingTime;

	// Start is called before the first frame update
	void Start()
    {
		//
		// Context
		eInGameState = EInGameState.countdown;

		//
		// UI
		UiP1Percentage.text = "0 %";
		UiP2Percentage.text = "0 %";

		//
		// Time-related
		RoundDurationMinute = (uint)(Mathf.Min(99, RoundDurationMinute));
		RoundDurationSeconds = (uint)(Mathf.Min(59, RoundDurationSeconds));
		fTotalTime = RoundDurationMinute * 60.0f + RoundDurationSeconds;
		fRemainingTime = fTotalTime;
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
				bool bFinished = false;

				//
				// Check stocks
				// TODO

				//
				// Check time
				fRemainingTime = Mathf.Max(fRemainingTime - Time.deltaTime, 0.0f);

				int iMinutesNb = (int) (fRemainingTime / 60);
				int iSecondsNb = (int)(fRemainingTime - iMinutesNb * 60.0f);
				UiTime.text = iMinutesNb.ToString("D2") + ":" + iSecondsNb.ToString("D2");

				if(fRemainingTime == 0.0f)
				{
					bFinished = true;
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
}
