using ChinookDbLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MvvmTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PLFPrep
{
    public class ChinookViewModel : ObservableObject
    {
        private readonly ChinookContext _db;
        private readonly ObservableCollection<Album> albumsDefault;

        public ChinookViewModel(ChinookContext db)
        {
            _db = db;
            albumsDefault = db.Albums.Include(a => a.Tracks).Include(a => a.Artist).OrderBy(a => a.Title).AsObservableCollection();
            Albums = albumsDefault;
        }

        private ObservableCollection<Album> _albums;

        public ObservableCollection<Album> Albums
        {
            get { return _albums; }
            set
            {
                _albums = value;
                NotifyPropertyChanged(nameof(Albums));
            }
        }

        private ObservableCollection<Track> _tracks;

        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                NotifyPropertyChanged(nameof(Tracks));
            }
        }

        private Album _selectedAlbum;

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                TrackSearchString = "";
                if (value != null) Tracks = value.Tracks.OrderBy(t => t.Name).AsObservableCollection();
                NotifyPropertyChanged(nameof(SelectedAlbum));
            }
        }

        private string _albumSearchString;

        public string AlbumSearchString
        {
            get { return _albumSearchString; }
            set
            {
                _albumSearchString = value;

                if (value.Length == 0) Albums = albumsDefault;
                else Albums = _db.Albums.Where(a => a.Title.Contains(value)).AsObservableCollection();

                NotifyPropertyChanged(nameof(AlbumSearchString));
            }
        }

        private string _trackSearchString;
        private string newPlaylistName = "";

        public string TrackSearchString
        {
            get { return _trackSearchString; }
            set
            {
                _trackSearchString = value;

                if (SelectedAlbum == null) return;

                if (value.Length > 0) Tracks = SelectedAlbum.Tracks.Where(t => t.Name.Contains(value)).AsObservableCollection();
                else if (value.Length == 0) Tracks = SelectedAlbum.Tracks.AsObservableCollection();

                NotifyPropertyChanged(nameof(TrackSearchString));
            }
        }

        public Track SelectedTrack { get; set; }

        public string NewPlaylistName
        {
            get => newPlaylistName;
            set
            {
                newPlaylistName = value;
                NotifyPropertyChanged(nameof(NewPlaylistName));
            }
        }

        public ICommand CreatePlaylistCommand => new RelayCommand<string>(_ =>
        {
            _db.Playlists.Add(new Playlist { PlaylistId = new Random().Next(), Name = NewPlaylistName });
            _db.SaveChanges();
            NewPlaylistName = "";
        }, _ => NewPlaylistName.Trim().Length > 0);

        public ICommand DeletePlaylistCommand => new RelayCommand<string>(DoDeletePlaylist, _ => SelectedPlaylist != null);

        private void DoDeletePlaylist(string? obj)
        {
            SelectedPlaylist!.Tracks.Clear();
            _db.Playlists.Remove(SelectedPlaylist);
            _db.SaveChanges();
        }

        public Playlist? SelectedPlaylist { get; set; } = null; // Set via Mainwindow, not a MVVM property

        public ICommand ExportPlaylistsCommand => new RelayCommand<string>(DoExportPlaylists, _ => true);

        private void DoExportPlaylists(string? obj)
        {
            SaveFileDialog dialog = new();
            dialog.FileName = "Playlists";
            dialog.DefaultExt = ".csv";

            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true)
            {
                string[] playlistLines = _db.Playlists
                .Include(p => p.Tracks)
                .Select(p => $"{p.Name},{string.Join(",", p.Tracks.Select(t => t.TrackId))}")
                .ToArray();
                File.WriteAllLines(dialog.FileName, playlistLines, System.Text.Encoding.UTF8);
            }
        }

        public ICommand ImportPlaylistsCommand => new RelayCommand<string>(DoImportPlaylists, _ => true);

        private void DoImportPlaylists(string? obj)
        {
            OpenFileDialog dialog = new();
            dialog.FileName = "Playlists";
            dialog.DefaultExt = ".csv";

            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true)
            {
                List<Playlist> listBuffer = new();
                string[] playlistLines = File.ReadAllLines(dialog.FileName);
                foreach (string line in playlistLines)
                {
                    string[] parts = line.Split(",");
                    string name = parts[0];
                    Playlist list = new() { PlaylistId = new Random().Next(), Name = name };

                    List<string> trackIdStrings = parts.ToList();
                    trackIdStrings.RemoveAt(0);
                    
                    int outId;
                    List<int> trackIds = trackIdStrings
                        .Select(id => int.TryParse(id, out outId) ? outId : -1)
                        .ToList();
                    
                    List<Track> tracks = new();
                    foreach (int tId in trackIds)
                    {
                        if (tId > 0) tracks.Add(_db.Tracks.First(t => t.TrackId == tId));
                        else continue;
                    }

                    list.Tracks = tracks;
                    listBuffer.Add(list);
                }
                _db.Database.ExecuteSqlRaw("DELETE FROM PlaylistTrack WHERE 1=1");
                _db.Database.ExecuteSqlRaw("DELETE FROM Playlist WHERE 1=1");
                listBuffer.ForEach(l => _db.Playlists.Add(l));
                _db.SaveChanges();
            }
        }
    }
}
