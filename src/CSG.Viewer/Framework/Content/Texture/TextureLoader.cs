namespace CSG.Viewer.Framework.Content
{
    using System;
    using System.Threading.Tasks;

    public class TextureLoader
    {
        private readonly IContentProvider contentProvider;

        public TextureLoader(IContentProvider contentProvider)
        {
            this.contentProvider = contentProvider ?? throw new ArgumentNullException(nameof(contentProvider));
        }

        public async Task<Texture2D> Load(string name)
        {
            if (name.StartsWith("v:"))
            {
                if (name == "v:white") return new Texture2D(1, 1, new byte[] { 255, 255, 255, 255 });
                if (name == "v:black") return new Texture2D(1, 1, new byte[] { 0, 0, 0, 255 });
                if (name == "v:checker") return new Texture2D(500, 500, CreateCheckerTexture(500, 500));
            }

            using (var stream = await contentProvider.Open(name).ConfigureAwait(false))
            {
                throw new NotImplementedException();
            }
        }

        private static byte[] CreateCheckerTexture(int width, int height, int rows = 5)
        {
            var dw = width / rows;
            var data = new byte[width * height * 4];
            for (int i = 0; i < data.Length; i += 4)
            {
                int pixelIndex = (i / 4);
                int x = pixelIndex / width;
                int y = pixelIndex % width;

                byte color = (x / dw + y / dw) % 2 == 0 ? (byte)255 : (byte)0;
                data[i + 0] = color;
                data[i + 1] = color;
                data[i + 2] = color;
                data[i + 3] = 255;
            }

            return data;
        }
    }
}
