using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
   public List<AudioClip> BGTracks;
    int randomIndex;
    AudioSource bgTrack;

	// Use this for initialization
	void Start () {
        bgTrack = gameObject.GetComponent<AudioSource>();

        BGTracks = new List<AudioClip>();
        BGTracks.Add(Resources.Load<AudioClip>("Audio/Soundtrack/bg_RelaxedMusic"));
        BGTracks.Add(Resources.Load<AudioClip>("Audio/Soundtrack/bg_SuspenseMusic"));
        BGTracks.Add(Resources.Load<AudioClip>("Audio/Soundtrack/bg_ActionMusic"));
        randomIndex = Random.Range(0, BGTracks.Count);

        bgTrack.clip = BGTracks[randomIndex];

        bgTrack.loop = true;

        bgTrack.Play();


        EventManager.AddInitNewTraincarListener(SwitchBGMusic);
    }


    // switch to different song
    private void SwitchBGMusic()
    {
        bgTrack.Stop();
        int newRandomIndex = Random.Range(0, BGTracks.Count);
        while (randomIndex == newRandomIndex)
        {
            newRandomIndex = Random.Range(0, BGTracks.Count);
        }
        randomIndex = newRandomIndex;
        bgTrack.clip = BGTracks[randomIndex];
        bgTrack.Play();
    }

}
