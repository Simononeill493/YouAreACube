namespace IAmACube
{
    static class TestScenarioGenerator
    {
        public static TextDialogueScenario GenerateTestScenario()
        {
            var scenario = new TextDialogueScenario();
            var sentence1 = new TextDialogueSentence("initial","Ready to play the demo?");
            scenario.AddInitialPage(sentence1);

            var sentence2 = new TextDialogueSentence("afterFirstClick","You moved to sentence 2!");
            scenario.AddPage(sentence2);

            sentence1.AddTrigger(new MouseClickTrigger("afterFirstClick"));
            sentence2.AddTrigger(new MouseClickTrigger("initial"));

            return scenario;
        }
    }

}
