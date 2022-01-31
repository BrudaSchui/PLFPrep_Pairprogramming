using ChinookDbLib;
using System;
using System.Windows;

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
        }
    }
}
