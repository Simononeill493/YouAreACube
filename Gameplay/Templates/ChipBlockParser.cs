using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipBlockParser
    {
        public static string ParseBlockIntoJson(ChipBlock toParse)
        {
            JObject outputObject = new JObject();
            var allChips = toParse.GetAllChipsAndSubChips();
            var chipToObject = new Dictionary<string, JObject>();
            var chipToData = new Dictionary<string, ChipData>();
            var chipToInputs = new Dictionary<string, List<Tuple<string,string>>>();

            foreach (var c in allChips)
            {
                var chipData = ChipDatabase.GetChipDataFromChip(c);
                chipToData[c.Name] = chipData;

                var chipJobject = new JObject();
                chipToObject[c.Name] = chipJobject;
                chipJobject["name"] = c.Name;
                chipJobject["type"] = chipData.Name;

                var inputsList = new List<Tuple<string, string>>();
                chipToInputs[c.Name] = inputsList;
                for (int i=0;i<chipData.NumInputs;i++)
                {
                    inputsList.Add(new Tuple<string, string>("",""));
                }

                _setGenericChipAttributes(c, chipJobject, chipData);
                _setSpecialChipAttributes(c, chipJobject, chipData);
            }

            foreach(var chip in allChips)
            {
                var chipData = chipToData[chip.Name];
                var chipType = chip.GetType();
                if(chipData.HasOutput)
                {
                    dynamic targets1 = chipType.GetField("Targets").GetValue(chip);
                    _setChipTargets(chip,0,targets1,chipToInputs);

                    dynamic targets2 = chipType.GetField("Targets2").GetValue(chip);
                    _setChipTargets(chip, 1,targets2, chipToInputs);

                    dynamic targets3 = chipType.GetField("Targets3").GetValue(chip);
                    _setChipTargets(chip, 2,targets3, chipToInputs);
                }
            }

            foreach(var chip in allChips)
            {
                var chipJObject = chipToObject[chip.Name];
                var inputs = chipToInputs[chip.Name];

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (inputs[i].Item1.Equals(""))
                    {
                        var inputPinValue = _getInputPinValue(chip, i);
                        inputs[i] = new Tuple<string, string>("Value", inputPinValue);
                    }
                }

                if (inputs.Count > 0)
                {
                    var inputsJObject = new List<JObject>();
                    foreach(var input in inputs)
                    {
                        var inputJObject = new JObject();
                        inputJObject["inputType"] = input.Item1;
                        inputJObject["inputValue"] = input.Item2;
                        inputsJObject.Add(inputJObject);
                    }

                    chipJObject["inputs"] = JToken.FromObject(inputsJObject);
                }

            }

            var allBlocks = toParse.GetBlockAndSubBlocks();
            var blockTokens = new List<JToken>();
            foreach(var block in allBlocks)
            {
                var chipsInBlock = new List<JToken>();
                foreach(var chip in block.Chips)
                {
                    chipsInBlock.Add(chipToObject[chip.Name]);
                }

                var token = new JObject();
                token["name"] = block.Name;
                token["chips"] = JToken.FromObject(chipsInBlock);

                blockTokens.Add(token);
            }

            var jobjectList = JToken.FromObject(blockTokens);
            return jobjectList.ToString();
        }

        private static void _setSpecialChipAttributes(IChip chip,JToken chipToken,ChipData data)
        {
            if(data.Name.Equals("If"))
            {
                var ifChip = (IfChip)chip;
                chipToken["yes"] = ifChip.Yes.Name;
                chipToken["no"] = ifChip.No.Name;
            }
            if(data.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chip;
                var keysAndEffects = new List<Tuple<string, string>>();
                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add(new Tuple<string, string>(keyBlock.Item1.ToString(), keyBlock.Item2.Name));
                }

                var keySwitchObject = JToken.FromObject(keysAndEffects);
                chipToken["keyEffects"] = keySwitchObject;
            }
        }

        private static void _setGenericChipAttributes(IChip chip, JToken chipToken, ChipData data)
        {
            if(data.IsGeneric)
            {
                var type = chip.GetType();
                var typeArgument = type.GenericTypeArguments.First();
                chipToken["typeArgument"] = typeArgument.Name;

            }
        }

        private static void _setChipTargets(IChip chip,int inputPinIndex,dynamic targets, Dictionary<string, List<Tuple<string, string>>> chipToInputs)
        {
            foreach (var target in targets)
            {
                IChip targetAsChip = (IChip)target;
                var targetInputs = chipToInputs[targetAsChip.Name];
                targetInputs[inputPinIndex] = new Tuple<string, string>("Reference",chip.Name);
            }
        }

        private static string _getInputPinValue(IChip chip,int pinIndex)
        {
            var propertyName = "ChipInput" + (pinIndex+1).ToString();
            var chipType = chip.GetType();
            var property = chipType.GetProperty(propertyName);
            var value = property.GetValue(chip);
            var valueAsString = value.ToString();

            return valueAsString;
        }


        public static ChipBlock ParseJsonToBlock(string json)
        {
            var chipBlocksToken = JToken.Parse(json);
            var blocks = new Dictionary<string, ChipBlock>();
            var chips = new Dictionary<string, IChip>();
            var data = new Dictionary<string, ChipData>();
            var chipTokens = new Dictionary<string, JToken>();

            foreach (var blockToken in chipBlocksToken)
            {
                var name = blockToken["name"].ToString();
                blocks[name] = new ChipBlock() { Name = name };

                foreach (var chipToken in blockToken["chips"])
                {
                    var chipName = chipToken["name"].ToString();
                    chipTokens[chipName] = chipToken;

                    var chipTypeName = chipToken["type"].ToString();
                    var chipType = ChipDatabase.BuiltInChips[chipTypeName];
                    data[chipName] = chipType;
                }
            }

            foreach(var chipToken in chipTokens)
            {
                var chipData = data[chipToken.Key];
                var constructedChip = _getConstructedChip(chipToken.Key, chipToken.Value, chipData);
                constructedChip.Name = chipToken.Key;
                chips[chipToken.Key] = constructedChip;
            }

            foreach (var chipToken in chipTokens)
            {
                var chipData = data[chipToken.Key];
                var constructedChip = chips[chipToken.Key];

                if (chipData.NumInputs > 0)
                {
                    var inputsList = chipToken.Value["inputs"];
                    for (int i = 0; i < chipData.NumInputs; i++)
                    {
                        var input = inputsList[i];
                        var inputType = input["inputType"].ToString();
                        if(inputType.Equals("Value"))
                        {
                            var typeName = chipData.GetInputType(i + 1);
                            var property = constructedChip.GetType().GetProperty("ChipInput" + (i + 1).ToString());

                            if (typeName.Equals("Template"))
                            {
                                var template = Templates.BlockTemplates[input["inputValue"].ToString()];
                                property.SetValue(constructedChip, template);
                            }
                            else
                            {
                                var type = ChipDatabase._allTypes[typeName];
                                var typeValue = _parseInputAsValue(type, input["inputValue"].ToString());
                                property.SetValue(constructedChip, typeValue);
                            }
                        }
                        else if(inputType.Equals("Reference"))
                        {
                            var inputChipName = input["inputValue"].ToString();
                            //var inputChip = 
                        }

                    }
                }

            }

            return null;
        }

        private static object _parseInputAsValue(Type t,string asString)
        {
            var parseMethod = t.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
            if(parseMethod!=null)
            {
                var parsed = parseMethod.Invoke(null, new object[] { asString });
                return parsed;
            }

            if (t.IsEnum)
            {
                var parsed = Enum.Parse(t, asString);
                return parsed;
            }

            throw new Exception();
        }


        private static IChip _getConstructedChip(string name,JToken token, ChipData data)
        {
            if(data.IsGeneric)
            {
                return ChipDatabase.GenerateChipFromData(data,token["typeArgument"].ToString());
            }

            return ChipDatabase.GenerateChipFromData(data);
        }
    }
}
