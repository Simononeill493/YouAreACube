using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputModel
    {
        [JsonIgnore]
        public string DisplayValue => InputOption.GetDisplayValue();
        public string StoredType => InputOption.GetStoredType();
        public string JsonValue => InputOption.GetJsonValue();

        [JsonIgnore]
        public BlockInputOption InputOption = BlockInputOption.Undefined;

        public BlockInputModel()
        {
            InputOption = BlockInputOption.Undefined;
        }
    }
}
