using System;
using System.Collections.Generic;

namespace AudioPlayer
{
    public class Playlist
    {
        private List<Music> musics { get; set; }
        
        private List<Music> list { get; set; }
        
        public string name { get; set; }

        private int index{ get; set; }

        public Playlist(List<Music> PlayList)
        {
            musics = PlayList;
            list = musics;
            index = -1;
        }

        public int getIndex()
        {
            return index;
        }

        public void Add(Music music)
        {
            musics.Add(music);
        }
        
        public void Del(Music music)
        {
            musics.Remove(music);
        }
        
        public bool SetIndex(int indexI)
        {
            if (indexI>-1 && indexI<list.Count)
            {
                index = indexI - 1;
                return true;
            }

            return false;
        }
        
        public bool SetMusic(Music musci)
        {
            int indexI = list.IndexOf(musci);
            if (indexI>-1)
            {
                index = indexI-1;
                return true;
            }

            return false;
        }

        public void RandomPlayList()
        {
            index = -1;
            Random random = new Random();
            var data = new List<Music>();
            foreach (var s in musics)
            {
                int j = random.Next(data.Count + 1);
                if (j == data.Count)
                {
                    data.Add(s);
                }
                else
                {
                    data.Add(data[j]);
                    data[j] = s;
                }
            }

            list = data;
        }
        
        public void StandartPlayList()
        {
            index = -1;
            list = musics;
        }

        public Music? GetNext()
        {
            if (index<list.Count-1)
            {
                index += 1;
                var t = list[index];
                return t;
            }

            return null;
        }
        
        public Music? Getlast()
        {
            if (index>0)
            {
                index -= 1;
                var t = list[index];
                return t;
            }

            return null;
        }
        
        public Music GetNow()
        {
            return list[index];
        }

        public bool IsNextMusic()
        {
            return index < list.Count-1;
        }
        
        public bool IsLastMusic()
        {
            return index>0;
        }
    }
}