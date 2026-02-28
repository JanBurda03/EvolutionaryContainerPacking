namespace EvolutionaryContainerPacking.Forms;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
/// <summary>
/// WinForms configuration dialog for the Elitist Genetic Algorithm
/// including optional memetic (hill climbing) extension.
/// </summary>
public class ElitistGeneticSettingForm : AlgorithmSettingForm
{
    // ============================================================
    // Early stopping
    // ============================================================
    private CheckBox useTargetFitnessCheckBox;
    private NumericUpDown targetFitnessNumeric;

    // ============================================================
    // Population
    // ============================================================
    private NumericUpDown numberOfIndividualsNumeric;
    private NumericUpDown numberOfGenerationsNumeric;

    // ============================================================
    // Genetic parameters
    // ============================================================
    private NumericUpDown percentageOfEliteNumeric;
    private NumericUpDown averageElementsFromNonEliteNumeric;
    private NumericUpDown averageElementsMutatedNumeric;
    private NumericUpDown tournamentSizeNumeric;
    private NumericUpDown percentageOfRandomNumeric;

    // ============================================================
    // Memetic extension
    // ============================================================
    private NumericUpDown hillClimbingIterationsNumeric;
    private NumericUpDown hillClimbingAverageElementsMutatedNumeric;

    private Button okButton;
    private Button cancelButton;

    public ElitistGeneticSettingForm() : base()
    {
    }

    protected override void InitializeComponents()
    {
        int labelWidth = 240;
        int leftInput = 260;
        int top = 10;
        int spacing = 30;

        // ============================================================
        // EARLY STOPPING
        // ============================================================

        useTargetFitnessCheckBox = new CheckBox
        {
            Text = "Enable Target Fitness (Early Stop)",
            Left = 10,
            Top = top,
            Width = 320
        };

        Controls.Add(useTargetFitnessCheckBox);
        top += spacing;

        AddLabel("Target Fitness:", 10, top, labelWidth);

        targetFitnessNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Minimum = 0,
            Maximum = decimal.MaxValue,
            Enabled = false
        };

        Controls.Add(targetFitnessNumeric);

        useTargetFitnessCheckBox.CheckedChanged += (s, e) =>
        {
            targetFitnessNumeric.Enabled = useTargetFitnessCheckBox.Checked;
        };

        top += spacing;

        // ============================================================
        // POPULATION
        // ============================================================

        AddLabel("Number of Individuals:", 10, top, labelWidth);
        numberOfIndividualsNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            Minimum = 1,
            Maximum = 100000,
            Value = 100
        };
        Controls.Add(numberOfIndividualsNumeric);
        top += spacing;

        AddLabel("Number of Generations:", 10, top, labelWidth);
        numberOfGenerationsNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            Minimum = 1,
            Maximum = 100000,
            Value = 200
        };
        Controls.Add(numberOfGenerationsNumeric);
        top += spacing;

        // ============================================================
        // GENETIC PARAMETERS
        // ============================================================

        AddLabel("Percentage of Elite Individuals:", 10, top, labelWidth);
        percentageOfEliteNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.1M
        };
        Controls.Add(percentageOfEliteNumeric);
        top += spacing;

        AddLabel("Average Elements From Non-Elite:", 10, top, labelWidth);
        averageElementsFromNonEliteNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1000,
            Value = 1.5M
        };
        Controls.Add(averageElementsFromNonEliteNumeric);
        top += spacing;

        AddLabel("Average Elements Mutated:", 10, top, labelWidth);
        averageElementsMutatedNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1000,
            Value = 1.5M
        };
        Controls.Add(averageElementsMutatedNumeric);
        top += spacing;

        AddLabel("Tournament Size:", 10, top, labelWidth);
        tournamentSizeNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            Minimum = 1,
            Maximum = 100,
            Value = 3
        };
        Controls.Add(tournamentSizeNumeric);
        top += spacing;

        AddLabel("Percentage of Random Individuals:", 10, top, labelWidth);
        percentageOfRandomNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.05M
        };
        Controls.Add(percentageOfRandomNumeric);
        top += spacing;

        // ============================================================
        // MEMETIC EXTENSION
        // ============================================================

        AddLabel("Hill Climbing Iterations:", 10, top, labelWidth);
        hillClimbingIterationsNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            Minimum = 0,
            Maximum = 1000,
            Value = 0
        };
        Controls.Add(hillClimbingIterationsNumeric);
        top += spacing;

        AddLabel("Hill Climbing Avg. Elements Mutated:", 10, top, labelWidth);
        hillClimbingAverageElementsMutatedNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.1M,
            Minimum = 0,
            Maximum = 1000,
            Enabled = false,
            Value = 1.0M
        };
        Controls.Add(hillClimbingAverageElementsMutatedNumeric);

        hillClimbingIterationsNumeric.ValueChanged += (s, e) =>
        {
            hillClimbingAverageElementsMutatedNumeric.Enabled =
                hillClimbingIterationsNumeric.Value > 0;
        };

        top += spacing;

        // ============================================================
        // BUTTONS
        // ============================================================

        okButton = new Button
        {
            Text = "OK",
            Left = 120,
            Top = top,
            Width = 100
        };
        okButton.Click += OkButton_Click;
        Controls.Add(okButton);

        cancelButton = new Button
        {
            Text = "Cancel",
            Left = 240,
            Top = top,
            Width = 100
        };
        cancelButton.Click += (s, e) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };
        Controls.Add(cancelButton);
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        double? targetFitness = useTargetFitnessCheckBox.Checked
            ? (double)targetFitnessNumeric.Value
            : null;

        double elite = (double)percentageOfEliteNumeric.Value;
        double random = (double)percentageOfRandomNumeric.Value;

        // Validate percentage ranges
        if (elite < 0 || elite > 1 || random < 0 || random > 1)
        {
            MessageBox.Show("Elite and Random percentages must be in range [0,1].");
            return;
        }

        EvolutionaryAlgorithmSetting = new ElitistGeneticAlgorithmSetting(
            targetFitness,
            (int)numberOfIndividualsNumeric.Value,
            (int)numberOfGenerationsNumeric.Value,
            elite,
            (double)averageElementsFromNonEliteNumeric.Value,
            (double)averageElementsMutatedNumeric.Value,
            (int)tournamentSizeNumeric.Value,
            random,
            (int)hillClimbingIterationsNumeric.Value,
            (double)hillClimbingAverageElementsMutatedNumeric.Value
        );

        DialogResult = DialogResult.OK;
        Close();
    }
}

