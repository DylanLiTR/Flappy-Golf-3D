using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display;
	[SerializeField, Range(0.1f, 2f)] float sampleDuration = 1f;

	int frames;
	float duration, bestDuration = float.MaxValue, worstDuration;

    void Update()
    {
		float frameDuration = Time.unscaledDeltaTime;
		frames += 1;
		duration += frameDuration;

		if (frameDuration < bestDuration)
		{
			bestDuration = frameDuration;
		}
		if (frameDuration > worstDuration)
		{
			worstDuration = frameDuration;
		}

		if (duration >= sampleDuration)
		{
			display.SetText(
				"FPS\n{0:0}\n{1:0}\n{2:0}",
				1f / bestDuration,
				frames / duration,
				1f / worstDuration
			);
			frames = 0;
			bestDuration = float.MaxValue;
			duration = 0f;
			worstDuration = 0f;
		}
	}
}
