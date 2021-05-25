using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AudioPlayerFullTest.Structs
{
    public struct Profile
    {
        public string name { get; set; }
        
        public string password { get; set; }
        
        public ObservableCollection<PlayListCollection> playLists { get; set; }
    }
}