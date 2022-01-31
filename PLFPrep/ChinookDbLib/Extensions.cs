namespace ChinookDbLib
{
    public partial class Artist
    {
        public override string ToString() => $"{Name}";
    }

    public partial class Album
    {
        public override string ToString() => $"{Title}";
    }

    public partial class Track
    {
        public override string ToString() => $"{Name}";
    }
}
