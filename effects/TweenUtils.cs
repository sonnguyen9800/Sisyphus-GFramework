using System.Threading.Tasks;
using Godot;


namespace SisyphusFramework {
    public static class TweenUtils
    {

        public enum AlertType
        {
            Normal, 
            Warning
        }


        public static async Task TweenAlphaFadeInAndOut(Node parentNode, GodotObject tweenTarget, float durationFadeIn = 1.0f, float durationFadeOut = 1.15f, float durationTransition = 1.8f)
        {
            var tween = parentNode.GetTree().CreateTween();
            tween.TweenProperty(tweenTarget, "modulate:a", 1, durationFadeIn).SetEase(Tween.EaseType.In);
            tween.Chain().TweenProperty(tweenTarget, "modulate:a", 0, durationFadeOut).SetEase(Tween.EaseType.In).SetDelay(durationTransition);
            await parentNode.ToSignal(tween, Tween.SignalName.Finished);
        }
        
        public static async Task TweenAlphaFadeIn(Node parentNode, GodotObject tweenTarget, float durationFadeIn = 1.0f)
        {
            var tween = parentNode.GetTree().CreateTween();
            tween.TweenProperty(tweenTarget, "modulate:a", 1, durationFadeIn).SetEase(Tween.EaseType.In);
            await parentNode.ToSignal(tween, Tween.SignalName.Finished);
        }
        public static async Task TweenAlphaFadeOut(Node parentNode, GodotObject tweenTarget, float durationFadeOut = 1.0f)
        {
            var tween = parentNode.GetTree().CreateTween();
            tween.TweenProperty(tweenTarget, "modulate:a", 0, durationFadeOut).SetEase(Tween.EaseType.In);
            await parentNode.ToSignal(tween, Tween.SignalName.Finished);
        }

        public static async Task TweenMoveUpAndFadeOut(Node parentNode, GodotObject tweentarget, float duration = 1.0f, AlertType type = AlertType.Normal, float upLengh = 3.0f)
        {
            var tween = parentNode.GetTree().CreateTween();

            tween.TweenProperty(tweentarget, "position", Vector3.Up * upLengh, duration);
            tween.Parallel().TweenProperty(tweentarget, "modulate:a", 0, duration).SetEase(Tween.EaseType.In);
            await parentNode.ToSignal(tween, Tween.SignalName.Finished);

        }
    }
}
