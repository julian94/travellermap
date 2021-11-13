namespace Traveller.Core.Features.Structures;

public record struct EHex
{
    private int RawValue { get; init; }

    public EHex(int v) => RawValue = v;
    public EHex(char v) => this = new EHex(v.ToString());
    public EHex(string v)
    {
        if (int.TryParse(v, out var number) &&
            number >= 0 &&
            number <= 9) RawValue = number;
        else if (Enum.TryParse<HexCodes>(v, out var code)) RawValue = (int)code;
        else RawValue = 0;
    }

    public override string ToString()
    {
        if (RawValue <= 9) return RawValue.ToString();
        else return ((HexCodes)RawValue).ToString();
    }

    public int GetRawValue() => RawValue;

    private enum HexCodes
    {
        A = 10,
        B = 11,
        C = 12,
        D = 13,
        E = 14,
        F = 15,
        G = 16,
        H = 17,
        J = 18, // Skipping I
        K = 19,
        L = 20,
        M = 21,
        N = 22,
        P = 23, // Skipping O
        Q = 24,
        R = 25,
        S = 26,
        T = 27,
        U = 28,
        V = 29,
        W = 30,
        X = 31,
        Y = 32,
        Z = 33,
    }
}
