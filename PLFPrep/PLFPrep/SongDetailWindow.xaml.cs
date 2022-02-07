using ChinookDbLib;
using System.Windows;

namespace PLFPrep
{
    public partial class SongDetailWindow : Window
    {
        private readonly Track _track;
        private readonly ChinookContext _db;

        public SongDetailWindow(ChinookViewModel viewModel)
        {
            InitializeComponent();
            _track = viewModel.SelectedTrack;
        }

        private void InitFields()
        {
            pName.Value = _track.Name;
            pArtist.Value = _track.Album.Artist.Name;
            pComposer.Value = _track.Composer;
            pDuration.Value = _track.Milliseconds.ToString();
            pPrice.Value = _track.UnitPrice.ToString();
            pBytes.Value = _track.Bytes.ToString();
        }

        private void SongDetailWindow_Loaded(object sender, RoutedEventArgs e) => InitFields();

        private void SongDetailsChanged(object sender, UserControlLib.ValueChangedEventArgs e)
        {
            _track.Name = pName.Value;
            _track.Composer = pComposer.Value;
        }
    }
}
