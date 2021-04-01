namespace IAmACube
{
    public interface IChip
    {
        string Name { get; set; }
        void Run(Block actor,UserInput userInput,ActionsList actions);
    }
}