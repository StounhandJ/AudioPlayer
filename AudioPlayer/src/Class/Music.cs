using System;
using System.ComponentModel;

namespace AudioPlayer
{
    public struct Music
    {
        public string source { get; set; }
        public string name { get; set; }
        public Uri sourceImg { get; set; }
    }
}