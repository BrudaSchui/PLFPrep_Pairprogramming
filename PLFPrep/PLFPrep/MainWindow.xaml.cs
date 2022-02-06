using ChinookDbLib;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PLFPrep
{
    public partial class MainWindow : Window
    {
        private readonly ChinookContext _db;
        private readonly ChinookViewModel _viewModel;

        public MainWindow(ChinookContext db, ChinookViewModel viewModel)
        {
            InitializeComponent();
            _db = db;
            DataContext = viewModel;
            _viewModel = viewModel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPlaylistTree();
        }

        private void AddSongToPlaylist(object sender, RoutedEventArgs e)
        {
            Track track = _viewModel.SelectedTrack;
            TreeViewItem? tvItem = treePlaylists.SelectedItem as TreeViewItem;
            if (track == null || tvItem == null) return;

            Playlist list = _db.Playlists.First(p => p.Name == (string)tvItem.Header);
            list.Tracks.Add(track);
            _db.SaveChanges();
            RefreshPlaylistTree();
        }

        public void RefreshPlaylistTree()
        {
            List<TreeViewItem> playlistItems = _db.Playlists
                .Include(p => p.Tracks)
                .Select(p => new TreeViewItem { Header = p.Name, ItemsSource = p.Tracks.Select(t => t.Name).OrderBy(t => t).ToList() })
                .OrderBy(i => i.Header)
                .ToList();

            treePlaylists.ItemsSource = playlistItems;
        }

        private void RefreshPlaylists(object sender, RoutedEventArgs e) => RefreshPlaylistTree();

        private void PlaylistTreeSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem? item = e.NewValue as TreeViewItem;
            _viewModel.SelectedPlaylist = item == null ? null : _db.Playlists
                .Include(p => p.Tracks)
                .First(p => p.Name == (string)item.Header);
        }
    }
}
