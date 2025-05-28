using System;

public struct Position
{
    public float X;
    public float Y;

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
