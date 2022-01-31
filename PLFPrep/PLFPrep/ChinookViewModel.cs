using ChinookDbLib;
using Microsoft.EntityFrameworkCore;
using MvvmTools;
using System.Collections.ObjectModel;
using System.Linq;

namespace PLFPrep
{
    public class ChinookViewModel : ObservableObject
    {
        private ChinookContext _db;
        private ObservableCollection<Album> albumsDefault;

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
                else Albums = Albums.Where(a => a.Title.StartsWith(value)).AsObservableCollection();
                NotifyPropertyChanged(nameof(AlbumSearchString));
            }
        }

        private string _trackSearchString;

        public string TrackSearchString
        {
            get { return _trackSearchString; }
            set
            {
                _trackSearchString = value;
                if (value.Length == 0) Tracks = _selectedAlbum.Tracks.OrderBy(t => t.Name).AsObservableCollection();
                else Tracks = Tracks.Where(t => t.Name.StartsWith(value)).AsObservableCollection();
                NotifyPropertyChanged(nameof(TrackSearchString));
            }
        }
    }
}
