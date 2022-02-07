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
        
        private static AudioClip[] _lastMenuMusicClips;
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
                    if (_lastMusicSelected == Plugin.CurrentConfig.MenuMusic && _lastDirectorySelected == Plugin.CurrentConfig.MenuMusicDirectory)
                    { 
                        ____defaultAudioClip = _lastMenuMusicClips[Plugin.Instance.RandomEngine.Next(0, _lastMenuMusicClips.Length)];
                    }
                    else
                    {
                        _lastMusicSelected = Plugin.CurrentConfig.MenuMusic;
                        _lastDirectorySelected = Plugin.CurrentConfig.MenuMusicDirectory;

                        if (_lastMusicSelected == "Random")
                        {
                            _lastMenuMusicClips = SoundLoader.LoadAudioClips(_lastDirectorySelected);
                        }
                        else
                        {
                            _lastMenuMusicClips = new AudioClip[] { SoundLoader.LoadAudioClip($"{_lastDirectorySelected}\\{_lastMusicSelected}") };
                        }

                        ____defaultAudioClip = _lastMenuMusicClips[Plugin.Instance.RandomEngine.Next(0, _lastMenuMusicClips.Length)];
                    }
                }
            }
        }
    }
}
