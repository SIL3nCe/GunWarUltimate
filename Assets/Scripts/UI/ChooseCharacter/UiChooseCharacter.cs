using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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

	private struct Character
	{
		public GameObject	character;
		public string		strName;
		public Material		material;

        public Character(GameObject charac, string name, Material mat)
        {
            character = charac;
            strName = name;
            material = mat;
        }
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

    // Start is called before the first frame update
    void Start()
    {
        //
        // Create character array
        CharacterArray = new Character[8];
        CharacterArray[0] = new Character(CharacterDropos, "Dropos", MaterialDropos);
        CharacterArray[1] = new Character(CharacterYannis, "Yannis", MaterialYannis);
        CharacterArray[2] = new Character(CharacterBilly, "Billy", MaterialBilly);
        CharacterArray[3] = new Character(CharacterJoe, "Joe", MaterialJoe);
        CharacterArray[4] = new Character(CharacterJudy, "Judy", MaterialJudy);
        CharacterArray[5] = new Character(CharacterMargharet, "Margharet", MaterialMargharet);
        CharacterArray[6] = new Character(CharacterRambi, "Rambi", MaterialRambi);
        CharacterArray[7] = new Character(CharacterR3D5, "R3D5", MaterialR3D5);

        //
        // Create selector array
        int iPlayerCount = System.Enum.GetValues(typeof(EPlayerEnum)).Length;
        aSelectors = new Selector[iPlayerCount];
        for (int iPlayerIndex = 0; iPlayerIndex < iPlayerCount; ++iPlayerIndex)
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
        // TODO Use InputAction here
        if (!aSelectors[0].bSelected)
        {
            if (Input.GetKeyUp(KeyCode.Q))
                MoveSelectionPrevious(ref aSelectors[0], 1);
            if (Input.GetKeyUp(KeyCode.D))
                MoveSelectionNext(ref aSelectors[0], 1);
            if (Input.GetKeyUp(KeyCode.S))
                MoveSelectionPrevious(ref aSelectors[0], 4);
            if (Input.GetKeyUp(KeyCode.Z))
                MoveSelectionNext(ref aSelectors[0], 4);
        }

        if (!aSelectors[1].bSelected)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                MoveSelectionPrevious(ref aSelectors[1], 1);
            if (Input.GetKeyUp(KeyCode.RightArrow))
                MoveSelectionNext(ref aSelectors[1], 1);
            if (Input.GetKeyUp(KeyCode.UpArrow))
                MoveSelectionPrevious(ref aSelectors[1], 4);
            if (Input.GetKeyUp(KeyCode.DownArrow))
                MoveSelectionNext(ref aSelectors[1], 4);
        }

        //
        // Selection
        if (Input.GetKeyUp(KeyCode.Keypad0))
            SelectCharacter(ref aSelectors[1]);
        if (Input.GetKeyUp(KeyCode.Space))
            SelectCharacter(ref aSelectors[0]);

        //
        // Start condition
        bool bStart = true;
        foreach (Selector s in aSelectors)
        {
            bStart &= s.bSelected;
        }
        if (bStart)
        {
            Invoke("LaunchGame", 3.0f);
        }
    }

	private void MoveSelectionNext(ref Selector selector, int valInc)
	{
		if (-1 == selector.iSelectionIndex)
		{
			selector.preview.SetActive(true);
			selector.iSelectionIndex = 0;
            selector.highlightRef.SetActive(true);
            selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[selector.iSelectionIndex].character.transform.position, CharacterArray[selector.iSelectionIndex].character.transform.rotation);
            selector.highlightRef.transform.position = new Vector3(selector.highlightRef.transform.position.x, selector.highlightRef.transform.position.y, 10.0f);
        }
		else
		{
			selector.iSelectionIndex = (selector.iSelectionIndex + valInc) % CharacterArray.Length;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[selector.iSelectionIndex].character.transform.position, CharacterArray[selector.iSelectionIndex].character.transform.rotation);
            selector.highlightRef.transform.position = new Vector3(selector.highlightRef.transform.position.x, selector.highlightRef.transform.position.y, 10.0f);
        }

		UpdateCharacterPreview(selector);
	}

	private void MoveSelectionPrevious(ref Selector selector, int valInc)
	{
		if (-1 == selector.iSelectionIndex)
		{
			selector.preview.SetActive(true);
			selector.iSelectionIndex = CharacterArray.Length - 1;
			selector.highlightRef.SetActive(true);
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[selector.iSelectionIndex].character.transform.position, CharacterArray[selector.iSelectionIndex].character.transform.rotation);
            selector.highlightRef.transform.position = new Vector3(selector.highlightRef.transform.position.x, selector.highlightRef.transform.position.y, 10.0f);
        }
		else
		{
			selector.iSelectionIndex = ((selector.iSelectionIndex - valInc) + CharacterArray.Length) % CharacterArray.Length;
			selector.highlightRef.transform.SetPositionAndRotation(CharacterArray[selector.iSelectionIndex].character.transform.position, CharacterArray[selector.iSelectionIndex].character.transform.rotation);
            selector.highlightRef.transform.position = new Vector3(selector.highlightRef.transform.position.x, selector.highlightRef.transform.position.y, 10.0f);
        }

		UpdateCharacterPreview(selector);
	}

	private void UpdateCharacterPreview(Selector selector)
	{
		if (null != selector.name)
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
        SceneManager.LoadScene("super_awesome_level", LoadSceneMode.Single);

        PlayerStaticData.P1Material = CharacterArray[aSelectors[0].iSelectionIndex].material;
        PlayerStaticData.P2Material = CharacterArray[aSelectors[1].iSelectionIndex].material;
    }
}
