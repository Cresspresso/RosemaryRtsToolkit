using Cresspresso.Rosemary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RosemaryGameOverSceneLoader : MonoBehaviour
{
	public float delay = 3.0f;

#pragma warning disable CS0649
	[SerializeField]
	private string m_sceneName = "Sample Scene";
#pragma warning restore CS0649
	public string sceneName {
		get => m_sceneName;
		set
		{
			m_sceneName = value;
			FindScene();
		}
	}

	private Scene sceneToLoad;

	private void FindScene()
	{
		sceneToLoad = SceneManager.GetSceneByName(sceneName);
		if (!sceneToLoad.IsValid())
		{
			Debug.LogError("Invalid scene name: " + sceneName, this);
		}
	}

	private void Awake()
	{
		RosemaryObjectiveManager.instance.onGameOverForPlayer += OnGameOver;
		FindScene();
	}

	private void OnGameOver(FactionLifeOutcome outcome)
	{
		FindScene();
		StartCoroutine(Co_LoadScene());
	}

	private void LoadScene()
	{
		if (!sceneToLoad.IsValid())
		{
			FindScene();
		}

		if (sceneToLoad.IsValid())
		{
			SceneManager.LoadScene(sceneToLoad.buildIndex);
		}
	}

	private IEnumerator Co_LoadScene()
	{
		yield return new WaitForSeconds(delay);
		LoadScene();
	}
}
