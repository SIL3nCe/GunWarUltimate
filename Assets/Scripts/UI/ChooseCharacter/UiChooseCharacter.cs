using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiChooseCharacter : MonoBehaviour
{
	//
	// Public characters
	public GameObject		CharacterJoe;
	public GameObject		CharacterBilly;
	public GameObject		CharacterYannis;
	public GameObject		CharacterDropos;
	public GameObject		CharacterR3D5;
	public GameObject		CharacterRambi;
	public GameObject		CharacterMargharet;
	public GameObject		CharacterJudy;
	public GameObject		HighLight1;
	public GameObject		HighLight2;
	public Text				CharacterNameP1;
	public Text				CharacterNameP2;
	public GameObject		CharacterPreviewP1;
	public GameObject		CharacterPreviewP2;
	public Material			MaterialJoe;
	public Material			MaterialBilly;
	public Material			MaterialYannis;
	public Material			MaterialDropos;
	public Material			MaterialR3D5;
	public Material			MaterialRambi;
	public Material			MaterialMargharet;
	public Material			MaterialJudy;
	public RawImage			CheckmarkP1;
	public RawImage			CheckmarkP2;
	public Camera			ChooseCharacterCamera;
	public Camera			GameCamera;
	public GameObject		GameScene;

	private struct Character
	{
		public GameObject	character;
		public string		strName;
		public Material		material;
	}
	private Character[]	CharacterArray;

	//
	// Selection-related
	private struct Selector
	{
		public int			iSelectionIndex;
		public Color		color;
		public GameObject	highlightRef;
		public bool			bSelected;
		public GameObject	preview;
		public RawImage		checkmark;
		public Text			name;
	}
	private Selector[] aSelectors;

	private bool bGameLaunched = false;

	// Start is called before the first frame update
	void Start()
	{
		//
		// Create character array
		CharacterArray = new Character[8];
		CharacterArray[0].character = CharacterJudy;		CharacterArray[0].strName = "Judy";			CharacterArray[0].material = MaterialJudy;
		CharacterArray[1].character = CharacterDropos;		CharacterArray[1].strName = "Dropos";		CharacterArray[1].material = MaterialDropos;
		CharacterArray[2].character = CharacterMargharet;	CharacterArray[2].strName = "Margharet";	CharacterArray[2].material = MaterialMargharet;
		CharacterArray[3].character = CharacterYannis;		CharacterArray[3].strName = "Yannis";		CharacterArray[3].material = MaterialYannis;
		CharacterArray[4].character = CharacterRambi;		CharacterArray[4].strName = "Rambi";		CharacterArray[4].material = MaterialRambi;
		CharacterArray[5].character = CharacterBilly;		CharacterArray[5].strName = "Billy";		CharacterArray[5].material = MaterialBilly;
		CharacterArray[6].character = CharacterR3D5;		CharacterArray[6].strName = "R3D5";			CharacterArray[6].material = MaterialR3D5;
		CharacterArray[7].character = CharacterJoe;			CharacterArray[7].strName = "Joe";			CharacterArray[7].material = MaterialJoe;

		//
		// Create selector array
		int iPlayerCount = System.Enum.GetValues(typeof(EPlayerEnum)).Length;
		aSelectors = new Selector[iPlayerCount];
		for(int iPlayerIndex = 0; iPlayerIndex < iPlayerCount; ++iPlayerIndex)
		{
			aSelectors[iPlayerIndex].iSelectionIndex = -1;
			aSelectors[iPlayerIndex].bSelected = false;
		}
		aSelectors[0].color = Color.red;
		aSelectors[1].color = Color.blue;
		aSelectors[0].highlightRef = HighLight1;
		aSelectors[1].highlightRef = HighLight2;
		aSelectors[0].checkmark = CheckmarkP1;
		aSelectors[1].checkmark = CheckmarkP2;
		aSelectors[0].preview = CharacterPreviewP1;
		aSelectors[1].preview = CharacterPreviewP2;
		aSelectors[0].name = CharacterNameP1;
		aSelectors[1].name = CharacterNameP2;
	}

	// Update is called once per frame
	void Update()
	{
		//
		// Selection Movement
		if (!aSelectors[0].bSelected)
		{
			if (Input.GetKeyUp(KeyCode.Q))				{	MoveSelectionPrevious(ref aSelectors[0]);	}
			if (Input.GetKeyUp(KeyCode.D))				{	MoveSelectionNext(ref aSelectors[0]);		}
		}
		if(!aSelectors[1].bSelected)
		{
			if (Input.GetKeyUp(KeyCode.LeftArrow))		{	MoveSelectionPrevious(ref aSelectors[1]);	}
			if (Input.GetKeyUp(KeyCode.RightArrow))		{	MoveSelectionNext(ref aSelectors[1]);		}
		}

		//
		// Selection
		if (Input.GetKeyUp(KeyCode.KeypadEnter))	{	SelectCharacter(ref aSelectors[1]);			}
		if (Input.GetKeyUp(KeyCode.Space))			{	SelectCharacter(ref aSelectors[0]);			}

		//
		// Start condition
		bool bStart = true;
		foreach(Selector s in aSelectors)
		{
			bStart &= s.bSelected;
		}
		if(bStart)
		{
			bGameLaunched = true;
			Invoke("LaunchGame", 3.0f);
		}
	}

	private void MoveSelectionNext(ref Selector selector)
	{
		if (-1 == selector.iSelectionIndex)
		{
			selector.preview.SetActive(true);
			selector.iSelectionIndex = 0;
			selector.highlightRef.SetActive(true);
			selector.highlightRef.GetComponent<Image>().color = selector.color;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[(int)selector.iSelectionIndex].character.transform.position, CharacterArray[(int)selector.iSelectionIndex].character.transform.rotation);
		}
		else if(selector.iSelectionIndex == CharacterArray.Length - 1)
		{
			selector.iSelectionIndex = -1;
			selector.highlightRef.SetActive(false);
			selector.preview.SetActive(false);
		}
		else
		{
			selector.preview.SetActive(true);
			++selector.iSelectionIndex;
			selector.highlightRef.SetActive(true);
			selector.highlightRef.GetComponent<Image>().color = selector.color;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[(int)selector.iSelectionIndex].character.transform.position, CharacterArray[(int)selector.iSelectionIndex].character.transform.rotation);
		}

		UpdateCharacterPreview(selector);
	}

	private void MoveSelectionPrevious(ref Selector selector)
	{
		if (-1 == selector.iSelectionIndex)
		{
			selector.preview.SetActive(true);
			selector.iSelectionIndex = CharacterArray.Length - 1;
			selector.highlightRef.SetActive(true);
			selector.highlightRef.GetComponent<Image>().color = selector.color;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[(int)selector.iSelectionIndex].character.transform.position, CharacterArray[(int)selector.iSelectionIndex].character.transform.rotation);
		}
		else if (selector.iSelectionIndex == 0)
		{
			selector.iSelectionIndex = -1;
			selector.highlightRef.SetActive(false);
			selector.preview.SetActive(false);
		}
		else
		{
			selector.preview.SetActive(true);
			selector.iSelectionIndex = selector.iSelectionIndex - 1;
			selector.highlightRef.SetActive(true);
			selector.highlightRef.GetComponent<Image>().color = selector.color;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[(int)selector.iSelectionIndex].character.transform.position, CharacterArray[(int)selector.iSelectionIndex].character.transform.rotation);
		}

		UpdateCharacterPreview(selector);
	}

	private void UpdateCharacterPreview(Selector selector)
	{
		if(null != selector.name)
		{
			if(-1 == selector.iSelectionIndex)
			{
				selector.name.text = "Choose your character";
			}
			else
			{
				selector.name.text = CharacterArray[selector.iSelectionIndex].strName;
			}
		}

		if(null != selector.preview)
		{
			if (-1 != selector.iSelectionIndex)
			{
				SkinnedMeshRenderer[] meshes = selector.preview.GetComponentsInChildren<SkinnedMeshRenderer>();
				for (int iMeshIndex = 0; iMeshIndex < meshes.Length; ++iMeshIndex)
				{
					meshes[iMeshIndex].material = CharacterArray[selector.iSelectionIndex].material;
				}
			}
		}
	}

	private void SelectCharacter(ref Selector selector)
	{
		if(-1 != selector.iSelectionIndex)
		{
			selector.bSelected = !selector.bSelected;
			if(null != selector.checkmark)
			{
				selector.checkmark.gameObject.SetActive(selector.bSelected);
			}
		}
	}

	private void LaunchGame()
	{
		ChooseCharacterCamera.enabled = false;
		GameCamera.enabled = true;
		GameScene.SetActive(true);
		gameObject.SetActive(false);
	}
}
