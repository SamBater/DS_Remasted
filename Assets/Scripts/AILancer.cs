using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILancer : PlayerInput
{
    private void Update() {
      pressRB = true;
      ac.Attack();  
    }
}
