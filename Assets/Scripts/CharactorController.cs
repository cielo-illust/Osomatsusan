using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactorController : MonoBehaviour {
	public GameObject sprite;
	public GameContrller gameContrller;
	public GameObject effect;
	public GameObject cutin;
	public GameObject cutin2;

	const float Oso = 1.0f;
	const float Kara = 2.0f;
	const float Choro = 3.0f;
	const float Ichi = 4.0f;
	const float Jushi = 5.0f;
	const float Todo = 6.0f;

	Animator animator;
	AudioSource sound;
	AudioSource soundSub;
	bool playNow = false;
	bool allPlayNow = false;
	float startTime = 0.0f;
	float endTime = 0.01f;
	bool playAll = false;
	List<float[]> times = new List<float[]>();
	List<float[]> subTimes = new List<float[]>();
	List<float[]> osoTimes = new List<float[]>();
	List<float[]> karaTimes = new List<float[]>();
	List<float[]> choroTimes = new List<float[]>();
	List<float[]> ichiTimes = new List<float[]>();
	List<float[]> jushiTimes = new List<float[]>();
	List<float[]> todoTimes = new List<float[]>();

	// Use this for initialization
	void Start () {
		AudioSource[] audioSources = GetComponents<AudioSource>();
		sound = audioSources[0];
		soundSub = audioSources[1];
		animator = sprite.GetComponent<Animator> ();

		initTimes ();
	}

	// Update is called once per frame
	void Update () {
	}

	public void Play() {
		if ((!playNow) && (gameContrller.getTime () != -1.0f)) {
			animator.SetBool ("Play", true);
			startTime = 0.0f;
			endTime = 0.0f;
			float thisTime = Time.time - gameContrller.getTime ();
			for (int i = 0; i < times.Count; i++) {
				float t0 = (i == 0) ? -0.5f : times[i-1][0];
				float t1 = times[i][0];
				float t2 = times[i][1];
				float flag = times[i][2];
				float chara = times[i][3];
				if (((t0 + 0.5f) <= thisTime) && ((t1 + 0.5f) > thisTime)) {
					startTime = t1;
					endTime = t2;
					if (flag == 1.0f) {	// 全員で
						gameContrller.playAll ();
						playAll = true;
					}
					if (flag == 2.0f) {	// 全員で(トド松以外)
						gameContrller.playAll2 ();
						playAll = true;
					}
					if (flag == 3.0f) {	// エフェクト
						Instantiate(cutin, new Vector3(0,-0.5f,-2), Quaternion.identity);
						Instantiate(cutin2, new Vector3(0,-2.5f,-1.9f), Quaternion.identity);
						Instantiate(effect, new Vector3(0,-4,-5), Quaternion.Euler(new Vector3(-90,0,0)));
					}
					if ((hitChar (chara)) && ((flag != 3.0f))) {
						Instantiate(effect, new Vector3(0,-4,-5), Quaternion.Euler(new Vector3(-90,0,0)));
						sound.time = startTime;
						sound.Play ();
					} else {
						// 該当しなかった場合はサブボイスを再生
						for (i = 0; i < subTimes.Count; i++) {
							t0 = (i == 0) ? -0.5f : subTimes[i-1][0];
							t1 = subTimes[i][0];
							t2 = subTimes[i][1];
							flag = subTimes[i][2];
							chara = subTimes[i][3];
							if (((t0 + 0.5f) <= thisTime) && ((t1 + 0.5f) > thisTime)) {
								startTime = t1;
								endTime = t2;
								soundSub.time = startTime;
								soundSub.Play ();
								break;
							}
						}
					}
					break;
				}
			}
			playNow = true;
			StartCoroutine(PlayStop(endTime - startTime));
		}
	}

	IEnumerator PlayStop(float time) {
		yield return new WaitForSeconds (time);
		sound.Stop();
		soundSub.Stop();
		playNow = false;

		if (!allPlayNow) {
			animator.SetBool ("Play", false);
		}

		// 全員で停止
		if (playAll) {
			gameContrller.playAllOff ();
			playAll = false;
		}
	}

	public void PlayAll() {
		allPlayNow = true;
		animator.SetBool ("Play", true);
	}

	public void PlayAllOff() {
		allPlayNow = false;
		animator.SetBool ("Play", false);
	}

	float tt(string t) {
		return float.Parse(t.Substring(0,1)) * 60.0f + float.Parse(t.Substring(2));
	}

	float[] addTime(string t1, string t2, float flag, float chara) {
		float[] t = new float[4];
		t[0] = tt(t1);
		t[1] = tt(t2);
		t[2] = flag;
		t[3] = chara;
		return t;
	}

	bool hitChar(float chara) {
		bool res = false;
		if (chara == 0.0f) res = true;
		if ((chara == Oso) && (sprite.tag == "OsomatsuSprite")) res = true;
		if ((chara == Kara) && (sprite.tag == "KaramatsuSprite")) res = true;
		if ((chara == Choro) && (sprite.tag == "ChoromatsuSprite")) res = true;
		if ((chara == Ichi) && (sprite.tag == "IchimatsuSprite")) res = true;
		if ((chara == Jushi) && (sprite.tag == "JushimatsuSprite")) res = true;
		if ((chara == Todo) && (sprite.tag == "TodomatsuSprite")) res = true;
		return res;
	}

	void initTimes () {
		times.Add (addTime ("0:01.9", "0:02.9", 0.0f, Oso));	// 松野おそ松！
		times.Add (addTime ("0:02.9", "0:03.9", 0.0f, Kara));	// 松野カラ松。
		times.Add (addTime ("0:04.0", "0:04.9", 0.0f, Choro));	// 松野チョロ松
		times.Add (addTime ("0:04.9", "0:06.0", 0.0f, Ichi));	// 松野一松…
		times.Add (addTime ("0:06.0", "0:07.1", 0.0f, Jushi));	// 松野十四松！
		times.Add (addTime ("0:07.1", "0:08.2", 0.0f, Todo));	// 松野トド松
		times.Add (addTime ("0:08.2", "0:12.1", 1.0f, 0.0f));	// おれたち六つ子！おんなじ顔が六つあったって…いいよな？
		// SHAKE!
		times.Add (addTime ("0:17.5", "0:18.6", 0.0f, Oso));	// さっき会ったよね？
		times.Add (addTime ("0:18.6", "0:20.4", 0.0f, Kara));	// 覚えてたぜ、お前の顔
		times.Add (addTime ("0:20.4", "0:22.0", 0.0f, Choro));	// 一回、二回じゃないんですよね
		times.Add (addTime ("0:22.0", "0:23.8", 0.0f, Ichi));	// 多分…これで六度目だよ。
		times.Add (addTime ("0:23.8", "0:26.1", 0.0f, Jushi));	// 今夜はサイコー！ねぇ、踊ろーよ～！
		times.Add (addTime ("0:26.1", "0:27.5", 0.0f, Todo));	// キミの笑顔、かわいいよね
		times.Add (addTime ("0:27.5", "0:28.7", 0.0f, Choro));	// あ、心配しないで！
		times.Add (addTime ("0:28.7", "0:29.8", 0.0f, Jushi));	// 僕らも６人っ！！
		times.Add (addTime ("0:29.8", "0:30.8", 0.0f, Kara));	// オレがアイツで、
		times.Add (addTime ("0:30.8", "0:31.7", 0.0f, Ichi));	// 僕たちは僕
		times.Add (addTime ("0:31.7", "0:33.5", 0.0f, Todo));	// うん、ボクたち六つ子なんだ
		times.Add (addTime ("0:33.5", "0:34.8", 0.0f, Oso));	// キミたちもそうだろ？
		// SHAKE!…ダレがダレでもおんなじざんす！
		// おフランスでミーは知ったざんす
		times.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		// 愛はチミたちのココロの中にあって
		times.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		// ほんのチョッピリわけてちょ、ミーにも…ウヒョ～！
		// Oh! So much,Sons!
		times.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		// “SIX SAME FACES”
		// ああ、なんてややこしいキョーダイざんしょ
		// マゼちゃって
		times.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// コンガラガってコンランざんす
		// おふざけやめてちょ！
		times.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		// “SIX SAME FACES”
		// どれがだれだか教えてちょ！
		// もう、ミーを主役にしてほしいざんす！
		// キャラが一番立ってるざんしょ？
		times.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!
		// まぁ、そろいもそろって…スットンキョ～な連中ざんす！
		times.Add (addTime ("1:22.1", "1:26.4", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("1:27.9", "1:29.0", 0.0f, Oso));	// 長男おそ松！
		times.Add (addTime ("1:29.0", "1:30.0", 0.0f, Kara));	// 次男カラ松。
		times.Add (addTime ("1:30.0", "1:31.0", 0.0f, Choro));	// 三男チョロ松！
		times.Add (addTime ("1:31.0", "1:32.1", 0.0f, Ichi));	// 四男一松。
		times.Add (addTime ("1:32.1", "1:33.2", 0.0f, Jushi));	// 五男十四松っ！
		times.Add (addTime ("1:33.2", "1:34.4", 0.0f, Todo));	// 末弟トド松
		times.Add (addTime ("1:34.4", "1:38.2", 1.0f, 0.0f));	// おれたち六つ子！まだまだ働かなくたって…いいよな？
		// SHAKE!
		times.Add (addTime ("1:34.4", "1:45.0", 0.0f, Choro));	// なんか…楽しいことしません？
		times.Add (addTime ("1:45.0", "1:46.9", 0.0f, Jushi));	// キミたちとあえてラッキーだよ！
		times.Add (addTime ("1:46.9", "1:48.6", 0.0f, Todo));	// これってホント…奇跡だよね
		times.Add (addTime ("1:48.6", "1:50.6", 0.0f, Ichi));	// 六つ子と六つ子の出会いなんて
		times.Add (addTime ("1:50.6", "1:51.9", 0.0f, Choro));	// ババ抜きにならないよね
		times.Add (addTime ("1:51.9", "1:54.3", 0.0f, Kara));	// フッ、確かに同じカードだからな
		times.Add (addTime ("1:54.3", "1:55.8", 0.0f, Ichi));	// 数字だけ違ってね
		times.Add (addTime ("1:55.8", "1:57.9", 0.0f, Jushi));	// ダレがアニキで弟かって？
		times.Add (addTime ("1:57.9", "1:59.7", 0.0f, Oso));	// ま、おれが一番かっこいいだろ？
		times.Add (addTime ("1:59.7", "2:01.8", 0.0f, Todo));	// ねぇ、…ボクを選んでよ
		// SHAKE! …ダレがダレでもおんなじざんす！
		// おフランスはミーの実家ざんす 
		times.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		// 今度チミたちもつれてってほしいざんしょ？ 
		times.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		// コート・ダ・ジョール、よいとこざんすよ…ウヒョ～！
		// Oh! So much,Suns!
		times.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		// “SIX SAME FACES”
		// ああ、なんてすばらしい人生ざんしょ！
		// イヤになって
		times.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// コンガラガってコンランざんす
		// おふざけやめてちょ！
		times.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// “SIX SAME FACES”
		// もっとガバチョと愛してちょ
		// チミ、ミーのおよめにきてほしいざんす
		// 誰でもいいのならミーにしてちょ！
		times.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// ほら、りっぱざんしょ…おフランスの洋服ざんす!
		times.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("2:53.2", "3:53.8", 0.0f, 0.0f));	// ついに世界初男女の六つ子による合コンに扱ぎつけたぞ～
		times.Add (addTime ("3:53.8", "3:57.3", 2.0f, 0.0f));	// 早退すんじゃねぇー!
		// “SIX SAME FACES”
		// ああ、なんてめずらしいキョーダイざんしょ！
		// ほうりだして
		times.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// Don't be a nuisance!
		// お見合いパーティー中ざんす
		// おふざけやめてちょ！
		times.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// “SIX SAME FACES”
		// だれが好みか教えてちょ
		// チミ、ミーをえらべばおしあわせざんす
		times.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		// 六つ子ちゃんたち、帰ってちょ～～っ！
		// まぁ、そろいもそろって…スットンキョ～な連中ざんす！
		times.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!
		times.Add (addTime ("4:31.0", "4:31.7", 1.0f, 0.0f));	// おそまつ！

		// おそ松
		osoTimes.Add (addTime ("0:00.9", "0:02.3", 0.0f, Oso));
		osoTimes.Add (addTime ("0:02.3", "0:04.5", 0.0f, Oso));
		osoTimes.Add (addTime ("0:04.5", "0:06.0", 0.0f, Oso));
		osoTimes.Add (addTime ("0:06.0", "0:07.7", 0.0f, Oso));
		osoTimes.Add (addTime ("0:07.9", "0:09.8", 0.0f, Oso));
		osoTimes.Add (addTime ("0:10.0", "0:12.2", 0.0f, Oso));

		osoTimes.Add (addTime ("0:16.0", "0:17.2", 0.0f, Oso));
		osoTimes.Add (addTime ("0:17.2", "0:19.4", 0.0f, Oso));
		osoTimes.Add (addTime ("0:19.6", "0:21.0", 0.0f, Oso));
		osoTimes.Add (addTime ("0:21.0", "0:22.9", 0.0f, Oso));
		osoTimes.Add (addTime ("0:22.9", "0:25.3", 0.0f, Oso));
		osoTimes.Add (addTime ("0:25.3", "0:27.0", 0.0f, Oso));
		osoTimes.Add (addTime ("0:27.0", "0:29.0", 0.0f, Oso));
		osoTimes.Add (addTime ("0:29.0", "0:30.6", 0.0f, Oso));
		osoTimes.Add (addTime ("0:30.8", "0:32.7", 0.0f, Oso));
		osoTimes.Add (addTime ("0:32.8", "0:34.1", 0.0f, Oso));

		osoTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		osoTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		osoTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));		// 「ひたすらあそんで暮らして～」
		osoTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		osoTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		osoTimes.Add (addTime ("1:27.0", "1:29.1", 0.0f, Oso));
		osoTimes.Add (addTime ("1:29.1", "1:30.7", 0.0f, Oso));
		osoTimes.Add (addTime ("1:30.9", "1:34.2", 0.0f, Oso));
		osoTimes.Add (addTime ("1:34.3", "1:36.3", 0.0f, Oso));
		osoTimes.Add (addTime ("1:36.3", "1:38.9", 0.0f, Oso));

		osoTimes.Add (addTime ("1:42.0", "1:43.9", 0.0f, Oso));
		osoTimes.Add (addTime ("1:44.0", "1:45.7", 0.0f, Oso));
		osoTimes.Add (addTime ("1:45.9", "1:47.7", 0.0f, Oso));
		osoTimes.Add (addTime ("1:47.7", "1:49.4", 0.0f, Oso));
		osoTimes.Add (addTime ("1:49.6", "1:50.7", 0.0f, Oso));
		osoTimes.Add (addTime ("1:50.9", "1:53.0", 0.0f, Oso));
		osoTimes.Add (addTime ("1:53.2", "1:54.6", 0.0f, Oso));
		osoTimes.Add (addTime ("1:54.9", "1:56.6", 0.0f, Oso));
		osoTimes.Add (addTime ("1:57.0", "1:58.6", 0.0f, Oso));
		osoTimes.Add (addTime ("1:58.9", "2:00.6", 0.0f, Oso));

		osoTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		osoTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		osoTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		osoTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		osoTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		osoTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		// カラ松
		karaTimes.Add (addTime ("0:01.0", "0:02.9", 0.0f, Kara));
		karaTimes.Add (addTime ("0:02.9", "0:04.1", 0.0f, Kara));
		karaTimes.Add (addTime ("0:04.1", "0:05.9", 0.0f, Kara));
		karaTimes.Add (addTime ("0:06.0", "0:07.6", 0.0f, Kara));
		karaTimes.Add (addTime ("0:07.8", "0:09.8", 0.0f, Kara));
		karaTimes.Add (addTime ("0:10.0", "0:12.6", 0.0f, Kara));

		karaTimes.Add (addTime ("0:15.7", "0:17.8", 0.0f, Kara));
		karaTimes.Add (addTime ("0:17.8", "0:19.2", 0.0f, Kara));
		karaTimes.Add (addTime ("0:19.2", "0:22.7", 0.0f, Kara));
		karaTimes.Add (addTime ("0:23.2", "0:25.1", 0.0f, Kara));
		karaTimes.Add (addTime ("0:25.3", "0:27.0", 0.0f, Kara));
		karaTimes.Add (addTime ("0:27.2", "0:28.8", 0.0f, Kara));
		karaTimes.Add (addTime ("0:29.0", "0:30.8", 0.0f, Kara));
		karaTimes.Add (addTime ("0:31.0", "0:32.9", 0.0f, Kara));
		karaTimes.Add (addTime ("0:33.2", "0:34.7", 0.0f, Kara));

		karaTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		karaTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		karaTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		karaTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		karaTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		karaTimes.Add (addTime ("1:27.2", "1:28.9", 0.0f, Kara));
		karaTimes.Add (addTime ("1:29.1", "1:30.6", 0.0f, Kara));
		karaTimes.Add (addTime ("1:30.9", "1:33.6", 0.0f, Kara));
		karaTimes.Add (addTime ("1:33.7", "1:35.6", 0.0f, Kara));
		karaTimes.Add (addTime ("1:35.9", "1:37.9", 0.0f, Kara));
		karaTimes.Add (addTime ("1:38.2", "1:39.1", 0.0f, Kara));

		karaTimes.Add (addTime ("1:42.2", "1:43.8", 0.0f, Kara));
		karaTimes.Add (addTime ("1:44.3", "1:45.7", 0.0f, Kara));
		karaTimes.Add (addTime ("1:45.8", "1:47.4", 0.0f, Kara));
		karaTimes.Add (addTime ("1:47.7", "1:50.5", 0.0f, Kara));
		karaTimes.Add (addTime ("1:50.8", "1:52.9", 0.0f, Kara));
		karaTimes.Add (addTime ("1:53.3", "1:55.2", 0.0f, Kara));
		karaTimes.Add (addTime ("1:55.4", "1:58.3", 0.0f, Kara));
		karaTimes.Add (addTime ("1:58.7", "2:00.9", 0.0f, Kara));

		karaTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		karaTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		karaTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		karaTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		karaTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		karaTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		// チョロ松
		choroTimes.Add (addTime ("0:01.0", "0:02.3", 0.0f, Choro));
		choroTimes.Add (addTime ("0:02.3", "0:04.2", 0.0f, Choro));
		choroTimes.Add (addTime ("0:04.4", "0:06.6", 0.0f, Choro));
		choroTimes.Add (addTime ("0:06.8", "0:07.8", 0.0f, Choro));
		choroTimes.Add (addTime ("0:08.0", "0:09.6", 0.0f, Choro));
		choroTimes.Add (addTime ("0:09.8", "0:12.2", 0.0f, Choro));

		choroTimes.Add (addTime ("0:16.1", "0:17.6", 0.0f, Choro));
		choroTimes.Add (addTime ("0:18.0", "0:19.5", 0.0f, Choro));
		choroTimes.Add (addTime ("0:19.7", "0:21.1", 0.0f, Choro));
		choroTimes.Add (addTime ("0:21.4", "0:22.9", 0.0f, Choro));
		choroTimes.Add (addTime ("0:23.2", "0:25.3", 0.0f, Choro));
		choroTimes.Add (addTime ("0:25.4", "0:26.9", 0.0f, Choro));
		choroTimes.Add (addTime ("0:26.9", "0:29.1", 0.0f, Choro));
		choroTimes.Add (addTime ("0:29.2", "0:30.7", 0.0f, Choro));
		choroTimes.Add (addTime ("0:30.9", "0:32.5", 0.0f, Choro));
		choroTimes.Add (addTime ("0:32.5", "0:34.6", 0.0f, Choro));

		choroTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		choroTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		choroTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		choroTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		choroTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		choroTimes.Add (addTime ("1:27.2", "1:29.2", 0.0f, Choro));
		choroTimes.Add (addTime ("1:29.3", "1:30.7", 0.0f, Choro));
		choroTimes.Add (addTime ("1:30.8", "1:33.5", 0.0f, Choro));
		choroTimes.Add (addTime ("1:33.8", "1:35.5", 0.0f, Choro));
		choroTimes.Add (addTime ("1:36.1", "1:38.5", 0.0f, Choro));

		choroTimes.Add (addTime ("1:42.3", "1:43.8", 0.0f, Choro));
		choroTimes.Add (addTime ("1:44.0", "1:45.8", 0.0f, Choro));
		choroTimes.Add (addTime ("1:46.1", "1:47.7", 0.0f, Choro));
		choroTimes.Add (addTime ("1:48.1", "1:49.4", 0.0f, Choro));
		choroTimes.Add (addTime ("1:49.7", "1:52.7", 0.0f, Choro));
		choroTimes.Add (addTime ("1:52.9", "1:54.8", 0.0f, Choro));
		choroTimes.Add (addTime ("1:55.0", "1:58.3", 0.0f, Choro));
		choroTimes.Add (addTime ("1:58.8", "2:00.5", 0.0f, Choro));

		choroTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		choroTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		choroTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		choroTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		choroTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		choroTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		// 一松
		ichiTimes.Add (addTime ("0:01.0", "0:01.7", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:02.0", "0:03.8", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:04.4", "0:06.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:06.7", "0:07.9", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:08.1", "0:09.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:09.9", "0:12.4", 0.0f, Ichi));

		ichiTimes.Add (addTime ("0:15.7", "0:16.9", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:17.2", "0:19.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:19.6", "0:21.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:21.6", "0:23.3", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:23.5", "0:25.6", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:25.8", "0:28.0", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:28.8", "0:30.6", 0.0f, Ichi));
		ichiTimes.Add (addTime ("0:31.0", "0:33.4", 0.0f, Ichi));

		ichiTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		ichiTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		ichiTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		ichiTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		ichiTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		ichiTimes.Add (addTime ("1:27.3", "1:30.0", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:30.5", "1:32.8", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:33.0", "1:35.5", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:35.8", "1:37.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:37.7", "1:38.9", 0.0f, Ichi));

		ichiTimes.Add (addTime ("1:42.3", "1:44.8", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:45.0", "1:46.6", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:46.7", "1:48.3", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:48.5", "1:50.0", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:50.2", "1:52.8", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:53.3", "1:55.4", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:55.7", "1:57.2", 0.0f, Ichi));
		ichiTimes.Add (addTime ("1:57.9", "1:59.7", 0.0f, Ichi));

		ichiTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		ichiTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		ichiTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		ichiTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		ichiTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		ichiTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		// 十四松
		jushiTimes.Add (addTime ("0:00.9", "0:02.1", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:02.4", "0:04.3", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:04.4", "0:06.4", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:06.6", "0:08.0", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:08.3", "0:09.4", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:09.6", "0:12.2", 0.0f, Jushi));

		jushiTimes.Add (addTime ("0:16.1", "0:17.9", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:17.9", "0:19.2", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:19.4", "0:21.5", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:21.7", "0:22.9", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:23.1", "0:24.3", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:24.3", "0:25.3", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:25.5", "0:26.9", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:27.0", "0:28.5", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:28.7", "0:30.0", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:30.2", "0:31.4", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:31.6", "0:33.3", 0.0f, Jushi));
		jushiTimes.Add (addTime ("0:33.3", "0:34.8", 0.0f, Jushi));

		jushiTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		jushiTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		jushiTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		jushiTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		jushiTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		jushiTimes.Add (addTime ("1:27.2", "1:29.8", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:30.0", "1:31.1", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:31.4", "1:32.6", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:32.6", "1:34.0", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:34.6", "1:36.7", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:36.9", "1:38.2", 0.0f, Jushi));

		jushiTimes.Add (addTime ("1:41.7", "1:43.5", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:43.8", "1:45.4", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:45.6", "1:47.2", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:47.5", "1:49.1", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:49.2", "1:51.0", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:51.2", "1:52.5", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:52.7", "1:55.3", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:55.5", "1:57.2", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:57.3", "1:58.7", 0.0f, Jushi));
		jushiTimes.Add (addTime ("1:59.0", "2:01.0", 0.0f, Jushi));

		jushiTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		jushiTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		jushiTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		jushiTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		jushiTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		jushiTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		// トド松
		todoTimes.Add (addTime ("0:01.0", "0:02.4", 0.0f, Todo));
		todoTimes.Add (addTime ("0:02.5", "0:04.3", 0.0f, Todo));
		todoTimes.Add (addTime ("0:04.5", "0:06.9", 0.0f, Todo));
		todoTimes.Add (addTime ("0:07.1", "0:08.8", 0.0f, Todo));
		todoTimes.Add (addTime ("0:09.0", "0:01.9", 0.0f, Todo));
		todoTimes.Add (addTime ("0:10.3", "0:12.1", 0.0f, Todo));

		todoTimes.Add (addTime ("0:15.9", "0:17.3", 0.0f, Todo));
		todoTimes.Add (addTime ("0:17.5", "0:19.4", 0.0f, Todo));
		todoTimes.Add (addTime ("0:19.6", "0:20.8", 0.0f, Todo));
		todoTimes.Add (addTime ("0:21.6", "0:22.8", 0.0f, Todo));
		todoTimes.Add (addTime ("0:23.3", "0:24.8", 0.0f, Todo));
		todoTimes.Add (addTime ("0:25.1", "0:26.6", 0.0f, Todo));
		todoTimes.Add (addTime ("0:26.7", "0:28.6", 0.0f, Todo));
		todoTimes.Add (addTime ("0:28.8", "0:30.3", 0.0f, Todo));
		todoTimes.Add (addTime ("0:30.5", "0:32.1", 0.0f, Todo));
		todoTimes.Add (addTime ("0:32.6", "0:33.8", 0.0f, Todo));

		todoTimes.Add (addTime ("0:41.9", "0:43.1", 1.0f, 0.0f));	// ～Who? Who?
		todoTimes.Add (addTime ("0:45.5", "0:46.8", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		todoTimes.Add (addTime ("0:51.5", "0:54.0", 3.0f, Oso));	// 「ひたすらあそんで暮らして～」
		todoTimes.Add (addTime ("1:00.7", "1:02.0", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("1:08.1", "1:09.4", 1.0f, 0.0f));	// SHAKE!SHAKE!
		todoTimes.Add (addTime ("1:15.6", "1:16.9", 1.0f, 0.0f));	// SHAKE!SHAKE!

		todoTimes.Add (addTime ("1:27.3", "1:29.5", 0.0f, Todo));
		todoTimes.Add (addTime ("1:29.7", "1:31.2", 0.0f, Todo));
		todoTimes.Add (addTime ("1:31.6", "1:32.6", 0.0f, Todo));
		todoTimes.Add (addTime ("1:32.8", "1:35.2", 0.0f, Todo));
		todoTimes.Add (addTime ("1:35.6", "1:37.0", 0.0f, Todo));
		todoTimes.Add (addTime ("1:37.0", "1:38.9", 0.0f, Todo));

		todoTimes.Add (addTime ("1:42.1", "1:43.7", 0.0f, Todo));
		todoTimes.Add (addTime ("1:44.0", "1:45.7", 0.0f, Todo));
		todoTimes.Add (addTime ("1:46.2", "1:48.0", 0.0f, Todo));
		todoTimes.Add (addTime ("1:48.2", "1:49.6", 0.0f, Todo));
		todoTimes.Add (addTime ("1:50.0", "1:51.5", 0.0f, Todo));
		todoTimes.Add (addTime ("1:51.8", "1:53.6", 0.0f, Todo));
		todoTimes.Add (addTime ("1:54.1", "1:56.0", 0.0f, Todo));
		todoTimes.Add (addTime ("1:56.4", "1:58.7", 0.0f, Todo));
		todoTimes.Add (addTime ("1:58.9", "2:00.6", 0.0f, Todo));

		todoTimes.Add (addTime ("2:08.2", "2:09.3", 1.0f, 0.0f));	// ～Who? Who?
		todoTimes.Add (addTime ("2:11.8", "2:13.0", 1.0f, 0.0f));	// ～Ho!Yell!Ho!Yell!
		todoTimes.Add (addTime ("2:17.6", "2:20.5", 3.0f, Kara));	// 「働かない我が人生、セラヴィ！」
		todoTimes.Add (addTime ("2:26.9", "2:28.1", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("2:34.4", "2:35.7", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("2:41.9", "2:43.2", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("2:48.4", "2:52.6", 1.0f, 0.0f));	// She,Yeah!!!!!!
		todoTimes.Add (addTime ("4:03.5", "4:04.8", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("4:11.0", "4:12.4", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("4:18.5", "4:19.6", 1.0f, 0.0f));	// SHAKE! SHAKE!
		todoTimes.Add (addTime ("4:24.9", "4:29.1", 1.0f, 0.0f));	// She,Yeah!!!!!!

		if (sprite.tag == "OsomatsuSprite") subTimes = osoTimes;
		if (sprite.tag == "KaramatsuSprite") subTimes = karaTimes;
		if (sprite.tag == "ChoromatsuSprite") subTimes = choroTimes;
		if (sprite.tag == "IchimatsuSprite") subTimes = ichiTimes;
		if (sprite.tag == "JushimatsuSprite") subTimes = jushiTimes;
		if (sprite.tag == "TodomatsuSprite") subTimes = todoTimes;
	}
}
