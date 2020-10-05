using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;

namespace VideoEffect
{

    /// <summary>
    /// a very basic video effect which just copies input frames to output nodes.
    /// </summary>
    public sealed class BasicVideoEffect : IBasicVideoEffect
    {
        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {

        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            //just copy frames from input to output
        }

        public void Close(MediaEffectClosedReason reason)
        {

        }

        public void DiscardQueuedFrames()
        {

        }

        public bool IsReadOnly => true;

        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties
        {
            get
            {
                List<VideoEncodingProperties> props = new List<VideoEncodingProperties>();
                return props.AsReadOnly();
            }
        }

        public MediaMemoryTypes SupportedMemoryTypes => MediaMemoryTypes.Gpu;

        public bool TimeIndependent => true;

        public void SetProperties(IPropertySet configuration)
        {

        }
    }
}
