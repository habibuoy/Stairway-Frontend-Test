using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class BasicClickable : Clickable
    {
        [SerializeField] protected Image buttonImage;
        
        protected override void OnHoverBegun()
        {
            base.OnHoverBegun();
            Color color = buttonImage.color;
            color.a = 0.25f;

            buttonImage.color = color;
        }

        protected override void OnHoverEnded() 
        {
            base.OnHoverEnded();
            Color color = buttonImage.color;
            color.a = 1f;

            buttonImage.color = color;
        }
    }
}