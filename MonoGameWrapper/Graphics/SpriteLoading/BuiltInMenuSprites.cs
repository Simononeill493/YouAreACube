using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BuiltInMenuSprites
    {
        public static string AppearanceEditTab = "AppearanceEditOptionButton";
        public static string LargeMenuRectangle = "EmptyMenuRectangleFull";
        public static string LargeMenuRectangle_BlocksetEditWindow = "EditPaneWindow";

        public static string MediumMenuRectangle = "EmptyMenuRectangleFull";
        public static string BlankPixel = "BlankPixel";
        public static string BasicDropdown = "Dropdown";
        public static string DropdownArrow = "DropdownArrow";
        public static string UncheckedRadioButton = "RadioButtonUnchecked";
        public static string CheckedRadioButton = "RadioButtonChecked";
        public static string SpriteBox = "TemplateItemContainer";
        public static string SpriteBox_Highlighted = "TemplateItemContainerHighlight";

        public static string BasicTabButton = "TabButton";
        public static string SmallRectangularButton = "EmptyButtonRectangleSmall";
        public static string SearchBar = "SearchBar";
        public static string BasicTextBox = "TextBoxMenuRectangle";
        public static string PreviewBlock = "ChipSmall";
        public static string SearchPane = "SearchPane";
        public static string BlocksetEditPane = "ChipEditPane";

        public static string PlusButton = "PlusButton";
        public static string MinusButton_Partial = "MinusButton_Partial";
        public static string MinusButton = "MinusButton";

        public static string TemplateListMenuSection = "EmptyMenuRectangleSection";
        public static string TemplateListMenuSection_Extended = "EmptyMenuRectangleSection_Extended";

        public static string TitleBackground = "TitleBackground";
        public static string MainMenuOkButton = "OkButton";
        public static string MainMenuCancelButton = "CancelButton";

        public static string MainMenuNewGameButton = "NewGameMenu";
        public static string MainMenuNewGameButton_Highlighted = "NewGameMenu_Highlight";

        public static string MainMenuLoadGameButton = "LoadGameMenu";
        public static string MainMenuLoadGameButton_Highlighted = "LoadGameMenu_Highlight";

        public static string Blockset_TopHandle = "TopOfChipset";
        public static string Block = "ChipFull";
        public static string BlockGreyed = "ChipFullGreyed";

        public static string BlockMiddle = "ChipFullMiddle";
        public static string BlockBottom = "ChipFullEnd";

        public static string SwitchBlockSideArrow = "SwitchChipSideArrow";
        public static string IfBlockSwitchButton = "IfChipSwitchButton";

        public static void ConfigureMenuSprites(List<(string fullname,string friendlyName)> allSprites,string directory)
        {
            var spriteFieldProperties = typeof(BuiltInMenuSprites).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (var property in spriteFieldProperties.ToList())
            {
                var spriteNameFriendly = (string)property.GetValue(null);
                var spriteNameFull = directory + '/' + spriteNameFriendly;

                allSprites.Add((spriteNameFull, spriteNameFriendly));
            }
        }
    }
}
