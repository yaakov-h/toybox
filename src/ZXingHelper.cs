using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

static class ZXingHelper
{
    public static byte[]? RenderBarcode(string text, BarcodeFormat format, EncodingOptions options)
    {
        var w = new BarcodeWriter
        {
            Format = format,
            Options = options
        };
        var bitmap = w.Write(text);

        using (var imageStream = new SKDynamicMemoryWStream())
        {
            var success = bitmap.Encode(imageStream, SKEncodedImageFormat.Png, 100);

            if (!success)
            {
                return null;
            }

            var data = imageStream.DetachAsData();
            return data.ToArray();
        }
    }
}