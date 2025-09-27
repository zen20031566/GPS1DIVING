using System;

public class WeightEvents
{
    public event Action<float> OnWeightChange;

    public void WeightChange(float weight)
    {
        OnWeightChange?.Invoke(weight);
    }
}
