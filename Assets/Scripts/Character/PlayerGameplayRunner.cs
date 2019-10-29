using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayRunner : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip[] m_aAudioClipsScream;
    public AudioClip m_aAudioClipImplose;
    private AudioSource m_audioSource;
    
	private Vector3 SpawnLocation;
    
    private Rigidbody rigidBody;

    [HideInInspector]
    public Material material;

    void Start()
    {
        if (material != null)
        {
            SkinnedMeshRenderer[] meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int iMeshIndex = 0; iMeshIndex < meshes.Length; ++iMeshIndex)
            {
                meshes[iMeshIndex].material = material;
            }
        }

        rigidBody = gameObject.GetComponent<Rigidbody>();

        SpawnLocation = rigidBody.position;

        m_audioSource = GetComponent<AudioSource>();
    }

    public void OnDie()
    {
        //
        // Emit die sounds
        int iSound = Random.Range(0, m_aAudioClipsScream.Length);
        m_audioSource.PlayOneShot(m_aAudioClipsScream[iSound], 0.6f);
        m_audioSource.PlayOneShot(m_aAudioClipImplose, 1.0f);

		//
		// Hide Character during death
		// !! SetActive on entire prefab will block Animator and let skeleton in mid-anim positions
		// Find a better way to achieve this if possible
		HideMeshes();

        // Disable inputs
        GetComponent<PlayerCharacterControllerRunner>().OnDisable();

        Invoke("Spawn", 1.0f);
    }

    private void Spawn()
    {
        //
        // Unhide Character during death
        // !! SetActive on entire prefab will block Animator and let skeleton in mid-anim positions
        // Find a better way to achieve this if possible
        ShowMeshes();

        // Enable inputs
        GetComponent<PlayerCharacterControllerRunner>().OnEnable();

        gameObject.transform.SetPositionAndRotation(SpawnLocation, gameObject.transform.rotation);
        if (null != rigidBody)
        {
            rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rigidBody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        }

        gameObject.layer = LayerMask.NameToLayer("RunnerBlock");

        StartCoroutine(Blink(4));
    }

    public IEnumerator Blink(int nBlink)
    {
        int blink = nBlink;
        while (0 != blink--)
        {
            HideMeshes();
            yield return new WaitForSeconds(0.3f);
            ShowMeshes();
            yield return new WaitForSeconds(0.3f);
        }

        gameObject.layer = LayerMask.NameToLayer("Character");
    }

    public void HideMeshes()
	{
		foreach (var component in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			component.enabled = false;
		}
	}

	public void ShowMeshes()
	{
		foreach (var component in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			component.enabled = true;
		}
	}
}
