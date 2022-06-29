using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTime_MicGet : MonoBehaviour
{
    private AudioSource source;
    private AudioClip mic;
    private int lastSample = 0;
    private float[] samples = null;
    //private List<float> writeSamples = null;
    private List<float> readSamples = null;
    private int channels = 0;
    private int readUpdateId = 0;
    private int previousReadUpdateId = -1;

    //private float WRITE_FLUSH_TIME = 0.5f;
    public float READ_FLUSH_TIME = 0.5f;
    private float writeFlushTimer = 0.0f;
    private float readFlushTimer = 0.0f;


    void Start()
    {
        //writeSamples = new List<float>(1024);
        readSamples = new List<float>();
        source = new GameObject("Mic").AddComponent<AudioSource>();
        mic = Microphone.Start("Microphone (High Definition Audio Device)", true, 100, 44100);//You can just give null for the mic name, it's gonna automatically detect the default mic of your system (k)
        channels = mic.channels;//mono or stereo, for me it's 1 (k)
    }

    // Update is called once per frame
    void Update()
    {
        ReadMic();
        PlayMic();

        /*source.clip = mic;
        if (!source.isPlaying){
            //Debug.Log("Play!");
            source.Play();
        }*/
    }

    private void ReadMic(){
			writeFlushTimer += Time.deltaTime;
			int pos = Microphone.GetPosition(null);
            //Debug.Log("pos: " + pos);
            //Debug.Log("lastSample: " + lastSample);
			int diff = pos - lastSample;

			if (diff > 0)
			{
				samples = new float[diff * channels];
				mic.GetData(samples, lastSample);

                readSamples.AddRange(samples);//readSamples gonna be converted to an audio clip and be played (k)
			}
			lastSample = pos;
		}

    private void PlayMic(){
        readFlushTimer += Time.deltaTime;

			if (readFlushTimer > READ_FLUSH_TIME) //0.5f (k)
			{
				if (readUpdateId != previousReadUpdateId && readSamples != null && readSamples.Count > 0)
				{
                    //Debug.Log("Read happened");
					previousReadUpdateId = readUpdateId;


                    source.clip = AudioClip.Create("Real_time", readSamples.Count, channels, 44100, false);
                    source.spatialBlend = 0;//2D sound
    
                    source.clip.SetData(readSamples.ToArray(), 0);
                    if (!source.isPlaying){
                        //Debug.Log("Play!");
                        source.Play();
                    }

                    readSamples.Clear();
                    readUpdateId++;
				}

				readFlushTimer = 0.0f;
			}
    }
}
