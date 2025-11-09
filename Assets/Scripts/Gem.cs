using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum Effect
{
    PainReduction,
    Metamorphosis,
    ManaIncrease
}

public enum GemColor
{
    Red,
    White,
    Blue
}

// Tak na szybko klasa, mo¿na j¹ bêdzie potem ca³kiem przerobiæ
public class Gem
{
    public Effect effect;
    public GemColor color;
    public int price;

    public Gem(GemColor color)
    {
        this.color = color;
        effect = colorToEffect[color];
        price = colorToPrice[color];
    }

    public Gem(Effect effect)
    {
        color = effectToColor[effect];
        this.effect = effect;
        price = colorToPrice[color];
    }

    public Dictionary<GemColor, int> colorToPrice = new Dictionary<GemColor, int>()
    {
        { GemColor.Red, 60 },
        { GemColor.White, 100 },
        { GemColor.Blue, 120 },
    };

    public Dictionary<GemColor, Effect> colorToEffect = new Dictionary<GemColor, Effect>()
    {
        { GemColor.Red, Effect.PainReduction },
        { GemColor.White, Effect.Metamorphosis },
    };

    public Dictionary<Effect, GemColor> effectToColor = new Dictionary<Effect, GemColor>()
    {
        { Effect.PainReduction, GemColor.Red },
        { Effect.Metamorphosis, GemColor.White },
    };

}

/*public static class GemProperties {

    public static int getPrice<T>(T property)
    {
        if(property.GetType() == typeof(GemColor)) {
            switch (property)
            {
                case GemColor.Red:
                    return 60;
                case GemColor.White:
                    return 100;
                case GemColor.Blue:
                    return 120;
                default:
                    return 50;
            }
        }
        if(property.GetType() == typeof(Effect))
        {
            switch (property)
            {
                case Effect.PainReduction:
                    return 60;
                case Effect.Metamorphosis:
                    return 100;
                case Effect.ManaIncrease:
                    return 120;
                default:
                    return 50;
            }
        }

        // If the type T was not GemColor of Effect
        return 0;

    }

    public static Dictionary<GemColor, int> colorToPrice = new Dictionary<GemColor, int>()
    {
        { GemColor.Red, 60 },
        { GemColor.White, 100 },
        { GemColor.Blue, 120 },
    };

    public static Dictionary<GemColor, Effect> colorToEffect = new Dictionary<GemColor, Effect>()
    {
        { GemColor.Red, Effect.PainReduction },
        { GemColor.White, Effect.Metamorphosis },
    };

    public static Dictionary<Effect, GemColor> effectToColor = new Dictionary<Effect, GemColor>()
    {
        { Effect.PainReduction, GemColor.Red },
        { Effect.Metamorphosis, GemColor.White },
    };
}*/