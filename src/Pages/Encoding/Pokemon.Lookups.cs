using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Toybox.Pages;

partial class PokemonModel
{
    public class Lookups
    {
        public enum Pokedex
        {
            [Display(Name = "Kanto (R/B/Y) / National")]
            Kanto,
            [Display(Name = "Johto (G/S/C)")]
            Johto,
            [Display(Name = "Hoenn (R/S/E)")]
            Hoenn,
            [Display(Name = "Sinnoh (Platinum)")]
            SinnohPlatinum,
            [Display(Name = "Unova (B2/W2)")]
            Unova,
            [Display(Name = "Kalos (X/Y) - Central")]
            KalosCentral,
            [Display(Name = "Kalos (X/Y) - Coastal")]
            KalosCoastal,
            [Display(Name = "Kalos (X/Y) - Mountain")]
            KalosMountain,
            [Display(Name = "Alola (US/UM)")]
            AlolaUltra,
            [Display(Name = "Galar (Sw/Sh)")]
            Galar,
            [Display(Name = "Paldea (S/V)")]
            Paldea,
        }

        public static ImmutableDictionary<Pokedex, ImmutableList<string>> PokemonLists { get; } = BuildPokemonLists();

        static ImmutableDictionary<Pokedex, ImmutableList<string>> BuildPokemonLists()
        {
            var lookup = ImmutableDictionary.CreateBuilder<Pokedex, ImmutableList<string>>();

            lookup.Add(Pokedex.Kanto, ReadFile("pokemon-1-kanto"));
            lookup.Add(Pokedex.Johto, ReadFile("pokemon-2-johto"));
            lookup.Add(Pokedex.Hoenn, ReadFile("pokemon-3-hoenn"));
            lookup.Add(Pokedex.SinnohPlatinum, ReadFile("pokemon-4-sinnoh"));
            lookup.Add(Pokedex.Unova, ReadFile("pokemon-5-unova"));
            lookup.Add(Pokedex.KalosCentral, ReadFile("pokemon-6-kalos-central"));
            lookup.Add(Pokedex.KalosCoastal, ReadFile("pokemon-6-kalos-coastal"));
            lookup.Add(Pokedex.KalosMountain, ReadFile("pokemon-6-kalos-mountain"));
            lookup.Add(Pokedex.AlolaUltra, ReadFile("pokemon-7-alola"));
            lookup.Add(Pokedex.Galar, ReadFile("pokemon-8-galar"));
            lookup.Add(Pokedex.Paldea, ReadFile("pokemon-9-paldea"));

            return lookup.ToImmutable();

            static ImmutableList<string> ReadFile(string name)
            {
                var list = ImmutableList.CreateBuilder<string>();

                using (var stream = typeof(PokemonModel).Assembly.GetManifestResourceStream($"toybox.data.{name}.txt"))
                using (var reader = new StreamReader(stream!))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                return list.ToImmutable();
            }
        }

        public static ImmutableDictionary<Pokedex, ImmutableDictionary<string, int>> PokemonLookups { get; } = PokemonLists.ToImmutableDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select((name, idx) => (name, idx)).ToImmutableDictionary(kvp => kvp.name, kvp => kvp.idx)
        );
    }
}