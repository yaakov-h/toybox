using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZXing;
using ZXing.Common;

namespace Toybox.Pages;

public partial class Code128Model : PageModel
{
    public Code128Model(ILogger<Code128Model> logger)
    {
        this.logger = logger;
    }
    
    readonly ILogger<Code128Model> logger;

    [BindProperty]
    public string? Value { get; set; }

    public string? ImageUri { get; set; }

    public string? Error { get; set; }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Value))
        {
            return;
        }

        if (Value.Length > 80)
        {
            Error = "Input must be between 1 and 80 characters (inclusive).";
            return;
        }

        var data = ZXingHelper.RenderBarcode(Value, BarcodeFormat.CODE_128, new EncodingOptions
        {
            Height = 50,
            PureBarcode = true
        });

        if (data == null)
        {
            logger.LogError("Failed to encode Code128 barcode.");
            return;
        }

        ImageUri = "data:image/png;base64," + Convert.ToBase64String(data);
    }
}
