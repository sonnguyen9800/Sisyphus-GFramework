using Godot;

namespace SisyphusFramework.Popup {
public  abstract partial class PopupBase : Control {
    public struct PopupParam {
        public PopupManager Manager {get; set;}
        public string PopupName {get; set;}
    }
    private PopupManager _manager = null;
    private string _name = string.Empty;
    public virtual void Setup(PopupParam param){
        _manager = param.Manager;
        _name = param.PopupName;
    }

    public virtual void OnShowing(object param = null){

    }

    public virtual void OnHiding(){

    }

    public virtual void OnHidden(){

    }

    public virtual void ClosePopup(){
        _manager.ClosePopup(_name);
        OnHidden();
        
    }
}
}
