using System.Collections.Generic;

namespace AudioPlayer
{
    public class Playlist
    {
        private List<Music> musics { get; set; }
        
        public string name { get; set; }

        private int index{ get; set; }

        public Playlist(List<Music> PlayList)
        {
            musics = PlayList;
            index = -1;
        }

        public void Add(Music music)
        {
            musics.Add(music);
        }

        public Music? GetNext()
        {
            if (index<musics.Count-1)
            {
                index += 1;
                var t = musics[index];
                return t;
            }

            return null;
        }
        
        public Music? Getlast()
        {
            if (index>0)
            {
                index -= 1;
                var t = musics[index];
                return t;
            }

            return null;
        }
        
        public Music? GetNow()
        {
            return musics[index];
        }

        public bool IsNextMusic()
        {
            return index < musics.Count-1;
        }
        
        public bool IsLastMusic()
        {
            return index>0;
        }
    }
}