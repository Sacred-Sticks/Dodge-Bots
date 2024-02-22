using UnityEngine.UIElements;

public static class VisualElementExtensions
{
    public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
    {
        var child = new VisualElement();
        foreach (var @class in classes)
        {
            child.AddToClassList(@class);
        }
        parent.AddChild(child);
        return child;
    }

    public static VisualElement AddChild(this VisualElement parent, VisualElement child)
    {
        parent.hierarchy.Add(child);
        return child;
    }
}