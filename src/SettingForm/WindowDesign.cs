public static class WindowDesign
{
    public static void SetDesign(Form form)
    {
        form.BackColor = Color.RebeccaPurple;

        foreach (Control ctrl in form.Controls)
        {
            switch (ctrl)
            {
                case Label lbl:
                    lbl.ForeColor = Color.White;
                    break;

                case Button btn:
                    btn.BackColor = Color.ForestGreen;
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    break;

                case ComboBox cmb:
                    cmb.BackColor = Color.White;
                    cmb.ForeColor = Color.Black;
                    cmb.FlatStyle = FlatStyle.Flat;
                    break;

                case CheckBox cb:
                    cb.ForeColor = Color.White;
                    break;
            }
        }
    }
}