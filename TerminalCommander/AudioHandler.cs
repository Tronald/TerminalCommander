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
    public class AudioManager
    {
        //Audio is not mutable
        public AudioClip jammerAudio = null;
        public AudioClip emergencyAudio = null;
        public AudioClip errorAudio = null;


        private string jammerPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "jammer.wav");
        private string errorPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "error.wav");
        private string emergencyPath = Path.Combine(Paths.BepInExRootPath, "plugins", "TerminalCommander.Audio", "emergency.wav");

      
        public void LoadAudio()
        {
            PathCheck();
            Debug.Log($"Loading {jammerPath}");
            LoadAudio(AudioItem.Jammer);
            Debug.Log($"Loading {errorPath}");
            LoadAudio(AudioItem.Error);
            Debug.Log($"Loading {emergencyPath}");
            LoadAudio(AudioItem.Emergency);
        }

        private void PathCheck()
        {
            //Thunderstore patch
            if (!File.Exists(jammerPath))
            {
                jammerPath = Path.Combine(Paths.BepInExRootPath, "plugins", "Tronald-TerminalCommander", "TerminalCommander.Audio", "jammer.wav");
                errorPath = Path.Combine(Paths.BepInExRootPath, "plugins", "Tronald-TerminalCommander", "TerminalCommander.Audio", "error.wav");
                emergencyPath = Path.Combine(Paths.BepInExRootPath, "plugins", "Tronald-TerminalCommander", "TerminalCommander.Audio", "emergency.wav");
            }
        }

        private async void LoadAudio(AudioItem t)
        {
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(getAudioPath(t), AudioType.WAV))
            {
                uwr.SendWebRequest();
                while (!uwr.isDone) { }

                if (uwr.error!=null)
                {
                    Debug.LogError(uwr.error);
                }
                else
                {          
                    if(t == AudioItem.Jammer) { jammerAudio= DownloadHandlerAudioClip.GetContent(uwr); }
                    else if(t == AudioItem.Emergency) {  emergencyAudio = DownloadHandlerAudioClip.GetContent(uwr); }
                    else if(t == AudioItem.Error) { errorAudio = DownloadHandlerAudioClip.GetContent(uwr); }                 
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
  
    }
    public enum AudioItem
    {
        Jammer, Emergency, Error
    }
}
