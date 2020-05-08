using UnityEngine;
using UnityEngine.UI;

namespace Layouter
{
    [RequireComponent(typeof(Image))]
    public class ImageLayout : BaseLayout
    {
        public override void Initialize()
        {
            base.Initialize();
            
            var image = gameObject.GetComponent<Image>();
            image.preserveAspect = true;
        }
    }
}
