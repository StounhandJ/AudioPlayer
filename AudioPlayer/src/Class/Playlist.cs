using System;
using System.Collections.Generic;

namespace AudioPlayer.Class
{
    public class Playlist
    {
        private List<Music> musics { get; set; }
        
        private List<Music> list { get; set; }

        private int index{ get; set; }
        
        /// <summary>
        /// Playlist name
        /// </summary>
        public string name { get; set; }

        public Playlist(List<Music> PlayList)
        {
            musics = PlayList;
            list = musics;
            index = -1;
        }

        /// <summary>
        /// Return music list
        /// </summary>
        public List<Music> getMusics()
        {
            return list;
        }

        /// <summary>
        /// Index of the playing music
        /// </summary>
        public int getIndex()
        {
            return index;
        }

        /// <summary>
        /// Add music to the playlistc
        /// </summary>
        public void Add(Music music)
        {
            musics.Add(music);
        }
        
        /// <summary>
        /// Delete music to the playlist
        /// </summary>
        public void Del(Music music)
        {
            musics.Remove(music);
        }
        
        /// <summary>
        /// Sets the playing music by index
        /// </summary>
        public bool SetIndex(int indexI)
        {
            if (indexI>-1 && indexI<list.Count)
            {
                index = indexI - 1;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Sets the playing music
        /// </summary>
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

        /// <summary>
        /// Shuffles the playlist
        /// </summary>
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
        
        /// <summary>
        /// Sets the original playlist
        /// </summary>
        public void StandartPlayList()
        {
            index = -1;
            list = musics;
        }

        /// <summary>
        /// Switches to the next music
        /// </summary>
        public Music? Next()
        {
            if (index<list.Count-1)
            {
                index += 1;
                var t = list[index];
                return t;
            }

            return null;
        }
        
        /// <summary>
        /// Switches to the last music
        /// </summary>
        public Music? Last()
        {
            if (index>0)
            {
                index -= 1;
                var t = list[index];
                return t;
            }

            return null;
        }
        
        /// <summary>
        /// Returns the music playing now
        /// </summary>
        public Music getNow()
        {
            return list[index];
        }

        /// <summary>
        /// Is there next music
        /// </summary>
        public bool IsNextMusic()
        {
            return index < list.Count-1;
        }
        
        /// <summary>
        /// Is there last music
        /// </summary>
        public bool IsLastMusic()
        {
            return index>0;
        }
    }
}