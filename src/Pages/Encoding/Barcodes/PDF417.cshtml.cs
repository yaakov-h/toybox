using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZXing;
using ZXing.PDF417;
using ZXing.PDF417.Internal;

namespace Toybox.Pages;

public partial class PDF417Model : PageModel
{
    public PDF417Model(ILogger<PDF417Model> logger)
    {
        this.logger = logger;
    }
    
    readonly ILogger<PDF417Model> logger;

    [BindProperty]
    public string? Value { get; set; }

    public string? ImageUri { get; set; }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Value))
        {
            return;
        }

        var data = ZXingHelper.RenderBarcode(Value, BarcodeFormat.PDF_417, new PDF417EncodingOptions
        {
            Dimensions = new Dimensions(1, 500, 1, 500)
        });

        if (data == null)
        {
            logger.LogError("Failed to encode PDF417 barcode.");
            return;
        }

        ImageUri = "data:image/png;base64," + Convert.ToBase64String(data);
    }
}
