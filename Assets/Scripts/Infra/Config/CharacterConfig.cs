using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;

public class CharacterConfig
{
    public string SpritesPath { get; init; }
    public string Portrait { get; init; }

    public static CharacterConfig Get(string id)
    {
        var config = Resources.Load<TextAsset>($"Configs/Characters/{id}");
        var input = new StringReader(config.text);

        var deserializer = new DeserializerBuilder()
        .WithNamingConvention(HyphenatedNamingConvention.Instance)
        .Build();

        return deserializer.Deserialize<CharacterConfig>(config.text);
    }
}