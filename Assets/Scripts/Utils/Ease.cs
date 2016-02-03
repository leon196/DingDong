// From
// https://gist.github.com/uzzu/6173576

using UnityEngine;

/// <summary>
/// Ease functions.
/// </summary>/
public static class Ease
{
	#region basic easing
	public static float Linear(float variation, float elapsed, float delay, float offset)
	{
		return variation * CalcProgress(elapsed, delay) + offset;
	}

	public static float Swing(float variation, float elapsed, float delay, float offset)
	{
		return QuadOut(variation, elapsed, delay, offset);
	}
	#endregion

	#region quad easing
	public static float QuadIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * progress * progress + offset;
	}

	public static float QuadOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return -variation * progress * (progress - 2f) + offset;
	}

	public static float QuadInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * progressInHalf * progressInHalf + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return -variationInHalf * (progressLastHalf) * (progressLastHalf - 2f) + offset + variationInHalf;
	}

	public static float QuadOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return -variationInHalf * progressInHalf * (progressInHalf - 2f) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * progressLastHalf * progressLastHalf + offset + variationInHalf;
	}
	#endregion

	#region sine easing
	public static float SineIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return -variation * Mathf.Cos(progress * Mathf.PI * 0.5f) + variation + offset;
	}

	public static float SineOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * Mathf.Sin(progress * Mathf.PI * 0.5f) + offset;
	}

	public static float SineInOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * 0.5f * (Mathf.Cos(progress * Mathf.PI) - 1f) + offset;
	}

	public static float SineOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * Mathf.Sin(progressInHalf * (Mathf.PI * 0.5f)) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return -variationInHalf * Mathf.Cos(progressLastHalf * (Mathf.PI * 0.5f)) + variationInHalf + offset + variationInHalf;
	}
	#endregion

	#region cubic easing
	public static float CubicIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * progress * progress * progress + offset;
	}

	public static float CubicOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * (Mathf.Pow(progress - 1f, 3f) + 1f) + offset;
	}

	public static float CubicInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * progressInHalf * progressInHalf * progressInHalf + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * (Mathf.Pow(progressLastHalf - 1f, 3f) + 1f) + offset + variationInHalf;
	}

	public static float CubicOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * (Mathf.Pow(progressInHalf - 1f, 3f) + 1f) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * progressLastHalf * progressLastHalf * progressLastHalf + offset + variationInHalf;
	}
	#endregion

	#region quart easing
	public static float QuartIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * progress * progress * progress * progress + offset;
	}

	public static float QuartOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return -variation * (Mathf.Pow(progress - 1f, 4f) - 1f) + offset;
	}

	public static float QuartInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * progressInHalf * progressInHalf * progressInHalf * progressInHalf + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return -variationInHalf * (Mathf.Pow(progressLastHalf - 1f, 4f) - 1f) + offset + variationInHalf;
	}

	public static float QuartOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return -variationInHalf * (Mathf.Pow(progressInHalf - 1f, 4f) - 1f) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * progressLastHalf * progressLastHalf * progressLastHalf * progressLastHalf + offset + variationInHalf;
	}
	#endregion

	#region quint easing
	public static float QuintIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * progress * progress * progress * progress * progress + offset;
	}

	public static float QuintOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return -variation * (Mathf.Pow(progress - 1f, 5f) + 1f) + offset;
	}

	public static float QuintInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * progressInHalf * progressInHalf * progressInHalf * progressInHalf * progressInHalf + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return -variationInHalf * (Mathf.Pow(progressLastHalf - 1f, 5f) + 1f) + offset + variationInHalf;
	}

	public static float QuintOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return -variationInHalf * (Mathf.Pow(progressInHalf - 1f, 5f) + 1f) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * progressLastHalf * progressLastHalf * progressLastHalf * progressLastHalf * progressLastHalf + offset + variationInHalf;
	}
	#endregion

	#region expo easing
	public static float ExpoIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * Mathf.Pow(2f, 10f * (progress - 1f)) + offset;
	}

	public static float ExpoOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * (1f - Mathf.Pow(2f, -10f * progress)) + offset;
	}

	public static float ExpoInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * Mathf.Pow(2f, 10 * (progressInHalf - 1f)) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * (1f - Mathf.Pow(2f, -10f * progressLastHalf)) + offset + variationInHalf;
	}

	public static float ExpoOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * (1f - Mathf.Pow(2f, -10f * progressInHalf)) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * Mathf.Pow(2f, 10f * (progressLastHalf - 1f)) + offset + variationInHalf;
	}
	#endregion

	#region circ easing
	public static float CircIn(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return -variation * (Mathf.Sqrt(1f - progress * progress) - 1f) + offset;
	}

	public static float CircOut(float variation, float elapsed, float delay, float offset)
	{
		float progress = CalcProgress(elapsed, delay);
		return variation * Mathf.Sqrt(1f - Mathf.Pow(progress - 1f, 2f)) + offset;
	}

	public static float CircInOut(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return -variationInHalf * (Mathf.Sqrt(1f - progressInHalf * progressInHalf) - 1f) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return variationInHalf * (Mathf.Sqrt(1f - Mathf.Pow(progressLastHalf - 1f, 2f))) + offset + variationInHalf;
	}

	public static float CircOutIn(float variation, float elapsed, float delay, float offset)
	{
		float progressInHalf = CalcProgressInHalf(elapsed, delay);
		float variationInHalf = variation * 0.5f;
		if (progressInHalf < 1f)
		{
			return variationInHalf * (Mathf.Sqrt(1f - Mathf.Pow(progressInHalf - 1f, 2f))) + offset;
		}
		float progressLastHalf = progressInHalf - 1f;
		return -variationInHalf * (Mathf.Sqrt(1f - progressLastHalf * progressLastHalf) - 1f) + offset + variationInHalf;
	}
	#endregion

	#region private methods
	static float CalcProgress(float elapsed, float delay)
	{
		return Mathf.Clamp(elapsed / delay, 0f, 1f);
	}

	static float CalcProgressInHalf(float elapsed, float delay)
	{
		return Mathf.Clamp(elapsed / (delay * 0.5f), 0f, 2f);
	}
	#endregion
}
