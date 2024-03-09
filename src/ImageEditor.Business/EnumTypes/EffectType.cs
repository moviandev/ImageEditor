namespace ImageEditor.Business.EnumTypes;
public enum EffectType
{
    NotSet = 0,
    Blur,
    Gray,
    Sepia,
    Vignette,
}

public static class EffectTypeExtension
{
    public static bool IsBlur(this EffectType effect)
        => effect == EffectType.Blur;


    public static bool IsVignette(this EffectType effect)
        => effect == EffectType.Vignette;

    public static bool IsGrayEffect(this EffectType effect)
        => effect == EffectType.Gray;

    public static bool IsSepiaEffect(this EffectType effect)
        => effect == EffectType.Sepia;
}