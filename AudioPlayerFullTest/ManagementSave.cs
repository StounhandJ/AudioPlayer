using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest
{
    public static class ManagementSave
    {
        public static string savePath = Directory.GetCurrentDirectory()+"\\profiles.json";
        
        public static async Task saveProfilesJSON(List<Profile> favorites)
        {
            using (StreamWriter sw = new StreamWriter(savePath, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(favorites));
            }
        }

        public static List<Profile> loadProfilesJSON()
        {
            if (File.Exists(savePath))
            {
                using (StreamReader fs = new StreamReader(savePath))
                {
                    string json = fs.ReadToEnd();
                    return JsonSerializer.Deserialize<List<Profile>>(json);
                }
            }
            return new List<Profile>();
        }
    }
}