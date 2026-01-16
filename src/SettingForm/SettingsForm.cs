
public class SettingsForm : Form
{
    private TextBox sourceJsonTextBox;
    private TextBox outputJsonTextBox;
    private TextBox evolutionStatisticsCsvTextBox;

    private CheckedListBox placementHeuristicsCheckedListBox;

    private CheckBox allowRotationsCheckBox;

    private ComboBox packingOrderComboBox;
    private ComboBox algorithmComboBox;

    private NumericUpDown numberOfIndividualsNumeric;
    private NumericUpDown numberOfGenerationsNumeric;

    private Button okButton;
    private Button cancelButton;


    public IOSetting IOSetting { get; private set; }
    public PackingSetting PackingSetting { get; private set; }
    public EvolutionSetting EvolutionSetting { get; private set; }

    public SettingsForm()
    {
        Text = "Program Setting";
        Width = 600;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;

        InitializeComponents();


        WindowDesign.SetDesign(this);
    }

    private void InitializeComponents()
    {
        int labelWidth = 180;
        int controlWidth = 350;
        int top = 10;
        int spacing = 30;

        AddSourceJSON();
        outputJsonTextBox = AddOutputJSON("Outpot JSON:", "output.json");
        evolutionStatisticsCsvTextBox = AddOutputCSV("Evolution Statistics CSV:", "evolutionStatistics.csv");

        // Placement heuristics
        AddLabel("Placement Heuristics:", 10, top, labelWidth);
        placementHeuristicsCheckedListBox = new CheckedListBox
        {
            Left = 200,
            Top = top,
            Width = controlWidth,
            Height = 80
        };
        placementHeuristicsCheckedListBox.Items.AddRange(PlacementHeuristics.PlacementHeuristicsList.ToArray());
        placementHeuristicsCheckedListBox.SetItemChecked(0, true);
        Controls.Add(placementHeuristicsCheckedListBox);
        top += 90;

        // Allow rotations
        allowRotationsCheckBox = new CheckBox
        {
            Left = 200,
            Top = top,
            Text = "Allow Rotations",
            AutoSize = true,
            Checked = true
        };
        this.Controls.Add(allowRotationsCheckBox);
        top += spacing;



        // Packing order heuristic
        AddLabel("Packing Order Heuristics:", 10, top, labelWidth);
        packingOrderComboBox = new ComboBox
        {
            Left = 200,
            Top = top,
            Width = controlWidth,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        

        packingOrderComboBox.Items.AddRange((new[] { "None" })
        .Concat(OrderHeuristics.OrderHeuristicsList)
        .ToArray());
        packingOrderComboBox.SelectedIndex = 0;
        this.Controls.Add(packingOrderComboBox);
        top += spacing;


        // Algorithm name
        AddLabel("Algorithm:", 10, top, labelWidth);
        algorithmComboBox = new ComboBox
        {
            Left = 200,
            Top = top,
            Width = controlWidth,
            DropDownStyle = ComboBoxStyle.DropDownList,
        };
        algorithmComboBox.Items.AddRange(EvolutionaryAlgorithms.EvolutionaryAlgorithmsArray);
        algorithmComboBox.SelectedIndex = 0;
        this.Controls.Add(algorithmComboBox);
        top += spacing;

        // Number of Individuals
        AddLabel("Number of Individuals:", 10, top, labelWidth);
        numberOfIndividualsNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            Minimum = 1,
            Maximum = 100000,
            Value = 1000,
        };
        Controls.Add(numberOfIndividualsNumeric);
        top += spacing;

        // Number of Generations
        AddLabel("Number of Generations:", 10, top, labelWidth);
        numberOfGenerationsNumeric = new NumericUpDown
        {
            Left = 200,
            Top = top,
            Width = 100,
            Minimum = 1,
            Maximum = 100000,
            Value = 100
        };
        Controls.Add(numberOfGenerationsNumeric);
        top += spacing;














        // OK Button
        okButton = new Button
        {
            Text = "OK",
            Left = 200,
            Top = top,
            Width = 100
        };
        okButton.Click += OkButton_Click;
        Controls.Add(okButton);

        // Cancel Button
        cancelButton = new Button
        {
            Text = "Cancel",
            Left = 310,
            Top = top,
            Width = 100
        };
        cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(cancelButton);


        



        void AddSourceJSON()
        {
            AddLabel("Source JSON:", 10, top, labelWidth);
            sourceJsonTextBox = new TextBox
            {
                Left = 200,
                Top = top,
                Width = controlWidth - 90,
                ReadOnly = true
            };
            var browseSourceButton = new Button
            {
                Text = "Browse...",
                Left = 200 + controlWidth - 85,
                Top = top - 1,
                Width = 80
            };
            browseSourceButton.Click += (s, e) =>
            {
                using var ofd = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Choose Input JSON File"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    sourceJsonTextBox.Text = ofd.FileName;
                }
            };
            Controls.Add(sourceJsonTextBox);
            Controls.Add(browseSourceButton);
            top += spacing;
        }


        

        TextBox AddOutputJSON(string text, string defaultName)
        {
            string defaultPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            defaultName);

            AddLabel(text, 10, top, labelWidth);
            TextBox tb = new TextBox
            {
                Left = 200,
                Top = top,
                Width = controlWidth - 90,
                ReadOnly = true,
                Text = defaultPath
            };
            var browseOutputButton = new Button
            {
                Text = "Browse",
                Left = 200 + controlWidth - 85,
                Top = top - 1,
                Width = 80
            };
            browseOutputButton.Click += (s, e) =>
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Choose Output JSON File"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    tb.Text = sfd.FileName;
                }
            };
            Controls.Add(tb);
            Controls.Add(browseOutputButton);
            top += spacing;
            return tb;
        }

        TextBox AddOutputCSV(string text, string defaultName)
        {
            string defaultPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                defaultName);

            AddLabel(text, 10, top, labelWidth);

            TextBox tb = new TextBox
            {
                Left = 200,
                Top = top,
                Width = controlWidth - 90,
                ReadOnly = true,
                Text = defaultPath
            };

            var browseOutputButton = new Button
            {
                Text = "Browse",
                Left = 200 + controlWidth - 85,
                Top = top - 1,
                Width = 80
            };

            browseOutputButton.Click += (s, e) =>
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    Title = "Choose Output CSV File",
                    DefaultExt = "csv",
                    AddExtension = true
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    tb.Text = sfd.FileName;
                }
            };

            Controls.Add(tb);
            Controls.Add(browseOutputButton);

            top += spacing;
            return tb;
        }



    }

    private void AddLabel(string text, int left, int top, int width)
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


    private void OkButton_Click(object sender, EventArgs e)
    {

        if (Errors())
        {
            return;
        }

        

 

        // IO setting
        IOSetting = new IOSetting(sourceJsonTextBox.Text, outputJsonTextBox.Text);




        // evolution setting
        string algorithmName = algorithmComboBox.SelectedItem.ToString();

        int numberOfIndividuals = (int)numberOfIndividualsNumeric.Value;
        int numberOfGenerations = (int)numberOfGenerationsNumeric.Value;


        
        EvolutionSetting = new EvolutionSetting(algorithmName, numberOfIndividuals, numberOfGenerations, evolutionStatisticsCsvTextBox.Text);



        // packing setting
        bool allowRotations = allowRotationsCheckBox.Checked;
        string[] selectedPlacementHeuristics = placementHeuristicsCheckedListBox.CheckedItems.Cast<string>().ToArray();
        string selectedPackingOrderHeuristic = packingOrderComboBox.SelectedItem.ToString();
        string? onlySelectedPackingOrderHeuristics = selectedPackingOrderHeuristic == "None" ? null : selectedPackingOrderHeuristic;
        PackingSetting = new PackingSetting(selectedPlacementHeuristics, allowRotations, onlySelectedPackingOrderHeuristics);


        DialogResult = DialogResult.OK;
        Close();
    }

    private bool Errors()
    {
        if (string.IsNullOrWhiteSpace(sourceJsonTextBox.Text))
        {
            MessageBox.Show("Please select a source JSON file.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        if (string.IsNullOrWhiteSpace(outputJsonTextBox.Text))
        {
            MessageBox.Show("Please select an output JSON file.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        if (string.IsNullOrWhiteSpace(evolutionStatisticsCsvTextBox.Text))
        {
            MessageBox.Show("Please select an evolution statistics JSON file.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        if (placementHeuristicsCheckedListBox.CheckedItems.Count == 0)
        {
            MessageBox.Show("Please select at least one placement heuristic.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }
        return false;
    }
}
