using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Toybox.Pages;

public partial class MorseModel : PageModel
{
    public MorseModel(ILogger<MorseModel> logger)
    {
        this.logger = logger;
    }
    
    readonly ILogger<MorseModel> logger;

    [BindProperty]
    public string? Mode { get; set; }

    [BindProperty]
    public string? Value { get; set; }

    public void OnPost()
    {
        if (Mode is null || Value is null)
        {
            return;
        }

        if (Mode.Equals("encode"))
        {
            Value = Encode(Value.ToString().Trim());
        }
        else if (Mode.Equals("decode"))
        {
            Value = Decode(Value.ToString().Trim());
        }
    }

    string Encode(ReadOnlySpan<char> text)
    {
        var output = new StringBuilder();

        while (text.Length > 0)
        {
            if (char.IsWhiteSpace(text[0]))
            {
                output.Append(" // ");
            }
            else if (InternationalMorseCode.TryGetValue(text[0], out var morse))
            {
                if (output.Length > 0)
                {
                    output.Append(" / ");
                }
                output.Append(morse);
            }
            else
            {
                logger.LogInformation("No known morse value for character '{Character}', skipping.", text[0]);
            }
            text = text.Slice(1);
        }

        return output.ToString();
        
    }

    string Decode(ReadOnlySpan<char> text)
    {
        var output = new StringBuilder();
        
        while (text.Length > 0)
        {
            if (text.Length >= 2 && text[0] == '/' && text[1] == '/')
            {
                output.Append(" ");
                text = text.Slice(2);
            }
            else if (text[0] == '/')
            {
                text = text.Slice(1);
            }
            else
            {
                var indexOfWhiteSpace = -1;
                for (var i = 0; i < text.Length; i++)
                {
                    if (char.IsWhiteSpace(text[i]))
                    {
                        indexOfWhiteSpace = i;
                        break;
                    }
                }

                if (indexOfWhiteSpace == 0)
                {
                    text = text.Slice(1);
                    continue;
                }

                ReadOnlySpan<char> next;
                if (indexOfWhiteSpace > 0)
                {
                    next = text.Slice(0, indexOfWhiteSpace);
                    text = text.Slice(indexOfWhiteSpace + 1);
                }
                else
                {
                    next = text;
                    text = text.Slice(next.Length);
                }

                foreach (var kvp in InternationalMorseCode)
                {
                    if (MemoryExtensions.Equals(next, kvp.Value, StringComparison.Ordinal))
                    {
                        output.Append(kvp.Key);
                        break;
                    }
                }
            }
        }

        return output.ToString();
    }
}
