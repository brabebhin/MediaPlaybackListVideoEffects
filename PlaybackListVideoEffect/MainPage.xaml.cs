using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PlaybackListVideoEffect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Create player + media playback list
        /// </summary>
        MediaPlayer player = new MediaPlayer();
        MediaPlaybackList playbackList = new MediaPlaybackList();



        public MainPage()
        {
            this.InitializeComponent();
            //assign some event handlers
            this.Loaded += MainPage_Loaded;
            playbackList.ItemFailed += PlaybackList_ItemFailed;
            playbackList.CurrentItemChanged += PlaybackList_CurrentItemChanged; ;
        }

        /// <summary>
        /// Used to seek to the very end of the stream, as to prevent testers from needlessly waiting for videos of various length to play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            ThreadPoolTimer.CreateTimer(seekToEnd, TimeSpan.FromSeconds(2));
        }

        private void seekToEnd(ThreadPoolTimer timer)
        {
            player.PlaybackSession.Position = player.PlaybackSession.NaturalDuration.Subtract(TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Show a popup in case of item failed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void PlaybackList_ItemFailed(MediaPlaybackList sender, MediaPlaybackItemFailedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                MessageDialog diag = new MessageDialog(args.Error.ExtendedError.Message, "Item fail");
                await diag.ShowAsync();
            });
        }

        /// <summary>
        /// generate a list of 100 playback items and play them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mkv");


            var videoFiles = await picker.PickMultipleFilesAsync();
            List<StorageFile> targetFiles = new List<StorageFile>();
            int i = 0;
            //get 1000 files.
            while (targetFiles.Count < 1000)
            {
                targetFiles.Add(videoFiles[i]);
                i++;
                i = i % videoFiles.Count;
            }

            foreach (var f in targetFiles)
            {
                MediaSource src = MediaSource.CreateFromStorageFile(f);
                MediaPlaybackItem item = new MediaPlaybackItem(src);
                playbackList.Items.Add(item);
            }
            mpeElement.MediaPlayer = player;
            //add the video effect
            //comment this line to remove the video effect
            player.AddVideoEffect("VideoEffect.BasicVideoEffect", true, new PropertySet());
            player.AutoPlay = true;
            playbackList.AutoRepeatEnabled = false;
            player.Source = playbackList;
        }
    }
}
