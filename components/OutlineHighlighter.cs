using Godot;

public partial class OutlineHighlighter : Node
{
    [Export] public CanvasGroup Visuals;
    [Export] public Color OutlineColour;
    [Export(PropertyHint.Range, "1, 10")]  public int OutlineThickness;

    public override void _Ready()
    {
        (Visuals.Material as ShaderMaterial).SetShaderParameter("line_color", OutlineColour);
    }

    public void ClearHighlight()
    {
        (Visuals.Material as ShaderMaterial).SetShaderParameter("line_thickness", 0);
    }
    public void SetHighlight()
    {
        (Visuals.Material as ShaderMaterial).SetShaderParameter("line_thickness", OutlineThickness);
    }
}
