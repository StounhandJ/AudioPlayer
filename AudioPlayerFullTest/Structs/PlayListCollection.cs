using System.Collections.ObjectModel;

namespace AudioPlayerFullTest.Structs
{
    public struct PlayListCollection
    {
        public string name { get; set; }

        public ObservableCollection<MusicNotifyChanged> musics { get; set; }
    }
}