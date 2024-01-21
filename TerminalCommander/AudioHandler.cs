using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TerminalCommander
{
    public class AudioManager : MonoBehaviour
    {
        //Audio is not mutable
        private AudioClip jammerAudio = null;
        private AudioSource jammerAudioSource = null;

        private AudioClip emergencyAudio = null;
        private AudioSource emergencyAudioSource = null;

        private AudioClip errorAudio = null;
        private AudioSource errorAudioSource = null;

        private string jammerPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "jammer.wav");
        private string errorPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "error.wav");
        private string emergencyPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "emergency.wav");

        void Start()
        {
            LoadAudio();
        }
        public void LoadAudio()
        {
          
           
            
            Debug.Log($"Loading Audio Clips");

            StartCoroutine(LoadAudio(AudioItem.Jammer));
            StartCoroutine(LoadAudio(AudioItem.Error));
            StartCoroutine(LoadAudio(AudioItem.Emergency));
        }

        private IEnumerator LoadAudio(AudioItem t)
        {
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(getAudioPath(t), AudioType.WAV))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError ||
                    uwr.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError(uwr.error);
                }
                else
                {          
                    if(t == AudioItem.Jammer) { jammerAudio= DownloadHandlerAudioClip.GetContent(uwr); }
                    else if(t == AudioItem.Emergency) {  emergencyAudio = DownloadHandlerAudioClip.GetContent(uwr); }
                    else if(t == AudioItem.Error) { errorAudio = DownloadHandlerAudioClip.GetContent(uwr); }
                                      
                    if (!isClipNull(t))
                    {
                        // Create an AudioSource and attach it to the GameObject
                        loadAudioItem(t);
                    }
                    else
                    {
                        Debug.Log($"Could not load {t} audio");
                    }
                }
            }
        }
        private string getAudioPath(AudioItem t)
        {
            if(t== AudioItem.Jammer) { return jammerPath; }
            else if(t == AudioItem.Emergency) { return emergencyPath; }
            else if(t == AudioItem.Error) { return errorPath; }
            return null;
        }
        private bool isClipNull(AudioItem t)
        {
            if (t == AudioItem.Jammer && jammerAudio == null) { return true; }
            if (t == AudioItem.Error && errorAudio == null) { return true; }
            if (t == AudioItem.Emergency && emergencyAudio == null) { return true; }
            return false;
        }
        private void loadAudioItem(AudioItem t)
        {
            if(t== AudioItem.Jammer)
            {
                jammerAudioSource = gameObject.AddComponent<AudioSource>();
                jammerAudioSource.clip = jammerAudio;            
            }
            else if(t == AudioItem.Error)
            {
                errorAudioSource= gameObject.AddComponent<AudioSource>();
                errorAudioSource.clip = errorAudio;
            }
            else if(t == AudioItem.Emergency)
            {
                emergencyAudioSource= gameObject.AddComponent<AudioSource>();
                emergencyAudioSource.clip = emergencyAudio;
            }
        }
        public void PlaySound(AudioItem t)
        {
            if (t == AudioItem.Jammer && jammerAudioSource != null)
            {
                jammerAudioSource.Play();
            }
            else if (t == AudioItem.Error && errorAudioSource != null)
            {
                errorAudioSource.Play();
            }       
            else if(t == AudioItem.Emergency && emergencyAudioSource != null)
            {
                emergencyAudioSource.Play();
            }
            else
            {
                Debug.LogError("Audio not loaded. Call LoadAudio() first.");
            }
        }
      
    }
    public enum AudioItem
    {
        Jammer, Emergency, Error
    }
}
