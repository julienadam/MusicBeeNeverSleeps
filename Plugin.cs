using System;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "MusicBee Nevers Sleeps";
            about.Description = "Prevents the display from going in standby mode when music is playing.";
            about.Author = "Julien Adam";
            about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            about.Type = PluginType.General;
            about.VersionMajor = 1;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = ReceiveNotificationFlags.PlayerEvents;
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            return false;
        }

        /// <summary>
        /// called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen. 
        /// its up to you to figure out whether anything has changed and needs updating
        /// </summary>
        public void SaveSettings()
        {
        }

        /// <summary>
        /// MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        /// </summary>
        /// <param name="reason"></param>
        public void Close(PluginCloseReason reason)
        {
            WinAPI.AllowMonitorPowerdown();
        }

        /// <summary>
        /// uninstall this plugin - clean up any persisted files
        /// </summary>
        public void Uninstall()
        {
            WinAPI.AllowMonitorPowerdown();
        }

        /// <summary>
        /// Receive event notifications from MusicBee
        /// </summary>
        /// <remarks>you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event</remarks>
        /// <param name="sourceFileUrl"></param>
        /// <param name="type"></param>
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PlayStateChanged:
                    var playState = mbApiInterface.Player_GetPlayState.Invoke();
                    switch (playState)
                    {
                        case PlayState.Playing:
                            WinAPI.PreventMonitorPowerdown();
                            break;
                        default:
                            WinAPI.AllowMonitorPowerdown();
                            break;
                    }
                    break;
            }
        }

        public string[] GetProviders()
        {
            return null;
        }

        public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider)
        {
            return null;
        }

        public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider)
        {
            return null;
        }
   }
}