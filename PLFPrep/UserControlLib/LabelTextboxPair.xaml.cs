using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace UserControlLib
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class LabelTextboxPair : UserControl
    {
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public LabelTextboxPair()
        {
            InitializeComponent();
        }

        private string _title;

        [Category("Data"), Description("Get or set the pair's title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; lblTitle.Content = value; }
        }

        private string _value;

        [Category("Data"), Description("Get or set the value the pair represents")]
        public string Value
        {
            get { return _value; }
            set { _value = value; tbValue.Text = value; }
        }

        private bool _readOnly;

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; tbValue.IsReadOnly = value; }
        }

        private void tbValue_TextChanged(object sender, TextChangedEventArgs e) =>
            ValueChanged?.Invoke(this, new ValueChangedEventArgs { NewValue = tbValue.Text });
    }
}
