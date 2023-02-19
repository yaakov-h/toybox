using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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
    public string? Pokedex { get; set; }

    [BindProperty]
    public string? Value { get; set; }

    public bool Selected(string text) => string.Equals(text, Pokedex, StringComparison.OrdinalIgnoreCase);

    public void OnPost()
    {
        if (Mode is null || Value is null)
        {
            return;
        }

        if (Pokedex is null || !Enum.TryParse<Lookups.Pokedex>(Pokedex, ignoreCase: true, out var pokedex))
        {
            logger.LogInformation("Unable to parse Pokedex value '{Pokedex}'", Pokedex);
            return;
        }

        if (Mode.Equals("encode"))
        {
            Value = Encode(pokedex, Value.ToString().Trim());
        }
        else if (Mode.Equals("decode"))
        {
            Value = Decode(pokedex, Value.ToString().Trim());
        }
    }

    string Encode(Lookups.Pokedex pokedex, ReadOnlySpan<char> text)
    {
        var output = new StringBuilder();
        var list = Lookups.PokemonLists[pokedex];

        while (text.Length > 0)
        {
            if (char.IsWhiteSpace(text[0]))
            {
                output.Append(" ");
            }
            else
            {
                var index = text[0] - 1; // The tables are 0-indexed but should be 1-indexed;
                if (index >= 0 && index < list.Count)
                {
                    if (output.Length > 0)
                    {
                        output.Append(" ");
                    }
                    output.Append(list[index]);
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

    string Decode(Lookups.Pokedex pokedex, ReadOnlySpan<char> text)
    {
        var output = new StringBuilder();
        var table = Lookups.PokemonLookups[pokedex];
        
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

                if (table.TryGetValue(key, out var charCode))
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
