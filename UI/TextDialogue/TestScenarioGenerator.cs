namespace IAmACube
{
    static class TestScenarioGenerator
    {
        public static TextDialogueScenario GenerateTestScenario()
        {
            var scenario = new TextDialogueScenario();
            var sentence1 = new TextDialogueSentence("initial", "Ready to play the demo?");
            var sentence2 = new TextDialogueSentence("yesOption", "You said yes!");
            var sentence3 = new TextDialogueSentence("noOption", "You said no!");

            sentence1.AddYesNo("yesOption", "noOption");
            sentence2.AddTrigger(new MouseClickTrigger("initial"));
            sentence3.AddTrigger(new MouseClickTrigger("initial"));

            scenario.AddInitialPage(sentence1);
            scenario.AddPage(sentence2);
            scenario.AddPage(sentence3);

            return scenario;
        }
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
