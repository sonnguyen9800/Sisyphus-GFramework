using Godot;
using System;

namespace SisyphusFramework.Popup
{
    public abstract partial class PopupBase : Control
    {
        public struct PopupParam
        {
            public PopupManager Manager { get; set; }
            public string PopupName { get; set; }
        }
        private PopupManager _manager = null;
        private string _name = string.Empty;
        private TextureButton _closeBackgroundBtn = null;


        private const string NodeName_CloseBtnRect = "BackgroundCloseButton";
        public override void _Ready()
        {
            base._Ready();
            Callable callable = Callable.From(ClosePopup);

            if (_closeBackgroundBtn == null)
            {
                _closeBackgroundBtn = GetNode<TextureButton>(NodeName_CloseBtnRect);
                if (_closeBackgroundBtn == null)
                {
                    GD.PrintErr("Cannot find CloseButton");
                    return;
                }
                if (_closeBackgroundBtn.IsConnected("pressed", callable))
                    _closeBackgroundBtn.Disconnect("pressed", callable);
                _closeBackgroundBtn.Connect("pressed", callable);
            }
        }
        public virtual void Setup(PopupParam param)
        {
            _manager = param.Manager;
            _name = param.PopupName;



        }


        public virtual void OnShowing(object param = null)
        {
            Visible = true;
        }

        public virtual void OnHiding()
        {

        }

        public virtual void OnHidden()
        {
            Visible = false;
        }

        public virtual void ClosePopup()
        {
            _manager.ClosePopup(_name);
            OnHidden();

        }
    }
}
