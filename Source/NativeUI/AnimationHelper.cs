using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GTA;

namespace NativeUI {
    /// <summary>
    /// Helper class for GIFs.
    /// </summary>
    public class AnimationHelper {

        #region Helper
        public static byte[] BitmapToByte(Bitmap bmp)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream()) {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        #endregion

        #region Variables and Properties
        private Task task;

        private List<Texture> images;
        private Image targetImage;
        private FrameDimension targetImageDimension;
        private int targetImageFrames;
        private int index = 0;

        public bool IsLooping { get; private set; }
        #endregion

        #region Events
        public delegate void AnimatedTextureReturnerDelegate(Texture texture);
        public event AnimatedTextureReturnerDelegate AnimatedTextureReturner;
        #endregion

        #region Constructor
        protected internal AnimationHelper(Image image)
        {
            images = new List<Texture>();
            targetImage = image;
            targetImageDimension = new FrameDimension(targetImage.FrameDimensionsList[0]);
            targetImageFrames = targetImage.GetFrameCount(targetImageDimension);

            // Load images
            for (int i = 0; i < (targetImageFrames - 1); i++) {
                targetImage.SelectActiveFrame(targetImageDimension, i);
                using (Bitmap bmp = new Bitmap(targetImage)) {
                    images.Add(new Texture(BitmapToByte(bmp)));
                }
            }
        }
        #endregion

        public void StartLoopingGIF()
        {
            index = 0;
            targetImage.SelectActiveFrame(targetImageDimension, index);
            IsLooping = true;

            task = Task.Run(() => {
                while (IsLooping) {
                    // Looping through all frames
                    if ((images.Count - 1) != 0) {
                        AnimatedTextureReturner?.Invoke(images[index]);
                        if (index >= (images.Count - 1)) {
                            index = 0;
                        }
                        else {
                            index++;
                        }
                    }

                    // Delay
                    if (UIMenu.Options.AnimatedBannerFrameRate != 0) {
                        task.Wait(UIMenu.Options.AnimatedBannerFrameRate);
                    }
                    else {
                        task.Wait(120);
                    }
                }
            });
        }

        public void StopLoopingGIF()
        {
            IsLooping = false;
        }

        public Texture GetFirstFrameOfImage()
        {
            return images[0];
        }

    }
}
