using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisyphusFramework
{
    public static class TweenAudioUtils
    {
        public enum TweenType
        {
            FadeIn,
            FadeOut,
        }

        public static async Task TweenSound(AudioStreamPlayer audio, float target, float durationFadeIn = 1.0f, TweenType type = TweenType.FadeIn)
        {
            var tween = audio.GetTree().CreateTween();
            tween.TweenProperty(audio, "volume_db", target, durationFadeIn).SetEase(type == TweenType.FadeIn ? Tween.EaseType.Out : Tween.EaseType.In);
            await audio.ToSignal(tween, Tween.SignalName.Finished);
        }
        public static async Task TweenSound(AudioStreamPlayer2D audio, float target, float durationFadeIn = 1.0f, TweenType type = TweenType.FadeIn)
        {
            var tween = audio.GetTree().CreateTween();
            tween.TweenProperty(audio, "volume_db", target, durationFadeIn).SetEase(type == TweenType.FadeIn ? Tween.EaseType.Out : Tween.EaseType.In);
            await audio.ToSignal(tween, Tween.SignalName.Finished);
        }
    }
}
