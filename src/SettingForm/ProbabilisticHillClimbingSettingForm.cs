namespace EvolutionaryContainerPacking.Forms;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;

/// <summary>
/// WinForms configuration dialog for the Probabilistic Hill Climbing algorithm.
/// 
/// Allows configuration of:
/// - Population size and number of generations
/// - Average mutation strength
/// - Optional early stopping via target fitness
/// - Optional probabilistic acceptance of worse solutions
/// </summary>
public class ProbabilisticHillClimbingSettingForm : AlgorithmSettingForm
{
    // === Early stopping ===
    private CheckBox useTargetFitnessCheckBox;
    private NumericUpDown targetFitnessNumeric;

    // === Population parameters ===
    private NumericUpDown numberOfIndividualsNumeric;
    private NumericUpDown numberOfGenerationsNumeric;

    // === Mutation parameter ===
    private NumericUpDown averageElementsChangedNumeric;

    // === Probabilistic acceptance parameters ===
    private CheckBox useProbabilisticCheckBox;
    private NumericUpDown startAcceptanceProbabilityNumeric;
    private NumericUpDown endAcceptanceProbabilityNumeric;
    private NumericUpDown acceptanceDecayFactorNumeric;

    // === Control buttons ===
    private Button okButton;
    private Button cancelButton;

    public ProbabilisticHillClimbingSettingForm() : base()
    {
    }

    /// <summary>
    /// Initializes UI controls and layout.
    /// </summary>
    protected override void InitializeComponents()
    {
        int labelWidth = 220;
        int leftInput = 250;
        int top = 10;
        int spacing = 30;

        // ============================================================
        // EARLY STOPPING SECTION
        // ============================================================

        // Checkbox enabling/disabling target fitness stopping
        useTargetFitnessCheckBox = new CheckBox
        {
            Text = "Enable Target Fitness (Early Stop)",
            Left = 10,
            Top = top,
            Width = 300
        };

        // Enable/disable numeric input dynamically
        useTargetFitnessCheckBox.CheckedChanged += (s, e) =>
        {
            targetFitnessNumeric.Enabled = useTargetFitnessCheckBox.Checked;
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
            Increment = 0.1M,
            Minimum = 0,
            Maximum = decimal.MaxValue,
            Enabled = false // disabled until checkbox is checked
        };

        Controls.Add(targetFitnessNumeric);
        top += spacing;

        // ============================================================
        // POPULATION SETTINGS
        // ============================================================

        AddLabel("Number of Individuals:", 10, top, labelWidth);

        numberOfIndividualsNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            Minimum = 1,
            Maximum = 100000,
            Value = 50
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
            Value = 100
        };

        Controls.Add(numberOfGenerationsNumeric);
        top += spacing;

        // ============================================================
        // MUTATION STRENGTH
        // ============================================================

        AddLabel("Average Elements Changed:", 10, top, labelWidth);

        averageElementsChangedNumeric = new NumericUpDown
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

        Controls.Add(averageElementsChangedNumeric);
        top += spacing;

        // ============================================================
        // PROBABILISTIC ACCEPTANCE SECTION
        // ============================================================

        // Enables simulated annealing-like behavior
        useProbabilisticCheckBox = new CheckBox
        {
            Text = "Use Probabilistic Acceptance",
            Left = 10,
            Top = top,
            Width = 300
        };

        // Dynamically enable/disable related numeric inputs
        useProbabilisticCheckBox.CheckedChanged += (s, e) =>
        {
            bool enabled = useProbabilisticCheckBox.Checked;
            startAcceptanceProbabilityNumeric.Enabled = enabled;
            endAcceptanceProbabilityNumeric.Enabled = enabled;
            acceptanceDecayFactorNumeric.Enabled = enabled;
        };

        Controls.Add(useProbabilisticCheckBox);
        top += spacing;

        AddLabel("Start Acceptance Probability:", 10, top, labelWidth);

        startAcceptanceProbabilityNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 1M,
            Enabled = false
        };

        Controls.Add(startAcceptanceProbabilityNumeric);
        top += spacing;

        AddLabel("End Acceptance Probability:", 10, top, labelWidth);

        endAcceptanceProbabilityNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 2,
            Increment = 0.01M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.1M,
            Enabled = false
        };

        Controls.Add(endAcceptanceProbabilityNumeric);
        top += spacing;

        AddLabel("Acceptance Decay Factor:", 10, top, labelWidth);

        acceptanceDecayFactorNumeric = new NumericUpDown
        {
            Left = leftInput,
            Top = top,
            Width = 120,
            DecimalPlaces = 4,
            Increment = 0.001M,
            Minimum = 0,
            Maximum = 1,
            Value = 0.99M,
            Enabled = false
        };

        Controls.Add(acceptanceDecayFactorNumeric);
        top += spacing;

        // ============================================================
        // BUTTONS
        // ============================================================

        okButton = new Button
        {
            Text = "OK",
            Left = 100,
            Top = top,
            Width = 100
        };

        okButton.Click += OkButton_Click;
        Controls.Add(okButton);

        cancelButton = new Button
        {
            Text = "Cancel",
            Left = 220,
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

    /// <summary>
    /// Validates user input and constructs ProbabilisticHillClimbingSetting.
    /// </summary>
    private void OkButton_Click(object sender, EventArgs e)
    {
        // --- Early stopping ---
        double? targetFitness = null;
        if (useTargetFitnessCheckBox.Checked)
            targetFitness = (double)targetFitnessNumeric.Value;

        // --- Population parameters ---
        int numberOfIndividuals = (int)numberOfIndividualsNumeric.Value;
        int numberOfGenerations = (int)numberOfGenerationsNumeric.Value;

        // --- Mutation parameter ---
        double averageElementsChanged = (double)averageElementsChangedNumeric.Value;

        // --- Probabilistic parameters ---
        double start = (double)startAcceptanceProbabilityNumeric.Value;
        double end = (double)endAcceptanceProbabilityNumeric.Value;
        double decay = (double)acceptanceDecayFactorNumeric.Value;

        // ============================================================
        // VALIDATION
        // ============================================================

        if (useProbabilisticCheckBox.Checked)
        {
            // Ensure probabilities are within [0,1]
            if (start < 0 || start > 1 || end < 0 || end > 1)
            {
                MessageBox.Show("Start and End acceptance probabilities must be in range [0,1].");
                return;
            }

            // Ensure monotonic cooling (end <= start)
            if (end > start)
            {
                MessageBox.Show("End Acceptance Probability must be less than or equal to Start Acceptance Probability.");
                return;
            }
        }
        else
        {
            // If probabilistic mode is disabled,
            // behave like pure greedy hill climbing.
            start = 0;
            end = 0;
            decay = 0;
        }

        // Create immutable configuration record
        EvolutionaryAlgorithmSetting = new ProbabilisticHillClimbingSetting(
            targetFitness,
            numberOfIndividuals,
            numberOfGenerations,
            averageElementsChanged,
            start,
            end,
            decay
        );

        DialogResult = DialogResult.OK;
        Close();
    }
}
