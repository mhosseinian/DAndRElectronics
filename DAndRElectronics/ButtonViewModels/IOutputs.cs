namespace DAndRElectronics.ButtonViewModels
{
    public interface IOutputs
    {
        bool[] Outputs { get; }
        int[] OutputPercents { get; }
        int[] OutputKeys { get; }
    }
}