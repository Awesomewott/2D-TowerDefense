using System.Collections.Generic;
using UnityEngine.UI;

public class ToggleGroupExtended : ToggleGroup
{
    public List<Toggle> GetToggles()
    {
        return m_Toggles;
    }
}
