using ChinookDbLib;
using System;
using System.Linq;
using System.Windows;

namespace PLFPrep
{
    public partial class MainWindow : Window
    {
        private readonly ChinookContext _db;
        private readonly IServiceProvider serviceProvider;

        public MainWindow(ChinookContext db, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this._db = db;
            this.serviceProvider = serviceProvider;
        }

        private void Mainwindow_Loaded(object sender, RoutedEventArgs e)
        {

            int nr = _db.Employees.Count();
             Title = $"{nr} employees";

            try
            {
            } catch (Exception ex)
            {
                Title = ex.Message;
            }
        }
    }
}
