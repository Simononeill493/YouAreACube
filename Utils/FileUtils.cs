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
        public static void SaveBinary(object toSave, string path,string name,string extension = "")
        {
            var savePath = Path.Combine(path, name) + extension;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, toSave);
            stream.Close();
        }

        public static T LoadBinary<T>(string path, string name)
        {
            var savePath = Path.Combine(path, name);
            IFormatter formatter = new BinaryFormatter();

            var stream = new FileStream(savePath, FileMode.Open, FileAccess.Read);
            T loaded = (T)formatter.Deserialize(stream);
            stream.Close();

            return loaded;
        }

        public static JObject LoadJson(string path)
        {
            var fileText = File.ReadAllText(path);
            JObject result = (JObject)JsonConvert.DeserializeObject(fileText);

            return result;
        }
    }
}
