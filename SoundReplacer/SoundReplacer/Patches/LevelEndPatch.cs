using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class LevelEndPatch
    {
        private static AudioClip[] _lastSuccessClips;
        private static string _lastSuccessSelected;
        private static string _lastSuccessDirectory;

        private static AudioClip[] _lastFailClips;
        private static string _lastFailSelected;
        private static string _lastFailDirectory;

        public static void DummyCallback()
        {
            /**/
        }

        private static readonly Action DummyAction = new Action(DummyCallback);

        [HarmonyPatch(typeof(ResultsViewController))]
        [HarmonyPatch("DidActivate", MethodType.Normal)]
        public class DidActivatePatch
        {
            public static void Prefix(bool addedToHierarchy, ref SongPreviewPlayer ____songPreviewPlayer, ref LevelCompletionResults ____levelCompletionResults)
            {
                if (!addedToHierarchy)
                    return;
                
                if (____levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
                {
                    if (!(Plugin.CurrentConfig.SuccessSound == "Default" || Plugin.CurrentConfig.SuccessSound == "None"))
                    {
                        AudioClip desiredSuccessClip;

                        if (_lastSuccessSelected == Plugin.CurrentConfig.SuccessSound && _lastSuccessDirectory == Plugin.CurrentConfig.SuccessSoundDirectory)
                        {
                            desiredSuccessClip = _lastSuccessClips[Plugin.Instance.RandomEngine.Next(0, _lastSuccessClips.Length)];
                        }
                        else
                        {
                            _lastSuccessSelected = Plugin.CurrentConfig.SuccessSound;
                            _lastSuccessDirectory = Plugin.CurrentConfig.SuccessSoundDirectory;
                            if (_lastSuccessSelected == "Random")
                            {
                                _lastSuccessClips = SoundLoader.LoadAudioClips(_lastSuccessDirectory);
                            }
                            else
                            {
                                _lastSuccessClips = new AudioClip[] { SoundLoader.LoadAudioClip($"{_lastSuccessDirectory}\\{_lastSuccessSelected}") };
                            } 
                            desiredSuccessClip = _lastSuccessClips[Plugin.Instance.RandomEngine.Next(0, _lastSuccessClips.Length)];
                        }
                         
                        ____songPreviewPlayer.CrossfadeTo(desiredSuccessClip, 0f, 0f, Math.Min(desiredSuccessClip.length, 20.0f), DummyAction);
                    }
                }

                if (____levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed)
                {
                    if (!(Plugin.CurrentConfig.FailSound == "Default" || Plugin.CurrentConfig.FailSound == "None"))
                    { 
                        AudioClip desiredFailClip;

                        if (_lastFailSelected == Plugin.CurrentConfig.FailSound && _lastFailDirectory == Plugin.CurrentConfig.FailSoundDirectory)
                        { 
                            desiredFailClip = _lastFailClips[Plugin.Instance.RandomEngine.Next(0, _lastFailClips.Length)]; 
                        }
                        else
                        { 
                            _lastFailSelected = Plugin.CurrentConfig.FailSound; 
                            _lastFailDirectory = Plugin.CurrentConfig.FailSoundDirectory;
                             
                            if (_lastFailSelected == "Random")
                            { 
                                _lastFailClips = SoundLoader.LoadAudioClips(_lastFailDirectory);
                            }
                            else
                            { 
                                _lastFailClips = new AudioClip[] { SoundLoader.LoadAudioClip($"{_lastFailDirectory}\\{_lastFailSelected}") };
                            } 
                            desiredFailClip = _lastFailClips[Plugin.Instance.RandomEngine.Next(0, _lastFailClips.Length)]; 
                        }
                         
                        ____songPreviewPlayer.CrossfadeTo(desiredFailClip, 0f, 0f, Math.Min(desiredFailClip.length, 20.0f), DummyAction);
                    }
                }
            }
        }
    }
}

