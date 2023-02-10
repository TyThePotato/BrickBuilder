public struct FloatRange {
    public float Min;
    public float Max;
    public float Size => Max - Min;

    public FloatRange(float min, float max) {
        // ensure correct order
        Min = min < max ? min : max;
        Max = max > min ? max : min;
    }

    /// <summary>
    /// Returns if a number is between this FloatRange's Min and Max values.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Within(float position) {
        return position >= Min && position <= Max;
    }

    /// <summary>
    /// Returns if a FloatRange is within this FloatRange
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Within(FloatRange other) {
        return Within(other.Min) || Within(other.Max);
    }

    /// <summary>
    /// Clamps Min and Max values to the specified area while [optionally] preserving original size.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="preserveSize"></param>
    public void Clamp(float min, float max, bool preserveSize = false) {
        float originalSize = Size;
        
        Min = Min < min ? min : Min > max ? max : Min;
        Max = Max < min ? min : Max > max ? max : Max;

        if (preserveSize && Size != originalSize) {
            float sizeDifference = originalSize - Size;
            if (Min == min) {
                // extend max
                Max = Max + sizeDifference > max ? max : Max + sizeDifference;
            } else if (Max == max) {
                // extend min
                Min = Min - sizeDifference < min ? min : Min - sizeDifference;
            }
        }
    }

    public override string ToString() {
        return $"({Min}, {Max})";
    }
}
