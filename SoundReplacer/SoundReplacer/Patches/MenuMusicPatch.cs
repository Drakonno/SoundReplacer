using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class MenuMusicPatch
    {
        private static AudioClip _originalMenuMusicClip;
        
        private static AudioClip _lastMenuMusicClip;
        private static string _lastMusicSelected;
        private static string _lastDirectorySelected;

        [HarmonyPatch(typeof(SongPreviewPlayer))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class SongPreviewPlayerPatch
        {
            public static void Prefix(ref AudioClip ____defaultAudioClip)
            { 
                if (_originalMenuMusicClip == null)
                {
                    _originalMenuMusicClip = ____defaultAudioClip;
                }

                if (Plugin.CurrentConfig.MenuMusic == "None")
                {
                    ____defaultAudioClip = SoundLoader.GetEmptyClip();
                }
                else if (Plugin.CurrentConfig.MenuMusic == "Default")
                {
                    ____defaultAudioClip = _originalMenuMusicClip;
                }
                else
                {
                    if (_lastMusicSelected == Plugin.CurrentConfig.MenuMusic && _lastDirectorySelected == Plugin.CurrentConfig.ClickSoundDirectory)
                    {
                        ____defaultAudioClip = _lastMenuMusicClip;
                    }
                    else
                    {
                        _lastMusicSelected = Plugin.CurrentConfig.MenuMusic;
                        _lastDirectorySelected = Plugin.CurrentConfig.ClickSoundDirectory;
                        _lastMenuMusicClip = SoundLoader.LoadAudioClip(_lastMusicSelected);

                        if (_lastMusicSelected == "Random")
                        {
                            var loadedClips = SoundLoader.LoadAudioClips(_lastDirectorySelected); 
                            _lastMenuMusicClip = loadedClips[Plugin.Instance.RandomEngine.Next(0, loadedClips.Length)];
                        }
                        else
                        {
                            _lastMenuMusicClip =SoundLoader.LoadAudioClip($"{_lastDirectorySelected}\\{_lastMenuMusicClip}");
                        }

                        ____defaultAudioClip = _lastMenuMusicClip;
                    }
                }
            }
        }
    }
}
