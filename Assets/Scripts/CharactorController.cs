using UnityEngine;
using System.Collections;

public class CharactorController : MonoBehaviour {
	public GameObject sprite;
	Animator animator;
	AudioSource sound;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
		animator = sprite.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Play() {
		animator.SetTrigger ("Play");//.SetBool ("Play", true);
		sound.Play();
	}

}
