using UnityEngine;
using System.Collections;

public class CharactorController : MonoBehaviour {
	public GameObject sprite;
	public GameContrller gameContrller;

	Animator animator;
	AudioSource sound;
	bool playNow = false;
	float startTime = 0.0f;
	float endTime = 0.01f;
	bool playAll = false;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
		animator = sprite.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Play() {
		if (!playNow) {
			float thisTime = Time.time - gameContrller.getTime ();
			if (thisTime < 8.2f) {
				if (sprite.tag == "OsomatsuSprite") {
					startTime = 1.9f;
					endTime = 2.9f;
				}
				if (sprite.tag == "KaramatsuSprite") {
					startTime = 2.9f;
					endTime = 3.9f;
				}
				if (sprite.tag == "ChoromatsuSprite") {
					startTime = 4.0f;
					endTime = 4.9f;
				}
				if (sprite.tag == "IchimatsuSprite") {
					startTime = 4.9f;
					endTime = 5.9f;
				}
				if (sprite.tag == "JushimatsuSprite") {
					startTime = 5.95f;
					endTime = 7.0f;
				}
				if (sprite.tag == "TodomatsuSprite") {
					startTime = 7.1f;
					endTime = 8.1f;
				}
			} else if (thisTime < 12.1f) {
				startTime = 8.2f;
				endTime = 12.1f;
				gameContrller.playAll ();
				playAll = true;
			} else {
				startTime = 0.0f;
				endTime = 0.01f;
			}

			animator.SetBool ("Play", true);
			sound.time = startTime;
			sound.Play ();
			playNow = true;
			StartCoroutine(PlayStop(endTime - startTime));
		}
	}

	public void PlayAllOff() {
		animator.SetBool ("Play", false);
	}

	public void PlayAll() {
		animator.SetBool ("Play", true);
	}

	IEnumerator PlayStop(float time) {
		yield return new WaitForSeconds (time);
		sound.Stop();
		playNow = false;
		animator.SetBool ("Play", false);
		if (playAll) {
			gameContrller.playAllOff ();
			playAll = false;
		}
	}
}
