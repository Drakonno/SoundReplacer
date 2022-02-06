using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class ClickSoundPatch
    {
        private static List<AudioClip> _originalClickClips;

        private static AudioClip[] _lastClickClips;
        private static string _lastClickSelected;
        private static string _lastDirectorySelected;

        [HarmonyPatch(typeof(BasicUIAudioManager))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class BasicUIAudioManagerPatch
        {
            public static void Prefix(ref AudioClip[] ____clickSounds)
            {
                if (_originalClickClips == null)
                {
                    _originalClickClips = new List<AudioClip>();
                    _originalClickClips.AddRange(____clickSounds);
                }

                if (Plugin.CurrentConfig.ClickSound == "None")
                {
                    ____clickSounds = new AudioClip[] { SoundLoader.GetEmptyClip() };
                }
                else if (Plugin.CurrentConfig.ClickSound == "Default")
                {
                    ____clickSounds = _originalClickClips.ToArray();
                }
                else
                {
                    if (_lastClickSelected == Plugin.CurrentConfig.ClickSound && _lastClickSelected == Plugin.CurrentConfig.ClickSoundDirectory)
                    {
                        ____clickSounds = _lastClickClips;
                    }
                    else
                    {
                        _lastDirectorySelected = Plugin.CurrentConfig.ClickSoundDirectory;
                        _lastClickSelected = Plugin.CurrentConfig.ClickSound;
                        if (_lastClickSelected == "Random")
                        {
                            _lastClickClips = SoundLoader.LoadAudioClips(_lastDirectorySelected);
                        }
                        else
                        {
                            _lastClickClips = new AudioClip[] { SoundLoader.LoadAudioClip($"{_lastDirectorySelected}\\{_lastClickSelected}") };
                        }
                        ____clickSounds = _lastClickClips;
                    }
                }
            }
        }
    }
}
