using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Toybox.Pages;

public partial class PokemonModel : PageModel
{
    public PokemonModel(ILogger<PokemonModel> logger)
    {
        this.logger = logger;
    }

    readonly ILogger<PokemonModel> logger;

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
                output.Append(" ");
            }
            else
            {
                var index = text[0] - 1; // The tables are 0-indexed but should be 1-indexed;
                if (index >= 0 && index < PokemonKanto.Count)
                {
                    if (output.Length > 0)
                    {
                        output.Append(" ");
                    }
                    output.Append(PokemonKanto[index]);
                }
                else
                {
                    logger.LogInformation("No known Pokemon value for character '{Character}' (for ASCII {ASCII}), skipping.", text[0], index);
                }
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
            if (text[0] == ' ')
            {
                output.Append(" ");
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

                var key = next.ToString();

                if (PokemonKantoReverse.TryGetValue(key, out var charCode))
                {
                    output.Append((char)(charCode + 1)); // Adjust for 0-indexing
                }
                else
                {
                    logger.LogInformation("No match for value {Value}", key);
                }
            }
        }

        return output.ToString();
    }
}
