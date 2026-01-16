public class ElitistGeneticSettingForm : AlgorithmSettingForm
{

    private NumericUpDown numberOfElitesNumeric;
    private NumericUpDown numberOfNewNumeric;
    private NumericUpDown averageElementsFromNonEliteNumeric;
    private NumericUpDown averageElementsMutatedNumeric;
    private NumericUpDown hardStopNumeric;

    private Button okButton;
    private Button cancelButton;

    public ElitistGeneticSettingForm():base()
    {   
    }

    protected override void InitializeComponents()
    {
        int labelWidth = 180;
        int top = 10;
        int spacing = 30;

        // number of elites
        AddLabel("Elites Number:", 10, top, labelWidth);
        numberOfElitesNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            Minimum = 1,
            Value = 2,
        };
        Controls.Add(numberOfElitesNumeric);
        top += spacing;

        // number of new
        AddLabel("Number of New:", 10, top, labelWidth);
        numberOfNewNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            Minimum = 0,
            Value = 0,
        };
        Controls.Add(numberOfNewNumeric);
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

        AddLabel("El. from Non Elite:", 10, top, labelWidth);
        averageElementsFromNonEliteNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1000, 
            Value = 1.5M
        };
        Controls.Add(averageElementsFromNonEliteNumeric);
        top += spacing;

        AddLabel("El. Mutated:", 10, top, labelWidth);
        averageElementsMutatedNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1000,
            Value = 1.5M
        };
        Controls.Add(averageElementsMutatedNumeric);
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

        int hardStop = (int)hardStopNumeric.Value;
        int numberOfElites = (int)numberOfElitesNumeric.Value;
        int numberOfNew = (int)numberOfNewNumeric.Value;
        double averageElementsMutated = (double)averageElementsMutatedNumeric.Value;
        double averageElementsFromNonElite = (double)averageElementsFromNonEliteNumeric.Value;


        EvolutionaryAlgorithmSetting = new ElitistGeneticSetting(hardStop, numberOfElites, averageElementsFromNonElite, averageElementsMutated, numberOfNew);


        DialogResult = DialogResult.OK;
        Close();
    }








}

