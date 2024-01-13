using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Epidemic/Config/Interactable Highlight Config")]
public class InteractableHighlightConfig : ScriptableObject
{
    [field: SerializeField] public Color Highlight { get; private set; }
    [field: SerializeField] public Color ChosedHighlight { get; private set; }
}
