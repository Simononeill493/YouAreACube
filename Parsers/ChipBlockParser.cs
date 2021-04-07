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
            var chipToObject = new Dictionary<string, ChipJSONData>();
            var chipToInputs = new Dictionary<string, List<ChipJSONInputData>>();

            foreach (var iChip in toParse.GetAllChipsAndSubChips())
            {
                var chipData = ChipDatabase.GetChipDataFromChip(iChip);

                var chipJobject = new ChipJSONData();
                chipJobject.Name = iChip.Name;
                chipJobject.Type = chipData.Name;

                chipJobject.ChipData = chipData;
                chipJobject.IChip = iChip;
                chipJobject.SetInputs();
                chipToInputs[chipJobject.Name] = chipJobject.Inputs;

                chipToObject[iChip.Name] = chipJobject;

                _setGenericChipAttributes(iChip, chipJobject, chipData);
                _setControlChipAttributes(iChip, chipJobject, chipData);
            }

            _setChipReferenceInputs(chipToObject.Values, chipToInputs);
            _setChipValueInputs(chipToObject.Values);

            var blocksJsonData = new ChipsetJSONData();
            foreach(var block in toParse.GetBlockAndSubBlocks())
            {
                var token = new ChipBlockJSONData();
                token.Name = block.Name;
                token.Chips = block.Chips.Select(chip => chipToObject[chip.Name]).ToList();

                blocksJsonData.Add(token);
            }

            blocksJsonData.Sort((c1,c2) => string.Compare(c1.Name,c2.Name));
            
            var jobjectList = JToken.FromObject(blocksJsonData, new JsonSerializer{NullValueHandling = NullValueHandling.Ignore});
            return jobjectList.ToString();
        }

        private static void _setChipReferenceInputs(IEnumerable<ChipJSONData> chipsJsonData, Dictionary<string, List<ChipJSONInputData>> chipInputs)
        {
            foreach (var chipJobject in chipsJsonData)
            {
                var chipData = chipJobject.ChipData;
                var chipType = chipJobject.IChip.GetType();

                if (chipData.HasOutput)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var targets = (IEnumerable<IChip>)chipType.GetField("Targets" + (i + 1)).GetValue(chipJobject.IChip);
                        _setChipReferenceTargets(chipJobject.IChip, i, targets, chipInputs);
                    }
                }
            }
        }
        private static void _setChipReferenceTargets(IChip chip, int inputPinIndex, IEnumerable<IChip> targets, Dictionary<string, List<ChipJSONInputData>> chipToInputs)
        {
            foreach (var target in targets)
            {
                var targetInputs = chipToInputs[target.Name];
                targetInputs[inputPinIndex] = new ChipJSONInputData("Reference", chip.Name);
            }
        }
        
        private static void _setChipValueInputs(IEnumerable<ChipJSONData> chipsJsonData)
        {
            foreach (var chipJobject in chipsJsonData)
            {
                var inputs = chipJobject.Inputs;

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (inputs[i].InputType.Equals(""))
                    {
                        inputs[i].InputType = "Value";
                        inputs[i].InputValue = _getInputPinValue(chipJobject.IChip, i);
                    }
                }
            }
        }
        private static string _getInputPinValue(IChip chip, int pinIndex)
        {
            var propertyName = "ChipInput" + (pinIndex + 1).ToString();
            var property = chip.GetType().GetProperty(propertyName);
            var value = property.GetValue(chip).ToString();

            return value;
        }

        private static void _setControlChipAttributes(IChip chip,ChipJSONData chipJObject,ChipData data)
        {
            if(data.Name.Equals("If"))
            {
                var ifChip = (IfChip)chip;
                chipJObject.Yes = ifChip.Yes.Name;
                chipJObject.No = ifChip.No.Name;
            }
            else if(data.Name.Equals("KeySwitch"))
            {
                var keySwitchChip = (KeySwitchChip)chip;
                var keysAndEffects = new List<Tuple<string, string>>();
                foreach (var keyBlock in keySwitchChip.KeyEffects)
                {
                    keysAndEffects.Add(new Tuple<string, string>(keyBlock.Item1.ToString(), keyBlock.Item2.Name));
                }

                chipJObject.KeyEffects = keysAndEffects;
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

            var initBlock = blocksDict.Values.First(v => v.Name.Equals("_Initial")).ChipBlock;
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
                        var type = TypeUtils.GetTypeByName(typeName);
                        var typeValue = TypeUtils.ParseType(type, input.InputValue);
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
