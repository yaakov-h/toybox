using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;

namespace Toybox.Pages;

public partial class QRCodeModel : PageModel
{
    public QRCodeModel(ILogger<QRCodeModel> logger)
    {
        this.logger = logger;
    }
    
    readonly ILogger<QRCodeModel> logger;

    [BindProperty]
    public string? Value { get; set; }

    public string? ImageUri { get; set; }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Value))
        {
            return;
        }

        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(Value, QRCodeGenerator.ECCLevel.Q);
        var code = new PngByteQRCode(data);
        var imageData = code.GetGraphic(pixelsPerModule: 4);

        ImageUri = "data:image/png;base64," + Convert.ToBase64String(imageData);
    }
}
