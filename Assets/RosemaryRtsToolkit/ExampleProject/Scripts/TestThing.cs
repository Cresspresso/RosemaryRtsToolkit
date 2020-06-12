using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestThing : MonoBehaviour
{
	public TestThing child;

	private void Log(string eventName)
	{
		Debug.Log($"{eventName} for '{name}'", this);
	}

	private void Awake()
	{
		Log("Awake");
	}

	private void Start()
	{
		Log("Start");
		if (child)
		{
			child.gameObject.SetActive(false);
			Log("Disabled Child");
		}
	}

	private bool b = false;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			b = !b;
			Log("Toggling Child");
			if (child) { child.gameObject.SetActive(b); }
			Log("Toggled Child");
		}
	}

	private void OnEnable()
	{
		Log("OnEnable");
	}

	private void OnDisable()
	{
		Log("OnDisable");
	}

	private void OnDestroy()
	{
		Log("OnDestroy");
	}
}
