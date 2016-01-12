using UnityEngine;
using System.Collections;

public class GameContrller : MonoBehaviour {
	public GameObject Osomatsu;
	public GameObject Karamatsu;
	public GameObject Choromatsu;
	public GameObject Ichimatsu;
	public GameObject Jushimatsu;
	public GameObject Todomatsu;

	AudioSource sound;

	float time = -1.0f;
	float time0 = 0.0f;
	public float getTime() {
		return time;
	}

	public void playAll() {
		Osomatsu.SendMessage("PlayAll");
		Karamatsu.SendMessage("PlayAll");
		Choromatsu.SendMessage("PlayAll");
		Ichimatsu.SendMessage("PlayAll");
		Jushimatsu.SendMessage("PlayAll");
		Todomatsu.SendMessage("PlayAll");
	}

	public void playAll2() {
		Osomatsu.SendMessage("PlayAll");
		Karamatsu.SendMessage("PlayAll");
		Choromatsu.SendMessage("PlayAll");
		Ichimatsu.SendMessage("PlayAll");
		Jushimatsu.SendMessage("PlayAll");
	}

	public void playAllOff() {
		Osomatsu.SendMessage("PlayAllOff");
		Karamatsu.SendMessage("PlayAllOff");
		Choromatsu.SendMessage("PlayAllOff");
		Ichimatsu.SendMessage("PlayAllOff");
		Jushimatsu.SendMessage("PlayAllOff");
		Todomatsu.SendMessage("PlayAllOff");
	}

	public void playBGM() {
		if (!sound.isPlaying) {
			time = Time.time - time0;
			sound.time = time0;
			sound.Play ();
		}
	}
	public void stopBGM() {
		if (sound.isPlaying) {
			time0 = Time.time - time;
			time = -1.0f;
			sound.Stop ();
		}
	}
	public void prevBGM() {
		if (sound.isPlaying) {
			time0 = Time.time - time - 5.0f;
			if (time0 <= 0.0f) time0 = 0.0f;
			time = Time.time - time0;
			sound.time = time0;
			sound.Play ();
		} else {
			time0 -= 5.0f;
			if (time0 <= 0.0f) time0 = 0.0f;
		}
	}
	public void nextBGM() {
		if (sound.isPlaying) {
			time0 = Time.time - time + 5.0f;
			if (time0 >= 276.0f) time0 = 276.0f;
			time = Time.time - time0;
			sound.time = time0;
			sound.Play ();
		} else {
			time0 += 5.0f;
			if (time0 >= 276.0f) time0 = 276.0f;
		}
	}

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.gameObject.tag == "Osomatsu") {
					Osomatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Karamatsu") {
					Karamatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Choromatsu") {
					Choromatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Ichimatsu") {
					Ichimatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Jushimatsu") {
					Jushimatsu.SendMessage("Play");
				}
				if (hit.collider.gameObject.tag == "Todomatsu") {
					Todomatsu.SendMessage("Play");
				}
			}
		}
	}
}
