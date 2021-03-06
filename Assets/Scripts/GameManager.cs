﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Vector3 SpawnPoint = Vector3.zero;
	public float SpawnRadius = 10f;

	public int gameHealth;
	private int incrementAmount = 5;
	private int decrementAmount = -2;

	//in seconds
	private float minWaitTime = 6.0f;
	private float maxWaitTime = 7.0f;

	//game level info
	private int cur_level = 0;
	private float level_length = 15.0f;

	public GameObject HeartPrefab;

	public static GameManager Instance;

	void Awake() {
		if (Instance == null)
			Instance = this;
	}

	// Use this for initialization
	void Start () {
		gameHealth = 50;
		StartCoroutine (ChangeGameLevels ());
		StartCoroutine (SpawnHearts ());
	}

	IEnumerator ChangeGameLevels() {
		while (true) {
			yield return new WaitForSeconds (level_length);
			cur_level++;
			setTimes (cur_level);
		}
	}

	void setTimes(int level) {
		switch (level) {
			case 1:
				minWaitTime = 4.0f;
				maxWaitTime = 5.0f;
				level_length = 7.0f;
				break;	
			case 2:
				minWaitTime = 2.0f;
				maxWaitTime = 3.0f;
				level_length = 4.0f;
				break;
			default:
				minWaitTime = 1.0f;
				maxWaitTime = 2.0f;
				level_length = 4.0f;
				break;
		}
	}
		
	IEnumerator SpawnHearts() {
		while (true) {
			CreateHeart ();
			yield return new WaitForSeconds (Random.Range(minWaitTime, maxWaitTime));
		}
	}

	public Heart CreateHeart() {
        //SMSManager.SMSData data = new SMSManager.SMSData(0, "asdfa", "Hello World");
        SMSManager.SMSData data = SMSManager.Instance.GetRandomSMS ();
        if (data != null) {
			Vector3 location = (new Vector3 (Random.Range (-1f, 1f), 0f, Random.Range (-1f, 1f))).normalized * SpawnRadius + SpawnPoint; 
			Heart heart = (Instantiate (HeartPrefab, location, Quaternion.identity) as GameObject).GetComponentInChildren<Heart> ();
			heart.SetSMSData (data);
			return heart;
		}
		return null;
	}

	public void decrementHealth() {
		gameHealth += decrementAmount;
		if (gameHealth < 0)
			gameHealth = 0;
	}

	public void incrementHealth() {
		gameHealth += incrementAmount;
		if (gameHealth > 100)
			gameHealth = 100;
	}
}
