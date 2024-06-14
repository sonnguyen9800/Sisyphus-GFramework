using Godot;
using System;

namespace SisyphusG {
public abstract partial class BaseSingleton<T> : Node where T : Node
{
	private static T _instance = null;
	public static T Instance => _instance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _EnterTree()
	{
		if (_instance != null)
		{
			this.QueueFree(); // The Singletone is already loaded, kill this instance
			GD.PrintErr("This singleton already loaded. Remove this");
		}
		_instance = this as T;
	}
}

}
