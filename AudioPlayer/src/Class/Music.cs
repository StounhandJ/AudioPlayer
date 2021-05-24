using System;

namespace AudioPlayer.Class
{
    public struct Music
    {
        /// <summary>
        /// The path to music
        /// </summary>
        public Uri source { get; set; }
        
        /// <summary>
        /// Music name
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// The path to image
        /// </summary>
        public Uri sourceImg { get; set; }
    }
}