using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZXing;
using ZXing.Datamatrix;

namespace Toybox.Pages;

public partial class DataMatrixModel : PageModel
{
    public DataMatrixModel(ILogger<DataMatrixModel> logger)
    {
        this.logger = logger;
    }
    
    readonly ILogger<DataMatrixModel> logger;

    [BindProperty]
    public string? Value { get; set; }

    public string? ImageUri { get; set; }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Value))
        {
            return;
        }

        var data = ZXingHelper.RenderBarcode(Value, BarcodeFormat.DATA_MATRIX, new DatamatrixEncodingOptions
        {
            Height = 128,
            Width = 128,
        });

        if (data == null)
        {
            logger.LogError("Failed to encode Data Matrix barcode.");
            return;
        }

        ImageUri = "data:image/png;base64," + Convert.ToBase64String(data);
    }
}
