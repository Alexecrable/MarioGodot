using Godot;
using System;
using Godot.Collections;


public partial class FireFlower : Powerup
{
	

    public override void _Ready()
    {
		  xVelocity = 0;
          powerEnum = (int)Mario.PowerEnum.FIRE;
		  base._Ready();
    }


	


	
}
