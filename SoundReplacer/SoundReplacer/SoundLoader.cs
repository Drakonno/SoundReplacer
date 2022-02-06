using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SoundReplacer
{ 
    internal static class SoundLoader
    {
        public static string BaseDirectory = Environment.CurrentDirectory + "\\UserData\\SoundReplacer";
        public static Dictionary<string, List<string>> GlobalSoundDictionary = new Dictionary<string, List<string>>(); 
        private static AudioClip _cachedEmpty;
          
        public static void GetSoundLists()
        {
            
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            // Load single audio files.
            Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Single files:");
            CacheAudioPathFromDirectory(BaseDirectory);

            // Load files from directories.
            var directiories = Directory.GetDirectories(BaseDirectory);
            foreach (var directory in directiories)
            {
                CacheAudioPathFromDirectory(directory);
            }


            Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Finalizing:");
            foreach (var entry in GlobalSoundDictionary)
            {
                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Key: {entry.Key}");
                foreach (var sound in entry.Value)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Value: {sound}"); 
                }
            }
            Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"------------");
        }

        private static void CacheAudioPathFromDirectory(string path)
        { 
            Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Directory: {path}");
            var directoryName = $"{path.Split(Path.DirectorySeparatorChar).Last()}";
            GlobalSoundDictionary.TryGetValue(directoryName, out var sounds);
            if (sounds == null)
            {
                sounds = new List<string>();
                sounds.Add("None");
                sounds.Add("Default");
                sounds.Add("Random");
                GlobalSoundDictionary.Add(directoryName, sounds);
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"File: {file}");
                var fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".ogg" ||
                    fileInfo.Extension == ".mp3" ||
                    fileInfo.Extension == ".wav")
                {
                    sounds.Add(fileInfo.Name);
                }
            }
            GlobalSoundDictionary[directoryName] = sounds;
        }

        private static string GetFullPath(string name)
        {
            var path = $"{BaseDirectory}\\{name}";
            var fileInfo = new FileInfo(path);
            return fileInfo.FullName;
        }

        private static UnityWebRequest GetRequest(string fullPath)
        {
            var fileUrl = "file:///" + fullPath;
            var fileInfo = new FileInfo(fullPath);
            var extension = fileInfo.Extension;
            switch (extension)
            {
                case ".ogg":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.OGGVORBIS);
                case ".mp3":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG);
                case ".wav":
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.WAV);
                default:
                    return UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.UNKNOWN);
            }
        }

        private static void ReplaceMissing(string name)
        {
            const string text = "Default";
            if (Plugin.CurrentConfig.GoodHitSound == name)
                Plugin.CurrentConfig.GoodHitSound = text;
            if (Plugin.CurrentConfig.BadHitSound == name)
                Plugin.CurrentConfig.BadHitSound = text;
            if (Plugin.CurrentConfig.ClickSound == name)
                Plugin.CurrentConfig.ClickSound = text;
            if (Plugin.CurrentConfig.FailSound == name)
                Plugin.CurrentConfig.FailSound = text;
            if (Plugin.CurrentConfig.SuccessSound == name)
                Plugin.CurrentConfig.SuccessSound = text;
            if (Plugin.CurrentConfig.MenuMusic == name)
                Plugin.CurrentConfig.MenuMusic = text;
        }

        public static AudioClip[] LoadAudioClips(string directory)
        {
            List<AudioClip> output = new List<AudioClip>();
            foreach(var file in GlobalSoundDictionary[directory])
            {
                output.Add(LoadAudioClip($"{directory}\\{file}"));
            }
            return output.ToArray();
        }

        public static AudioClip LoadAudioClip(string name)
        {
            var fullPath = GetFullPath(name);
            var request = GetRequest(fullPath);

            AudioClip loadedAudio = null;
            var task = request.SendWebRequest();
            
            // while I would normally kill people for this
            // we are loading a local file, so it should be
            // basically instant success or error
            while (!task.isDone) { }

            if (request.isNetworkError || request.isHttpError)
            {
                Plugin.Log.Error($"Failed to load file {name} with error {request.error}");
                ReplaceMissing(name);
                return GetEmptyClip();
            }

            loadedAudio = DownloadHandlerAudioClip.GetContent(request);
            return loadedAudio;
        }

        public static AudioClip GetEmptyClip()
        {
            if (_cachedEmpty != null)
                return _cachedEmpty;

            _cachedEmpty = AudioClip.Create("Empty", 10, 1, 44100 * 2, false);
            return _cachedEmpty;
        }
    }
}
