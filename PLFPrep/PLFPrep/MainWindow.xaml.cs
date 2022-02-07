using ChinookDbLib;
using Microsoft.EntityFrameworkCore;
using System;
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) => RefreshPlaylistTree();

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
            List<Playlist> playlists = _db.Playlists.Include(p => p.Tracks).ToList();
            List<TreeViewItem> items = new();
            playlists.ForEach(list =>
            {
                TreeViewItem listItem = new() { Header = list.Name, Tag = list.PlaylistId };
                list.Tracks.ToList().ForEach(track => listItem.Items.Add(new TreeViewItem { Header = track.Name }));
                items.Add(listItem);
            });

            treePlaylists.ItemsSource = items.OrderBy(tvItem => tvItem.Header).ToList();
        }

        private void RefreshPlaylists(object sender, RoutedEventArgs e) => RefreshPlaylistTree();

        private void PlaylistTreeSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem? item = e.NewValue as TreeViewItem;
            if (item == null) return;
            if (item.Parent == null)
            {
                _viewModel.SelectedPlaylist = GetPlaylistForItem(item);
            }
            else
            {
                _viewModel.SelectedPlaylist = GetPlaylistForItem((TreeViewItem)item.Parent);
                _viewModel.SelectedPlaylistTrack = _db.Tracks.First(t => t.Name == (string)item.Header);
            }
        }

        private Playlist GetPlaylistForItem(TreeViewItem item) => _db.Playlists.First(p => p.PlaylistId == long.Parse(item.Tag.ToString()!));


    }
}
