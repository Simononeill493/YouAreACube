using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class FileUtils
    {
        public static string GetFilePath(string defaultLocation, string fileName)
        {
            var defaultPath = Path.Combine(defaultLocation, fileName);
            if (File.Exists(defaultPath) | Directory.Exists(defaultPath))
            {
                return defaultPath;
            }

            var workingDirPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(workingDirPath) | Directory.Exists(workingDirPath))
            {
                return workingDirPath;
            }

            return null;
        }
        public static List<string> GetDirectoryContents(string path)
        {
            var contents = Directory.GetFiles(path).Select(l => Path.GetFileName(l).ToLower()).ToList();
            var subDirectories = Directory.GetDirectories(path).Select(l => Path.GetFileName(l).ToLower());
            contents.AddRange(subDirectories);
            return contents;
        }

        public static T LoadBinary<T>(string path, string name, string extension = "")
        {
            var savePath = Path.Combine(path, name + extension);

            var stream = new FileStream(savePath, FileMode.Open, FileAccess.Read);
            var loaded = (T)new BinaryFormatter().Deserialize(stream);
            stream.Close();

            return loaded;
        }
        public static void SaveBinary(object toSave, string path,string name,string extension = "")
        {
            var savePath = Path.Combine(path, name + extension);
            var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write);

            new BinaryFormatter().Serialize(stream, toSave);
            stream.Close();
        }

        public static JObject LoadJson(string path)
        {
            var fileText = File.ReadAllText(path);
            JObject result = (JObject)JsonConvert.DeserializeObject(fileText);

            return result;
        }
    }
}
