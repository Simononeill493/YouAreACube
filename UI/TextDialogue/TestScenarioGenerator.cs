using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IAmACube
{
    static class TestScenarioGenerator
    {
        public static TextDialogueScenario GeneratePreDemoScenario(Action startDemo)
        {
            var scenario = new TextDialogueScenario();
            var sentence1 = new TextDialoguePageSentence("initial", "Want to play the demo?");
            var sentence2 = new TextDialoguePageSentence("yesOption", "Ok enjoy!") { Tags = TagGen.Make("Happy") };
            var sentence3 = new TextDialoguePageSentence("noOption", "Ok?") { Tags = TagGen.Make("Confused") };
            var sentence4 = new TextDialoguePageSentence("um", "um,");
            var sentence5 = new TextDialoguePageSentence("bye!", "bye!") { Tags = TagGen.Make("Happy") };
            var closePc = new TextDialoguePageCustomAction("close", ()=> { MonoGameWindow.CloseGame(); }) { Tags = TagGen.Make("Happy") };
            var start = new TextDialoguePageCustomAction("startDemo", startDemo) { Tags = TagGen.Make("Happy") };

            sentence1.AddYesNo("yesOption", "noOption");
            sentence2.AddTrigger(new MouseClickTrigger("startDemo"));
            sentence3.AddTrigger(new MouseClickTrigger("um"));
            sentence4.AddTrigger(new MouseClickTrigger("bye!"));
            sentence5.AddTrigger(new MouseClickTrigger("close"));

            scenario.AddInitialPage(sentence1);
            scenario.AddPage(sentence2);
            scenario.AddPage(sentence3);
            scenario.AddPage(sentence4);
            scenario.AddPage(sentence5);
            scenario.AddPage(closePc);
            scenario.AddPage(start);

            return scenario;
        }
    }

    static class TagGen
    {
        public static List<string> Make(params string[]tags) => tags.ToList();
    }

    static class DialoguePageUtils
    {
        public static void AddYesNo(this TextDialoguePage page,string yesId,string noId)
        {
            page.InputConfig = new TextDialogueInputYesNo();
            page.AddTrigger(new InputOverlayTrigger("Yes", yesId));
            page.AddTrigger(new InputOverlayTrigger("No", noId));
        }
    }

}
