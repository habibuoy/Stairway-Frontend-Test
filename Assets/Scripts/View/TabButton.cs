using Game.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TabButton : Clickable
    {
        [SerializeField] protected Image buttonImage;
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected string path;

        public string Path => path;

        public void Awake()
        {
            nameText.text = path;
        }

        public virtual void ToggleActive(bool active) { }
    }
}