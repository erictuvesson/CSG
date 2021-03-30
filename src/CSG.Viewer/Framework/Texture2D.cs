namespace CSG.Viewer.Framework
{
    using System;
    using Veldrid;

    public class Texture2D
    {
        public PixelFormat Format { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }
        public uint MipLevels { get; set; }
        public uint ArrayLayers { get; set; }
        public byte[] Data { get; set; }

        public Texture2D(uint width, uint height, byte[] data)
            : this(PixelFormat.R8_G8_B8_A8_UNorm, width, height, 1, 1, 1, data)
        {

        }

        public Texture2D(PixelFormat format, uint width, uint height, uint depth,
                         uint mipLevels, uint arrayLayers, byte[] data)
        {
            this.Format = format;
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            this.MipLevels = mipLevels;
            this.ArrayLayers = arrayLayers;
            this.Data = data;

            // Validate the format
            GetFormatSize(format);
        }

        public unsafe Texture CreateDeviceTexture(GraphicsDevice gd, ResourceFactory rf, TextureUsage usage)
        {
            Texture texture = rf.CreateTexture(new TextureDescription(
                Width, Height, Depth, MipLevels, ArrayLayers, Format, usage, TextureType.Texture2D));

            Texture staging = rf.CreateTexture(new TextureDescription(
                Width, Height, Depth, MipLevels, ArrayLayers, Format, TextureUsage.Staging, TextureType.Texture2D));

            ulong offset = 0;
            fixed (byte* texDataPtr = &Data[0])
            {
                for (uint level = 0; level < MipLevels; level++)
                {
                    uint mipWidth = GetDimension(Width, level);
                    uint mipHeight = GetDimension(Height, level);
                    uint mipDepth = GetDimension(Depth, level);
                    uint subresourceSize = mipWidth * mipHeight * mipDepth * GetFormatSize(Format);

                    for (uint layer = 0; layer < ArrayLayers; layer++)
                    {
                        gd.UpdateTexture(
                            staging, (IntPtr)(texDataPtr + offset), subresourceSize,
                            0, 0, 0, mipWidth, mipHeight, mipDepth,
                            level, layer);
                        offset += subresourceSize;
                    }
                }
            }

            CommandList cl = rf.CreateCommandList();
            cl.Begin();
            cl.CopyTexture(staging, texture);
            cl.End();
            gd.SubmitCommands(cl);

            return texture;
        }

        private uint GetFormatSize(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8_G8_B8_A8_UNorm: return 4;
                case PixelFormat.BC3_UNorm: return 1;
                default: throw new NotSupportedException(nameof(format));
            }
        }

        public static uint GetDimension(uint largestLevelDimension, uint mipLevel)
        {
            uint ret = largestLevelDimension;
            for (uint i = 0; i < mipLevel; i++)
            {
                ret /= 2;
            }

            return Math.Max(1, ret);
        }
    }
}
