public abstract class AlgorithmSettingForm : Form
{
    public EvolutionaryAlgorithmSetting EvolutionaryAlgorithmSetting { get; protected set; }

    public AlgorithmSettingForm()
    {
        Text = "Algorithm Setting";
        Width = 400;
        Height = 300;
        StartPosition = FormStartPosition.CenterScreen;

        InitializeComponents();


        WindowDesign.SetDesign(this);
    }

    protected abstract void InitializeComponents();

    protected void AddLabel(string text, int left, int top, int width)
    {
        var label = new Label
        {
            Text = text,
            Left = left,
            Top = top + 3,
            Width = width
        };
        Controls.Add(label);
    }
}