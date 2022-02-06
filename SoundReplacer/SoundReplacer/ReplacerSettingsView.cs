using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SoundReplacer
{
    internal class ReplacerSettingsView : BSMLResourceViewController
    {
        public override string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);

        [UIParams]
        private BSMLParserParams parserParams;

        [UIComponent("ddl-good-hitsound")]
        DropDownListSetting ddlsGoodHitsound;

        [UIComponent("ddl-bad-hitsound")]
        DropDownListSetting ddlsBadHitsound;

        [UIComponent("ddl-menu-music")]
        DropDownListSetting ddlsMenuMusic;

        [UIComponent("ddl-click-sound")]
        DropDownListSetting ddlsClickSound;

        [UIComponent("ddl-success-sound")]
        DropDownListSetting ddlsSuccessSound;

        [UIComponent("ddl-fail-sound")]
        DropDownListSetting ddlsFailSound;


        // Actions
        [UIAction("#directory-list-reload")]
        public void OnDirectoryListReload() => ReloadDirectoriesList();

        private void ReloadDirectoriesList()
        {
            Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"ReloadDirectoriesList");
            SettingsGoodHitSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsGoodHitSoundDirectory]); 
            ddlsGoodHitsound.values = SettingsGoodHitSoundList; 
            ddlsGoodHitsound.UpdateChoices();

            SettingsBadHitSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsBadHitSoundDirectory]); 
            ddlsBadHitsound.values = SettingsBadHitSoundList;
            ddlsBadHitsound.UpdateChoices();

            SettingsClickSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsClickSoundDirectory]); 
            ddlsClickSound.values = SettingsClickSoundList;
            ddlsClickSound.UpdateChoices();

            SettingsMenuMusicList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsMenuMusicDirectory]); 
            ddlsMenuMusic.values = SettingsMenuMusicList;
            ddlsMenuMusic.UpdateChoices();

            SettingsSuccessSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsSuccessSoundDirectory]);
            ddlsSuccessSound.values = SettingsSuccessSoundList;
            ddlsSuccessSound.UpdateChoices();

            SettingsFailSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[SettingsFailSoundDirectory]);
            ddlsFailSound.values = SettingsFailSoundList;
            ddlsFailSound.UpdateChoices();
        }
         
        // Directories --- 
        [UIValue("good-hitsound-directories")]
        public List<object> SettingsGoodHitSoundDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("good-hitsound-directory-value")]
        protected string SettingsGoodHitSoundDirectory
        {
            get => Plugin.CurrentConfig.GoodHitSoundDirectory;
            set
            {
                Plugin.CurrentConfig.GoodHitSoundDirectory = value;
                SettingsGoodHitSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingCurrentGoodHitSoundDirectory with entries:");
                foreach (var element in SettingsGoodHitSoundList)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("good-hitsound-list");
                NotifyPropertyChanged("good-hitsound-list-value");
            }
        }

        [UIValue("bad-hitsound-directories")]
        public List<object> SettingsBadHitSoundDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("bad-hitsound-directory-value")]
        protected string SettingsBadHitSoundDirectory
        {
            get => Plugin.CurrentConfig.BadHitSoundDirectory;
            set
            {
                Plugin.CurrentConfig.BadHitSoundDirectory = value;
                SettingsBadHitSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingCurrentBadHitSoundDirectory with entries:");
                foreach (var element in SettingsBadHitSoundList)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("bad-hitsound-list");
                NotifyPropertyChanged("bad-hitsound-list-value");
            }
        }

        [UIValue("menu-music-directories")]
        public List<object> SettingsMenuMusicDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("menu-music-directory-value")]
        protected string SettingsMenuMusicDirectory
        {
            get => Plugin.CurrentConfig.MenuMusicDirectory;
            set
            {
                Plugin.CurrentConfig.MenuMusicDirectory = value;
                SettingsMenuMusicList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingsMenuMusicDirectory with entries:");
                foreach (var element in SettingsMenuMusicList)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("menu-music-list");
                NotifyPropertyChanged("menu-music-list-value");
            }
        }

        [UIValue("click-sound-directories")]
        public List<object> SettingsClickSoundDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("click-sound-directory-value")]
        protected string SettingsClickSoundDirectory
        {
            get => Plugin.CurrentConfig.ClickSoundDirectory;
            set
            {
                Plugin.CurrentConfig.ClickSoundDirectory = value;
                SettingsClickSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingsClickSoundDirectory with entries:");
                foreach (var element in SettingsClickSoundList)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("click-sound-list");
                NotifyPropertyChanged("click-sound-list-value");
            }
        }

        [UIValue("success-sound-directories")]
        public List<object> SettingsSuccessSoundDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("success-sound-directory-value")]
        protected string SettingsSuccessSoundDirectory
        {
            get => Plugin.CurrentConfig.SuccessSoundDirectory;
            set
            {
                Plugin.CurrentConfig.SuccessSoundDirectory = value;
                SettingsSuccessSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingsSuccessSoundDirectory with entries:");
                foreach (var element in SettingsSuccessSoundList)
                {
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("success-sound-list");
                NotifyPropertyChanged("success-sound-list-value");
            }
        }

        [UIValue("fail-sound-directories")]
        public List<object> SettingsFailSoundDirectories = new List<object>(SoundLoader.GlobalSoundDictionary.Keys.ToList());

        [UIValue("fail-sound-directory-value")]
        protected string SettingsFailSoundDirectory
        {
            get => Plugin.CurrentConfig.FailSoundDirectory;
            set
            {
                Plugin.CurrentConfig.FailSoundDirectory = value;
                SettingsFailSoundList = new List<object>(SoundLoader.GlobalSoundDictionary[value]);

                Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"Set directory: {value} for SettingsFailSoundDirectory with entries:");
                foreach (var element in SettingsFailSoundList)
                { 
                    Plugin.Log.Log(IPA.Logging.Logger.Level.Info, $"{element}");
                }

                parserParams.EmitEvent("directory-list-reload");
                NotifyPropertyChanged("fail-sound-list");
                NotifyPropertyChanged("fail-sound-list-value");
            }
        }
        // --------------
         
        // Values ---
        [UIValue("good-hitsound-list")]
        public List<object> SettingsGoodHitSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("good-hitsound-list-value")]
        protected string SettingCurrentGoodHitSound
        {
            get => Plugin.CurrentConfig.GoodHitSound;
            set => Plugin.CurrentConfig.GoodHitSound = value;
        }

        [UIValue("bad-hitsound-list")]
        public List<object> SettingsBadHitSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("bad-hitsound-list-value")]
        protected string SettingCurrentBadHitSound
        {
            get => Plugin.CurrentConfig.BadHitSound;
            set => Plugin.CurrentConfig.BadHitSound = value;
        }

        [UIValue("menu-music-list")]
        public List<object> SettingsMenuMusicList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("menu-music-list-value")]
        protected string SettingCurrentMenuMusic
        {
            get => Plugin.CurrentConfig.MenuMusic;
            set
            {
                Plugin.CurrentConfig.MenuMusic = value;
                Helper.RefreshMenuMusic();
            }
        }

        [UIValue("click-sound-list")]
        public List<object> SettingsClickSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("click-sound-list-value")]
        protected string SettingCurrentClickSound
        {
            get => Plugin.CurrentConfig.ClickSound;
            set
            {
                Plugin.CurrentConfig.ClickSound = value;
                Helper.RefreshClickSounds();
            }
        }

        [UIValue("success-sound-list")]
        public List<object> SettingsSuccessSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("success-sound-list-value")]
        protected string SettingCurrentSuccessSound
        {
            get => Plugin.CurrentConfig.SuccessSound;
            set => Plugin.CurrentConfig.SuccessSound = value;
        }

        [UIValue("fail-sound-list")]
        public List<object> SettingsFailSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("fail-sound-list-value")]
        protected string SettingCurrentFailSound
        {
            get => Plugin.CurrentConfig.FailSound;
            set => Plugin.CurrentConfig.FailSound = value;
        }
        // ----------
    }
}
