namespace EvolutionaryContainerPacking.Forms;

using EvolutionaryContainerPacking.Evolution;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;
using EvolutionaryContainerPacking.Packing;
public class SettingsForm : Form
{
    // ================================
    // IO
    // ================================
    private TextBox sourceJsonTextBox;
    private TextBox outputJsonTextBox;
    private TextBox evolutionStatisticsCsvTextBox;
    private CheckBox exportStatisticsCheckBox;

    // ================================
    // Packing
    // ================================
    private CheckedListBox placementHeuristicsCheckedListBox;
    private CheckBox allowRotationsCheckBox;

    private CheckBox usePackingOrderCheckBox;
    private ComboBox packingOrderComboBox;

    // ================================
    // Algorithm
    // ================================
    private ComboBox algorithmComboBox;

    private Button okButton;
    private Button cancelButton;

    // ============================================================
    // Publicly exposed configuration parts
    // ============================================================

    public string SourceFile => sourceJsonTextBox.Text;
    public string OutputFile => outputJsonTextBox.Text;
    public string? EvolutionStatisticsFile =>
        exportStatisticsCheckBox.Checked ? evolutionStatisticsCsvTextBox.Text : null;

    public string AlgorithmName => algorithmComboBox.SelectedItem!.ToString()!;

    public PackingSetting PackingSetting { get; private set; }

    public SettingsForm()
    {
        Text = "Program Settings";
        Width = 600;
        Height = 520;
        StartPosition = FormStartPosition.CenterScreen;

        InitializeComponents();
        WindowDesign.SetDesign(this);
    }

    private void InitializeComponents()
    {
        int labelWidth = 220;     // widened labels
        int leftInput = 240;      // pushed inputs to the right
        int controlWidth = 320;   // adjusted width to fit nicely
        int top = 10;
        int spacing = 30;

        // ============================================================
        // SOURCE JSON
        // ============================================================

        AddLabel("Source JSON:", 10, top, labelWidth);

        sourceJsonTextBox = new TextBox
        {
            Left = leftInput,
            Top = top,
            Width = controlWidth - 90,
            ReadOnly = true
        };

        var browseSourceButton = new Button
        {
            Text = "Browse...",
            Left = leftInput + controlWidth - 85,
            Top = top - 1,
            Width = 80
        };

        browseSourceButton.Click += (s, e) =>
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
                sourceJsonTextBox.Text = ofd.FileName;
        };

        Controls.Add(sourceJsonTextBox);
        Controls.Add(browseSourceButton);
        top += spacing;

        // ============================================================
        // OUTPUT JSON
        // ============================================================

        AddLabel("Output JSON:", 10, top, labelWidth);

        outputJsonTextBox = CreateSaveFileTextBox(
            top,
            leftInput,
            controlWidth,
            "output.json",
            "JSON files (*.json)|*.json|All files (*.*)|*.*");

        top += spacing;

        // ============================================================
        // STATISTICS EXPORT
        // ============================================================

        exportStatisticsCheckBox = new CheckBox
        {
            Text = "Export Evolution Statistics (CSV)",
            Left = 10,
            Top = top,
            Width = 300
        };

        Controls.Add(exportStatisticsCheckBox);
        top += spacing;

        AddLabel("Statistics CSV:", 10, top, labelWidth);

        evolutionStatisticsCsvTextBox = CreateSaveFileTextBox(
            top,
            leftInput,
            controlWidth,
            "evolutionStatistics.csv",
            "CSV files (*.csv)|*.csv|All files (*.*)|*.*");

        evolutionStatisticsCsvTextBox.Enabled = false;

        exportStatisticsCheckBox.CheckedChanged += (s, e) =>
        {
            evolutionStatisticsCsvTextBox.Enabled = exportStatisticsCheckBox.Checked;
        };

        top += spacing;

        // ============================================================
        // PLACEMENT HEURISTICS
        // ============================================================

        AddLabel("Placement Heuristics:", 10, top, labelWidth);

        placementHeuristicsCheckedListBox = new CheckedListBox
        {
            Left = leftInput,
            Top = top,
            Width = controlWidth,
            Height = 80
        };

        placementHeuristicsCheckedListBox.Items
            .AddRange(PlacementHeuristics.PlacementHeuristicsList.ToArray());

        placementHeuristicsCheckedListBox.SetItemChecked(0, true);

        Controls.Add(placementHeuristicsCheckedListBox);
        top += 90;

        allowRotationsCheckBox = new CheckBox
        {
            Left = leftInput,
            Top = top,
            Text = "Allow Rotations",
            Checked = true,
            AutoSize = true
        };

        Controls.Add(allowRotationsCheckBox);
        top += spacing;

        // ============================================================
        // PACKING ORDER
        // ============================================================

        usePackingOrderCheckBox = new CheckBox
        {
            Text = "Use Packing Order Heuristic",
            Left = 10,
            Top = top,
            Width = 250
        };

        Controls.Add(usePackingOrderCheckBox);
        top += spacing;

        AddLabel("Packing Order Heuristic:", 10, top, labelWidth);

        packingOrderComboBox = new ComboBox
        {
            Left = leftInput,
            Top = top,
            Width = controlWidth,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Enabled = false
        };

        packingOrderComboBox.Items
            .AddRange(OrderHeuristics.OrderHeuristicsList.ToArray());

        if (packingOrderComboBox.Items.Count > 0)
            packingOrderComboBox.SelectedIndex = 0;

        Controls.Add(packingOrderComboBox);

        usePackingOrderCheckBox.CheckedChanged += (s, e) =>
        {
            packingOrderComboBox.Enabled = usePackingOrderCheckBox.Checked;
        };

        top += spacing;

        // ============================================================
        // ALGORITHM
        // ============================================================

        AddLabel("Algorithm:", 10, top, labelWidth);

        algorithmComboBox = new ComboBox
        {
            Left = leftInput,
            Top = top,
            Width = controlWidth,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        algorithmComboBox.Items
            .AddRange(EvolutionaryAlgorithms.EvolutionaryAlgorithmsArray);

        algorithmComboBox.SelectedIndex = 0;

        Controls.Add(algorithmComboBox);
        top += spacing;

        // ============================================================
        // BUTTONS
        // ============================================================

        okButton = new Button
        {
            Text = "OK",
            Left = leftInput,
            Top = top,
            Width = 100
        };

        okButton.Click += OkButton_Click;
        Controls.Add(okButton);

        cancelButton = new Button
        {
            Text = "Cancel",
            Left = leftInput + 110,
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

    private TextBox CreateSaveFileTextBox(
        int top,
        int leftInput,
        int controlWidth,
        string defaultName,
        string filter)
    {
        string defaultPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            defaultName);

        var tb = new TextBox
        {
            Left = leftInput,
            Top = top,
            Width = controlWidth - 90,
            ReadOnly = true,
            Text = defaultPath
        };

        var browseButton = new Button
        {
            Text = "Browse",
            Left = leftInput + controlWidth - 85,
            Top = top - 1,
            Width = 80
        };

        browseButton.Click += (s, e) =>
        {
            using var sfd = new SaveFileDialog
            {
                Filter = filter
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                tb.Text = sfd.FileName;
        };

        Controls.Add(tb);
        Controls.Add(browseButton);

        return tb;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        if (HasErrors())
            return;

        string[] selectedPlacementHeuristics =
            placementHeuristicsCheckedListBox.CheckedItems.Cast<string>().ToArray();

        string? packingOrder =
            usePackingOrderCheckBox.Checked
                ? packingOrderComboBox.SelectedItem?.ToString()
                : null;

        PackingSetting = new PackingSetting(
            selectedPlacementHeuristics,
            allowRotationsCheckBox.Checked,
            packingOrder
        );

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool HasErrors()
    {
        if (string.IsNullOrWhiteSpace(sourceJsonTextBox.Text))
        {
            MessageBox.Show("Please select a source JSON file.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(outputJsonTextBox.Text))
        {
            MessageBox.Show("Please select an output JSON file.");
            return true;
        }

        if (placementHeuristicsCheckedListBox.CheckedItems.Count == 0)
        {
            MessageBox.Show("Please select at least one placement heuristic.");
            return true;
        }

        return false;
    }

    private void AddLabel(string text, int left, int top, int width)
    {
        Controls.Add(new Label
        {
            Text = text,
            Left = left,
            Top = top + 3,
            Width = width
        });
    }
}