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
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(ChinookContext db, IServiceProvider serviceProvider, ChinookViewModel viewModel)
        {
            InitializeComponent();
            _db = db;
            _serviceProvider = serviceProvider;
            DataContext = viewModel;
            
            //List<Playlist> playlists = _db.Playlists.Include(p => p.Tracks).ToList();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<TreeViewItem> playlistItems = _db.Playlists.Include(p => p.Tracks).Select(p => new TreeViewItem { Header = p.Name }).ToList();
            
            treePlaylists.ItemsSource = playlistItems;
        }
    }
}
