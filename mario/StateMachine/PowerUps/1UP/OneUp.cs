using Godot;
using System;
using Godot.Collections;


public partial class OneUp : Powerup
{


  


  public override void _Ready()
  {

    base._Ready();
    powerEnum = (int)Mario.PowerEnum.UP;
    xVelocity = 100;
  }






}
