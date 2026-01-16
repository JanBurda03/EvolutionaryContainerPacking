public class HillDescendingSettingForm : AlgorithmSettingForm
{


    private NumericUpDown acceptanceDecayNumeric;
    private NumericUpDown averageElementsMutatedNumeric;
    private NumericUpDown hardStopNumeric;
    private NumericUpDown startValueNumeric;
    private NumericUpDown endValueNumeric;

    private Button okButton;
    private Button cancelButton;

    public HillDescendingSettingForm() : base()
    {
    }

    protected override void InitializeComponents()
    {
        int labelWidth = 180;
        int top = 10;
        int spacing = 30;

        AddLabel("Accept. Decay:", 10, top, labelWidth);
        acceptanceDecayNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.90M
        };
        Controls.Add(acceptanceDecayNumeric);
        top += spacing;

        // hard stop
        AddLabel("Hard Stop:", 10, top, labelWidth);
        hardStopNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            Minimum = 0,
            Value = 0,
        };
        Controls.Add(hardStopNumeric);
        top += spacing;


        AddLabel("El. Changed:", 10, top, labelWidth);
        averageElementsMutatedNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1000,
            Value = 1.5M
        };
        Controls.Add(averageElementsMutatedNumeric);
        top += spacing;

        AddLabel("Start Value:", 10, top, labelWidth);
        startValueNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 1M
        };
        Controls.Add(startValueNumeric);
        top += spacing;

        AddLabel("End Value:", 10, top, labelWidth);
        endValueNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.1M
        };
        Controls.Add(endValueNumeric);
        top += spacing;

        // OK Button
        okButton = new Button
        {
            Text = "OK",
            Left = 50,
            Top = top,
            Width = 100
        };
        okButton.Click += OkButton_Click;
        Controls.Add(okButton);

        // Cancel Button
        cancelButton = new Button
        {
            Text = "Cancel",
            Left = 160,
            Top = top,
            Width = 100
        };
        cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(cancelButton);
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        double acceptanceDecay = (double)acceptanceDecayNumeric.Value;
        int hardStop = (int)hardStopNumeric.Value;
        double averageElementsMutated = (double)averageElementsMutatedNumeric.Value;

        double startValue = (double)startValueNumeric.Value;
        double endValue = (double)endValueNumeric.Value;



        EvolutionaryAlgorithmSetting = new HillDescendingSetting(hardStop, averageElementsMutated, acceptanceDecay, startValue, endValue);


        DialogResult = DialogResult.OK;
        Close();
    }








}
