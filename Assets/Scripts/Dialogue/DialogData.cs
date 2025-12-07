using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogEntry {
    public Color textColor = Color.white;
    public string text;
}

[CreateAssetMenu(menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogEntry> entries;
}
