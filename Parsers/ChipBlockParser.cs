using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipBlockParser
    {
        #region blockToJson
        public static string ParseBlockToJson(ChipBlock toParse)
        {
            var allChips = toParse.GetAllChipsAndSubChips();
            var chipToObject = new Dictionary<string, ChipJSONData>();
            var chipToData = new Dictionary<string, ChipData>();
            var chipToInputs = new Dictionary<string, List<Tuple<string,string>>>();

            foreach (var c in allChips)
            {
                var chipData = ChipDatabase.GetChipDataFromChip(c);
                chipToData[c.Name] = chipData;

                var chipJobject = new ChipJSONData();
                chipToObject[c.Name] = chipJobject;
                chipJobject.Name = c.Name;
                chipJobject.Type = chipData.Name;

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
                    for(int i=0;i<3;i++)
                    {
                        IEnumerable targets = (IEnumerable)chipType.GetField("Targets" + (i+1)).GetValue(chip);
                        _setChipTargets(chip, i, targets, chipToInputs);
                    }
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
                    var inputsJObject = new List<ChipJSONInputData>();
                    foreach(var input in inputs)
                    {
                        var inputJObject = new ChipJSONInputData();
                        inputJObject.InputType = input.Item1;
                        inputJObject.InputValue = input.Item2;
                        inputsJObject.Add(inputJObject);
                    }

                    chipJObject.Inputs = inputsJObject;
                }
            }

            var allBlocks = toParse.GetBlockAndSubBlocks();
            var blockTokens = new List<ChipBlockJSONData>();
            foreach(var block in allBlocks)
            {
                var chipsInBlock = new List<ChipJSONData>();
                foreach(var chip in block.Chips)
                {
                    chipsInBlock.Add(chipToObject[chip.Name]);
                }

                var token = new ChipBlockJSONData();
                token.Name = block.Name;
                token.Chips = chipsInBlock;

                blockTokens.Add(token);
            }

            var jobjectList = JToken.FromObject(blockTokens);
            return jobjectList.ToString();
        }

        private static void _setSpecialChipAttributes(IChip chip,ChipJSONData chipToken,ChipData data)
        {
            if(data.Name.Equals("If"))
            {
                var ifChip = (IfChip)chip;
                chipToken.Yes = ifChip.Yes.Name;
                chipToken.No = ifChip.No.Name;
            }
            else if(data.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chip;
                var keysAndEffects = new List<Tuple<string, string>>();
                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add(new Tuple<string, string>(keyBlock.Item1.ToString(), keyBlock.Item2.Name));
                }

                chipToken.KeyEffects = keysAndEffects;
            }
        }
        private static void _setGenericChipAttributes(IChip chip, ChipJSONData chipToken, ChipData data)
        {
            if(data.IsGeneric)
            {
                var type = chip.GetType();
                var typeArgument = type.GenericTypeArguments.First();
                chipToken.TypeArgument = typeArgument.Name;
            }
        }
        private static void _setChipTargets(IChip chip,int inputPinIndex,IEnumerable targets, Dictionary<string, List<Tuple<string, string>>> chipToInputs)
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
        #endregion

        #region jsonToBlock
        public static ChipBlock ParseJsonToBlock(string json)
        {
            var chipsetJson = JsonConvert.DeserializeObject<ChipsetJSONData>(json);
            var blocksDict = chipsetJson.GetBlocksDict();
            var chipsDict = chipsetJson.GetChipsDict();

            chipsetJson.SetChipData();
            chipsetJson.CreateChipBlocks();
            chipsetJson.CreateIChips();

            foreach (var blockToken in chipsetJson)
            {
                foreach (var chipToken in blockToken.Chips)
                {
                    blockToken.ChipBlock.AddChip(chipToken.IChip);
                }
            }

            foreach (var chipToken in chipsDict.Values)
            {
                var chipData = chipToken.ChipData;
                var constructedChip = chipToken.IChip;

                if (chipData.NumInputs > 0)
                {
                    _setChipInputs(chipToken, chipsDict);
                }

                if(chipData.ChipDataType == ChipType.Control)
                {
                    _setControlChipTargets(chipToken, blocksDict);
                }
            }

            var initBlock = blocksDict.Values.First(v => v.Name.Equals("Initial")).ChipBlock;
            return initBlock;
        }

        private static void _setChipInputs(ChipJSONData chipToken, Dictionary<string, ChipJSONData> chipsDict)
        {
            var chipData = chipToken.ChipData;
            var constructedChip = chipToken.IChip;

            var inputsList = chipToken.Inputs;
            for (int i = 0; i < chipData.NumInputs; i++)
            {
                var input = inputsList[i];
                if (input.InputType.Equals("Value"))
                {
                    var typeName = chipData.GetInputType(i + 1);
                    var property = constructedChip.GetType().GetProperty("ChipInput" + (i + 1).ToString());

                    if (typeName.Equals("Template"))
                    {
                        var template = Templates.BlockTemplates[input.InputValue];
                        property.SetValue(constructedChip, template);
                    }
                    else
                    {
                        if (typeName.Equals("int")) { typeName = "Int32"; }
                        var type = ChipDatabase._allTypes[typeName];
                        var typeValue = _parseInputAsValue(type, input.InputValue);
                        property.SetValue(constructedChip, typeValue);
                    }
                }
                else if (input.InputType.Equals("Reference"))
                {
                    var inputChip = chipsDict[input.InputValue].IChip;

                    var targetField = inputChip.GetType().GetField("Targets" + (i + 1).ToString());
                    IList targetsList = (IList)targetField.GetValue(inputChip);
                    targetsList.Add(constructedChip);
                }
            }
        }

        private static object _parseInputAsValue(Type t,string asString)
        {
            if (t.IsEnum)
            {
                var parsed = Enum.Parse(t, asString);
                return parsed;
            }

            var parseMethod = t.GetMethod("Parse", new Type[] { typeof(string) }, new ParameterModifier[]{ new ParameterModifier(1)});
            if (parseMethod!=null)
            {
                var parsed = parseMethod.Invoke(null, new object[] { asString });
                return parsed;
            }

            throw new Exception();
        }
        
        private static void _setControlChipTargets(ChipJSONData chipToken, Dictionary<string, ChipBlockJSONData> blocksDict)
        {
            var chipData = chipToken.ChipData;
            var constructedChip = chipToken.IChip;

            if (chipData.Name.Equals("If"))
            {
                var ifChip = (IfChip)constructedChip;
                var yesBLock = blocksDict[chipToken.Yes].ChipBlock;
                ifChip.Yes = yesBLock;

                var noBLock = blocksDict[chipToken.No].ChipBlock;
                ifChip.No = noBLock;
            }
            if (chipData.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)constructedChip;
                foreach (var effect in chipToken.KeyEffects)
                {
                    var effectKey = (Keys)Enum.Parse(typeof(Keys), effect.Item1);
                    var effectBlockName = effect.Item2;
                    var effectBlock = blocksDict[effectBlockName].ChipBlock;

                    keySwitchChip.AddKeyEffect(effectKey, effectBlock);
                }
            }
        }
        #endregion
    }
}
