using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPooleable<T>
{
   T SetRecycleAction(Action<T> action);
   void TurnOn();
   void TurnOff();
}
 