using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class BlockInputSection
    {
        public List<string> SanityTest((List<string> blocksetNames, List<string> blockNames) names)
        {
            var output = new List<string>();

            if(CurrentlySelected==null)
            {
                output.Add("No input option selected");
            }
            else
            {
                if (CurrentlySelected.BaseType==null)
                {
                    output.Add("Input option base type is null");
                }
                else if(CurrentlySelected.BaseType.Length==0)
                {
                    output.Add("Input option base type is empty");
                }

                if (CurrentlySelected.OptionType == InputOptionType.Undefined)
                {
                    output.Add("InputOptionType is undefined");
                }
                else if(CurrentlySelected.OptionType == InputOptionType.Reference)
                {
                    var refrenceInput = (BlockInputOptionReference)CurrentlySelected;
                    if(refrenceInput.BlockReference==null)
                    {
                        output.Add("Input option block reference is null");
                    }
                    else if(!names.blockNames.Contains(refrenceInput.BlockReference.Name))
                    {
                        output.Add("Input option block reference refers to a block which does not exist: " + refrenceInput.BlockReference.Name);
                    }
                }
                else if (CurrentlySelected.OptionType == InputOptionType.Parseable)
                {
                    var parseableInput = (BlockInputOptionParseable)CurrentlySelected;
                    if (parseableInput.StringRepresentation == null)
                    {
                        output.Add("Parseable input is null");
                    }
                    else if(parseableInput.BaseType==null)
                    {
                        output.Add("Parseable input cannot be resolved to any base types: " + parseableInput.StringRepresentation);
                    }
                }


            }



            return output;
        }

    }
}
