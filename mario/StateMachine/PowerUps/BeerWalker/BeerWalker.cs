using Godot;
using System;
using Godot.Collections;


public partial class BeerWalker : Powerup
{


  


  public override void _Ready()
  {

    base._Ready();
    powerEnum = (int)Mario.PowerEnum.BIG;
    xVelocity = 100;
  }






}
