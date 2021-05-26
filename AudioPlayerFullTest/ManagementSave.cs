using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AudioPlayerFullTest.Structs;
using Newtonsoft.Json;

namespace AudioPlayerFullTest
{
    public static class ManagementSave
    {
        public static string savePath = Directory.GetCurrentDirectory()+"\\profiles.json";

        private static string g;
        public static async Task saveProfilesJSON(List<Profile> favorites)
        {
            using (StreamWriter sw = new StreamWriter(savePath, false, System.Text.Encoding.UTF8))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(favorites));
            }
        }

        public static List<Profile> loadProfilesJSON()
        {
            if (File.Exists(savePath))
            {
                using (StreamReader fs = new StreamReader(savePath))
                {
                    string json = fs.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Profile>>(json);
                }
            }
            return new List<Profile>();
        }
    }
}